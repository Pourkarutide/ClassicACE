using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

using log4net;

using ACE.Common;
using ACE.Common.Extensions;
using ACE.Database;
using ACE.DatLoader;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Models;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;
using System;

namespace ACE.Server.Network.Handlers
{
    public static class CharacterHandler
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [GameMessage(GameMessageOpcode.CharacterCreate, SessionState.AuthConnected)]
        public static void CharacterCreate(ClientMessage message, Session session)
        {
            string clientString = message.Payload.ReadString16L();

            if (clientString != session.Account)
                return;

            if (ServerManager.ShutdownInProgress)
            {
                session.SendCharacterError(CharacterError.LogonServerFull);
                return;
            }

            if (WorldManager.WorldStatus == WorldManager.WorldStatusState.Open || session.AccessLevel > AccessLevel.Player)
                CharacterCreateEx(message, session);
            else
                session.SendCharacterError(CharacterError.LogonServerFull);
        }

        private static void CharacterCreateEx(ClientMessage message, Session session)
        {
            var characterCreateInfo = new CharacterCreateInfo();
            characterCreateInfo.Unpack(message.Payload);
            
            if (PropertyManager.GetBool("taboo_table").Item && DatManager.PortalDat.TabooTable.ContainsBadWord(characterCreateInfo.Name.ToLowerInvariant()))
            {
                SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.NameBanned);
                return;
            }

            if (PropertyManager.GetBool("creature_name_check").Item && DatabaseManager.World.IsCreatureNameInWorldDatabase(characterCreateInfo.Name))
            {
                SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.NameBanned);
                return;
            }

            DatabaseManager.Shard.IsCharacterNameAvailable(characterCreateInfo.Name, isAvailable =>
            {
                if (!isAvailable)
                {
                    SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.NameInUse);
                    return;
                }
            });

            // Only allow era appropriate heritages.
            if (ConfigManager.Config.Server.WorldRuleset <= Common.Ruleset.Infiltration && characterCreateInfo.Heritage != HeritageGroup.Aluvian && characterCreateInfo.Heritage != HeritageGroup.Gharundim && characterCreateInfo.Heritage != HeritageGroup.Sho)
            {
                SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.Pending);
                session.Network.EnqueueSend(new GameEvent.Events.GameEventPopupString(session, "Only Aluvian, Gharu'ndim and Sho heritages are allowed on this server."));
                return;
            }

            if (!PropertyManager.GetBool("allow_skill_specialization").Item)
            {
                if (characterCreateInfo.SkillAdvancementClasses.Any(s => s == SkillAdvancementClass.Specialized))
                {
                    SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.Pending);
                    session.Network.EnqueueSend(new GameEvent.Events.GameEventPopupString(session, "You may not specialize any skills in this realm."));
                    return;
                }
            }

            if ((characterCreateInfo.Heritage == HeritageGroup.Olthoi || characterCreateInfo.Heritage == HeritageGroup.OlthoiAcid) && PropertyManager.GetBool("olthoi_play_disabled").Item)
            {
                SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.Pending);
                return;
            }

            Weenie weenie;
            if (ConfigManager.Config.Server.Accounts.OverrideCharacterPermissions)
            {
                if (session.AccessLevel >= AccessLevel.Developer && session.AccessLevel <= AccessLevel.Admin)
                    weenie = DatabaseManager.World.GetCachedWeenie("admin");
                else if (session.AccessLevel >= AccessLevel.Sentinel && session.AccessLevel <= AccessLevel.Envoy)
                    weenie = DatabaseManager.World.GetCachedWeenie("sentinel");
                else
                    weenie = DatabaseManager.World.GetCachedWeenie("human");

                if (characterCreateInfo.Heritage == HeritageGroup.Olthoi && weenie.WeenieType == WeenieType.Admin)
                    weenie = DatabaseManager.World.GetCachedWeenie("olthoiadmin");

                if (characterCreateInfo.Heritage == HeritageGroup.OlthoiAcid && weenie.WeenieType == WeenieType.Admin)
                    weenie = DatabaseManager.World.GetCachedWeenie("olthoiacidadmin");
            }
            else
                weenie = DatabaseManager.World.GetCachedWeenie("human");

            if (characterCreateInfo.Heritage == HeritageGroup.Olthoi && weenie.WeenieType == WeenieType.Creature)
                weenie = DatabaseManager.World.GetCachedWeenie("olthoiplayer");

            if (characterCreateInfo.Heritage == HeritageGroup.OlthoiAcid && weenie.WeenieType == WeenieType.Creature)
                weenie = DatabaseManager.World.GetCachedWeenie("olthoiacidplayer");

            if (characterCreateInfo.IsSentinel && session.AccessLevel >= AccessLevel.Sentinel)
                weenie = DatabaseManager.World.GetCachedWeenie("sentinel");

            if (characterCreateInfo.IsAdmin && session.AccessLevel >= AccessLevel.Developer)
                weenie = DatabaseManager.World.GetCachedWeenie("admin");

            if (weenie == null)
                weenie = DatabaseManager.World.GetCachedWeenie("human"); // Default catch-all

            if (weenie == null) // If it is STILL null after the above catchall, the database is missing critical data and cannot continue with character creation.
            {
                SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.DatabaseDown);
                log.Error("Database does not contain the weenie for human (1). Characters cannot be created until the missing weenie is restored.");
                return;
            }

            var guid = GuidManager.NewPlayerGuid();

            var weenieType = weenie.WeenieType;

            // If Database didn't have Sentinel/Admin weenies, alter the weenietype coming in.
            if (ConfigManager.Config.Server.Accounts.OverrideCharacterPermissions)
            {
                if (session.AccessLevel >= AccessLevel.Developer && session.AccessLevel <= AccessLevel.Admin && weenieType != WeenieType.Admin)
                    weenieType = WeenieType.Admin;
                else if (session.AccessLevel >= AccessLevel.Sentinel && session.AccessLevel <= AccessLevel.Envoy && weenieType != WeenieType.Sentinel)
                    weenieType = WeenieType.Sentinel;
            }


            var result = PlayerFactory.Create(characterCreateInfo, weenie, guid, session.AccountId, weenieType, out var player);

            if (result != PlayerFactory.CreateResult.Success || player == null)
            {
                if (result == PlayerFactory.CreateResult.ClientServerSkillsMismatch)
                {
                    session.Terminate(SessionTerminationReason.ClientVersionIncorrect, new GameMessageBootAccount(" because you do not have the correct data files for this server"));
                    return;
                }
                else if (result == PlayerFactory.CreateResult.CustomGameplayModesDisabled)
                {
                    SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.Pending);
                    session.Network.EnqueueSend(new GameEvent.Events.GameEventPopupString(session, "Custom gameplay modes are currently disabled for new characters."));
                    return;
                }

                SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.Corrupt);
                return;
            }

            DatabaseManager.Shard.IsCharacterNameAvailable(characterCreateInfo.Name, isAvailable =>
            {
                if (!isAvailable)
                {
                    SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.NameInUse);
                    return;
                }

                var possessions = player.GetAllPossessions();
                var possessedBiotas = new Collection<(Biota biota, ReaderWriterLockSlim rwLock)>();
                foreach (var possession in possessions)
                    possessedBiotas.Add((possession.Biota, possession.BiotaDatabaseLock));

                // We must await here -- 
                DatabaseManager.Shard.AddCharacterInParallel(player.Biota, player.BiotaDatabaseLock, possessedBiotas, player.Character, player.CharacterDatabaseLock, saveSuccess =>
                {
                    if (!saveSuccess)
                    {
                        SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.DatabaseDown);
                        return;
                    }

                    PlayerManager.AddOfflinePlayer(player);
                    session.Characters.Add(player.Character);

                    var msg = $"Character {player.Name} has been created, welcome!";
                    log.Info(msg);

                    if (PropertyManager.GetBool("broadcast_player_life").Item && PropertyManager.GetBool("broadcast_player_create").Item)
                    {
                        PlayerManager.BroadcastToAll(new GameMessageSystemChat("[Global] " + msg, ChatMessageType.Broadcast));
                        _ = TurbineChatHandler.SendWebhookedChat("", msg, null, "Welcome");

                    }

                    SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.Ok, player.Guid, characterCreateInfo.Name);
                });
            });
        }

        private static void SendCharacterCreateResponse(Session session, CharacterGenerationVerificationResponse response, ObjectGuid guid = default(ObjectGuid), string charName = "")
        {
            session.Network.EnqueueSend(new GameMessageCharacterCreateResponse(response, guid, charName));
        }


        [GameMessage(GameMessageOpcode.CharacterEnterWorldRequest, SessionState.AuthConnected)]
        public static void CharacterEnterWorldRequest(ClientMessage message, Session session)
        {
            if (ServerManager.ShutdownInProgress)
            {
                session.SendCharacterError(CharacterError.LogonServerFull);
                return;
            }

            if (WorldManager.WorldStatus == WorldManager.WorldStatusState.Open || session.AccessLevel > AccessLevel.Player)
                session.Network.EnqueueSend(new GameMessageCharacterEnterWorldServerReady());
            else
                session.SendCharacterError(CharacterError.LogonServerFull);
        }

        [GameMessage(GameMessageOpcode.CharacterEnterWorld, SessionState.AuthConnected)]
        public static void CharacterEnterWorld(ClientMessage message, Session session)
        {
            var guid = message.Payload.ReadUInt32();

            string clientString = message.Payload.ReadString16L();

            if (ServerManager.ShutdownInProgress)
            {
                session.SendCharacterError(CharacterError.LogonServerFull);
                return;
            }

            if (clientString != session.Account)
            {
                session.SendCharacterError(CharacterError.EnterGameCharacterNotOwned);
                return;
            }

            var character = session.Characters.SingleOrDefault(c => c.Id == guid);
            if (character == null)
            {
                session.SendCharacterError(CharacterError.EnterGameCharacterNotOwned);
                return;
            }

            if (character.IsDeleted || character.DeleteTime > 0)
            {
                session.SendCharacterError(CharacterError.EnterGameCharacterNotOwned);
                return;
            }

            if (PlayerManager.GetOnlinePlayer(guid) != null)
            {
                // If this happens, it could be that the previous session for this Player terminated in a way that didn't transfer the player to offline via PlayerManager properly.
                session.SendCharacterError(CharacterError.EnterGameCharacterInWorld);
                return;
            }

            var offlinePlayer = PlayerManager.GetOfflinePlayer(guid);

            if (offlinePlayer == null)
            {
                // This would likely only happen if the account tried to log in a character that didn't exist.
                session.SendCharacterError(CharacterError.EnterGameGeneric);
                return;
            }

            if (offlinePlayer.IsDeleted || offlinePlayer.IsPendingDeletion)
            {
                session.SendCharacterError(CharacterError.EnterGameCharacterNotOwned);
                return;
            }

            if ((offlinePlayer.Heritage == (int)HeritageGroup.Olthoi || offlinePlayer.Heritage == (int)HeritageGroup.OlthoiAcid) && PropertyManager.GetBool("olthoi_play_disabled").Item)
            {
                session.SendCharacterError(CharacterError.EnterGameCouldntPlaceCharacter);
                return;
            }

            session.InitSessionForWorldLogin();

            session.State = SessionState.WorldConnected;

            WorldManager.PlayerEnterWorld(session, character);

            try
            {
                DatabaseManager.Shard.LogCharacterLogin(session.AccountId, session.Account, session.EndPointC2S.Address.ToString(), character.Id, character.Name);
            }
            catch (Exception)
            {

            }
        }


        [GameMessage(GameMessageOpcode.CharacterLogOff, SessionState.WorldConnected)]
        public static void CharacterLogOff(ClientMessage message, Session session)
        {
            session.LogOffPlayer();
        }


        [GameMessage(GameMessageOpcode.CharacterDelete, SessionState.AuthConnected)]
        public static void CharacterDelete(ClientMessage message, Session session)
        {
            string clientString = message.Payload.ReadString16L();
            uint characterSlot = message.Payload.ReadUInt32();

            if (ServerManager.ShutdownInProgress)
            {
                session.SendCharacterError(CharacterError.Delete);
                return;
            }

            if (WorldManager.WorldStatus == WorldManager.WorldStatusState.Closed && session.AccessLevel < AccessLevel.Advocate)
            {
                session.SendCharacterError(CharacterError.LogonServerFull);
                return;
            }

            if (clientString != session.Account)
            {
                session.SendCharacterError(CharacterError.Delete);
                return;
            }

            var character = session.Characters[(int)characterSlot];
            if (character == null)
            {
                session.SendCharacterError(CharacterError.Delete);
                return;
            }

            var offlinePlayer = PlayerManager.GetOfflinePlayer(session.Characters[(int)characterSlot].Id);

            if (offlinePlayer == null || offlinePlayer.IsDeleted || offlinePlayer.IsPendingDeletion)
            {
                session.SendCharacterError(CharacterError.Delete);
                return;
            }

            session.Network.EnqueueSend(new GameMessageCharacterDelete());

            var charRestoreTime = PropertyManager.GetLong("char_delete_time", 3600).Item;
            character.DeleteTime = (ulong)(Time.GetUnixTime() + charRestoreTime);
            character.IsDeleted = false;

            DatabaseManager.Shard.SaveCharacter(character, new ReaderWriterLockSlim(), result =>
            {
                if (result)
                {
                    session.Network.EnqueueSend(new GameMessageCharacterList(session.Characters, session));

                    PlayerManager.HandlePlayerDelete(character.Id, character.Name);
                }
                else
                    session.SendCharacterError(CharacterError.Delete);
            });
        }

        [GameMessage(GameMessageOpcode.CharacterRestore, SessionState.AuthConnected)]
        public static void CharacterRestore(ClientMessage message, Session session)
        {
            var guid = message.Payload.ReadUInt32();

            if (ServerManager.ShutdownInProgress)
            {
                session.SendCharacterError(CharacterError.EnterGameCouldntPlaceCharacter);
                return;
            }

            if (WorldManager.WorldStatus == WorldManager.WorldStatusState.Closed && session.AccessLevel < AccessLevel.Advocate)
            {
                session.SendCharacterError(CharacterError.LogonServerFull);
                return;
            }

            var character = session.Characters.SingleOrDefault(c => c.Id == guid);
            if (character == null)
                return;

            if (PlayerManager.IsRecentlyRestored(character.Id) || Time.GetUnixTime() > character.DeleteTime || character.IsDeleted)
            {
                session.SendCharacterError(CharacterError.EnterGameCharacterNotOwned);
                return;
            }

            DatabaseManager.Shard.IsCharacterNameAvailable(character.Name, isAvailable =>
            {
                if (!isAvailable)
                {
                    SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.NameInUse);
                }
                else
                {
                    character.DeleteTime = 0;
                    character.IsDeleted = false;

                    DatabaseManager.Shard.SaveCharacter(character, new ReaderWriterLockSlim(), result =>
                    {
                        var name = character.Name;

                        if (ConfigManager.Config.Server.Accounts.OverrideCharacterPermissions && session.AccessLevel > AccessLevel.Advocate)
                            name = "+" + name;
                        else if (!ConfigManager.Config.Server.Accounts.OverrideCharacterPermissions && character.IsPlussed)
                            name = "+" + name;

                        if (result)
                        {
                            session.Network.EnqueueSend(new GameMessageCharacterRestore(guid, name, 0u));

                            var msg = $"Character {name} has had second thoughts about his life, welcome back!";
                            log.Info(msg);

                            if (PropertyManager.GetBool("broadcast_player_life").Item && PropertyManager.GetBool("broadcast_player_restore").Item)
                            {
                                PlayerManager.BroadcastToAll(new GameMessageSystemChat("[Global] " + msg, ChatMessageType.Broadcast));
                                _ = TurbineChatHandler.SendWebhookedChat("", msg, null, "Welcome");
                            }

                            PlayerManager.OnRestore(character.Id);
                        }
                        else
                            SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.Corrupt);
                    });
                }
            });
        }
    }
}
