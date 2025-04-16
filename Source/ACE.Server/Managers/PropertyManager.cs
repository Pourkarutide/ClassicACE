using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;

using log4net;

using ACE.Database;
using ACE.Server.WorldObjects;

namespace ACE.Server.Managers
{
    public static class PropertyManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // caching internally to the server
        private static readonly ConcurrentDictionary<string, ConfigurationEntry<bool>> CachedBooleanSettings = new ConcurrentDictionary<string, ConfigurationEntry<bool>>();
        private static readonly ConcurrentDictionary<string, ConfigurationEntry<long>> CachedLongSettings = new ConcurrentDictionary<string, ConfigurationEntry<long>>();
        private static readonly ConcurrentDictionary<string, ConfigurationEntry<double>> CachedDoubleSettings = new ConcurrentDictionary<string, ConfigurationEntry<double>>();
        private static readonly ConcurrentDictionary<string, ConfigurationEntry<string>> CachedStringSettings = new ConcurrentDictionary<string, ConfigurationEntry<string>>();

        private static Timer _workerThread;

        internal static bool IsLoaded { get; private set; }

        /// <summary>
        /// Initializes the PropertyManager.
        /// Run this only once per server instance.
        /// </summary>
        /// <param name="loadDefaultValues">Should we use the DefaultPropertyManager to load the default properties for keys?</param>
        public static void Initialize(bool loadDefaultValues = true)
        {
            if (loadDefaultValues)
                DefaultPropertyManager.LoadDefaultProperties();

            LoadPropertiesFromDB();

            if (Program.IsRunningInContainer && !GetString("content_folder").Equals("/ace/Content"))
                ModifyString("content_folder", "/ace/Content");

            _workerThread = new Timer(300000);
            _workerThread.Elapsed += DoWork;
            _workerThread.AutoReset = true;
            _workerThread.Start();

            IsLoaded = true;
        }


        /// <summary>
        /// Loads the variables from the database directly into the cache.
        /// </summary>
        private static void LoadPropertiesFromDB()
        {
            foreach (var i in DatabaseManager.ShardConfig.GetAllBools())
                CachedBooleanSettings[i.Key] = new ConfigurationEntry<bool>(false, i.Value, i.Description);

            foreach (var i in DatabaseManager.ShardConfig.GetAllLongs())
                CachedLongSettings[i.Key] = new ConfigurationEntry<long>(false, i.Value, i.Description);

            foreach (var i in DatabaseManager.ShardConfig.GetAllDoubles())
                CachedDoubleSettings[i.Key] = new ConfigurationEntry<double>(false, i.Value, i.Description);

            foreach (var i in DatabaseManager.ShardConfig.GetAllStrings())
                CachedStringSettings[i.Key] = new ConfigurationEntry<string>(false, i.Value, i.Description);
        }

        /// <summary>
        /// Resyncs the variables with the database manually.
        /// Disables the timer so that the elapsed event cannot run during the update operation.
        /// </summary>
        public static void ResyncVariables()
        {
            _workerThread.Stop();

            DoWork(null, null);

            _workerThread.Start();
        }

        /// <summary>
        /// Stops updating the cached store from the database.
        /// </summary>
        public static void StopUpdating()
        {
            if (_workerThread != null)
                _workerThread.Stop();
        }

        private static void AssertLoaded()
        {
            if (!IsLoaded)
                throw new InvalidOperationException("PropertyManager not loaded yet");
        }


        /// <summary>
        /// Retrieves a boolean property from the cache or database
        /// </summary>
        /// <param name="key">The string key for the property</param>
        /// <param name="fallback">The value to return if the property cannot be found.</param>
        /// <param name="cacheFallback">Whether or not the fallback property should be cached.</param>
        /// <returns>A boolean value representing the property</returns>
        public static Property<bool> GetBool(string key, bool fallback = false, bool cacheFallback = true)
        {
            AssertLoaded();
            // first, check the cache. If the key exists in the cache, grab it regardless of its modified value
            // then, check the database. if the key exists in the database, grab it and cache it
            // finally, set it to a default of false.
            if (CachedBooleanSettings.ContainsKey(key))
                return new Property<bool>(CachedBooleanSettings[key].Item, CachedBooleanSettings[key].Description);

            var dbValue = DatabaseManager.ShardConfig.GetBool(key);

            bool useFallback = dbValue?.Value == null;

            var value = dbValue?.Value ?? fallback;

            if (!useFallback || cacheFallback)
                CachedBooleanSettings[key] = new ConfigurationEntry<bool>(useFallback, value, dbValue?.Description);

            return new Property<bool>(value, dbValue?.Description);
        }

        /// <summary>
        /// Modifies a boolean value in the cache and marks it for being synced on the next cycle.
        /// </summary>
        /// <param name="key">The string key for the property</param>
        /// <param name="newVal">The value to replace the old value with</param>
        /// <returns>true if the property was modified, false if no property exists with the given key</returns>
        public static bool ModifyBool(string key, bool newVal)
        {
            if (!DefaultPropertyManager.DefaultBooleanProperties.ContainsKey(key))
                return false;

            if (CachedBooleanSettings.ContainsKey(key))
                CachedBooleanSettings[key].Modify(newVal);
            else
                CachedBooleanSettings[key] = new ConfigurationEntry<bool>(true, newVal, DefaultPropertyManager.DefaultBooleanProperties[key].Description);

            return true;
        }

        public static void ModifyBoolDescription(string key, string description)
        {
            if (CachedBooleanSettings.ContainsKey(key))
                CachedBooleanSettings[key].ModifyDescription(description);
            else
                log.Warn($"Attempted to modify {key} which did not exist in the BOOL cache.");
        }

        /// <summary>
        /// Retreives an integer property from the cache or database
        /// </summary>
        /// <param name="key">The string key for the property</param>
        /// <param name="fallback">The value to return if the property cannot be found.</param>
        /// <param name="cacheFallback">Whether or not the fallback property should be cached</param>
        /// <returns>An integer value representing the property</returns>
        public static Property<long> GetLong(string key, long fallback = 0, bool cacheFallback = true)
        {
            AssertLoaded();

            if (CachedLongSettings.ContainsKey(key))
                return new Property<long>(CachedLongSettings[key].Item, CachedLongSettings[key].Description);

            var dbValue = DatabaseManager.ShardConfig.GetLong(key);

            bool useFallback = dbValue?.Value == null;

            var value = dbValue?.Value ?? fallback;

            if (!useFallback || cacheFallback)
                CachedLongSettings[key] = new ConfigurationEntry<long>(useFallback, value, dbValue?.Description);

            return new Property<long>(value, dbValue?.Description);
        }

        /// <summary>
        /// Modifies an integer value in the cache and marks it for being synced on the next cycle.
        /// </summary>
        /// <param name="key">The string key for the property</param>
        /// <param name="newVal">The value to replace the old value with</param>
        /// <returns>true if the property was modified, false if no property exists with the given key</returns>
        public static bool ModifyLong(string key, long newVal)
        {
            if (!DefaultPropertyManager.DefaultLongProperties.ContainsKey(key))
                return false;

            if (CachedLongSettings.ContainsKey(key))
                CachedLongSettings[key].Modify(newVal);
            else
                CachedLongSettings[key] = new ConfigurationEntry<long>(true, newVal, DefaultPropertyManager.DefaultLongProperties[key].Description);
            return true;
        }

        public static void ModifyLongDescription(string key, string description)
        {
            if (CachedLongSettings.ContainsKey(key))
                CachedLongSettings[key].ModifyDescription(description);
            else
                log.Warn($"Attempted to modify {key} which did not exist in the LONG cache.");
        }

        /// <summary>
        /// Retrieves a float property from the cache or database
        /// </summary>
        /// <param name="key">The string key for the property</param>
        /// <param name="fallback">The value to return if the property cannot be found.</param>
        /// <param name="cacheFallback">Whether or not the fallpack property should be cached</param>
        /// <returns>A float value representing the property</returns>
        public static Property<double> GetDouble(string key, double fallback = 0.0f, bool cacheFallback = true, bool allowWhileInitializing = false)
        {
            if (!allowWhileInitializing)
                AssertLoaded();

            if (CachedDoubleSettings.ContainsKey(key))
                return new Property<double>(CachedDoubleSettings[key].Item, CachedDoubleSettings[key].Description);

            var dbValue = DatabaseManager.ShardConfig.GetDouble(key);

            bool useFallback = dbValue?.Value == null;

            var value = dbValue?.Value ?? fallback;

            if (!useFallback || cacheFallback)
                CachedDoubleSettings[key] = new ConfigurationEntry<double>(useFallback, value, dbValue?.Description);

            return new Property<double>(value, dbValue?.Description);
        }

        public static double GetInterpolatedDouble(int interpolationKey, string lowKey, string highKey, string referenceLowKey, string referenceHighKey)
        {
            var referenceLow = GetDouble(referenceLowKey).Item;
            var referenceHigh = GetDouble(referenceHighKey).Item;
            var lowValue = GetDouble(lowKey).Item;
            var highValue = GetDouble(highKey).Item;

            if (lowValue == highValue)
                return lowValue;
            else if (referenceLow > interpolationKey)
                return lowValue;
            else if (referenceHigh < interpolationKey)
                return highValue;

            var stepsBetweenRefences = referenceHigh - referenceLow;
            var stepValue = (highValue - lowValue) / stepsBetweenRefences;
            var steps = interpolationKey - referenceLow;

            return lowValue + (steps * stepValue);
        }

        /// <summary>
        /// Modifies a float value in the cache and marks it for being synced on the next cycle.
        /// </summary>
        /// <param name="key">The string key for the property</param>
        /// <param name="newVal">The value to replace the old value with</param>
        public static bool ModifyDouble(string key, double newVal, bool init = false)
        {
            if (!DefaultPropertyManager.DefaultDoubleProperties.ContainsKey(key))
                return false;
            if (CachedDoubleSettings.ContainsKey(key))
                CachedDoubleSettings[key].Modify(newVal);
            else
                CachedDoubleSettings[key] = new ConfigurationEntry<double>(true, newVal, DefaultPropertyManager.DefaultDoubleProperties[key].Description);

            if (!init)
            {
                switch (key)
                {
                    case "cantrip_drop_rate":
                        Factories.Tables.CantripChance.ApplyNumCantripsMod(newVal);
                        break;
                    case "minor_cantrip_drop_rate":
                    case "major_cantrip_drop_rate":
                    case "epic_cantrip_drop_rate":
                    case "legendary_cantrip_drop_rate":
                        Factories.Tables.CantripChance.ApplyNumCantripsMod(newVal);
                        break;
                }
            }
            return true;
        }

        public static void ModifyDoubleDescription(string key, string description)
        {
            if (CachedDoubleSettings.ContainsKey(key))
                CachedDoubleSettings[key].ModifyDescription(description);
            else
                log.Warn($"Attempted to modify the description of {key} which did not exist in the DOUBLE cache.");
        }

        /// <summary>
        /// Retreives a string property from the cache or database
        /// </summary>
        /// <param name="key">The string key for the property</param>
        /// <param name="fallback">The value to return if the property cannot be found.</param>
        /// <param name="cacheFallback">Whether or not the fallback value will be cached.</param>
        /// <returns>A string value representing the property</returns>
        public static Property<string> GetString(string key, string fallback = "", bool cacheFallback = true)
        {
            AssertLoaded();

            if (CachedStringSettings.ContainsKey(key))
                return new Property<string>(CachedStringSettings[key].Item, CachedStringSettings[key].Description);

            var dbValue = DatabaseManager.ShardConfig.GetString(key);

            bool useFallback = dbValue?.Value == null;

            var value = dbValue?.Value ?? fallback;

            if (!useFallback || cacheFallback)
                CachedStringSettings[key] = new ConfigurationEntry<string>(useFallback, value, dbValue?.Description);

            return new Property<string>(value, dbValue?.Description);
        }

        /// <summary>
        /// Modifies a string value in the cache and marks it for being synced on the next cycle
        /// </summary>
        /// <param name="key">The string key for the property</param>
        /// <param name="newVal">The value to replace the old value with</param>
        /// <returns>true if the property was modified, false if no property exists with the given key</returns>
        public static bool ModifyString(string key, string newVal)
        {
            if (!DefaultPropertyManager.DefaultStringProperties.ContainsKey(key))
                return false;

            if (CachedStringSettings.ContainsKey(key))
                CachedStringSettings[key].Modify(newVal);
            else
                CachedStringSettings[key] = new ConfigurationEntry<string>(true, newVal, DefaultPropertyManager.DefaultStringProperties[key].Description);
            return true;
        }

        public static void ModifyStringDescription(string key, string description)
        {
            if (CachedStringSettings.ContainsKey(key))
                CachedStringSettings[key].ModifyDescription(description);
            else
                log.Warn($"Attempted to modify {key} which did not exist in the STRING cache.");
        }


        /// <summary>
        /// Writes all of the updated boolean values from the cache into the database.
        /// </summary>
        private static void WriteBoolToDB()
        {
            foreach (var i in CachedBooleanSettings.Where(r => r.Value.Modified))
            {
                // this probably should be upsert. This does 2 queries per modified datapoint.
                // perhaps run a transaction to queue all the queries at once.
                if (DatabaseManager.ShardConfig.BoolExists(i.Key))
                    DatabaseManager.ShardConfig.SaveBool(new Database.Models.Shard.ConfigPropertiesBoolean { Key = i.Key, Value = i.Value.Item, Description = i.Value.Description });
                else
                    DatabaseManager.ShardConfig.AddBool(i.Key, i.Value.Item, i.Value.Description);
            }
        }

        /// <summary>
        /// Writes all of the updated integer values from the cache into the database.
        /// </summary>
        private static void WriteLongToDB()
        {
            foreach (var i in CachedLongSettings.Where(r => r.Value.Modified))
            {
                // todo: see boolean section for caveat in this approach
                if (DatabaseManager.ShardConfig.LongExists(i.Key))
                    DatabaseManager.ShardConfig.SaveLong(new Database.Models.Shard.ConfigPropertiesLong { Key = i.Key, Value = i.Value.Item, Description = i.Value.Description });
                else
                    DatabaseManager.ShardConfig.AddLong(i.Key, i.Value.Item, i.Value.Description);
            }
        }

        /// <summary>
        /// Writes all of the updated float values from the cache into the database.
        /// </summary>
        private static void WriteDoubleToDB()
        {
            foreach (var i in CachedDoubleSettings.Where(r => r.Value.Modified))
            {
                // todo: see boolean section for caveat in this approach
                if (DatabaseManager.ShardConfig.DoubleExists(i.Key))
                    DatabaseManager.ShardConfig.SaveDouble(new Database.Models.Shard.ConfigPropertiesDouble { Key = i.Key, Value = i.Value.Item, Description = i.Value.Description });
                else
                    DatabaseManager.ShardConfig.AddDouble(i.Key, i.Value.Item, i.Value.Description);
            }
        }

        /// <summary>
        /// Writes all of the updated string values from the cache into the database.
        /// </summary>
        private static void WriteStringToDB()
        {
            foreach (var i in CachedStringSettings.Where(r => r.Value.Modified))
            {
                // todo: see boolean section for caveat in this approach
                if (DatabaseManager.ShardConfig.StringExists(i.Key))
                    DatabaseManager.ShardConfig.SaveString(new Database.Models.Shard.ConfigPropertiesString { Key = i.Key, Value = i.Value.Item, Description = i.Value.Description });
                else
                    DatabaseManager.ShardConfig.AddString(i.Key, i.Value.Item, i.Value.Description);
            }
        }

        private static void DoWork(Object source, ElapsedEventArgs e)
        {
            var startTime = DateTime.UtcNow;

            // first, check for variables updated on the server-side. Write those to the DB.
            // then, compare variables to DB and update from DB as necessary. (needs to minimize r/w)

            WriteBoolToDB();
            WriteLongToDB();
            WriteDoubleToDB();
            WriteStringToDB();

            // next, we need to fetch all of the variables from the DB and compare them quickly.
            LoadPropertiesFromDB();

            log.DebugFormat("PropertyManager DoWork took {0:N0} ms", (DateTime.UtcNow - startTime).TotalMilliseconds);
        }
        public static string ListProperties()
        {
            string props = "Boolean properties:\n";
            foreach (var item in DefaultPropertyManager.DefaultBooleanProperties)
                props += string.Format("\t{0}: {1} (current is {2}, default is {3})\n", item.Key, item.Value.Description, GetBool(item.Key).Item, item.Value.Item);

            props += "\nLong properties:\n";
            foreach (var item in DefaultPropertyManager.DefaultLongProperties)
                props += string.Format("\t{0}: {1} (current is {2}, default is {3})\n", item.Key, item.Value.Description, GetLong(item.Key).Item, item.Value.Item);

            props += "\nDouble properties:\n";
            foreach (var item in DefaultPropertyManager.DefaultDoubleProperties)
                props += string.Format("\t{0}: {1} (current is {2}, default is {3})\n", item.Key, item.Value.Description, GetDouble(item.Key).Item, item.Value.Item);

            props += "\nString properties:\n";
            foreach (var item in DefaultPropertyManager.DefaultStringProperties)
                props += string.Format("\t{0}: {1} (default is hidden)\n", item.Key, item.Value.Description);

            return props;
        }
    }

    public struct Property<T>
    {
        public Property(T item, string description) : this()
        {
            Item = item;
            Description = description;
        }

        public T Item { get; }
        public string Description { get; }
    }

    class ConfigurationEntry<T>
    {
        public bool Modified;
        public T Item;
        public string Description;

        public ConfigurationEntry(bool modified, T item)
        {
            Modified = modified;
            Item = item;
        }

        public ConfigurationEntry(bool modified, T item, string description)
        {
            Modified = modified;
            Item = item;
            Description = description;
        }

        public void Modify(T item)
        {
            Item = item;
            Modified = true;
        }

        public void ModifyDescription(string description)
        {
            Description = description;
            Modified = true;
        }

        public override string ToString()
        {
            return Item + " " + Modified;
        }
    }

    public static class DefaultPropertyManager
    {
        private static ReadOnlyDictionary<A, V> DictOf<A, V>()
        {
            return new ReadOnlyDictionary<A, V>(new Dictionary<A, V>());
        }

        private static ReadOnlyDictionary<A, V> DictOf<A, V>(params (A, V)[] pairs)
        {
            return new ReadOnlyDictionary<A, V>(pairs.ToDictionary
            (
                tup => tup.Item1,
                tup => tup.Item2
            ));
        }

        // For Dekarutide
        public const bool SEASON3_DEFAULTS = false;
        public const bool SEASON3_PATCH_1 = false;
        public const bool SEASON3_PATCH_2 = false;
        public const bool SEASON3_PATCH_3 = false;

        public const bool SEASON4_DEFAULTS = true;
        public const bool SEASON4_PATCH_1_6 = true;
        public const bool SEASON4_PATCH_1_9 = true;
        public const bool SEASON4_PATCH_1_10 = true;
        public const bool SEASON4_PATCH_1_16 = true;
        public const bool SEASON4_PATCH_1_17 = true;
        public const bool SEASON4_PATCH_1_18 = true;
        public const bool SEASON4_PATCH_1_19 = true;
        public const bool SEASON4_PATCH_1_20 = true;
        public const bool SEASON4_PATCH_1_24 = true;

        public static void LoadDefaultProperties()
        {
            // Place any default properties to load in here

            //bool
            foreach (var item in DefaultBooleanProperties)
                PropertyManager.ModifyBool(item.Key, item.Value.Item);

            //float
            foreach (var item in DefaultDoubleProperties)
                PropertyManager.ModifyDouble(item.Key, item.Value.Item, true);

            //int
            foreach (var item in DefaultLongProperties)
                PropertyManager.ModifyLong(item.Key, item.Value.Item);

            //string
            foreach (var item in DefaultStringProperties)
                PropertyManager.ModifyString(item.Key, item.Value.Item);

            // Alternative ruleset's default overrides
            if (Common.ConfigManager.Config.Server.WorldRuleset == Common.Ruleset.Infiltration)
            {
                PropertyManager.ModifyBool("corpse_destroy_pyreals", false);
                PropertyManager.ModifyBool("item_dispel", true);
                PropertyManager.ModifyBool("vendor_shop_uses_generator", true);
                PropertyManager.ModifyBool("allow_xp_at_max_level", true);
                PropertyManager.ModifyBool("allow_fast_chug", false); // Having this on causes the drinking potion animation to get stuck mid-drink quite often.

                PropertyManager.ModifyLong("max_level", 126);

                PropertyManager.ModifyBool("show_dat_warning", true);
                PropertyManager.ModifyString("dat_older_warning_msg", "The location you are attempting to enter is not present in your data files.");
            }
            else if (Common.ConfigManager.Config.Server.WorldRuleset == Common.Ruleset.CustomDM)
            {
                PropertyManager.ModifyBool("corpse_destroy_pyreals", false);
                PropertyManager.ModifyBool("item_dispel", true);
                PropertyManager.ModifyBool("vendor_shop_uses_generator", true);
                PropertyManager.ModifyBool("increase_minimum_encounter_spawn_density", true);
                PropertyManager.ModifyBool("show_dot_messages", true);
                PropertyManager.ModifyBool("salvage_handle_overages", true);
                PropertyManager.ModifyBool("allow_fast_chug", false);
                PropertyManager.ModifyBool("allow_jump_loot", false);
                PropertyManager.ModifyBool("allow_pkl_bump", false);
                PropertyManager.ModifyBool("fix_chest_missing_inventory_window", true);
                PropertyManager.ModifyBool("runrate_add_hooks", true);
                PropertyManager.ModifyBool("house_15day_account", false);
                PropertyManager.ModifyBool("house_30day_cooldown", false);
                PropertyManager.ModifyBool("house_per_char", true);

                PropertyManager.ModifyLong("mansion_min_rank", -1);
                PropertyManager.ModifyLong("fellowship_even_share_level", 80);

                PropertyManager.ModifyBool("show_dat_warning", true);
                PropertyManager.ModifyString("dat_older_warning_msg", "The location you are attempting to enter is not present in your data files.");

                PropertyManager.ModifyDouble("vendor_unique_rot_time", 1800);
                PropertyManager.ModifyDouble("quest_mindelta_rate", 0.2412);

                PropertyManager.ModifyBool("pathfinding", true);

                PropertyManager.ModifyBool("useable_gems", false);
                PropertyManager.ModifyBool("recall_warden", false);

                if (SEASON3_DEFAULTS)
                {
                    // Hard Mode - Progression Caps
                    PropertyManager.ModifyLong("max_level", 40);
                    PropertyManager.ModifyBool("allow_xp_at_max_level", false);
                    PropertyManager.ModifyBool("allow_skill_specialization", false);

                    // Hard Mode - Progression Rates
                    PropertyManager.ModifyDouble("quest_xp_modifier", 0.0);
                    PropertyManager.ModifyDouble("xp_modifier", 0.25);
                    PropertyManager.ModifyDouble("surface_bonus_xp", 0.0);
                    PropertyManager.ModifyDouble("cantrip_drop_rate", 0.25);
                    PropertyManager.ModifyBool("vendor_allow_special_mutations", false);
                    PropertyManager.ModifyDouble("salvage_amount_multiplier", 0.4);
                    PropertyManager.ModifyBool("gateway_ties_summonable", false); // No easy portal bots. People won't be able to evade pvp as easily
                    PropertyManager.ModifyDouble("hot_dungeon_chance", 0.1); // Mitigate lucky gains from relying on hot dungeons being uncontested
                    PropertyManager.ModifyLong("quest_mindelta_rate_longest", 600000); // Just under 1 week for longest quest timers
                    PropertyManager.ModifyBool("allow_allegiance_passup", false);
                    PropertyManager.ModifyDouble("spell_extraction_scroll_base_chance", 0.25);
                    PropertyManager.ModifyDouble("spell_extraction_scroll_chance_per_extra_spell", 0.05);
                    PropertyManager.ModifyDouble("coin_stack_multiplier", 0.5);
                    PropertyManager.ModifyBool("neuter_trade_note_rewards", true);

                    // Hard Mode - PvE Combat
                    PropertyManager.ModifyDouble("customdm_mob_damage_scale", 1.25);
                    PropertyManager.ModifyDouble("customdm_player_war_damage_scale_pve", 0.85);
                    PropertyManager.ModifyDouble("bleed_pve_dmg_mod", 0.5);
                    PropertyManager.ModifyDouble("customdm_mob_war_damage_scale", 1.0); // Normally 0.5

                    // Hard Mode - Death penalty
                    PropertyManager.ModifyDouble("vitae_penalty", 0.20);
                    PropertyManager.ModifyDouble("vitae_penalty_max", 0.60);
                    PropertyManager.ModifyLong("min_level_drop_wielded_on_death", 20);
                    PropertyManager.ModifyLong("min_level_eligible_to_drop_items_on_death", 1);
                    PropertyManager.ModifyBool("use_fixed_death_item_formula", true);
                    PropertyManager.ModifyLong("max_items_dropped_per_death", 25);

                    // Disabling Features
                    PropertyManager.ModifyDouble("elite_mob_spawn_rate", 0.0);
                    PropertyManager.ModifyBool("customdm_mutate_quest_items", false);

                    // Outdoor Nerfs
                    PropertyManager.ModifyBool("override_encounter_spawn_rates", true);
                    PropertyManager.ModifyLong("encounter_regen_interval", 1800);
                    PropertyManager.ModifyDouble("mob_awareness_range", 1.25);

                    // QoL
                    PropertyManager.ModifyBool("fellow_busy_no_recruit", false);
                    PropertyManager.ModifyBool("container_opener_name", true);
                    PropertyManager.ModifyBool("house_15day_account", false);
                    PropertyManager.ModifyBool("permit_corpse_all", true);
                    PropertyManager.ModifyBool("usable_gems_generated_with_1_mana_cost", true);

                    // Non-gameplay configs
                    PropertyManager.ModifyBool("world_closed", true); // require /world open to open server after start
                    PropertyManager.ModifyBool("block_vpn_connections", true);
                    PropertyManager.ModifyBool("player_receive_immediate_save", true);
                    PropertyManager.ModifyBool("house_30day_cooldown", false); // Doesn't matter for apartments but decided to change it
                    PropertyManager.ModifyLong("player_save_interval", 60); // Less rollback for players on crash

                    // Cosmetic
                    PropertyManager.ModifyBool("npc_hairstyle_fullrange", true);

                    // PvP
                    PropertyManager.ModifyBool("pk_server", true);
                    PropertyManager.ModifyLong("pk_timer", 60);
                    PropertyManager.ModifyDouble("pk_cast_radius", 8.0);
                    PropertyManager.ModifyBool("pve_death_respite", true);
                    PropertyManager.ModifyDouble("pve_death_respite_timer", 60);

                    // Scalars: At level 30 (pvp_dmg_mod_low_level), you have low mod for the given weapon
                    //          At level 40 (pvp_dmg_mod_high_level), you have high mod for the given weapon
                    //          And the mod goes gradually from low to high proportionally between the level ranges
                    //          For example, at level 30, you do 1x (pvp_dmg_mod_low_axe) damage with Axe in pvp.
                    //          At 35, you do 1.25x, and at 40, 1.5x (pvp_dmg_mod_high_axe)
                    PropertyManager.ModifyLong("pvp_dmg_mod_low_level", 30); // 30-40, from 10-80.
                    PropertyManager.ModifyLong("pvp_dmg_mod_high_level", 40);

                    PropertyManager.ModifyDouble("pvp_dmg_mod_low_axe", 1.0); // 1.0-1.5, from 0.85-1.5
                    PropertyManager.ModifyDouble("pvp_dmg_mod_high_axe", 1.5);

                    PropertyManager.ModifyDouble("pvp_dmg_mod_high_mace", 1.2); // 1.0-1.2, from 1.0-1.0

                    PropertyManager.ModifyDouble("pvp_dmg_mod_low_spear", 1.15); // 1.15-1.5, from 1.15-1.8
                    PropertyManager.ModifyDouble("pvp_dmg_mod_high_spear", 1.5);

                    PropertyManager.ModifyDouble("pvp_dmg_mod_low_staff", 1.5); // Same
                    PropertyManager.ModifyDouble("pvp_dmg_mod_high_staff", 1.5);

                    PropertyManager.ModifyDouble("pvp_dmg_mod_high_sword", 1.5); // 1.0-1.5, from 1.0-2.0

                    PropertyManager.ModifyDouble("pvp_dmg_mod_high_dagger", 1.2); // 1.0-1.2, from 1.0-1.5

                    PropertyManager.ModifyDouble("pvp_dmg_mod_low_unarmed", 1.3);
                    PropertyManager.ModifyDouble("pvp_dmg_mod_high_unarmed", 1.5); // 1.3-1.5, from 1.3-2.2

                    PropertyManager.ModifyDouble("pvp_dmg_mod_low_unarmed_war", 0.65); // Same
                    PropertyManager.ModifyDouble("pvp_dmg_mod_high_unarmed_war", 0.85);

                    PropertyManager.ModifyDouble("pvp_dmg_mod_low_bow", 1.0); // 1.0-1.7, from 1.45-1.9
                    PropertyManager.ModifyDouble("pvp_dmg_mod_high_bow", 1.7);

                    PropertyManager.ModifyDouble("pvp_dmg_mod_low_crossbow", 1.0); // 1.0-1.5, from 1.5-1.5
                    PropertyManager.ModifyDouble("pvp_dmg_mod_high_crossbow", 1.5);

                    PropertyManager.ModifyDouble("pvp_dmg_mod_low_thrown", 0.90); // 0.90-1.3 from 0.75-1.3
                    PropertyManager.ModifyDouble("pvp_dmg_mod_high_thrown", 1.3);


                    PropertyManager.ModifyDouble("pvp_dmg_mod_low_dot", 0.75); // Same
                    PropertyManager.ModifyDouble("pvp_dmg_mod_high_dot", 0.75);
                    PropertyManager.ModifyDouble("pvp_dmg_mod_low_void_dot", 0.75); // Same, don't think this actually is in the game anyway
                    PropertyManager.ModifyDouble("pvp_dmg_mod_high_void_dot", 0.75);

                    PropertyManager.ModifyDouble("pvp_dmg_mod_high_war", 1.0); // was 1.2

                    // Grandfathered in from Existing non-default server configs
                    PropertyManager.ModifyBool("dekaru_dual_wield_speed_mod", false);
                    PropertyManager.ModifyBool("craft_exact_msg", true); // QoL crafting success chance, mitigates advantages given to plugin devs/users
                    PropertyManager.ModifyBool("assess_creature_pve_always_succeed", true); // Fixes loot delays with vtank in some situations
                    PropertyManager.ModifyBool("house_per_char", true); // Allow multiple houses per account
                    PropertyManager.ModifyBool("show_discord_chat_ingame", true);
                    PropertyManager.ModifyBool("spellcast_recoil_queue", true);
                    PropertyManager.ModifyDouble("spellcast_max_angle", 40.0);
                    PropertyManager.ModifyBool("useable_gems", true);
                    PropertyManager.ModifyDouble("dekaru_dagger_ms_animation_speed_1h", 1.6);
                    PropertyManager.ModifyDouble("dekaru_dagger_ms_animation_speed_dualwield", 1.7);
                    PropertyManager.ModifyDouble("dekaru_tw_animation_speed", 3.0);
                    PropertyManager.ModifyDouble("fast_missile_modifier", 3.0);

                    // Not grandathered in, reverting to default
                    // fall_damage_enabled: true
                    // fall_damage_multiplier: 1.0
                    // vpn_account_whitelist

                    if (SEASON3_PATCH_1)
                    {
                        // New properties as of this patch
                        PropertyManager.ModifyLong("pk_escape_max_level_difference", 20);
                        PropertyManager.ModifyBool("stackable_trophy_rewards_use_tar", true);
                        PropertyManager.ModifyBool("drop_all_coins_on_death", true);

                        // Existing properties, needs manual modify as admin for servers that are not new
                        PropertyManager.ModifyDouble("pvp_dmg_mod_high_spear", 1.4); // From 1.5, -6.7%
                        PropertyManager.ModifyDouble("pvp_dmg_mod_high_axe", 1.4); // From 1.5, -6.7%
                        PropertyManager.ModifyDouble("pvp_dmg_mod_high_sword", 1.65); // From 1.5, +10%
                        PropertyManager.ModifyDouble("pvp_dmg_mod_high_unarmed", 1.65); // From 1.5, +10%
                        PropertyManager.ModifyDouble("pvp_dmg_mod_high_crossbow", 1.7); // From 1.5, +13%
                        PropertyManager.ModifyDouble("pvp_dmg_mod_high_dagger", 1.25); // From 1.2, +4%
                        PropertyManager.ModifyDouble("pvp_dmg_mod_high_thrown", 1.4); // From 1.3, + 7%
                        PropertyManager.ModifyDouble("dekaru_dagger_ms_animation_speed_dualwield", 1.6); // From 1.8, -12%
                        PropertyManager.ModifyDouble("dekaru_dagger_ms_animation_speed_1h", 1.7); // From 1.6, +6%
                    }

                    if (SEASON3_PATCH_2)
                    {
                        PropertyManager.ModifyBool("dekarutide_season3_alternate_weapon_wield_reqs", true);
                        PropertyManager.ModifyBool("dekarutide_season3_alternate_loot_valuations", true);
                        PropertyManager.ModifyBool("ai_anti_perch", false);
                        PropertyManager.ModifyBool("ai_custom_pathfind", false);
                        PropertyManager.ModifyDouble("spelltransfer_over_tier_success_chance", 0.5);
                        PropertyManager.ModifyBool("die_command_enabled", false);
                    }

                    if (SEASON3_PATCH_3)
                    {
                        PropertyManager.ModifyLong("max_items_dropped_per_death", 18);
                        PropertyManager.ModifyBool("cmd_pop_last_24_hours", true);
                        PropertyManager.ModifyDouble("vitae_penalty", 0.15);
                        PropertyManager.ModifyDouble("extra_vitae_penalty_pvp", 0.15);
                        PropertyManager.ModifyDouble("vitae_penalty_max", 0.30);
                        PropertyManager.ModifyLong("pk_timer", 37);
                    }
                }

                if (SEASON4_DEFAULTS)
                {
                    // Hard Mode - Progression
                    // Caps on player progression
                    PropertyManager.ModifyLong("max_level", 20);
                    PropertyManager.ModifyBool("allow_xp_at_max_level", false);
                    PropertyManager.ModifyBool("allow_skill_specialization", true);

                    // Rates affecting XP, loot, and quests
                    PropertyManager.ModifyDouble("quest_xp_modifier", 1.0);
                    PropertyManager.ModifyDouble("xp_modifier", 1.0);
                    PropertyManager.ModifyDouble("surface_bonus_xp", 0.0);
                    PropertyManager.ModifyDouble("cantrip_drop_rate", 0.25);
                    PropertyManager.ModifyBool("vendor_allow_special_mutations", false);
                    PropertyManager.ModifyDouble("salvage_amount_multiplier", 0.4);
                    PropertyManager.ModifyBool("gateway_ties_summonable", false); // No easy portal bots. People won't be able to evade PvP as easily
                    PropertyManager.ModifyDouble("hot_dungeon_chance", 0.1); // Mitigate lucky gains from relying on hot dungeons being uncontested
                    PropertyManager.ModifyLong("quest_mindelta_rate_longest", 600000); // Just under 1 week for longest quest timers
                    PropertyManager.ModifyBool("allow_allegiance_passup", false);
                    PropertyManager.ModifyDouble("spell_extraction_scroll_base_chance", 0.25);
                    PropertyManager.ModifyDouble("spell_extraction_scroll_chance_per_extra_spell", 0.05);
                    PropertyManager.ModifyDouble("coin_stack_multiplier", 0.5);
                    PropertyManager.ModifyBool("neuter_trade_note_rewards", true);

                    // Hard Mode - PvE Combat
                    PropertyManager.ModifyDouble("customdm_mob_damage_scale", 0.5);
                    PropertyManager.ModifyDouble("customdm_mob_war_damage_scale", 0.5); // Normally 0.5
                    PropertyManager.ModifyDouble("customdm_player_war_damage_scale_pve", 1.0);
                    PropertyManager.ModifyDouble("bleed_pve_dmg_mod", 0.5);

                    // Hard Mode - Death Penalty
                    PropertyManager.ModifyDouble("vitae_penalty", 0.15);
                    PropertyManager.ModifyDouble("vitae_penalty_max", 0.30);
                    PropertyManager.ModifyLong("min_level_drop_wielded_on_death", 20);
                    PropertyManager.ModifyLong("min_level_eligible_to_drop_items_on_death", 1);
                    PropertyManager.ModifyBool("use_fixed_death_item_formula", true);
                    PropertyManager.ModifyLong("max_items_dropped_per_death", 18);
                    PropertyManager.ModifyBool("drop_all_coins_on_death", true); // Moved from Grandfathered, fits death mechanics

                    // Gameplay Adjustments
                    // Disabled Features
                    PropertyManager.ModifyDouble("elite_mob_spawn_rate", 0.02);
                    PropertyManager.ModifyBool("customdm_mutate_quest_items", false);

                    // Outdoor Nerfs
                    PropertyManager.ModifyBool("override_encounter_spawn_rates", true);
                    PropertyManager.ModifyLong("encounter_regen_interval", 1800);
                    PropertyManager.ModifyDouble("mob_awareness_range", 1.25);

                    // Quality of Life (QoL)
                    // Player Experience
                    PropertyManager.ModifyBool("fellow_busy_no_recruit", false);
                    PropertyManager.ModifyBool("container_opener_name", true);
                    PropertyManager.ModifyBool("permit_corpse_all", true);
                    PropertyManager.ModifyBool("usable_gems_generated_with_1_mana_cost", true);

                    // Housing
                    PropertyManager.ModifyBool("house_15day_account", false);
                    PropertyManager.ModifyBool("house_30day_cooldown", false); // Doesn't matter for apartments but decided to change it
                    PropertyManager.ModifyBool("house_per_char", true); // Allow multiple houses per account, moved from Grandfathered

                    // Server Configs
                    // Non-Gameplay
                    PropertyManager.ModifyBool("world_closed", true); // Require /world open to open server after start
                    PropertyManager.ModifyBool("block_vpn_connections", true);
                    PropertyManager.ModifyBool("player_receive_immediate_save", true);
                    PropertyManager.ModifyLong("player_save_interval", 60); // Less rollback for players on crash
                    PropertyManager.ModifyBool("cmd_pop_last_24_hours", true); // Moved from Grandfathered, fits server monitoring

                    // Cosmetic
                    PropertyManager.ModifyBool("npc_hairstyle_fullrange", true);

                    // PvP
                    // General PvP Settings
                    PropertyManager.ModifyBool("pk_server", true);
                    PropertyManager.ModifyLong("pk_timer", 37);
                    PropertyManager.ModifyDouble("pk_cast_radius", 8.0);
                    PropertyManager.ModifyLong("pk_escape_max_level_difference", 20); // Moved from Grandfathered, PvP-related
                    PropertyManager.ModifyDouble("extra_vitae_penalty_pvp", 0.15); // Moved from Grandfathered, fits PvP death mechanics

                    // PvP Damage Scalars
                    // At level 30 (pvp_dmg_mod_low_level), you have low mod for the given weapon
                    // At level 40 (pvp_dmg_mod_high_level), you have high mod for the given weapon
                    // Mod scales proportionally between levels (e.g., Axe: 1.0 at 30, 1.25 at 35, 1.4 at 40)
                    PropertyManager.ModifyLong("pvp_dmg_mod_low_level", 30); // 30-40, from 10-80
                    PropertyManager.ModifyLong("pvp_dmg_mod_high_level", 40);

                    PropertyManager.ModifyDouble("pvp_dmg_mod_low_axe", 1.0); // 1.0-1.4, from 0.85-1.5
                    PropertyManager.ModifyDouble("pvp_dmg_mod_high_axe", 1.4); // From 1.5, -6.7%

                    PropertyManager.ModifyDouble("pvp_dmg_mod_high_mace", 1.2); // 1.0-1.2, from 1.0-1.0

                    PropertyManager.ModifyDouble("pvp_dmg_mod_low_spear", 1.15); // 1.15-1.4, from 1.15-1.8
                    PropertyManager.ModifyDouble("pvp_dmg_mod_high_spear", 1.4); // From 1.5, -6.7%

                    PropertyManager.ModifyDouble("pvp_dmg_mod_low_staff", 1.5); // Same
                    PropertyManager.ModifyDouble("pvp_dmg_mod_high_staff", 1.5);

                    PropertyManager.ModifyDouble("pvp_dmg_mod_high_sword", 1.65); // From 1.5, +10%

                    PropertyManager.ModifyDouble("pvp_dmg_mod_high_dagger", 1.25); // From 1.2, +4%

                    PropertyManager.ModifyDouble("pvp_dmg_mod_low_unarmed", 1.3);
                    PropertyManager.ModifyDouble("pvp_dmg_mod_high_unarmed", 1.65); // From 1.5, +10%

                    PropertyManager.ModifyDouble("pvp_dmg_mod_low_unarmed_war", 0.65); // Same
                    PropertyManager.ModifyDouble("pvp_dmg_mod_high_unarmed_war", 0.85);

                    PropertyManager.ModifyDouble("pvp_dmg_mod_low_bow", 1.0); // 1.0-1.7, from 1.45-1.9
                    PropertyManager.ModifyDouble("pvp_dmg_mod_high_bow", 1.7);

                    PropertyManager.ModifyDouble("pvp_dmg_mod_low_crossbow", 1.0); // 1.0-1.7, from 1.5-1.5
                    PropertyManager.ModifyDouble("pvp_dmg_mod_high_crossbow", 1.7); // From 1.5, +13%

                    PropertyManager.ModifyDouble("pvp_dmg_mod_low_thrown", 0.90); // 0.90-1.4, from 0.75-1.3
                    PropertyManager.ModifyDouble("pvp_dmg_mod_high_thrown", 1.4); // From 1.3, +7%

                    PropertyManager.ModifyDouble("pvp_dmg_mod_low_dot", 0.75); // Same
                    PropertyManager.ModifyDouble("pvp_dmg_mod_high_dot", 0.75);
                    PropertyManager.ModifyDouble("pvp_dmg_mod_low_void_dot", 0.75); // Same, possibly unused
                    PropertyManager.ModifyDouble("pvp_dmg_mod_high_void_dot", 0.75);

                    PropertyManager.ModifyDouble("pvp_dmg_mod_high_war", 1.0); // Was 1.2

                    // Grandfathered Configs from Previous Seasons
                    // Combat & Animation
                    PropertyManager.ModifyBool("dekaru_dual_wield_speed_mod", false);
                    PropertyManager.ModifyDouble("dekaru_dagger_ms_animation_speed_1h", 1.6);
                    PropertyManager.ModifyDouble("dekaru_dagger_ms_animation_speed_dualwield", 1.7);
                    PropertyManager.ModifyDouble("dekaru_dagger_ms_animation_speed_shielded", 1.8);
                    PropertyManager.ModifyDouble("dekaru_sword_ms_animation_speed_1h", 1.6);
                    PropertyManager.ModifyDouble("dekaru_sword_ms_animation_speed_dualwield", 1.6);
                    PropertyManager.ModifyDouble("dekaru_sword_ms_animation_speed_shielded", 1.8);
                    PropertyManager.ModifyDouble("dekaru_tw_animation_speed", 3.0);
                    PropertyManager.ModifyDouble("fast_missile_modifier", 3.0);

                    // Loot & Crafting
                    PropertyManager.ModifyBool("craft_exact_msg", true); // QoL crafting success chance, mitigates plugin advantages
                    PropertyManager.ModifyBool("stackable_trophy_rewards_use_tar", true);
                    PropertyManager.ModifyBool("useable_gems", true); // Typo? Should it match "usable_gems_generated..."?
                    PropertyManager.ModifyDouble("spelltransfer_over_tier_success_chance", 0.5);
                    PropertyManager.ModifyBool("dekarutide_season3_alternate_weapon_wield_reqs", true);
                    PropertyManager.ModifyBool("dekarutide_season3_alternate_loot_valuations", true);

                    // Miscellaneous
                    PropertyManager.ModifyBool("assess_creature_pve_always_succeed", true); // Fixes loot delays with vtank in some situations
                    PropertyManager.ModifyBool("show_discord_chat_ingame", true);
                    PropertyManager.ModifyBool("spellcast_recoil_queue", true);
                    PropertyManager.ModifyDouble("spellcast_max_angle", 40.0);
                    PropertyManager.ModifyBool("ai_anti_perch", false);
                    PropertyManager.ModifyBool("ai_custom_pathfind", false);
                    PropertyManager.ModifyBool("die_command_enabled", false);

                    if (SEASON4_PATCH_1_6)
                    {
                        PropertyManager.ModifyLong("arenas_min_level", 5);
                        PropertyManager.ModifyLong("arenas_reward_min_level", 1);
                        PropertyManager.ModifyLong("arenas_reward_min_age", 1);
                        PropertyManager.ModifyDouble("arena_corpse_rot_seconds", 900);
                        PropertyManager.ModifyString("arenas_blacklist", "");
                        PropertyManager.ModifyString("arena_globals_webhook", "");
                        PropertyManager.ModifyBool("disable_arenas", false);
                        PropertyManager.ModifyBool("arena_allow_same_ip_match", false);
                        PropertyManager.ModifyBool("arena_allow_observers", true);
                    }

                    if (SEASON4_PATCH_1_9)
                    {
                        PropertyManager.ModifyLong("player_corpse_permissible_age", 7200);

                        // Reason: Economy is too hard
                        PropertyManager.ModifyDouble("coin_stack_multiplier", 1.0);
                        PropertyManager.ModifyDouble("spell_extraction_scroll_base_chance", 0.5);
                        PropertyManager.ModifyDouble("spell_extraction_scroll_chance_per_extra_spell", 0.1);
                        // Reason: Dekaru's Arcane Lore changes make this unnecessary to penalize people for
                        PropertyManager.ModifyDouble("spelltransfer_over_tier_success_chance", 1.0);
                        // Reason: More hot dungeons is good for the morale of the people
                        PropertyManager.ModifyDouble("hot_dungeon_interval", 3600.0);
                        PropertyManager.ModifyDouble("hot_dungeon_chance", 0.33);
                        // Reason: Item loss is bad enough
                        PropertyManager.ModifyDouble("extra_vitae_penalty_pvp", 0.0);
                        // Reason: Global xp is already reduced by 25%, no need to keep quest xp reduced on top of it
                        PropertyManager.ModifyDouble("quest_xp_modifier", 1.0);


                        PropertyManager.ModifyDouble("mob_awareness_range_indoors", 1.0);

                        // Reason: Death penalty is too harsh for newer players
                        PropertyManager.ModifyLong("min_level_drop_wielded_on_death", 30); // previously was 20
                        PropertyManager.ModifyLong("max_items_dropped_per_death", 10); // previously was 18

                        // Reason: Voted by the majority of the playerbase
                        PropertyManager.ModifyBool("require_spell_comps", true); // previously was false
                    }

                    if (SEASON4_PATCH_1_10)
                    {
                        // Reason: Aggro is too difficult outdoors
                        PropertyManager.ModifyDouble("mob_awareness_range", 1.0); // previously 1.25
                        // Reason: Value was relevent for the previous season, but not for this one
                        PropertyManager.ModifyDouble("salvage_amount_multiplier", 1.0); // previously 0.4
                    }

                    if (SEASON4_PATCH_1_16)
                    {
                        // Reason: Reaching exploration contract location still uses this modifier and it needs to be increased
                        PropertyManager.ModifyDouble("exploration_bonus_xp", 0.5); // previously 0
                        // Reason: New property for exploration contract treasure
                        PropertyManager.ModifyDouble("exploration_bonus_xp_treasure", 0.5);
                    }

                    if (SEASON4_PATCH_1_17)
                    {
                        // Reason: New property for exploration contract location discovery
                        PropertyManager.ModifyDouble("exploration_bonus_xp_contract_location", 0.5);
                    }

                    if (SEASON4_PATCH_1_18)
                    {
                        // Reason: Boost xp for discovering a contract dungeon  
                        PropertyManager.ModifyDouble("exploration_bonus_xp_contract_location", 0.5125); // was 0.5

                        // Reason: Boost xp for treasure maps, making them more rewarding 
                        PropertyManager.ModifyDouble("exploration_bonus_xp_treasure", 0.55);  // was 0.5

                        // Reason: Broadcast player life actions such as creation/deletion/restoration
                        PropertyManager.ModifyBool("broadcast_player_life", true);
                        PropertyManager.ModifyBool("broadcast_player_delete", true);
                        PropertyManager.ModifyBool("broadcast_player_create", false);
                        PropertyManager.ModifyBool("broadcast_player_restore", true);
                    }

                    if (SEASON4_PATCH_1_19)
                    {
                        // Reason: Give unlimited respecs to all players
                        PropertyManager.ModifyLong("unlimited_respec_max_level", 126);  // was 20

                        // Reason: Fixed bug for exploration bonus xp
                        PropertyManager.ModifyDouble("exploration_bonus_xp_treasure", 5.0);  // was 0.55
                        PropertyManager.ModifyDouble("exploration_bonus_xp_markers", 5.0);  // was 0.55
                        PropertyManager.ModifyDouble("exploration_bonus_xp_contract_location", 5.0);  // was 0.5125
                    }

                    if (SEASON4_PATCH_1_20)
                    {
                        PropertyManager.ModifyLong("max_level", 60);  // was 40
                        PropertyManager.ModifyLong("previous_max_level", 40);
                        PropertyManager.ModifyDouble("catchup_xp_modifier", 2.0);
                    }

                    if (SEASON4_PATCH_1_24)
                    {
                        // Reason: Small catchup xp boost
                        PropertyManager.ModifyDouble("catchup_xp_modifier", 3.0); // was 2.0
                    }
                }
            }
        }

        // ==================================================================================
        // To change these values for the server,
        // please use the /modifybool, /modifylong, /modifydouble, and /modifystring commands
        // ==================================================================================

        public static readonly ReadOnlyDictionary<string, Property<bool>> DefaultBooleanProperties =
            DictOf(
                ("account_login_boots_in_use", new Property<bool>(true, "if FALSE, oldest connection to account is not booted when new connection occurs")),
                ("advanced_combat_pets", new Property<bool>(false, "(non-retail function) If enabled, Combat Pets can cast spells")),
                ("advocate_fane_auto_bestow", new Property<bool>(false, "If enabled, Advocate Fane will automatically bestow new advocates to advocate_fane_auto_bestow_level")),
                ("aetheria_heal_color", new Property<bool>(false, "If enabled, changes the aetheria healing over time messages from the default retail red color to green")),
                ("allow_combat_mode_crafting", new Property<bool>(false, "If enabled, allows players to do crafting (recipes) from all stances. Forces players to NonCombat first, then continues to recipe action.")),
                ("allow_door_hold", new Property<bool>(true, "enables retail behavior where standing on a door while it is closing keeps the door as ethereal until it is free from collisions, effectively holding the door open for other players")),
                ("allow_fast_chug", new Property<bool>(true, "enables retail behavior where a player can consume food and drink faster than normal by breaking animation")),
                ("allow_jump_loot", new Property<bool>(true, "enables retail behavior where a player can quickly loot items while jumping, bypassing the 'crouch down' animation")),
                ("allow_negative_dispel_resist", new Property<bool>(true, "enables retail behavior where #-# negative dispels can be resisted")),
                ("allow_negative_rating_curve", new Property<bool>(true, "enables retail behavior where negative DRR from void dots didn't switch to the reverse rating formula, resulting in a possibly unintended curve that quickly ramps up as -rating goes down, eventually approaching infinity / divide by 0 for -100 rating. less than -100 rating would produce negative numbers.")),
                ("allow_pkl_bump", new Property<bool>(true, "enables retail behavior where /pkl checks for entry collisions, bumping the player position over if standing on another PKLite. This effectively enables /pkl door skipping from retail")),
                ("allow_summoning_killtask_multicredit", new Property<bool>(true, "enables retail behavior where a summoner can get multiple killtask credits from a monster")),
                ("assess_creature_mod", new Property<bool>(false, "(non-retail function) If enabled, re-enables former skill formula, when assess creature skill is not trained or spec'ed")),
                ("attribute_augmentation_safety_cap", new Property<bool>(true, "if TRUE players are not able to use attribute augmentations if the innate value of the target attribute is >= 96. All normal restrictions to these augmentations still apply.")),
                ("bz_snitch_hcpk_top10", new Property<bool>(true, "if TRUE the BZ location snitch in HCPK mode will give the location of players only from the top 10, to any HCPK player.")),
                ("chat_disable_general", new Property<bool>(false, "disable general global chat channel")),
                ("chat_disable_lfg", new Property<bool>(false, "disable lfg global chat channel")),
                ("chat_disable_olthoi", new Property<bool>(false, "disable olthoi global chat channel")),
                ("chat_disable_roleplay", new Property<bool>(false, "disable roleplay global chat channel")),
                ("chat_disable_trade", new Property<bool>(false, "disable trade global chat channel")),
                ("chat_echo_only", new Property<bool>(false, "global chat returns to sender only")),
                ("chat_echo_reject", new Property<bool>(false, "global chat returns to sender on reject")),
                ("chat_inform_reject", new Property<bool>(true, "global chat informs sender on reason for reject")),
                ("chat_log_abuse", new Property<bool>(false, "log abuse chat")),
                ("chat_log_admin", new Property<bool>(false, "log admin chat")),
                ("chat_log_advocate", new Property<bool>(false, "log advocate chat")),
                ("chat_log_allegiance", new Property<bool>(false, "log allegiance chat")),
                ("chat_log_audit", new Property<bool>(true, "log audit chat")),
                ("chat_log_debug", new Property<bool>(false, "log debug chat")),
                ("chat_log_fellow", new Property<bool>(false, "log fellow chat")),
                ("chat_log_general", new Property<bool>(false, "log general chat")),
                ("chat_log_global", new Property<bool>(false, "log global broadcasts")),
                ("chat_log_help", new Property<bool>(false, "log help chat")),
                ("chat_log_lfg", new Property<bool>(false, "log LFG chat")),
                ("chat_log_olthoi", new Property<bool>(false, "log olthoi chat")),
                ("chat_log_qa", new Property<bool>(false, "log QA chat")),
                ("chat_log_roleplay", new Property<bool>(false, "log roleplay chat")),
                ("chat_log_sentinel", new Property<bool>(false, "log sentinel chat")),
                ("chat_log_society", new Property<bool>(false, "log society chat")),
                ("chat_log_trade", new Property<bool>(false, "log trade chat")),
                ("chat_log_townchans", new Property<bool>(false, "log advocate town chat")),
                ("chat_requires_account_15days", new Property<bool>(false, "global chat privileges requires accounts to be 15 days or older")),
                ("chess_enabled", new Property<bool>(true, "if FALSE then chess will be disabled")),
                ("use_cloak_proc_custom_scale", new Property<bool>(false, "If TRUE, the calculation for cloak procs will be based upon the values set by the server oeprator.")),
                ("client_movement_formula", new Property<bool>(false, "If enabled, server uses DoMotion/StopMotion self-client movement methods instead of apply_raw_movement")),
                ("container_opener_name", new Property<bool>(false, "If enabled, when a player tries to open a container that is already in use by someone else, replaces 'someone else' in the message with the actual name of the player")),
                ("corpse_decay_tick_logging", new Property<bool>(false, "If ENABLED then player corpse ticks will be logged")),
                ("corpse_destroy_pyreals", new Property<bool>(true, "If FALSE then pyreals will not be completely destroyed on player death")),
                ("craft_exact_msg", new Property<bool>(false, "If TRUE, and player has crafting chance of success dialog enabled, shows them an additional message in their chat window with exact %")),
                ("creature_name_check", new Property<bool>(true, "if enabled, creature names in world database restricts player names during character creation")),
                ("creatures_drop_createlist_wield", new Property<bool>(false, "If FALSE then Wielded items in CreateList will not drop. Retail defaulted to TRUE but there are currently data errors")),
                ("equipmentsetid_enabled", new Property<bool>(true, "enable this to allow adding EquipmentSetIDs to loot armor")),
                ("equipmentsetid_name_decoration", new Property<bool>(false, "enable this to add the EquipmentSet name to loot armor name")),
                ("fastbuff", new Property<bool>(true, "If TRUE, enables the fast buffing trick from retail.")),
                ("fellow_busy_no_recruit", new Property<bool>(true, "if FALSE, fellows can be recruited while they are busy, different from retail")),
                ("fellow_kt_killer", new Property<bool>(true, "if FALSE, fellowship kill tasks will share with the fellowship, even if the killer doesn't have the quest")),
                ("fellow_kt_landblock", new Property<bool>(false, "if TRUE, fellowship kill tasks will share with landblock range (192 distance radius, or entire dungeon)")),
                ("fellow_quest_bonus", new Property<bool>(false, "if TRUE, applies EvenShare formula to fellowship quest reward XP (300% max bonus, defaults to false in retail)")),
                ("fix_chest_missing_inventory_window", new Property<bool>(false, "Very non-standard fix. This fixes an acclient bug where unlocking a chest, and then quickly opening it before the client has received the Locked=false update from server can result in the chest opening, but with the chest inventory window not displaying. Bug has a higher chance of appearing with more network latency.")),
                ("gateway_ties_summonable", new Property<bool>(true, "if disabled, players cannot summon ties from gateways. defaults to enabled, as in retail")),
                ("gearknight_core_plating", new Property<bool>(true, "if disabled, Gear Knight players are not required to use core plating devices for armor and clothing. defaults to enabled, as in retail")),
                ("house_15day_account", new Property<bool>(true, "if disabled, houses can be purchased with accounts created less than 15 days old")),
                ("house_30day_cooldown", new Property<bool>(true, "if disabled, houses can be purchased without waiting 30 days between each purchase")),
                ("house_hook_limit", new Property<bool>(true, "if disabled, house hook limits are ignored")),
                ("house_hookgroup_limit", new Property<bool>(true, "if disabled, house hook group limits are ignored")),
                ("house_per_char", new Property<bool>(false, "if TRUE, allows 1 house per char instead of 1 house per account")),
                ("house_purchase_requirements", new Property<bool>(true, "if disabled, requirements to purchase/rent house are not checked")),
                ("house_rent_enabled", new Property<bool>(true, "If FALSE then rent is not required")),
                ("iou_trades", new Property<bool>(false, "(non-retail function) If enabled, IOUs can be traded for objects that are missing in DB but added/restored later on")),
                ("item_dispel", new Property<bool>(false, "if enabled, allows players to dispel items. defaults to end of retail, where item dispels could only target creatures")),
                ("lifestone_broadcast_death", new Property<bool>(true, "if true, player deaths are additionally broadcast to other players standing near the destination lifestone")),
                ("loot_quality_mod", new Property<bool>(true, "if FALSE then the loot quality modifier of a Death Treasure profile does not affect loot generation")),
                ("npc_hairstyle_fullrange", new Property<bool>(false, "if TRUE, allows generated creatures to use full range of hairstyles. Retail only allowed first nine (0-8) out of 51")),
                ("offline_xp_passup_limit", new Property<bool>(true, "if FALSE, allows unlimited xp to passup to offline characters in allegiances")),
                ("olthoi_play_disabled", new Property<bool>(false, "if false, allows players to create and play as olthoi characters")),
                ("override_encounter_spawn_rates", new Property<bool>(false, "if enabled, landblock encounter spawns are overidden by double properties below.")),
                ("permit_corpse_all", new Property<bool>(false, "If TRUE, /permit grants permittees access to all corpses of the permitter. Defaults to FALSE as per retail, where /permit only grants access to 1 locked corpse")),
                ("persist_movement", new Property<bool>(false, "If TRUE, persists autonomous movements such as turns and sidesteps through non-autonomous server actions. Retail didn't appear to do this, but some players may prefer this.")),
                ("pet_stow_replace", new Property<bool>(false, "pet stowing for different pet devices becomes a stow and replace. defaults to retail value of false")),
                ("player_config_command", new Property<bool>(false, "If enabled, players can use /config to change their settings via text commands")),
                ("player_receive_immediate_save", new Property<bool>(false, "if enabled, when the player receives items from an NPC, they will be saved immediately")),
                ("pk_server", new Property<bool>(false, "set this to TRUE for darktide servers")),
                ("pk_server_safe_training_academy", new Property<bool>(false, "set this to TRUE to disable pk fighting in training academy and time to exit starter town safely")),
                ("pkl_server", new Property<bool>(false, "set this to TRUE for pink servers")),
                ("quest_info_enabled", new Property<bool>(false, "toggles the /myquests player command")),
                ("rares_real_time", new Property<bool>(true, "allow for second chance roll based on an rng seeded timestamp for a rare on rare eligible kills that do not generate a rare, rares_max_seconds_between defines maximum seconds before second chance kicks in")),
                ("rares_real_time_v2", new Property<bool>(false, "chances for a rare to be generated on rare eligible kills are modified by the last time one was found per each player, rares_max_days_between defines maximum days before guaranteed rare generation")),
                ("runrate_add_hooks", new Property<bool>(false, "if TRUE, adds some runrate hooks that were missing from retail (exhaustion done, raise skill/attribute")),
                ("reportbug_enabled", new Property<bool>(false, "toggles the /reportbug player command")),
                ("require_spell_comps", new Property<bool>(true, "if FALSE, spell components are no longer required to be in inventory to cast spells. defaults to enabled, as in retail")),
                ("safe_spell_comps", new Property<bool>(false, "if TRUE, disables spell component burning for everyone")),
                ("salvage_handle_overages", new Property<bool>(false, "in retail, if 2 salvage bags were combined beyond 100 structure, the overages would be lost")),
                ("show_ammo_buff", new Property<bool>(false, "shows active enchantments such as blood drinker on equipped missile ammo during appraisal")),
                ("show_aura_buff", new Property<bool>(false, "shows active aura enchantments on wielded items during appraisal")),
                ("show_dat_warning", new Property<bool>(false, "if TRUE, will alert player (dat_warning_msg) when client attempts to download from server and boot them from game, disabled by default")),
                ("show_dot_messages", new Property<bool>(false, "enabled, shows combat messages for DoT damage ticks. defaults to disabled, as in retail")),
                ("show_first_login_gift", new Property<bool>(false, "if TRUE, will show on first login that the player earned bonus item (Blackmoor's Favor and/or Asheron's Benediction), disabled by default because msg is kind of odd on an emulator")),
                ("show_mana_conv_bonus_0", new Property<bool>(true, "if disabled, only shows mana conversion bonus if not zero, during appraisal of casting items")),
                ("smite_uses_takedamage", new Property<bool>(false, "if enabled, smite applies damage via TakeDamage")),
                ("spellcast_recoil_queue", new Property<bool>(false, "if true, players can queue the next spell to cast during recoil animation")),
                ("spell_projectile_ethereal", new Property<bool>(false, "broadcasts all spell projectiles as ethereal to clients only, and manually send stop velocity on collision. can fix various issues with client missing target id.")),
                ("suicide_instant_death", new Property<bool>(false, "if enabled, @die command kills player instantly. defaults to disabled, as in retail")),
                ("taboo_table", new Property<bool>(true, "if enabled, taboo table restricts player names during character creation")),
                ("tailoring_intermediate_uieffects", new Property<bool>(false, "If true, tailoring intermediate icons retain the magical/elemental highlight of the original item")),
                ("trajectory_alt_solver", new Property<bool>(false, "use the alternate trajectory solver for missiles and spell projectiles")),
                ("universal_masteries", new Property<bool>(true, "if TRUE, matches end of retail masteries - players wielding almost any weapon get +5 DR, except if the weapon \"seems tough to master\". " +
                                                                 "if FALSE, players start with mastery of 1 melee and 1 ranged weapon type based on heritage, and can later re-select these 2 masteries")),
                ("unlimited_sequence_gaps", new Property<bool>(false, "upon startup, allows server to find all unused guids in a range instead of a set hard limit")),
                ("use_generator_rotation_offset", new Property<bool>(true, "enables or disables using the generator's current rotation when offseting relative positions")),
                ("use_portal_max_level_requirement", new Property<bool>(true, "disable this to ignore the max level restriction on portals")),
                ("use_turbine_chat", new Property<bool>(true, "enables or disables global chat channels (General, LFG, Roleplay, Trade, Olthoi, Society, Allegience)")),
                ("use_wield_requirements", new Property<bool>(true, "disable this to bypass wield requirements. mostly for dev debugging")),
                ("version_info_enabled", new Property<bool>(false, "toggles the /aceversion player command")),
                ("vendor_shop_uses_generator", new Property<bool>(false, "enables or disables vendors using generator system in addition to createlist to create artificial scarcity")),
                ("world_closed", new Property<bool>(false, "enable this to startup world as a closed to players world")),
                ("allow_xp_at_max_level", new Property<bool>(false, "enable this to allow players to continue earning xp after reaching max level")),
                ("block_vpn_connections", new Property<bool>(false, "enable this to block user sessions from IPs identified as VPN proxies")),
                ("increase_minimum_encounter_spawn_density", new Property<bool>(false, "enable this to increase the density of random encounters that spawn in low density landblocks")),
                ("command_who_enabled", new Property<bool>(true, "disable this to prevent players from listing online players in their allegiance")),
                ("enforce_player_movement", new Property<bool>(false, "enable this to enforce server side verification of player movement")),
                ("enforce_player_movement_speed", new Property<bool>(false, "enable this to enforce server side verification of player movement speed")),
                ("enforce_player_movement_kick", new Property<bool>(false, "enable this to kick players that fail movement verification too frenquently")),
                ("useable_gems", new Property<bool>(true, "Allows loot generated gems to be used to cast their spells")),
                ("allow_PKs_to_go_NPK", new Property<bool>(true, "Allows PKs to go back to being NPKs by using the appropriate altar")),
                ("allow_multiple_accounts_hc", new Property<bool>(false, "Toggles whether multiple accounts are allowed in Hardcore mode")),
                ("show_discord_chat_ingame", new Property<bool>(false, "Display messages posted to Discord in general chat")),
                ("assess_creature_pve_always_succeed", new(false, "enable this to bypass assess creature PvE skill checks (workaround to fix 5 second delay on vtank looting which occurs due to failed assess)")),
                ("relive_bonus_applies_to_received_fellow_xp", new(true, "Toggles whether incoming xp received from fellowship members benefits from the relive bonus.")),
                ("fall_damage_enabled", new(true, "Toggles whether fall damage is enabled")),
                ("dekaru_dual_wield_speed_mod", new(true, "Toggles whether Dekaru's dual wield speed changes (other than for dagger) are enabled")),
                ("dekaru_hc_keep_non_equippable_bonded_on_death", new(true, "Toggles whether bonded items are kept on a hardcore death despite being non-equippable")),
                ("vendor_allow_special_mutations", new(true, "Toggles whether items on vendors can have special mutations like slayer, critical strike, etc.")),
                ("customdm_mutate_quest_items", new(false, "Toggles whether quest item mutations are enabled")),
                ("allow_allegiance_passup", new(true, "Toggles whether allegiance passup is enabled")),
                ("allow_skill_specialization", new(true, "Toggles whether skill specialization is allowed")),
                ("usable_gems_generated_with_1_mana_cost", new(false, "Toggles whether usable gems are generated with a cost of 1 mana, for virtually unlimited use, instead of the normal amount. Gems should cost at least 1 mana to mitigate any compatibility issues with plugins or other logic")),
                ("neuter_trade_note_rewards", new(false, "If enabled, trade note quest rewards will be reduced to a single I note")),
                ("stackable_trophy_rewards_use_tar", new(false, "If enabled, stackable quest rewards from trophies will be modified by TAR, scaling from 120% to 20%")),
                ("drop_all_coins_on_death", new(false, "If enabled, all coins will drop on death instead of half")),
                ("dekarutide_season3_alternate_weapon_wield_reqs", new(false, "If enabled, use Dekarutide''s Alternate Weapon Wield Requirements formula for Season 3")),
                ("dekarutide_season3_alternate_loot_valuations", new(false, "If enabled, use Dekarutide''s Alternate Loot Valuation formula for Season 3")),
                ("ai_anti_perch", new(true, "If enabled, use Dekaru''s anti-perch AI")),
                ("ai_custom_pathfind", new(true, "If enabled, use custom pathfinding AI")),
                ("die_command_enabled", new(true, "If disabled, prevents the use of /die")),
                ("use_fixed_death_item_formula", new(false, "If enabled, a fixed number of items will be dropped on death instead of variable with level. The max_items_dropped_per_death property will be used for this.")),
                ("allow_custom_gameplay_modes", new Property<bool>(true, "CustomDM: Allow creation of new characters using gameplay modes such as hardcore and solo self-found")),
                ("hardcore_death_keep_bonded", new Property<bool>(false, "Allow hardcore characters to keep their bonded equipment on death")),
                ("hardcore_death_keep_spells", new Property<bool>(false, "Allow hardcore characters to keep their spells on death")),
                ("hardcore_death_keep_housing", new Property<bool>(false, "Allow hardcore characters to keep their housing on death.")),
                ("hardcore_death_keep_allegiance", new Property<bool>(false, "Allow hardcore characters to keep their allegiance on death")),
                ("hardcore_pk_grant_ring_of_impermanency", new Property<bool>(false, "Give new hardcore PKs a ring that buffs their run and jump skills that they can wear up to level 20")),
                ("pathfinding", new Property<bool>(false, "CustomDM: Allows creatures to use pathfinding to navigate dungeons")),
                ("cmd_pop_show_current", new Property<bool>(true, "Allow the pop command to show current online population count")),
                ("cmd_pop_show_24_hours", new Property<bool>(true, "Allow the pop command to show the 24 hours unique IPs count")),
                ("cmd_pop_show_7_days", new Property<bool>(true, "Allow the pop command to show the 7 days unique IPs count")),
                ("cmd_pop_show_30_days", new Property<bool>(true, "Allow the pop command to show the 30 days unique IPs count")),
                ("recall_in_dungeon", new Property<bool>(true, "Allow players to recall in dungeon")),
                ("unlimited_respec", new Property<bool>(false, "Allow players to respec their skills/attributes without a quest timer")),
                ("recall_warden", new Property<bool>(false, "Toggles the Anti-Recall_Warden")),
                ("pve_death_respite", new Property<bool>(false, "Toggles pk respite even for pve deaths (players become npk even from pve deaths)")),
                ("disable_arenas", new Property<bool>(false, "set to true to disable arena events")),
                ("arena_allow_same_ip_match", new Property<bool>(false, "enable this allow two characters connected from the same IP to be matched in an arena event")),
                ("arena_allow_observers", new Property<bool>(true, "enable this to allow players to watch arena matches as invisible observers")),
                ("broadcast_player_life", new Property<bool>(false, "enable this to braodcast player life messages such as player creation/deletion/restoration")),
                ("broadcast_player_delete", new Property<bool>(false, "enable this to braodcast player deletion actions")),
                ("broadcast_player_create", new Property<bool>(false, "enable this to braodcast player creation actions")),
                ("broadcast_player_restore", new Property<bool>(false, "enable this to braodcast player restoration actions")),
                ("bz_whispers_enabled", new Property<bool>(true, "CustomDM: Enables/Disables whispers from Bael'Zharon revealing the location of other PK players")),

                // Do not edit below this line
                ("null_bool", new(false, "No effect, just included here as a last item on the list to prevent related lines from being changed in git upon new property additions."))
                );

        public static readonly ReadOnlyDictionary<string, Property<long>> DefaultLongProperties =
            DictOf(
                ("char_delete_time", new Property<long>(3600, "the amount of time in seconds a deleted character can be restored")),
                ("chat_requires_account_time_seconds", new Property<long>(0, "the amount of time in seconds an account is required to have existed for for global chat privileges")),
                ("chat_requires_player_age", new Property<long>(0, "the amount of time in seconds a player is required to have played for global chat privileges")),
                ("chat_requires_player_level", new Property<long>(0, "the level a player is required to have for global chat privileges")),
                ("corpse_spam_limit", new Property<long>(15, "the number of corpses a player is allowed to leave on a landblock at one time")),
                ("default_subscription_level", new Property<long>(1, "retail defaults to 1, 1 = standard subscription (same as 2 and 3), 4 grants ToD pre-order bonus item Asheron's Benediction")),
                ("fellowship_even_share_level", new Property<long>(50, "level when fellowship XP sharing is no longer restricted")),
                ("mansion_min_rank", new Property<long>(6, "overrides the default allegiance rank required to own a mansion")),
                ("max_chars_per_account", new Property<long>(11, "retail defaults to 11, client supports up to 20")),
                ("pk_timer", new Property<long>(20, "the number of seconds where a player cannot perform certain actions (ie. teleporting) after becoming involved in a PK battle")),
                ("player_save_interval", new Property<long>(300, "the number of seconds between automatic player saves")),
                ("rares_max_days_between", new Property<long>(45, "for rares_real_time_v2: the maximum number of days a player can go before a rare is generated on rare eligible creature kills")),
                ("rares_max_seconds_between", new Property<long>(5256000, "for rares_real_time: the maximum number of seconds a player can go before a second chance at a rare is allowed on rare eligible creature kills that did not generate a rare")),
                ("summoning_killtask_multicredit_cap", new Property<long>(2, "if allow_summoning_killtask_multicredit is enabled, the maximum # of killtask credits a player can receive from 1 kill")),
                ("teleport_visibility_fix", new Property<long>(0, "Fixes some possible issues with invisible players and mobs. 0 = default / disabled, 1 = players only, 2 = creatures, 3 = all world objects")),
                ("max_level", new Property<long>(275, "Set the max character level.")),
                ("discord_channel_id", new Property<long>(0, "Messages posted to this Discord channel will be shown in General Chat")),
                ("quest_mindelta_rate_shortest", new Property<long>(72000, "Quest min deltas below this won't be affected by quest_mindelta_rate, additionally modified min deltas that would fall under this value will be set to this value instead.")),
                ("quest_mindelta_rate_longest", new(600000, "Quest min deltas above this will be set to this value instead.")),
                ("dekaru_imbue_magic_defense_per_imbue", new(3, "Number of magic defense points to increase per magic defense imbue on an item.")),
                ("dekaru_imbue_melee_defense_per_imbue", new(3, "Number of melee defense points to increase per magic defense imbue on an item.")),
                ("dekaru_imbue_missile_defense_per_imbue", new(3, "Number of missile defense points to increase per magic defense imbue on an item.")),
                ("elite_mob_loot_count", new(20, "Number of random items on an elite corpse.")),
                ("min_level_drop_wielded_on_death", new(35, "Minimum character level before wielded items may drop on player death.")),
                ("min_level_eligible_to_drop_items_on_death", new(11, "Minimum character level before items may drop on player death.")),
                ("pk_escape_max_level_difference", new(10, "The maximum level difference, in either direction, where a player may fast escape from another player in pvp. This includes logouts, portals, and recalls.")),
                ("bz_snitch_level_difference", new(10, "The maximum level difference, in either direction, where a player may receive a bz snitch (location reveal). Doesn't affect hardcore mode.")),
                ("max_items_dropped_per_death", new(Player.MaxItemsDropped, "The maximum number of items dropped on death. This is not a simple cap on death items. If changed from the default, the number of actual items dropped per death will be scaled lower or higher depending on the proportion of this versus the default.")),
                ("unlimited_respec_max_level", new(20, "The maximum level you can be to qualify for unlimited respec skills/attributes without a quest timer.")),
                ("arenas_min_level", new Property<long>(25, "the minimum level required to join an arena queue")),
                ("arenas_reward_min_level", new Property<long>(25, "the minimum level required to get arena rewards")),
                ("arenas_reward_min_age", new Property<long>(864000, "the minimum in-game age in seconds required to get arena rewards")),
                ("player_corpse_permissible_age", new Property<long>(7200, "the amount of time in milliseconds before a player's corpse becomes lootable by anyone. Default is 2 hours")),
                ("previous_max_level", new Property<long>(0, "Set the previously set max character level.")),
                ("bz_whispers_min_pop", new Property<long>(5, "CustomDM: Minimum required online PK players for bz whispers to be sent")),
                ("bz_whispers_login_delay", new Property<long>(3600, "CustomDM: How long a player must remain online before being able to receive a bz whisper")),
                ("bz_whispers_interval", new Property<long>(600, "CustomDM: How often a player can receive a bz whisper")),


                // Do not edit below this line
                ("null_long", new(0, "No effect, just included here as a last item on the list to prevent related lines from being changed in git upon new property additions."))
                );

        public static readonly ReadOnlyDictionary<string, Property<double>> DefaultDoubleProperties =
            DictOf(

                ("cantrip_drop_rate", new Property<double>(1.0, "Scales the chance for cantrips to drop in each tier. Defaults to 1.0, as per end of retail")),
                ("cloak_cooldown_seconds", new Property<double>(5.0, "The number of seconds between possible cloak procs.")),
                ("cloak_max_proc_base", new Property<double>(0.25, "The max proc chance of a cloak.")),
                ("cloak_max_proc_damage_percentage", new Property<double>(0.30, "The damage percentage at which cloak proc chance plateaus.")),
                ("cloak_min_proc", new Property<double>(0, "The minimum proc chance of a cloak.")),
                ("minor_cantrip_drop_rate", new Property<double>(1.0, "Scales the chance for minor cantrips to drop, relative to other cantrip levels in the tier. Defaults to 1.0, as per end of retail")),
                ("major_cantrip_drop_rate", new Property<double>(1.0, "Scales the chance for major cantrips to drop, relative to other cantrip levels in the tier. Defaults to 1.0, as per end of retail")),
                ("epic_cantrip_drop_rate", new Property<double>(1.0, "Scales the chance for epic cantrips to drop, relative to other cantrip levels in the tier. Defaults to 1.0, as per end of retail")),
                ("legendary_cantrip_drop_rate", new Property<double>(1.0, "Scales the chance for legendary cantrips to drop, relative to other cantrip levels in the tier. Defaults to 1.0, as per end of retail")),

                ("advocate_fane_auto_bestow_level", new Property<double>(1, "the level that advocates are automatically bestowed by Advocate Fane if advocate_fane_auto_bestow is true")),
                ("aetheria_drop_rate", new Property<double>(1.0, "Modifier for Aetheria drop rate, 1 being normal")),
                ("chess_ai_start_time", new Property<double>(-1.0, "the number of seconds for the chess ai to start. defaults to -1 (disabled)")),
                ("encounter_delay", new Property<double>(1800, "the number of seconds a generator profile for regions is delayed from returning to free slots")),
                ("encounter_regen_interval", new Property<double>(600, "the number of seconds a generator for regions at which spawns its next set of objects")),
                ("equipmentsetid_drop_rate", new Property<double>(1.0, "Modifier for EquipmentSetID drop rate, 1 being normal")),
                ("fast_missile_modifier", new Property<double>(1.2, "The speed multiplier applied to fast missiles. Defaults to retail value of 1.2")),
                ("ignore_magic_armor_pvp_scalar", new Property<double>(1.0, "Scales the effectiveness of IgnoreMagicArmor (ie. hollow weapons) in pvp battles. 1.0 = full effectiveness / ignore all enchantments on armor (default), 0.5 = half effectiveness / use half enchantments from armor, 0.0 = no effectiveness / use full enchantments from armor")),
                ("ignore_magic_resist_pvp_scalar", new Property<double>(1.0, "Scales the effectiveness of IgnoreMagicResist (ie. hollow weapons) in pvp battles. 1.0 = full effectiveness / ignore all resistances from life enchantments (default), 0.5 = half effectiveness / use half resistances from life enchantments, 0.0 = no effectiveness / use full resistances from life enchantments")),
                ("luminance_modifier", new Property<double>(1.0, "Scales the amount of luminance received by players")),
                ("melee_max_angle", new Property<double>(0.0, "for melee players, the maximum angle before a TurnTo is required. retail appeared to have required a TurnTo even for the smallest of angle offsets.")),
                ("mob_awareness_range", new Property<double>(1.0, "Scales the distance the monsters become alerted and aggro the players")),
                ("mob_awareness_range_indoors", new Property<double>(1.0, "Scales the distance the monsters become alerted and aggro the players inside dungeons")),
                ("pk_new_character_grace_period", new Property<double>(300, "the number of seconds, in addition to pk_respite_timer, that a player killer is set to non-player killer status after first exiting training academy")),
                ("pk_respite_timer", new Property<double>(300, "the number of seconds that a player killer is set to non-player killer status after dying to another player killer")),
                ("quest_lum_modifier", new Property<double>(1.0, "Scale multiplier for amount of quest luminance received by players.  Quest lum is also modified by 'luminance_modifier'.")),
                ("quest_mindelta_rate", new Property<double>(1.0, "scales all quest min delta time between solves, 1 being normal")),
                ("quest_xp_modifier", new Property<double>(1.0, "Scale multiplier for amount of quest XP received by players.  Quest XP is also modified by 'xp_modifier'.")),
                ("rare_drop_rate_percent", new Property<double>(0.04, "Adjust the chance of a rare to spawn as a percentage. Default is 0.04, or 1 in 2,500. Max is 100, or every eligible drop.")),
                ("spellcast_max_angle", new Property<double>(20.0, "for advanced player spell casting, the maximum angle to target release a spell projectile. retail seemed to default to value of around 20, although some players seem to prefer a higher 45 degree angle")),
                ("trophy_drop_rate", new Property<double>(1.0, "Modifier for trophies dropped on creature death")),
                ("unlocker_window", new Property<double>(10.0, "The number of seconds a player unlocking a chest has exclusive access to first opening the chest.")),
                ("vendor_unique_rot_time", new Property<double>(300, "the number of seconds before unique items sold to vendors disappear")),
                ("vitae_penalty", new Property<double>(0.05, "the amount of vitae penalty a player gets per death")),
                ("vitae_penalty_max", new Property<double>(0.40, "the maximum vitae penalty a player can have")),
                ("void_pvp_modifier", new Property<double>(0.5, "Scales the amount of damage players take from Void Magic. Defaults to 0.5, as per retail. For earlier content where DRR isn't as readily available, this can be adjusted for balance.")),
                ("xp_modifier", new Property<double>(1.0, "Globally scales the amount of xp received by players, note that this multiplies the other xp_modifier options.")),
                ("xp_modifier_kill_tier1", new Property<double>(1.0, "Scales the amount of xp received by players for killing tier 1 creatures or unspecified tier creatures below level 28.")),
                ("xp_modifier_kill_tier2", new Property<double>(1.0, "Scales the amount of xp received by players for killing tier 2 creatures or unspecified tier creatures between level 28 and level 65.")),
                ("xp_modifier_kill_tier3", new Property<double>(1.0, "Scales the amount of xp received by players for killing tier 3 creatures or unspecified tier creatures between level 65 and level 95.")),
                ("xp_modifier_kill_tier4", new Property<double>(1.0, "Scales the amount of xp received by players for killing tier 4 creatures or unspecified tier creatures between level 95 and level 110.")),
                ("xp_modifier_kill_tier5", new Property<double>(1.0, "Scales the amount of xp received by players for killing tier 5 creatures or unspecified tier creatures between level 110 and level 135.")),
                ("xp_modifier_kill_tier6", new Property<double>(1.0, "Scales the amount of xp received by players for killing tier 6 creatures or unspecified tier creatures above level 135.")),
                ("xp_modifier_reward_tier1", new Property<double>(1.0, "Scales the amount of xp received by players for completing tier 1 quests or unspecified level quests while being under level 16.")),
                ("xp_modifier_reward_tier2", new Property<double>(1.0, "Scales the amount of xp received by players for completing tier 2 quests or unspecified level quests while being between level 16 and 36.")),
                ("xp_modifier_reward_tier3", new Property<double>(1.0, "Scales the amount of xp received by players for completing tier 3 quests or unspecified level quests while being between level 36 and 56.")),
                ("xp_modifier_reward_tier4", new Property<double>(1.0, "Scales the amount of xp received by players for completing tier 4 quests or unspecified level quests while being between level 56 and 76.")),
                ("xp_modifier_reward_tier5", new Property<double>(1.0, "Scales the amount of xp received by players for completing tier 5 quests or unspecified level quests while being between level 76 and 96.")),
                ("xp_modifier_reward_tier6", new Property<double>(1.0, "Scales the amount of xp received by players for completing tier 6 quests or unspecified level quests while being over level 96.")),
                ("salvage_amount_multiplier", new Property<double>(1.0, "Scales the amount of salvage a player gets from items.")),

                // Dagger Attack Speed Modifiers (PvE and PvP)
                ("dekaru_dagger_ms_animation_speed_1h", new(1.8, "Multiplier for dagger attack animation speed, if one handed with no shield (with a shield is hard-coded to 1.0).")),
                ("dekaru_dagger_ms_animation_speed_dualwield", new(1.8, "Multiplier for dagger attack animation speed, if dual wielding.")),
                ("dekaru_dagger_ms_animation_speed_shielded", new(1.8, "Multiplier for dagger attack animation speed, if using a shield.")),

                // Sword Attack Speed Modifiers (PvE and PvP)
                ("dekaru_sword_ms_animation_speed_1h", new(1.8, "Multiplier for sword attack animation speed, if one handed with no shield (with a shield is hard-coded to 1.0).")),
                ("dekaru_sword_ms_animation_speed_dualwield", new(1.8, "Multiplier for sword attack animation speed, if dual wielding.")),
                ("dekaru_sword_ms_animation_speed_shielded", new(1.8, "Multiplier for sword attack animation speed, if using a shield.")),

                // Dagger bleed mod in pve
                ("bleed_pve_dmg_mod", new Property<double>(1.0, "Damage mod for dagger bleed in PvE")),

                // Thrown Weapon Attack Speed Modifier
                ("dekaru_tw_animation_speed", new(1.0, "Multiplier for thrown weapon attack animation speed")),

                // PvP Damage modifiers
                //
                // For CustomDM:
                // Axe = Axe and Mace
                // Spear = Spear and Staff
                // Bow = Bow and Crossbow
                //
                // For EoR:
                // Axe = Light Weapons
                // Sword = Heavy Weapons
                // Dagger = Finesse Weapons
                // Bow = Missile Weapons
                //
                ("pvp_dmg_mod_low_level", new Property<double>(10.0, "Reference low level.")),
                ("pvp_dmg_mod_high_level", new Property<double>(80.0, "Reference high level.")),

                ("pvp_dmg_mod_low", new Property<double>(1.0, "Scales damage.")),
                ("pvp_dmg_mod_low_axe", new Property<double>(1.0, "Scales axe damage.")),
                ("pvp_dmg_mod_low_dagger", new Property<double>(1.0, "Scales dagger damage.")),
                ("pvp_dmg_mod_low_mace", new Property<double>(1.0, "Scales mace damage.")),
                ("pvp_dmg_mod_low_spear", new Property<double>(1.0, "Scales spear damage.")),
                ("pvp_dmg_mod_low_staff", new Property<double>(1.0, "Scales staff damage.")),
                ("pvp_dmg_mod_low_sword", new Property<double>(1.0, "Scales sword damage.")),
                ("pvp_dmg_mod_low_unarmed", new Property<double>(1.0, "Scales unarmed combat damage.")),
                ("pvp_dmg_mod_low_unarmed_war", new Property<double>(1.0, "Scales unarmed combat war magic spell damage.")),

                ("pvp_dmg_mod_low_bow", new Property<double>(1.0, "Scales bow damage.")),
                ("pvp_dmg_mod_low_crossbow", new Property<double>(1.0, "Scales crossbow damage.")),
                ("pvp_dmg_mod_low_thrown", new Property<double>(1.0, "Scales thrown weapons damage.")),

                ("pvp_dmg_mod_low_war", new Property<double>(1.0, "Scales war magic spell(not including streaks) damage.")),
                ("pvp_dmg_mod_low_void", new Property<double>(1.0, "Scales void magic spell(not including streaks and DoTs) damage.")),

                ("pvp_dmg_mod_low_war_streak", new Property<double>(1.0, "Scales war magic streaks damage.")),
                ("pvp_dmg_mod_low_void_streak", new Property<double>(1.0, "Scales void magic streaks damage.")),

                ("pvp_dmg_mod_low_dot", new Property<double>(1.0, "Scales damage over time damage.")),
                ("pvp_dmg_mod_low_void_dot", new Property<double>(1.0, "Scales void magic damage over time damage.")),

                // PvP Crippling Blow and Crushing Blow modifiers
                ("pvp_dmg_mod_low_crit_dmg", new Property<double>(1.0, "Scales Crippling Blow and Crushing Blow damage.")),
                ("pvp_dmg_mod_low_axe_crit_dmg", new Property<double>(1.0, "Scales axe Crippling Blow and Crushing Blow damage.")),
                ("pvp_dmg_mod_low_dagger_crit_dmg", new Property<double>(1.0, "Scales dagger Crippling Blow and Crushing Blow damage.")),
                ("pvp_dmg_mod_low_mace_crit_dmg", new Property<double>(1.0, "Scales mace Crippling Blow and Crushing Blow damage.")),
                ("pvp_dmg_mod_low_spear_crit_dmg", new Property<double>(1.0, "Scales spear Crippling Blow and Crushing Blow damage.")),
                ("pvp_dmg_mod_low_staff_crit_dmg", new Property<double>(1.0, "Scales staff Crippling Blow and Crushing Blow damage.")),
                ("pvp_dmg_mod_low_sword_crit_dmg", new Property<double>(1.0, "Scales sword Crippling Blow and Crushing Blow damage.")),
                ("pvp_dmg_mod_low_unarmed_crit_dmg", new Property<double>(1.0, "Scales unarmed combat Crippling Blow and Crushing Blow damage.")),

                ("pvp_dmg_mod_low_bow_crit_dmg", new Property<double>(1.0, "Scales bow Crippling Blow and Crushing Blow damage.")),
                ("pvp_dmg_mod_low_crossbow_crit_dmg", new Property<double>(1.0, "Scales crossbow Crippling Blow and Crushing Blow damage.")),
                ("pvp_dmg_mod_low_thrown_crit_dmg", new Property<double>(1.0, "Scales thrown weapons Crippling Blow and Crushing Blow damage.")),

                ("pvp_dmg_mod_low_war_crit_dmg", new Property<double>(1.0, "Scales war magic Crippling Blow and Crushing Blow damage.")),
                ("pvp_dmg_mod_low_life_crit_dmg", new Property<double>(1.0, "Scales life magic Crippling Blow and Crushing Blow damage.")),
                ("pvp_dmg_mod_low_void_crit_dmg", new Property<double>(1.0, "Scales void magic Crippling Blow and Crushing Blow damage.")),

                // PvP Critical Strike and Biting Strike modifiers
                ("pvp_dmg_mod_low_crit_chance", new Property<double>(1.0, "Scales Critical Strike and Biting Strike critical hit chance.")),
                ("pvp_dmg_mod_low_axe_crit_chance", new Property<double>(1.0, "Scales axe Critical Strike and Biting Strike critical hit chance.")),
                ("pvp_dmg_mod_low_dagger_crit_chance", new Property<double>(1.0, "Scales dagger Critical Strike and Biting Strike critical hit chance.")),
                ("pvp_dmg_mod_low_mace_crit_chance", new Property<double>(1.0, "Scales mace Critical Strike and Biting Strike critical hit chance.")),
                ("pvp_dmg_mod_low_spear_crit_chance", new Property<double>(1.0, "Scales spear Critical Strike and Biting Strike critical hit chance.")),
                ("pvp_dmg_mod_low_staff_crit_chance", new Property<double>(1.0, "Scales staff Critical Strike and Biting Strike critical hit chance.")),
                ("pvp_dmg_mod_low_sword_crit_chance", new Property<double>(1.0, "Scales sword Critical Strike and Biting Strike critical hit chance.")),
                ("pvp_dmg_mod_low_unarmed_crit_chance", new Property<double>(1.0, "Scales unarmed combat Critical Strike and Biting Strike critical hit chance.")),

                ("pvp_dmg_mod_low_bow_crit_chance", new Property<double>(1.0, "Scales bow Critical Strike and Biting Strike critical hit chance.")),
                ("pvp_dmg_mod_low_crossbow_crit_chance", new Property<double>(1.0, "Scales crossbow Critical Strike and Biting Strike critical hit chance.")),
                ("pvp_dmg_mod_low_thrown_crit_chance", new Property<double>(1.0, "Scales thrown weapons Critical Strike and Biting Strike critical hit chance.")),

                ("pvp_dmg_mod_low_war_crit_chance", new Property<double>(1.0, "Scales war magic Critical Strike and Biting Strike critical hit chance.")),
                ("pvp_dmg_mod_low_life_crit_chance", new Property<double>(1.0, "Scales life magic Critical Strike and Biting Strike critical hit chance.")),
                ("pvp_dmg_mod_low_void_crit_chance", new Property<double>(1.0, "Scales void magic Critical Strike and Biting Strike critical hit chance.")),

                // PvP Armor Rending and Armor Cleaving modifiers
                ("pvp_dmg_mod_low_armor_ignore", new Property<double>(1.0, "Scales Armor Rending and Armor Cleaving armor ignore ratio.")),
                ("pvp_dmg_mod_low_axe_armor_ignore", new Property<double>(1.0, "Scales axe Armor Rending and Armor Cleaving armor ignore ratio.")),
                ("pvp_dmg_mod_low_dagger_armor_ignore", new Property<double>(1.0, "Scales dagger Armor Rending and Armor Cleaving armor ignore ratio.")),
                ("pvp_dmg_mod_low_mace_armor_ignore", new Property<double>(1.0, "Scales mace Armor Rending and Armor Cleaving armor ignore ratio.")),
                ("pvp_dmg_mod_low_spear_armor_ignore", new Property<double>(1.0, "Scales spear Armor Rending and Armor Cleaving armor ignore ratio.")),
                ("pvp_dmg_mod_low_staff_armor_ignore", new Property<double>(1.0, "Scales staff Armor Rending and Armor Cleaving armor ignore ratio.")),
                ("pvp_dmg_mod_low_sword_armor_ignore", new Property<double>(1.0, "Scales sword Armor Rending and Armor Cleaving armor ignore ratio.")),
                ("pvp_dmg_mod_low_unarmed_armor_ignore", new Property<double>(1.0, "Scales unarmed combat Armor Rending and Armor Cleaving armor ignore ratio.")),

                ("pvp_dmg_mod_low_bow_armor_ignore", new Property<double>(1.0, "Scales bow Armor Rending and Armor Cleaving armor ignore ratio.")),
                ("pvp_dmg_mod_low_crossbow_armor_ignore", new Property<double>(1.0, "Scales crossbow Armor Rending and Armor Cleaving armor ignore ratio.")),
                ("pvp_dmg_mod_low_thrown_armor_ignore", new Property<double>(1.0, "Scales thrown weapons Armor Rending and Armor Cleaving armor ignore ratio.")),

                // PvP Hollow damage modifiers
                ("pvp_dmg_mod_low_hollow", new Property<double>(1.0, "Scales hollow weapon damage.")),
                ("pvp_dmg_mod_low_axe_hollow", new Property<double>(1.0, "Scales hollow axe damage.")),
                ("pvp_dmg_mod_low_dagger_hollow", new Property<double>(1.0, "Scales hollow dagger damage.")),
                ("pvp_dmg_mod_low_mace_hollow", new Property<double>(1.0, "Scales hollow mace damage.")),
                ("pvp_dmg_mod_low_spear_hollow", new Property<double>(1.0, "Scales hollow spear damage.")),
                ("pvp_dmg_mod_low_staff_hollow", new Property<double>(1.0, "Scales hollow staff damage.")),
                ("pvp_dmg_mod_low_sword_hollow", new Property<double>(1.0, "Scales hollow sword damage.")),
                ("pvp_dmg_mod_low_unarmed_hollow", new Property<double>(1.0, "Scales hollow unarmed combat damage.")),

                ("pvp_dmg_mod_low_bow_hollow", new Property<double>(1.0, "Scales hollow bow damage.")),
                ("pvp_dmg_mod_low_crossbow_hollow", new Property<double>(1.0, "Scales hollow crossbow damage.")),
                ("pvp_dmg_mod_low_thrown_hollow", new Property<double>(1.0, "Scales hollow thrown weapons damage.")),

                // PvP Phantom damage modifiers
                ("pvp_dmg_mod_low_phantom", new Property<double>(1.0, "Scales phantom weapon damage.")),
                ("pvp_dmg_mod_low_axe_phantom", new Property<double>(1.0, "Scales phantom axe damage.")),
                ("pvp_dmg_mod_low_dagger_phantom", new Property<double>(1.0, "Scales phantom dagger damage.")),
                ("pvp_dmg_mod_low_mace_phantom", new Property<double>(1.0, "Scales phantom mace damage.")),
                ("pvp_dmg_mod_low_spear_phantom", new Property<double>(1.0, "Scales phantom spear damage.")),
                ("pvp_dmg_mod_low_staff_phantom", new Property<double>(1.0, "Scales phantom staff damage.")),
                ("pvp_dmg_mod_low_sword_phantom", new Property<double>(1.0, "Scales phantom sword damage.")),
                ("pvp_dmg_mod_low_unarmed_phantom", new Property<double>(1.0, "Scales phantom unarmed combat damage.")),

                ("pvp_dmg_mod_low_bow_phantom", new Property<double>(1.0, "Scales phantom bow damage.")),
                ("pvp_dmg_mod_low_crossbow_phantom", new Property<double>(1.0, "Scales phantom crossbow damage.")),
                ("pvp_dmg_mod_low_thrown_phantom", new Property<double>(1.0, "Scales phantom thrown weapon damage.")),

                // PvP Miscellaneous modifiers
                ("pvp_dmg_mod_low_shieldcleave", new Property<double>(1.0, "Scales the Shield Cleave amount.")),
                ("pvp_dmg_mod_low_2h_shieldcleave", new Property<double>(1.0, "Scales the Shield Cleave amount for two-handed weapons.")),

                ("pvp_dmg_mod_low_armor_level", new Property<double>(1.0, "Scales the base armor level.")),
                ("pvp_dmg_mod_low_shield_level", new Property<double>(1.0, "Scales the base shield level.")),
                ("pvp_dmg_mod_low_shield_block_chance", new Property<double>(1.0, "Scales the base shield block chance.")),

                ("pvp_dmg_mod_low_sneak", new Property<double>(1.0, "Scales the sneak attack damage multiplier.")),

                ("pvp_dmg_mod_high", new Property<double>(1.0, "Scales damage.")),
                ("pvp_dmg_mod_high_axe", new Property<double>(1.0, "Scales axe damage.")),
                ("pvp_dmg_mod_high_dagger", new Property<double>(1.0, "Scales dagger damage.")),
                ("pvp_dmg_mod_high_mace", new Property<double>(1.0, "Scales mace damage.")),
                ("pvp_dmg_mod_high_spear", new Property<double>(1.0, "Scales spear damage.")),
                ("pvp_dmg_mod_high_staff", new Property<double>(1.0, "Scales staff damage.")),
                ("pvp_dmg_mod_high_sword", new Property<double>(1.0, "Scales sword damage.")),
                ("pvp_dmg_mod_high_unarmed", new Property<double>(1.0, "Scales unarmed combat damage.")),
                ("pvp_dmg_mod_high_unarmed_war", new Property<double>(1.0, "Scales unamed combat war magic spell damage.")),

                ("pvp_dmg_mod_high_bow", new Property<double>(1.0, "Scales bow damage.")),
                ("pvp_dmg_mod_high_crossbow", new Property<double>(1.0, "Scales crossbow damage.")),
                ("pvp_dmg_mod_high_thrown", new Property<double>(1.0, "Scales thrown weapons damage.")),

                ("pvp_dmg_mod_high_war", new Property<double>(1.0, "Scales war magic spell(not including streaks) damage.")),
                ("pvp_dmg_mod_high_void", new Property<double>(1.0, "Scales void magic spell(not including streaks and DoTs) damage.")),

                ("pvp_dmg_mod_high_war_streak", new Property<double>(1.0, "Scales war magic streaks damage.")),
                ("pvp_dmg_mod_high_void_streak", new Property<double>(1.0, "Scales void magic streaks damage.")),

                ("pvp_dmg_mod_high_dot", new Property<double>(1.0, "Scales damage over time damage.")),
                ("pvp_dmg_mod_high_void_dot", new Property<double>(1.0, "Scales void magic damage over time damage.")),

                // PvP Crippling Blow and Crushing Blow modifiers
                ("pvp_dmg_mod_high_crit_dmg", new Property<double>(1.0, "Scales Crippling Blow and Crushing Blow damage.")),
                ("pvp_dmg_mod_high_axe_crit_dmg", new Property<double>(1.0, "Scales axe Crippling Blow and Crushing Blow damage.")),
                ("pvp_dmg_mod_high_dagger_crit_dmg", new Property<double>(1.0, "Scales dagger Crippling Blow and Crushing Blow damage.")),
                ("pvp_dmg_mod_high_mace_crit_dmg", new Property<double>(1.0, "Scales mace Crippling Blow and Crushing Blow damage.")),
                ("pvp_dmg_mod_high_spear_crit_dmg", new Property<double>(1.0, "Scales spear Crippling Blow and Crushing Blow damage.")),
                ("pvp_dmg_mod_high_staff_crit_dmg", new Property<double>(1.0, "Scales staff Crippling Blow and Crushing Blow damage.")),
                ("pvp_dmg_mod_high_sword_crit_dmg", new Property<double>(1.0, "Scales sword Crippling Blow and Crushing Blow damage.")),
                ("pvp_dmg_mod_high_unarmed_crit_dmg", new Property<double>(1.0, "Scales unarmed combat Crippling Blow and Crushing Blow damage.")),

                ("pvp_dmg_mod_high_bow_crit_dmg", new Property<double>(1.0, "Scales bow Crippling Blow and Crushing Blow damage.")),
                ("pvp_dmg_mod_high_crossbow_crit_dmg", new Property<double>(1.0, "Scales crossbow Crippling Blow and Crushing Blow damage.")),
                ("pvp_dmg_mod_high_thrown_crit_dmg", new Property<double>(1.0, "Scales thrown weapons Crippling Blow and Crushing Blow damage.")),

                ("pvp_dmg_mod_high_war_crit_dmg", new Property<double>(1.0, "Scales war magic Crippling Blow and Crushing Blow damage.")),
                ("pvp_dmg_mod_high_life_crit_dmg", new Property<double>(1.0, "Scales life magic Crippling Blow and Crushing Blow damage.")),
                ("pvp_dmg_mod_high_void_crit_dmg", new Property<double>(1.0, "Scales void magic Crippling Blow and Crushing Blow damage.")),

                // PvP Critical Strike and Biting Strike modifiers
                ("pvp_dmg_mod_high_crit_chance", new Property<double>(1.0, "Scales Critical Strike and Biting Strike critical hit chance.")),
                ("pvp_dmg_mod_high_axe_crit_chance", new Property<double>(1.0, "Scales axe Critical Strike and Biting Strike critical hit chance.")),
                ("pvp_dmg_mod_high_dagger_crit_chance", new Property<double>(1.0, "Scales dagger Critical Strike and Biting Strike critical hit chance.")),
                ("pvp_dmg_mod_high_mace_crit_chance", new Property<double>(1.0, "Scales mace Critical Strike and Biting Strike critical hit chance.")),
                ("pvp_dmg_mod_high_spear_crit_chance", new Property<double>(1.0, "Scales spear Critical Strike and Biting Strike critical hit chance.")),
                ("pvp_dmg_mod_high_staff_crit_chance", new Property<double>(1.0, "Scales staff Critical Strike and Biting Strike critical hit chance.")),
                ("pvp_dmg_mod_high_sword_crit_chance", new Property<double>(1.0, "Scales sword Critical Strike and Biting Strike critical hit chance.")),
                ("pvp_dmg_mod_high_unarmed_crit_chance", new Property<double>(1.0, "Scales unarmed combat Critical Strike and Biting Strike critical hit chance.")),

                ("pvp_dmg_mod_high_bow_crit_chance", new Property<double>(1.0, "Scales bow Critical Strike and Biting Strike critical hit chance.")),
                ("pvp_dmg_mod_high_crossbow_crit_chance", new Property<double>(1.0, "Scales crossbow Critical Strike and Biting Strike critical hit chance.")),
                ("pvp_dmg_mod_high_thrown_crit_chance", new Property<double>(1.0, "Scales thrown weapons Critical Strike and Biting Strike critical hit chance.")),

                ("pvp_dmg_mod_high_war_crit_chance", new Property<double>(1.0, "Scales war magic Critical Strike and Biting Strike critical hit chance.")),
                ("pvp_dmg_mod_high_life_crit_chance", new Property<double>(1.0, "Scales life magic Critical Strike and Biting Strike critical hit chance.")),
                ("pvp_dmg_mod_high_void_crit_chance", new Property<double>(1.0, "Scales void magic Critical Strike and Biting Strike critical hit chance.")),

                // PvP Armor Rending and Armor Cleaving modifiers
                ("pvp_dmg_mod_high_armor_ignore", new Property<double>(1.0, "Scales Armor Rending and Armor Cleaving armor ignore ratio.")),
                ("pvp_dmg_mod_high_axe_armor_ignore", new Property<double>(1.0, "Scales axe Armor Rending and Armor Cleaving armor ignore ratio.")),
                ("pvp_dmg_mod_high_dagger_armor_ignore", new Property<double>(1.0, "Scales dagger Armor Rending and Armor Cleaving armor ignore ratio.")),
                ("pvp_dmg_mod_high_mace_armor_ignore", new Property<double>(1.0, "Scales mace Armor Rending and Armor Cleaving armor ignore ratio.")),
                ("pvp_dmg_mod_high_spear_armor_ignore", new Property<double>(1.0, "Scales spear Armor Rending and Armor Cleaving armor ignore ratio.")),
                ("pvp_dmg_mod_high_staff_armor_ignore", new Property<double>(1.0, "Scales staff Armor Rending and Armor Cleaving armor ignore ratio.")),
                ("pvp_dmg_mod_high_sword_armor_ignore", new Property<double>(1.0, "Scales sword Armor Rending and Armor Cleaving armor ignore ratio.")),
                ("pvp_dmg_mod_high_unarmed_armor_ignore", new Property<double>(1.0, "Scales unarmed combat Armor Rending and Armor Cleaving armor ignore ratio.")),

                ("pvp_dmg_mod_high_bow_armor_ignore", new Property<double>(1.0, "Scales bow Armor Rending and Armor Cleaving armor ignore ratio.")),
                ("pvp_dmg_mod_high_crossbow_armor_ignore", new Property<double>(1.0, "Scales crossbow Armor Rending and Armor Cleaving armor ignore ratio.")),
                ("pvp_dmg_mod_high_thrown_armor_ignore", new Property<double>(1.0, "Scales thrown weapons Armor Rending and Armor Cleaving armor ignore ratio.")),

                // PvP Hollow damage modifiers
                ("pvp_dmg_mod_high_hollow", new Property<double>(1.0, "Scales hollow weapon damage.")),
                ("pvp_dmg_mod_high_axe_hollow", new Property<double>(1.0, "Scales hollow axe damage.")),
                ("pvp_dmg_mod_high_dagger_hollow", new Property<double>(1.0, "Scales hollow dagger damage.")),
                ("pvp_dmg_mod_high_mace_hollow", new Property<double>(1.0, "Scales hollow mace damage.")),
                ("pvp_dmg_mod_high_spear_hollow", new Property<double>(1.0, "Scales hollow spear damage.")),
                ("pvp_dmg_mod_high_staff_hollow", new Property<double>(1.0, "Scales hollow staff damage.")),
                ("pvp_dmg_mod_high_sword_hollow", new Property<double>(1.0, "Scales hollow sword damage.")),
                ("pvp_dmg_mod_high_unarmed_hollow", new Property<double>(1.0, "Scales hollow unarmed combat damage.")),

                ("pvp_dmg_mod_high_bow_hollow", new Property<double>(1.0, "Scales hollow bow damage.")),
                ("pvp_dmg_mod_high_crossbow_hollow", new Property<double>(1.0, "Scales hollow crossbow damage.")),
                ("pvp_dmg_mod_high_thrown_hollow", new Property<double>(1.0, "Scales hollow thrown weapons damage.")),

                // PvP Phantom damage modifiers
                ("pvp_dmg_mod_high_phantom", new Property<double>(1.0, "Scales phantom weapon damage.")),
                ("pvp_dmg_mod_high_axe_phantom", new Property<double>(1.0, "Scales phantom axe damage.")),
                ("pvp_dmg_mod_high_dagger_phantom", new Property<double>(1.0, "Scales phantom dagger damage.")),
                ("pvp_dmg_mod_high_mace_phantom", new Property<double>(1.0, "Scales phantom mace damage.")),
                ("pvp_dmg_mod_high_spear_phantom", new Property<double>(1.0, "Scales phantom spear damage.")),
                ("pvp_dmg_mod_high_staff_phantom", new Property<double>(1.0, "Scales phantom staff damage.")),
                ("pvp_dmg_mod_high_sword_phantom", new Property<double>(1.0, "Scales phantom sword damage.")),
                ("pvp_dmg_mod_high_unarmed_phantom", new Property<double>(1.0, "Scales phantom unarmed combat damage.")),

                ("pvp_dmg_mod_high_bow_phantom", new Property<double>(1.0, "Scales phantom bow damage.")),
                ("pvp_dmg_mod_high_crossbow_phantom", new Property<double>(1.0, "Scales phantom crossbow damage.")),
                ("pvp_dmg_mod_high_thrown_phantom", new Property<double>(1.0, "Scales phantom thrown weapon damage.")),

                // PvP Miscellaneous modifiers
                ("pvp_dmg_mod_high_shieldcleave", new Property<double>(1.0, "Scales the Shield Cleave amount.")),
                ("pvp_dmg_mod_high_2h_shieldcleave", new Property<double>(1.0, "Scales the Shield Cleave amount for two-handed weapons.")),

                ("pvp_dmg_mod_high_armor_level", new Property<double>(1.0, "Scales the base armor level.")),
                ("pvp_dmg_mod_high_shield_level", new Property<double>(1.0, "Scales the base shield level.")),
                ("pvp_dmg_mod_high_shield_block_chance", new Property<double>(1.0, "Scales the base shield block chance.")),

                ("pvp_dmg_mod_high_sneak", new Property<double>(1.0, "Scales the sneak attack damage multiplier.")),

                ("pk_cast_radius", new Property<double>(6.0, "The distance in meters a player can travel from their starting cast position. if they exceed this distance, they fizzle the spell.")),

                // Hardcore settings
                ("hardcore_npk_death_xp_loss_percent", new Property<double>(0.5, "Percentage of total xp lost on death for Hardcore NPK gameplay mode. A value of 1.0 means deaths reset the player to level 1.")),
                ("hardcore_pk_pvp_death_xp_loss_percent", new Property<double>(0.25, "Percentage of total xp lost on death for Hardcore PK gameplay mode when dying in PvP. A value of 1.0 means deaths reset the player to level 1.")),
                ("hardcore_pk_pve_death_xp_loss_percent", new Property<double>(0.5, "Percentage of total xp lost on death for Hardcore PK gameplay mode when dying in PvE. A value of 1.0 means deaths reset the player to level 1.")),
                ("hardcore_npk_xp_modifier", new Property<double>(1.0, "Scales the amount of xp received by hardcore NPK players.")),
                ("hardcore_pk_xp_modifier", new Property<double>(1.0, "Scales the amount of xp received by hardcore PK players.")),
                ("hardcore_pk_xp_modifier_pvp_kill", new(1.0, "Scales the amount of xp received by hardcore PK players for a PK kill.")),
                ("fall_damage_multiplier", new(1.0, "Global multiplier for fall damage. Use fall_damage_enabled=false instead of setting this to 0 to disable completely.")),

                ("hot_dungeon_interval", new Property<double>(7800.0, "The minimum possible duration (in seconds) before a new hot dungeon can be automatically rolled after one was previously activated.")),
                ("hot_dungeon_duration", new Property<double>(7200.0, "The total duration (in seconds) which a hot dungeon will be active.")),
                ("hot_dungeon_roll_delay", new Property<double>(1200.0, "The duration (in seconds) between each chance to automatically roll a new hot dungeon (only applies while there are no hot dungeons active).")),
                ("hot_dungeon_chance", new Property<double>(0.33, "The percentage chance (between 0 and 1) when the server will activate a hot dungeon at each roll interval.")),

                ("exploration_bonus_xp", new Property<double>(0.5, "Extra xp earned while completing exploration assignment's objectives. 1.0 means 100% more xp.")),
                ("relive_bonus_xp", new Property<double>(1.0, "Extra xp earned while reliving levels after a death that resulted in lost levels. 1.0 means 100% more xp.")),

                ("riposte_proc_chance", new(0.9, "The percentage chance (between 0 and 1) that riposte will trigger when evading an attack.")),
                ("dual_wield_riposte_proc_chance", new(0.4, "The percentage chance (between 0 and 1) that a dual wielding player defender will trigger an extra offhand attack when evading the attacker (this does not require riposte technique equipped).")),

                ("surface_bonus_xp", new(0.25, "Extra xp earned for kills when hunting outside dungeons. 1.0 means 100% more xp.")),
                ("hot_dungeon_bonus_xp", new(1.0, "Extra xp earned for kills when inside hot dungeons. 1.0 means 100% more xp.")),
                ("exploration_bonus_xp_markers", new(1.0, "Extra xp earned while completing exploration assignment's marker objectives. 1.0 means 100% more xp.")),
                ("exploration_bonus_xp_kills", new(0.5, "Extra xp earned while completing exploration assignment's kill objectives. 1.0 means 100% more xp.")),
                ("exploration_bonus_xp_treasure", new(0.5, "Extra xp earned while completing exploration treasure map hunting. 1.0 means 100% more xp.")),
                ("exploration_bonus_xp_contract_location", new(0.5, "Extra xp earned while completing exploration treasure map hunting. 1.0 means 100% more xp.")),

                ("elite_mob_spawn_rate", new(0.00, "Probability of a creature spawning as an elite mob. 1.0 means 100%")),
                ("elite_mob_loot_quality", new(0.5, "Loot quality mod of elite mob (For reference, normal is 1.0, chests are 1.2, Awareness chests are 1.4")),

                ("quest_mutation_tier_1_major_chance", new(0.10, "The % chance a tier 1 quest item cantrip mutation will be a major cantrip (otherwise will be a minor cantrip).")),
                ("quest_mutation_tier_2_major_chance", new(0.25, "The % chance a tier 2 quest item cantrip mutation will be a major cantrip (otherwise will be a minor cantrip).")),
                ("quest_mutation_tier_3_major_chance", new(0.90, "The % chance a tier 3 quest item cantrip mutation will be a major cantrip (otherwise will be a minor cantrip).")),
                ("customdm_mob_war_damage_scale", new(0.5, "Scales creature war damage in CustomDM. A value of 0.75 means 75% of normal damage")),
                ("customdm_player_war_damage_scale_pve", new(1.0, "Scales player war damage in CustomDM, in PvE. A value of 0.75 means 75% of normal damage")),
                ("customdm_mob_damage_scale", new(1.0, "Scales mob damage vs players in CustomDM. A value of 0.75 means 75% of normal damage")),
                ("spell_extraction_scroll_base_chance", new(0.5, "The base chance of a spell extraction scroll to successfully extract a spell. A value of 0.50 means 50%")),
                ("spell_extraction_scroll_chance_per_extra_spell", new(0.1, "The additional spell extraction chance added per spell starting from the 2nd spell. A value of 0.1 means 10%")),
                ("coin_stack_multiplier", new(1.0, "Scales the amount of pyreals awarded from mob kills.")),
                ("bz_snitch_chance", new(0.3, "The chance to proc a bz snitch per tick (PvP player location reveal).")),
                ("spelltransfer_over_tier_success_chance", new(1.0, "The chance to successfully transfer a spell that is higher than the tier of the target item without destroying the target")),
                ("extra_vitae_penalty_pvp", new(0.0, "The extra vitae penalty for a PvP death. A value of 0.05 means an extra 5% vitae on top of the usual 5%, for 10% total")),
                ("pve_death_respite_timer", new Property<double>(60, "Respite timer for pve deaths (players become npk even from pve deaths)")),
                ("arena_corpse_rot_seconds", new Property<double>(900, "the number of seconds a corpse that is generated in an arena landblock takes to rot. Default 15 mins.")),
                ("catchup_xp_modifier", new Property<double>(2.0, "Globally scales the amount of xp received by players who are below the previous_max_level threshold, note that this possibly multiplies the other xp_modifier options.")),
                ("bz_whispers_chance", new Property<double>(0.2, "CustomDM: The chance a player will receive a bz whisper every bz_whispers_interval")),

                // Do not edit below this line
                ("null_double", new(0, "No effect, just included here as a last item on the list to prevent related lines from being changed in git upon new property additions."))
                );

        public static readonly ReadOnlyDictionary<string, Property<string>> DefaultStringProperties =
            DictOf(
                ("content_folder", new Property<string>("Content", "for content creators to live edit weenies. defaults to Content folder found in same directory as ACE.Server.dll")),
                ("dat_older_warning_msg", new Property<string>("Your DAT files are incomplete.\nThis server does not support dynamic DAT updating at this time.\nPlease visit https://emulator.ac/how-to-play to download the complete DAT files.", "Warning message displayed (if show_dat_warning is true) to player if client attempts DAT download from server")),
                ("dat_newer_warning_msg", new Property<string>("Your DAT files are newer than expected.\nPlease visit https://emulator.ac/how-to-play to download the correct DAT files.", "Warning message displayed (if show_dat_warning is true) to player if client connects to this server")),
                ("popup_header", new Property<string>("Welcome to Asheron's Call!", "Welcome message displayed when you log in")),
                ("popup_welcome", new Property<string>("To begin your training, speak to the Society Greeter. Walk up to the Society Greeter using the 'W' key, then double-click on her to initiate a conversation.", "Welcome message popup in training halls")),
                ("popup_welcome_olthoi", new Property<string>("Welcome to the Olthoi hive! Be sure to talk to the Olthoi Queen to receive the Olthoi protections granted by the energies of the hive.", "Welcome message displayed on the first login for an Olthoi Player")),
                ("popup_motd", new Property<string>("", "Popup message of the day")),
                ("server_motd", new Property<string>("", "Server message of the day")),
                ("server_motd2", new Property<string>("", "Server message of the day - Second message")),
                ("server_motd3", new Property<string>("", "Server message of the day - Third message")),
                ("server_motd4", new Property<string>("", "Server message of the day - Fourth message")),
                ("turbine_chat_webhook", new Property<string>("", "Webhook to be used for turbine chat. This is for copying ingame general chat channels to a Discord channel.")),
                ("turbine_chat_webhook_audit", new Property<string>("", "Webhook to be used for ingame audit log.")),
                ("proxycheck_api_key", new Property<string>("", "API key for proxycheck.io service for VPN detection")),
                ("vpn_account_whitelist", new Property<string>("", "A comma separated list of account names for which VPN detection is bypassed")),
                ("discord_login_token", new Property<string>("", "Login Token used for Discord chat integration")),
                ("arenas_blacklist", new Property<string>("", "A comma separated list of CharacterID values that cannot participate in Arenas")),
                ("arena_globals_webhook", new Property<string>("", "Webhook to be send Arena global messages.")),

                // Do not edit below this line
                ("null_string", new("", "No effect, just included here as a last item on the list to prevent related lines from being changed in git upon new property additions."))
                );

    }
}

