using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

using log4net;

using ACE.Common;
using ACE.Common.Extensions;
using ACE.Database.Entity;
using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Database.Models.Auth;
using System.Text;

namespace ACE.Database
{
    public class ShardDatabase
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool Exists(bool retryUntilFound)
        {
            var config = Common.ConfigManager.Config.MySql.Shard;

            for (; ; )
            {
                using (var context = new ShardDbContext())
                {
                    if (((RelationalDatabaseCreator)context.Database.GetService<IDatabaseCreator>()).Exists())
                    {
                        log.DebugFormat("[DATABASE] Successfully connected to {0} database on {1}:{2}.", config.Database, config.Host, config.Port);
                        return true;
                    }
                }

                log.Error($"[DATABASE] Attempting to reconnect to {config.Database} database on {config.Host}:{config.Port} in 5 seconds...");

                if (retryUntilFound)
                    Thread.Sleep(5000);
                else
                    return false;
            }
        }


        /// <summary>
        /// Will return uint.MaxValue if no records were found within the range provided.
        /// </summary>
        public uint GetMaxGuidFoundInRange(uint min, uint max)
        {
            using (var context = new ShardDbContext())
            {
                var result = context.Biota
                    .AsNoTracking()
                    .Where(r => r.Id >= min && r.Id <= max)
                    .OrderByDescending(r => r.Id)
                    .FirstOrDefault();

                if (result == null)
                    return uint.MaxValue;

                return result.Id;
            }
        }

        /// <summary>
        /// This will return available id's, in the form of sequence gaps starting from min.<para />
        /// If a gap is just 1 value wide, then both start and end will be the same number.
        /// </summary>
        public List<(uint start, uint end)> GetSequenceGaps(uint min, uint limitAvailableIDsReturned)
        {
            // References:
            // https://stackoverflow.com/questions/4340793/how-to-find-gaps-in-sequential-numbering-in-mysql/29736658#29736658
            // https://stackoverflow.com/questions/50402015/how-to-execute-sqlquery-with-entity-framework-core-2-1

            // This query is ugly, but very fast.
            var sql = "SET @available_ids=0, @rownum=0;"                                                + Environment.NewLine +
                      "SELECT"                                                                          + Environment.NewLine +
                      " z.gap_starts_at, z.gap_ends_at_not_inclusive, @available_ids:=@available_ids+(z.gap_ends_at_not_inclusive - z.gap_starts_at) as running_total_available_ids" + Environment.NewLine +
                      "FROM ("                                                                          + Environment.NewLine +
                      " SELECT"                                                                         + Environment.NewLine +
                      "  @rownum:=@rownum+1 AS gap_starts_at,"                                          + Environment.NewLine +
                      "  @available_ids:=0,"                                                            + Environment.NewLine +
                      "  IF(@rownum=id, 0, @rownum:=id) AS gap_ends_at_not_inclusive"                   + Environment.NewLine +
                      " FROM"                                                                           + Environment.NewLine +
                      "  (SELECT @rownum:=(SELECT MIN(id)-1 FROM biota WHERE id > " + min + ")) AS a"   + Environment.NewLine +
                      "  JOIN biota"                                                                    + Environment.NewLine +
                      "  WHERE id > " + min                                                             + Environment.NewLine +
                      "  ORDER BY id"                                                                   + Environment.NewLine +
                      " ) AS z" + Environment.NewLine;
            if (limitAvailableIDsReturned != uint.MaxValue)
                sql += "WHERE z.gap_ends_at_not_inclusive!=0 AND @available_ids<" + limitAvailableIDsReturned + "; ";
            else
                sql += "WHERE z.gap_ends_at_not_inclusive!=0;";

            using (var context = new ShardDbContext())
            {
                context.Database.SetCommandTimeout(TimeSpan.FromMinutes(5));

                var connection = context.Database.GetDbConnection();
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = sql;
                var reader = command.ExecuteReader();

                var gaps = new List<(uint start, uint end)>();

                while (reader.Read())
                {
                    var gap_starts_at               = reader.GetFieldValue<long>(0);
                    var gap_ends_at_not_inclusive   = reader.GetFieldValue<decimal>(1);
                    //var running_total_available_ids = reader.GetFieldValue<double>(2);

                    gaps.Add(((uint)gap_starts_at, (uint)gap_ends_at_not_inclusive - 1));
                }

                return gaps;
            }
        }


        public int GetBiotaCount()
        {
            using (var context = new ShardDbContext())
                return context.Biota.Count();
        }

        public int GetEstimatedBiotaCount(string dbName)
        {
            // https://mariadb.com/kb/en/incredibly-slow-count-on-mariadb-mysql/

            var sql = $"SELECT TABLE_ROWS FROM information_schema.tables" + Environment.NewLine +
                      $"WHERE TABLE_SCHEMA = '{dbName}'" + Environment.NewLine +
                      $"AND TABLE_NAME = 'biota';";

            using (var context = new ShardDbContext())
            {
                var connection = context.Database.GetDbConnection();
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = sql;
                var reader = command.ExecuteReader();

                var biotaEstimatedCount = 0;

                while (reader.Read())
                {
                    biotaEstimatedCount = reader.GetFieldValue<int>(0);
                }

                return biotaEstimatedCount;
            }    
        }

        [Flags]
        public enum PopulatedCollectionFlags
        {
            BiotaPropertiesAnimPart             = 0x1,
            BiotaPropertiesAttribute            = 0x2,
            BiotaPropertiesAttribute2nd         = 0x4,
            BiotaPropertiesBodyPart             = 0x8,
            BiotaPropertiesBook                 = 0x10,
            BiotaPropertiesBookPageData         = 0x20,
            BiotaPropertiesBool                 = 0x40,
            BiotaPropertiesCreateList           = 0x80,
            BiotaPropertiesDID                  = 0x100,
            BiotaPropertiesEmote                = 0x200,
            BiotaPropertiesEnchantmentRegistry  = 0x400,
            BiotaPropertiesEventFilter          = 0x800,
            BiotaPropertiesFloat                = 0x1000,
            BiotaPropertiesGenerator            = 0x2000,
            BiotaPropertiesIID                  = 0x4000,
            BiotaPropertiesInt                  = 0x8000,
            BiotaPropertiesInt64                = 0x10000,
            BiotaPropertiesPalette              = 0x20000,
            BiotaPropertiesPosition             = 0x40000,
            BiotaPropertiesSkill                = 0x80000,
            BiotaPropertiesSpellBook            = 0x100000,
            BiotaPropertiesString               = 0x200000,
            BiotaPropertiesTextureMap           = 0x400000,
            HousePermission                     = 0x800000,
            BiotaPropertiesAllegiance           = 0x1000000,
        }

        public static void SetBiotaPopulatedCollections(Biota biota)
        {
            PopulatedCollectionFlags populatedCollectionFlags = 0;

            if (biota.BiotaPropertiesAnimPart != null && biota.BiotaPropertiesAnimPart.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesAnimPart;
            if (biota.BiotaPropertiesAttribute != null && biota.BiotaPropertiesAttribute.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesAttribute;
            if (biota.BiotaPropertiesAttribute2nd != null && biota.BiotaPropertiesAttribute2nd.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesAttribute2nd;
            if (biota.BiotaPropertiesBodyPart != null && biota.BiotaPropertiesBodyPart.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesBodyPart;
            if (biota.BiotaPropertiesBook != null) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesBook;
            if (biota.BiotaPropertiesBookPageData != null && biota.BiotaPropertiesBookPageData.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesBookPageData;
            if (biota.BiotaPropertiesBool != null && biota.BiotaPropertiesBool.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesBool;
            if (biota.BiotaPropertiesCreateList != null && biota.BiotaPropertiesCreateList.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesCreateList;
            if (biota.BiotaPropertiesDID != null && biota.BiotaPropertiesDID.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesDID;
            if (biota.BiotaPropertiesEmote != null && biota.BiotaPropertiesEmote.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesEmote;
            if (biota.BiotaPropertiesEnchantmentRegistry != null && biota.BiotaPropertiesEnchantmentRegistry.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesEnchantmentRegistry;
            if (biota.BiotaPropertiesEventFilter != null && biota.BiotaPropertiesEventFilter.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesEventFilter;
            if (biota.BiotaPropertiesFloat != null && biota.BiotaPropertiesFloat.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesFloat;
            if (biota.BiotaPropertiesGenerator != null && biota.BiotaPropertiesGenerator.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesGenerator;
            if (biota.BiotaPropertiesIID != null && biota.BiotaPropertiesIID.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesIID;
            if (biota.BiotaPropertiesInt != null && biota.BiotaPropertiesInt.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesInt;
            if (biota.BiotaPropertiesInt64 != null && biota.BiotaPropertiesInt64.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesInt64;
            if (biota.BiotaPropertiesPalette != null && biota.BiotaPropertiesPalette.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesPalette;
            if (biota.BiotaPropertiesPosition != null && biota.BiotaPropertiesPosition.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesPosition;
            if (biota.BiotaPropertiesSkill != null && biota.BiotaPropertiesSkill.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesSkill;
            if (biota.BiotaPropertiesSpellBook != null && biota.BiotaPropertiesSpellBook.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesSpellBook;
            if (biota.BiotaPropertiesString != null && biota.BiotaPropertiesString.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesString;
            if (biota.BiotaPropertiesTextureMap != null && biota.BiotaPropertiesTextureMap.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesTextureMap;
            if (biota.HousePermission != null && biota.HousePermission.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.HousePermission;
            if (biota.BiotaPropertiesAllegiance != null && biota.BiotaPropertiesAllegiance.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesAllegiance;

            biota.PopulatedCollectionFlags = (uint)populatedCollectionFlags;
        }

        public virtual Biota GetBiota(ShardDbContext context, uint id, bool doNotAddToCache = false)
        {
            var biota = context.Biota
                .FirstOrDefault(r => r.Id == id);

            if (biota == null)
                return null;

            PopulatedCollectionFlags populatedCollectionFlags = (PopulatedCollectionFlags)biota.PopulatedCollectionFlags;

            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesAnimPart)) biota.BiotaPropertiesAnimPart = context.BiotaPropertiesAnimPart.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesAttribute)) biota.BiotaPropertiesAttribute = context.BiotaPropertiesAttribute.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesAttribute2nd)) biota.BiotaPropertiesAttribute2nd = context.BiotaPropertiesAttribute2nd.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesBodyPart)) biota.BiotaPropertiesBodyPart = context.BiotaPropertiesBodyPart.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesBook)) biota.BiotaPropertiesBook = context.BiotaPropertiesBook.FirstOrDefault(r => r.ObjectId == biota.Id);
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesBookPageData)) biota.BiotaPropertiesBookPageData = context.BiotaPropertiesBookPageData.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesBool)) biota.BiotaPropertiesBool = context.BiotaPropertiesBool.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesCreateList)) biota.BiotaPropertiesCreateList = context.BiotaPropertiesCreateList.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesDID)) biota.BiotaPropertiesDID = context.BiotaPropertiesDID.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesEmote)) biota.BiotaPropertiesEmote = context.BiotaPropertiesEmote.Include(r => r.BiotaPropertiesEmoteAction).Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesEnchantmentRegistry)) biota.BiotaPropertiesEnchantmentRegistry = context.BiotaPropertiesEnchantmentRegistry.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesEventFilter)) biota.BiotaPropertiesEventFilter = context.BiotaPropertiesEventFilter.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesFloat)) biota.BiotaPropertiesFloat = context.BiotaPropertiesFloat.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesGenerator)) biota.BiotaPropertiesGenerator = context.BiotaPropertiesGenerator.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesIID)) biota.BiotaPropertiesIID = context.BiotaPropertiesIID.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesInt)) biota.BiotaPropertiesInt = context.BiotaPropertiesInt.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesInt64)) biota.BiotaPropertiesInt64 = context.BiotaPropertiesInt64.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesPalette)) biota.BiotaPropertiesPalette = context.BiotaPropertiesPalette.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesPosition)) biota.BiotaPropertiesPosition = context.BiotaPropertiesPosition.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesSkill)) biota.BiotaPropertiesSkill = context.BiotaPropertiesSkill.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesSpellBook)) biota.BiotaPropertiesSpellBook = context.BiotaPropertiesSpellBook.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesString)) biota.BiotaPropertiesString = context.BiotaPropertiesString.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesTextureMap)) biota.BiotaPropertiesTextureMap = context.BiotaPropertiesTextureMap.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.HousePermission)) biota.HousePermission = context.HousePermission.Where(r => r.HouseId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesAllegiance)) biota.BiotaPropertiesAllegiance = context.BiotaPropertiesAllegiance.Where(r => r.AllegianceId == biota.Id).ToList();

            return biota;
        }

        public virtual Biota GetBiota(uint id, bool doNotAddToCache = false)
        {
            using (var context = new ShardDbContext())
                return GetBiota(context, id, doNotAddToCache);
        }

        public List<Biota> GetBiotasByWcid(uint wcid)
        {
            using (var context = new ShardDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var results = context.Biota.Where(r => r.WeenieClassId == wcid);

                var biotas = new List<Biota>();
                foreach (var result in results)
                {
                    var biota = GetBiota(result.Id);
                    biotas.Add(biota);
                }

                return biotas;
            }
        }

        public List<Biota> GetBiotasByType(WeenieType type)
        {
            // warning: this query is currently unindexed!
            using (var context = new ShardDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var iType = (int)type;

                var results = context.Biota.Where(r => r.WeenieType == iType);

                var biotas = new List<Biota>();
                foreach (var result in results)
                {
                    var biota = GetBiota(result.Id);
                    biotas.Add(biota);
                }

                return biotas;
            }
        }

        protected bool DoSaveBiota(ShardDbContext context, Biota biota)
        {
            SetBiotaPopulatedCollections(biota);

            Exception firstException = null;
            retry:

            try
            {
                context.SaveChanges();

                if (firstException != null)
                    log.DebugFormat("[DATABASE] DoSaveBiota 0x{0:X8}:{1} retry succeeded after initial exception of: {2}", biota.Id, biota.GetProperty(PropertyString.Name), firstException.GetFullMessage());

                return true;
            }
            catch (Exception ex)
            {
                if (firstException == null)
                {
                    firstException = ex;
                    goto retry;
                }

                // Character name might be in use or some other fault
                log.Error($"[DATABASE] DoSaveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed first attempt with exception: {firstException.GetFullMessage()}");
                log.Error($"[DATABASE] DoSaveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed second attempt with exception: {ex.GetFullMessage()}");
                return false;
            }
        }

        public void ProcessPartialSave(ref ACE.Entity.Models.Biota biota)
        {
            if (biota.IsPartiallyPersistant)
            {
                ACE.Entity.Models.Biota partialBiota = new ACE.Entity.Models.Biota();
                partialBiota.Id = biota.Id;
                partialBiota.WeenieClassId = biota.WeenieClassId;
                partialBiota.WeenieType = biota.WeenieType;
                partialBiota.PartialPersitanceFilter = biota.PartialPersitanceFilter;

                foreach (var entry in biota.PartialPersitanceFilter)
                {
                    if (entry.PropertyType == PropertyType.PropertyBool)
                    {
                        if (biota.PropertiesBool.TryGetValue((PropertyBool)entry.Property, out var entryValue))
                        {
                            if (partialBiota.PropertiesBool == null)
                                partialBiota.PropertiesBool = new Dictionary<PropertyBool, bool>();
                            partialBiota.PropertiesBool.Add((PropertyBool)entry.Property, entryValue);
                        }
                    }
                    else if (entry.PropertyType == PropertyType.PropertyDataId)
                    {
                        if (biota.PropertiesDID.TryGetValue((PropertyDataId)entry.Property, out var entryValue))
                        {
                            if (partialBiota.PropertiesDID == null)
                                partialBiota.PropertiesDID = new Dictionary<PropertyDataId, uint>();
                            partialBiota.PropertiesDID.Add((PropertyDataId)entry.Property, entryValue);
                        }
                    }
                    else if (entry.PropertyType == PropertyType.PropertyFloat)
                    {
                        if (biota.PropertiesFloat.TryGetValue((PropertyFloat)entry.Property, out var entryValue))
                        {
                            if (partialBiota.PropertiesFloat == null)
                                partialBiota.PropertiesFloat = new Dictionary<PropertyFloat, double>();
                            partialBiota.PropertiesFloat.Add((PropertyFloat)entry.Property, entryValue);
                        }
                    }
                    else if (entry.PropertyType == PropertyType.PropertyInstanceId)
                    {
                        if (biota.PropertiesIID.TryGetValue((PropertyInstanceId)entry.Property, out var entryValue))
                        {
                            if (partialBiota.PropertiesIID == null)
                                partialBiota.PropertiesIID = new Dictionary<PropertyInstanceId, uint>();
                            partialBiota.PropertiesIID.Add((PropertyInstanceId)entry.Property, entryValue);
                        }
                    }
                    else if (entry.PropertyType == PropertyType.PropertyInt)
                    {
                        if (biota.PropertiesInt.TryGetValue((PropertyInt)entry.Property, out var entryValue))
                        {
                            if (partialBiota.PropertiesInt == null)
                                partialBiota.PropertiesInt = new Dictionary<PropertyInt, int>();
                            partialBiota.PropertiesInt.Add((PropertyInt)entry.Property, entryValue);
                        }
                    }
                    else if (entry.PropertyType == PropertyType.PropertyInt64)
                    {
                        if (biota.PropertiesInt64.TryGetValue((PropertyInt64)entry.Property, out var entryValue))
                        {
                            if (partialBiota.PropertiesInt64 == null)
                                partialBiota.PropertiesInt64 = new Dictionary<PropertyInt64, long>();
                            partialBiota.PropertiesInt64.Add((PropertyInt64)entry.Property, entryValue);
                        }
                    }
                    else if (entry.PropertyType == PropertyType.PropertyString)
                    {
                        if (biota.PropertiesString.TryGetValue((PropertyString)entry.Property, out var entryValue))
                        {
                            if (partialBiota.PropertiesString == null)
                                partialBiota.PropertiesString = new Dictionary<PropertyString, string>();
                            partialBiota.PropertiesString.Add((PropertyString)entry.Property, entryValue);
                        }
                    }
                    // Todo: Add partial collection support for the following properties if we ever need them:
                    // PropertiesPosition
                    // PropertiesSpellBook
                    // PropertiesAnimPart
                    // PropertiesPalette
                    // PropertiesTextureMap
                    // PropertiesCreateList
                    // PropertiesEmote
                    // PropertiesEventFilter
                    // PropertiesGenerator
                    // PropertiesAttribute
                    // PropertiesAttribute2nd
                    // PropertiesBodyPart
                    // PropertiesSkill
                    // PropertiesBook
                    // PropertiesBookPageData
                    // PropertiesAllegiance
                    // PropertiesEnchantmentRegistry
                    // HousePermissions
                }

                biota = partialBiota;
            }
        }

        public virtual bool SaveBiota(ACE.Entity.Models.Biota biota, ReaderWriterLockSlim rwLock, bool doNotAddToCache = false)
        {
            ProcessPartialSave(ref biota);

            using (var context = new ShardDbContext())
            {
                var existingBiota = GetBiota(context, biota.Id, doNotAddToCache);

                rwLock.EnterReadLock();
                try
                {
                    if (existingBiota == null)
                    {
                        existingBiota = ACE.Database.Adapter.BiotaConverter.ConvertFromEntityBiota(biota);

                        context.Biota.Add(existingBiota);
                    }
                    else
                    {
                        ACE.Database.Adapter.BiotaUpdater.UpdateDatabaseBiota(context, biota, existingBiota);
                    }
                }
                finally
                {
                    rwLock.ExitReadLock();
                }

                return DoSaveBiota(context, existingBiota);
            }
        }

        public bool SaveBiotasInParallel(IEnumerable<(ACE.Entity.Models.Biota biota, ReaderWriterLockSlim rwLock)> biotas, bool doNotAddToCache = false)
        {
            var result = true;

            Parallel.ForEach(biotas, ConfigManager.Config.Server.Threading.DatabaseParallelOptions, biota =>
            {
                if (!SaveBiota(biota.biota, biota.rwLock, doNotAddToCache))
                    result = false;
            });

            return result;
        }

        public virtual bool RemoveBiota(uint id)
        {
            using (var context = new ShardDbContext())
            {
                var existingBiota = context.Biota
                    .AsNoTracking()
                    .FirstOrDefault(r => r.Id == id);

                if (existingBiota == null)
                    return true;

                context.Biota.Remove(existingBiota);

                Exception firstException = null;
                retry:

                try
                {
                    context.SaveChanges();

                    if (firstException != null)
                        log.DebugFormat("[DATABASE] RemoveBiota 0x{0:X8} retry succeeded after initial exception of: {1}", id, firstException.GetFullMessage());

                    return true;
                }
                catch (Exception ex)
                {
                    if (firstException == null)
                    {
                        firstException = ex;
                        goto retry;
                    }

                    // Character name might be in use or some other fault
                    log.Error($"[DATABASE] RemoveBiota 0x{id:X8} failed first attempt with exception: {firstException.GetFullMessage()}");
                    log.Error($"[DATABASE] RemoveBiota 0x{id:X8} failed second attempt with exception: {ex.GetFullMessage()}");
                    return false;
                }
            }
        }

        public bool RemoveBiotasInParallel(IEnumerable<uint> ids)
        {
            var result = true;

            Parallel.ForEach(ids, ConfigManager.Config.Server.Threading.DatabaseParallelOptions, id =>
            {
                if (!RemoveBiota(id))
                    result = false;
            });

            return result;
        }


        public PossessedBiotas GetPossessedBiotasInParallel(uint id)
        {
            var inventory = GetInventoryInParallel(id, true);

            var wieldedItems = GetWieldedItemsInParallel(id);

            return new PossessedBiotas(inventory, wieldedItems);
        }

        public List<Biota> GetInventoryInParallel(uint parentId, bool includedNestedItems)
        {
            var inventory = new ConcurrentBag<Biota>();

            using (var context = new ShardDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var results = context.BiotaPropertiesIID
                    .Where(r => r.Type == (ushort)PropertyInstanceId.Container && r.Value == parentId)
                    .ToList();

                Parallel.ForEach(results, ConfigManager.Config.Server.Threading.DatabaseParallelOptions, result =>
                {
                    var biota = GetBiota(result.ObjectId);

                    if (biota != null)
                    {
                        inventory.Add(biota);

                        if (includedNestedItems && biota.WeenieType == (int)WeenieType.Container)
                        {
                            var subItems = GetInventoryInParallel(biota.Id, false);

                            foreach (var subItem in subItems)
                                inventory.Add(subItem);
                        }
                    }
                });
            }

            return inventory.ToList();
        }

        public List<Biota> GetWieldedItemsInParallel(uint parentId)
        {
            var wieldedItems = new ConcurrentBag<Biota>();

            using (var context = new ShardDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var results = context.BiotaPropertiesIID
                    .Where(r => r.Type == (ushort)PropertyInstanceId.Wielder && r.Value == parentId)
                    .ToList();

                Parallel.ForEach(results, ConfigManager.Config.Server.Threading.DatabaseParallelOptions, result =>
                {
                    var biota = GetBiota(result.ObjectId);

                    if (biota != null)
                        wieldedItems.Add(biota);
                });
            }

            return wieldedItems.ToList();
        }

        public List<Biota> GetStaticObjectsByLandblock(ushort landblockId)
        {
            var staticObjects = new List<Biota>();

            var staticLandblockId = (uint)(0x70000 | landblockId);

            var min = staticLandblockId << 12;
            var max = min | 0xFFF;

            using (var context = new ShardDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var results = context.Biota.Where(b => b.Id >= min && b.Id <= max).ToList();

                foreach (var result in results)
                {
                    var biota = GetBiota(result.Id);
                    staticObjects.Add(biota);
                }
            }

            return staticObjects;
        }

        public List<Biota> GetDynamicObjectsByLandblock(ushort landblockId)
        {
            var dynamics = new List<Biota>();

            var min = (uint)(landblockId << 16);
            var max = min | 0xFFFF;

            using (var context = new ShardDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var results = context.BiotaPropertiesPosition
                    .Where(p => p.PositionType == 1 && p.ObjCellId >= min && p.ObjCellId <= max && p.ObjectId >= 0x80000000)
                    .ToList();

                foreach (var result in results)
                {
                    var biota = GetBiota(result.ObjectId);

                    // Filter out objects that are in a container
                    if (biota.BiotaPropertiesIID.FirstOrDefault(r => r.Type == 2 && r.Value != 0) != null)
                        continue;

                    // Filter out wielded objects
                    if (biota.BiotaPropertiesIID.FirstOrDefault(r => r.Type == 3 && r.Value != 0) != null)
                        continue;

                    dynamics.Add(biota);
                }
            }

            return dynamics;
        }

        public List<Biota> GetHousesOwned()
        {
            using (var context = new ShardDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var query = from biota in context.Biota
                            join iid in context.BiotaPropertiesIID on biota.Id equals iid.ObjectId
                            where biota.WeenieType == (int)WeenieType.SlumLord && iid.Type == (ushort)PropertyInstanceId.HouseOwner
                            select biota;

                var results = query.ToList();

                return results;
            }
        }


        public bool IsCharacterNameAvailable(string name)
        {
            using (var context = new ShardDbContext())
            {
                var result = context.Character
                    .AsNoTracking()
                    .Where(r => !r.IsDeleted)
                    .Where(r => !(r.DeleteTime > 0))
                    .FirstOrDefault(r => r.Name == name);

                return result == null;
            }
        }

        private static readonly ConditionalWeakTable<Character, ShardDbContext> CharacterContexts = new ConditionalWeakTable<Character, ShardDbContext>();

        public List<Character> GetCharacters(uint accountId, bool includeDeleted)
        {
            return GetCharacterList(accountId, includeDeleted);
        }

        public Character GetCharacter(uint characterId)
        {
            return GetCharacterList(0, true, characterId).FirstOrDefault();
        }

        private static List<Character> GetCharacterList(uint accountID, bool includeDeleted, uint characterID = 0)
        {
            var context = new ShardDbContext();

            IQueryable<Character> query;

            if (accountID > 0)
                query = context.Character.Where(r => r.AccountId == accountID && (includeDeleted || !r.IsDeleted));
            else
                query = context.Character.Where(r => r.Id == characterID && (includeDeleted || !r.IsDeleted));

            var results = query.ToList();

            for (int i = 0; i < results.Count; i++)
            {
                // Do we have a reference to this Character already?
                var existingChar = CharacterContexts.FirstOrDefault(r => r.Key.Id == results[i].Id);

                if (existingChar.Key != null)
                    results[i] = existingChar.Key;
                else
                {
                    // No reference, pull all the properties and add it to the cache
                    query.Include(r => r.CharacterPropertiesContractRegistry).Load();
                    query.Include(r => r.CharacterPropertiesFillCompBook).Load();
                    query.Include(r => r.CharacterPropertiesFriendList).Load();
                    query.Include(r => r.CharacterPropertiesQuestRegistry).Load();
                    query.Include(r => r.CharacterPropertiesShortcutBar).Load();
                    query.Include(r => r.CharacterPropertiesSpellBar).Load();
                    query.Include(r => r.CharacterPropertiesSquelch).Load();
                    query.Include(r => r.CharacterPropertiesTitleBook).Load();
                    query.Include(r => r.CharacterPropertiesCampRegistry).Load();

                    CharacterContexts.Add(results[i], context);
                }
            }

            return results;
        }

        public Character GetCharacterStubByName(string name) // When searching by name, only non-deleted characters matter
        {
            var context = new ShardDbContext();

            var result = context.Character
                .FirstOrDefault(r => r.Name == name && !r.IsDeleted);

            return result;
        }

        public Character GetCharacterStubByGuid(uint guid)
        {
            var context = new ShardDbContext();

            var result = context.Character
                .FirstOrDefault(r => r.Id == guid);

            return result;
        }

        public bool SaveCharacter(Character character, ReaderWriterLockSlim rwLock)
        {
            if (CharacterContexts.TryGetValue(character, out var cachedContext))
            {
                rwLock.EnterReadLock();
                try
                {
                    Exception firstException = null;
                    retry:

                    try
                    {
                        cachedContext.SaveChanges();

                        if (firstException != null && log.IsDebugEnabled)
                            log.DebugFormat("[DATABASE] SaveCharacter-1 0x{0:X8}:{1} retry succeeded after initial exception of: {2}", character.Id, character.Name, firstException.GetFullMessage());

                        return true;
                    }
                    catch (Exception ex)
                    {
                        if (firstException == null)
                        {
                            firstException = ex;
                            goto retry;
                        }

                        // Character name might be in use or some other fault
                        log.Error($"[DATABASE] SaveCharacter-1 0x{character.Id:X8}:{character.Name} failed first attempt with exception: {firstException.GetFullMessage()}");
                        log.Error($"[DATABASE] SaveCharacter-1 0x{character.Id:X8}:{character.Name} failed second attempt with exception: {ex.GetFullMessage()}");
                        return false;
                    }
                }
                finally
                {
                    rwLock.ExitReadLock();
                }
            }

            var context = new ShardDbContext();

            CharacterContexts.Add(character, context);

            rwLock.EnterReadLock();
            try
            {
                context.Character.Add(character);

                Exception firstException = null;
                retry:

                try
                {
                    context.SaveChanges();

                    if (firstException != null && log.IsDebugEnabled)
                        log.DebugFormat("[DATABASE] SaveCharacter-2 0x{0:X8}:{1} retry succeeded after initial exception of: {2}", character.Id, character.Name, firstException.GetFullMessage());

                    return true;
                }
                catch (Exception ex)
                {
                    if (firstException == null)
                    {
                        firstException = ex;
                        goto retry;
                    }

                    // Character name might be in use or some other fault
                    log.Error($"[DATABASE] SaveCharacter-2 0x{character.Id:X8}:{character.Name} failed first attempt with exception: {firstException.GetFullMessage()}");
                    log.Error($"[DATABASE] SaveCharacter-2 0x{character.Id:X8}:{character.Name} failed second attempt with exception: {ex.GetFullMessage()}");
                    return false;
                }
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }


        public bool AddCharacterInParallel(ACE.Entity.Models.Biota biota, ReaderWriterLockSlim biotaLock, IEnumerable<(ACE.Entity.Models.Biota biota, ReaderWriterLockSlim rwLock)> possessions, Character character, ReaderWriterLockSlim characterLock)
        {
            if (!SaveBiota(biota, biotaLock))
                return false; // Biota save failed which mean Character fails.

            if (!SaveBiotasInParallel(possessions))
                return false;

            if (!SaveCharacter(character, characterLock))
                return false;

            return true;
        }


        /// <summary>
        /// This will get all player biotas that are backed by characters that are not deleted.
        /// </summary>
        public List<ACE.Entity.Models.Biota> GetAllPlayerBiotasInParallel()
        {
            var biotas = new ConcurrentBag<ACE.Entity.Models.Biota>();

            using (var context = new ShardDbContext())
            {
                var results = context.Character
                    .Where(r => !r.IsDeleted)
                    .AsNoTracking()
                    .ToList();

                Parallel.ForEach(results, result =>
                {
                    var biota = GetBiota(result.Id, true);

                    if (biota != null)
                    {
                        var convertedBiota = ACE.Database.Adapter.BiotaConverter.ConvertToEntityBiota(biota);

                        biotas.Add(convertedBiota);
                    }
                    else
                        log.Error($"ShardDatabase.GetAllPlayerBiotasInParallel() - couldn't find biota for character 0x{result.Id:X8}");
                });
            }

            return biotas.ToList();
        }

        public int? GetCharacterGameplayMode(uint characterId)
        {
            using (var context = new ShardDbContext())
            {
                var query = from biota in context.Biota
                            join pInt in context.BiotaPropertiesInt on biota.Id equals pInt.ObjectId
                            where pInt.Type == (int)PropertyInt.GameplayMode && pInt.ObjectId == characterId
                            select pInt.Value;

                return query.FirstOrDefault();
            }
        }
        public uint? GetAllegianceID(uint monarchID)
        {
            using (var context = new ShardDbContext())
            {
                var query = from biota in context.Biota
                            join iid in context.BiotaPropertiesIID on biota.Id equals iid.ObjectId
                            where biota.WeenieType == (int)WeenieType.Allegiance && iid.Type == (int)PropertyInstanceId.Monarch && iid.Value == monarchID
                            select biota.Id;

                return query.FirstOrDefault();
            }
        }

        public bool RenameCharacter(Character character, string newName, ReaderWriterLockSlim rwLock)
        {
            if (CharacterContexts.TryGetValue(character, out var cachedContext))
            {
                rwLock.EnterReadLock();
                try
                {
                    Exception firstException = null;
                retry:

                    try
                    {
                        character.Name = newName;
                        cachedContext.SaveChanges();

                        if (firstException != null && log.IsDebugEnabled)
                            log.DebugFormat("[DATABASE] RenameCharacter 0x{0:X8}:{1} retry succeeded after initial exception of: {2}", character.Id, character.Name, firstException.GetFullMessage());

                        return true;
                    }
                    catch (Exception ex)
                    {
                        if (firstException == null)
                        {
                            firstException = ex;
                            goto retry;
                        }

                        // Character name might be in use or some other fault
                        log.Error($"[DATABASE] RenameCharacter 0x{character.Id:X8}:{character.Name} failed first attempt with exception: {firstException.GetFullMessage()}");
                        log.Error($"[DATABASE] RenameCharacter 0x{character.Id:X8}:{character.Name} failed second attempt with exception: {ex.GetFullMessage()}");
                        return false;
                    }
                }
                finally
                {
                    rwLock.ExitReadLock();
                }
            }

            character.Name = newName;

            var context = new ShardDbContext();

            CharacterContexts.Add(character, context);

            rwLock.EnterReadLock();
            try
            {
                context.Character.Add(character);

                Exception firstException = null;
            retry:

                try
                {
                    context.SaveChanges();

                    if (firstException != null && log.IsDebugEnabled)
                        log.DebugFormat("[DATABASE] RenameCharacter 0x{0:X8}:{1} retry succeeded after initial exception of: {2}", character.Id, character.Name, firstException.GetFullMessage());

                    return true;
                }
                catch (Exception ex)
                {
                    if (firstException == null)
                    {
                        firstException = ex;
                        goto retry;
                    }

                    // Character name might be in use or some other fault
                    log.Error($"[DATABASE] RenameCharacter 0x{character.Id:X8}:{character.Name} failed first attempt with exception: {firstException.GetFullMessage()}");
                    log.Error($"[DATABASE] RenameCharacter 0x{character.Id:X8}:{character.Name} failed second attempt with exception: {ex.GetFullMessage()}");
                    return false;
                }
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public void LogAccountSessionStart(uint accountId, string accountName, string sessionIP)
        {
            var logEntry = new AccountSessionLog();

            try
            {
                logEntry.AccountId = accountId;
                logEntry.AccountName = accountName;
                logEntry.SessionIP = sessionIP;
                logEntry.LoginDateTime = DateTime.Now;

                using (var context = new ShardDbContext())
                {
                    context.AccountSessions.Add(logEntry);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error($"Exception in LogAccountSessionStart saving session log data to DB. Ex: {ex}");
            }

            return;
        }

        public void LogCharacterLogin(uint accountId, string accountName, string sessionIP, uint characterId, string characterName)
        {
            var logEntry = new CharacterLoginLog();

            try
            {
                logEntry.AccountId = accountId;
                logEntry.AccountName = accountName;
                logEntry.SessionIP = sessionIP;
                logEntry.CharacterId = characterId;
                logEntry.CharacterName = characterName;
                logEntry.LoginDateTime = DateTime.Now;

                using (var context = new ShardDbContext())
                {
                    context.CharacterLogins.Add(logEntry);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error($"Exception in LogCharacterLogin saving character login info to DB. Ex: {ex}");
            }
        }
        public void CreatePKKill(uint victimId, uint killerId, uint? victimMonarchId, uint? killerMonarchId)
        {
            var kill = new PKKill();

            try
            {
                kill.VictimId = victimId;
                kill.KillerId = killerId;
                kill.VictimMonarchId = victimMonarchId;
                kill.KillerMonarchId = killerMonarchId;
                kill.KillDateTime = DateTime.Now;

                using (var context = new ShardDbContext())
                {
                    context.PKKills.Add(kill);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error($"Exception in CreateKill saving kill data to PKKills DB. Ex: {ex}");
            }
        }

        public void CreateArenaPKKill(uint victimId, uint killerId, uint? victimMonarchId, uint? killerMonarchId)
        {
            var kill = new ArenaPKKill();

            try
            {
                kill.VictimId = victimId;
                kill.KillerId = killerId;
                kill.VictimMonarchId = victimMonarchId;
                kill.KillerMonarchId = killerMonarchId;
                kill.KillDateTime = DateTime.Now;

                using (var context = new ShardDbContext())
                {
                    context.ArenaPKKills.Add(kill);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error($"Exception in CreateKill saving kill data to PKKills DB. Ex: {ex}");
            }
        }

        public void LogPlayerDeath(uint accountId, uint characterId, string characterName, int characterLevel, string killerName, int killerLevel, uint landblockId, int gameplayMode, bool wasPvP, int kills, long xp, int age, DateTime timeOfDeath, uint? monarchId)
        {
            var entry = new CharacterObituary();

            try
            {
                entry.AccountId = accountId;
                entry.CharacterId = characterId;
                entry.CharacterName = characterName;
                entry.CharacterLevel = characterLevel;
                entry.KillerName = killerName;
                entry.KillerLevel = killerLevel;
                entry.LandblockId = landblockId;
                entry.GameplayMode = gameplayMode;
                entry.WasPvP = wasPvP;
                entry.Kills = kills;
                entry.XP = xp;
                entry.Age = age;
                entry.TimeOfDeath = timeOfDeath;
                entry.MonarchId = monarchId;

                using (var context = new ShardDbContext())
                {
                    context.CharacterObituary.Add(entry);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error($"Exception in CharacterObituary saving character death info to DB. Ex: {ex}");
            }
        }

        public List<CharacterObituary> GetCharacterObituary()
        {
            using (var context = new ShardDbContext())
            {
                var results = context.CharacterObituary
                    .AsNoTracking()
                    .ToList();

                return results;
            }
        }

        public List<CharacterObituary> GetCharacterObituaryByGameplayMode(GameplayModes gameplayMode)
        {
            using (var context = new ShardDbContext())
            {
                var results = context.CharacterObituary
                    .AsNoTracking()
                    .Where(p => p.GameplayMode == (int)gameplayMode)
                    .ToList();

                return results;
            }
        }

        public virtual int GetUniqueIPsInTheLast(TimeSpan timeSpan)
        {
            var since = DateTime.Now - timeSpan;

            using (var context = new ShardDbContext())
            {
                var count = context.AccountSessions
                    .Where(r => r.LoginDateTime > since)
                    .GroupBy(r => r.SessionIP).Count();

                return count;
            }
        }

        #region Arena
        public uint SaveArenaEvent(ArenaEvent arenaEvent)
        {
            try
            {
                using (var context = new ShardDbContext())
                {
                    if (arenaEvent.Id <= 0)
                    {
                        context.ArenaEvents.Add(arenaEvent);
                    }
                    else
                    {
                        context.Entry(arenaEvent).State = EntityState.Modified;
                    }

                    context.SaveChanges();

                    foreach (var arenaPlayer in arenaEvent.Players)
                    {
                        arenaPlayer.EventId = arenaEvent.Id;

                        if (arenaPlayer.Id <= 0)
                        {
                            context.ArenaPlayers.Add(arenaPlayer);
                        }
                        else
                        {
                            context.Entry(arenaPlayer).State = EntityState.Modified;
                        }
                    }

                    context.SaveChanges();

                    return arenaEvent.Id;
                }
            }
            catch (Exception ex)
            {
                log.Error($"Exception in SaveArenaEvent. Ex: {ex}");
            }

            return 0;
        }

        public void AddToArenaStats(uint characterId, string characterName, string eventType, uint totalMatches, uint totalWins, uint totalDraws, uint totalLosses, uint totalDisqualified, uint totalDeaths, uint totalKills, uint totalDmgDealt, uint totalDmgReceived, uint? newRankPoints = null)
        {
            try
            {
                using (var context = new ShardDbContext())
                {
                    var stats = context.ArenaCharacterStats.FirstOrDefault(x => x.CharacterId == characterId && x.EventType.Equals(eventType));
                    if (stats == null)
                    {
                        stats = new ArenaCharacterStats();
                        stats.CharacterId = characterId;
                        stats.CharacterName = characterName;
                        stats.EventType = eventType;
                        context.ArenaCharacterStats.Add(stats);
                    }
                    else
                    {
                        context.Entry(stats).State = EntityState.Modified;
                    }

                    stats.TotalMatches += totalMatches;
                    stats.TotalWins += totalWins;
                    stats.TotalDraws += totalDraws;
                    stats.TotalLosses += totalLosses;
                    stats.TotalDisqualified += totalDisqualified;
                    stats.TotalDeaths += totalDeaths;
                    stats.TotalKills += totalKills;
                    stats.TotalDmgDealt += totalDmgDealt;
                    stats.TotalDmgReceived += totalDmgReceived;
                    stats.RankPoints = newRankPoints.HasValue ? newRankPoints.Value : stats.RankPoints;

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error($"Exception saving ArenaCharacterStats. ex: {ex}");
            }
        }

        public ArenaCharacterStats GetCharacterArenaStatsByEvent(uint characterId, string eventType)
        {
            try
            {
                using (var context = new ShardDbContext())
                {
                    return context.ArenaCharacterStats.FirstOrDefault(x => x.CharacterId == characterId && x.EventType.Equals(eventType));
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error in GetCharacterArenaStatsByEvent. ex:{ex}");
            }

            return null;
        }

        public string GetArenaStatsByCharacterId(uint characterId, string characterName)
        {
            var returnMsg = new StringBuilder();

            try
            {
                using (var context = new ShardDbContext())
                {
                    var stats = context.ArenaCharacterStats.Where(x => x.CharacterId == characterId)?.ToList();
                    if (stats == null)
                    {
                        stats = new List<ArenaCharacterStats>();
                    }

                    returnMsg.Append($"********* Arena Stats for {characterName} *********\n\n");
                    var onesStats = stats.FirstOrDefault(x => x.EventType.Equals("1v1"));
                    if (onesStats == null)
                        onesStats = new ArenaCharacterStats();

                    returnMsg.Append($"1v1\n");
                    returnMsg.Append($"  Rank: {DatabaseManager.Shard.BaseDatabase.GetArenaRank("1v1", onesStats.RankPoints).ToString("n0")}\n");
                    returnMsg.Append($"  Rank Points: {onesStats.RankPoints.ToString("n0")}\n");
                    returnMsg.Append($"  Matches: {onesStats.TotalMatches.ToString("n0")}\n");
                    returnMsg.Append($"  Wins: {onesStats.TotalWins.ToString("n0")}\n");
                    returnMsg.Append($"  Draws: {onesStats.TotalDraws.ToString("n0")}\n");
                    returnMsg.Append($"  Losses: {onesStats.TotalLosses.ToString("n0")}\n");
                    returnMsg.Append($"  Disqualified: {onesStats.TotalDisqualified.ToString("n0")}\n");
                    returnMsg.Append($"  Kills: {onesStats.TotalKills.ToString("n0")}\n");
                    returnMsg.Append($"  Deaths: {onesStats.TotalDeaths.ToString("n0")}\n");
                    returnMsg.Append($"  Damage Dealt: {onesStats.TotalDmgDealt.ToString("n0")}\n");
                    returnMsg.Append($"  Damage Received: {onesStats.TotalDmgReceived.ToString("n0")}\n\n");

                    var twosStats = stats.FirstOrDefault(x => x.EventType.Equals("2v2"));
                    if (twosStats == null)
                        twosStats = new ArenaCharacterStats();

                    returnMsg.Append($"2v2\n");
                    returnMsg.Append($"  Rank: {DatabaseManager.Shard.BaseDatabase.GetArenaRank("2v2", twosStats.RankPoints).ToString("n0")}\n");
                    returnMsg.Append($"  Rank Points: {twosStats.RankPoints.ToString("n0")}\n");
                    returnMsg.Append($"  Matches: {twosStats.TotalMatches.ToString("n0")}\n");
                    returnMsg.Append($"  Wins: {twosStats.TotalWins.ToString("n0")}\n");
                    returnMsg.Append($"  Draws: {twosStats.TotalDraws.ToString("n0")}\n");
                    returnMsg.Append($"  Losses: {twosStats.TotalLosses.ToString("n0")}\n");
                    returnMsg.Append($"  Disqualified: {twosStats.TotalDisqualified.ToString("n0")}\n");
                    returnMsg.Append($"  Kills: {twosStats.TotalKills.ToString("n0")}\n");
                    returnMsg.Append($"  Deaths: {twosStats.TotalDeaths.ToString("n0")}\n");
                    returnMsg.Append($"  Damage Dealt: {twosStats.TotalDmgDealt.ToString("n0")}\n");
                    returnMsg.Append($"  Damage Received: {twosStats.TotalDmgReceived.ToString("n0")}\n\n");

                    var ffaStats = stats.FirstOrDefault(x => x.EventType.Equals("ffa"));
                    if (ffaStats == null)
                        ffaStats = new ArenaCharacterStats();

                    returnMsg.Append($"FFA\n");
                    returnMsg.Append($"  Rank: {DatabaseManager.Shard.BaseDatabase.GetArenaRank("ffa", ffaStats.RankPoints).ToString("n0")}\n");
                    returnMsg.Append($"  Rank Points: {ffaStats.RankPoints.ToString("n0")}\n");
                    returnMsg.Append($"  Matches: {ffaStats.TotalMatches.ToString("n0")}\n");
                    returnMsg.Append($"  Wins: {ffaStats.TotalWins.ToString("n0")}\n");
                    returnMsg.Append($"  Draws: {ffaStats.TotalDraws.ToString("n0")}\n");
                    returnMsg.Append($"  Losses: {ffaStats.TotalLosses.ToString("n0")}\n");
                    returnMsg.Append($"  Disqualified: {ffaStats.TotalDisqualified.ToString("n0")}\n");
                    returnMsg.Append($"  Kills: {ffaStats.TotalKills.ToString("n0")}\n");
                    returnMsg.Append($"  Deaths: {ffaStats.TotalDeaths.ToString("n0")}\n");
                    returnMsg.Append($"  Damage Dealt: {ffaStats.TotalDmgDealt.ToString("n0")}\n");
                    returnMsg.Append($"  Damage Received: {ffaStats.TotalDmgReceived.ToString("n0")}\n\n");

                    var groupStats = stats.FirstOrDefault(x => x.EventType.Equals("group"));
                    if (groupStats == null)
                        groupStats = new ArenaCharacterStats();

                    returnMsg.Append($"Group\n");
                    returnMsg.Append($"  Matches: {groupStats.TotalMatches.ToString("n0")}\n");
                    returnMsg.Append($"  Wins: {groupStats.TotalWins.ToString("n0")}\n");
                    returnMsg.Append($"  Draws: {groupStats.TotalDraws.ToString("n0")}\n");
                    returnMsg.Append($"  Losses: {groupStats.TotalLosses.ToString("n0")}\n");
                    returnMsg.Append($"  Disqualified: {groupStats.TotalDisqualified.ToString("n0")}\n");
                    returnMsg.Append($"  Kills: {groupStats.TotalKills.ToString("n0")}\n");
                    returnMsg.Append($"  Deaths: {groupStats.TotalDeaths.ToString("n0")}\n");
                    returnMsg.Append($"  Damage Dealt: {groupStats.TotalDmgDealt.ToString("n0")}\n");
                    returnMsg.Append($"  Damage Received: {groupStats.TotalDmgReceived.ToString("n0")}\n\n");

                    returnMsg.Append($"Totals:\n");
                    returnMsg.Append($"  Total Matches: {stats.Sum(x => x.TotalMatches).ToString("n0")}\n");
                    returnMsg.Append($"  Total Wins: {stats.Sum(x => x.TotalWins).ToString("n0")}\n");
                    returnMsg.Append($"  Total Draws: {stats.Sum(x => x.TotalDraws).ToString("n0")}\n");
                    returnMsg.Append($"  Total Losses: {stats.Sum(x => x.TotalLosses).ToString("n0")}\n");
                    returnMsg.Append($"  Total Disqualified: {stats.Sum(x => x.TotalDisqualified).ToString("n0")}\n");
                    returnMsg.Append($"  Total Kills: {stats.Sum(x => x.TotalKills).ToString("n0")}\n");
                    returnMsg.Append($"  Total Deaths: {stats.Sum(x => x.TotalDeaths).ToString("n0")}\n");
                    returnMsg.Append($"  Total Damage Dealt: {stats.Sum(x => x.TotalDmgDealt).ToString("n0")}\n");
                    returnMsg.Append($"  Total Damage Received: {stats.Sum(x => x.TotalDmgReceived).ToString("n0")}\n\n");

                    returnMsg.Append($"*****************************\n");
                }
            }
            catch (Exception ex)
            {
                log.Error($"Exception in GetEventStatsByCharacterId for characterId = {characterId}. ex: {ex}");
            }

            return returnMsg.ToString();
        }

        public int GetArenaRank(string eventType, uint rankPoints)
        {
            try
            {
                using (var context = new ShardDbContext())
                {
                    var higherPlayers = context.ArenaCharacterStats.Where(x => x.EventType.Equals(eventType) && x.RankPoints > rankPoints);
                    if (higherPlayers != null)
                    {
                        return higherPlayers.Count() + 1;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error in GetArenaRank. ex:{ex}");
            }

            return -1;
        }

        public List<ArenaCharacterStats> GetArenaTopRankedByEventType(string eventType)
        {
            try
            {
                using (var context = new ShardDbContext())
                {
                    var topTenPlayers = context.ArenaCharacterStats.Where(x => x.EventType.Equals(eventType))?.OrderByDescending(x => x.RankPoints)?.Take(10);

                    if (topTenPlayers != null)
                    {
                        return topTenPlayers.ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error in GetArenaTopRankedByEventType. ex:{ex}");
            }

            return new List<ArenaCharacterStats>();
        }

        public uint CreateArenaPlayer(ArenaPlayer player)
        {
            try
            {
                using (var context = new ShardDbContext())
                {
                    context.ArenaPlayers.Add(player);
                    context.SaveChanges();

                    return player.Id;
                }
            }
            catch (Exception ex)
            {
                log.Error($"Exception in CreateArenaEvent. Ex: {ex}");
            }

            return 0;
        }

        public void UpdateArenaPlayer(ArenaPlayer player)
        {
            using (var context = new ShardDbContext())
            {
                context.Entry(player).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public List<ArenaEvent> GetAllArenaEvents()
        {
            List<ArenaEvent> eventList = null;

            try
            {
                using (var context = new ShardDbContext())
                {
                    eventList = context.ArenaEvents
                            .AsNoTracking()
                            .OrderByDescending(r => r.StartDateTime)
                            .Where(r => r.EndDateTime.HasValue)?.ToList() ?? new List<ArenaEvent>();
                }

                foreach (var arenaEvent in eventList)
                {
                    arenaEvent.Players = GetAllArenaPlayersByEvent(arenaEvent.Id);
                }
            }
            catch (Exception ex)
            {
                log.Error($"Exception in GetAllArenaEvents. Ex: {ex}");
            }

            return eventList ?? new List<ArenaEvent>();
        }

        public List<ArenaPlayer> GetAllArenaPlayersByEvent(uint eventId)
        {
            List<ArenaPlayer> playerList = null;

            try
            {
                using (var context = new ShardDbContext())
                {
                    var result =
                        context.ArenaPlayers
                            .AsNoTracking();

                    result = result.Where(x => x.EventId == (uint?)eventId);
                    playerList = result?.ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error($"Exception in GetAllArenaPlayersByEvent. Ex: {ex}");
            }

            return playerList ?? new List<ArenaPlayer>();
        }

        #endregion Arena

    }
}
