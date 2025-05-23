using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

using ACE.Common.Extensions;
using ACE.Database.Models.Auth;
using ACE.DatLoader;
using ACE.DatLoader.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Command.Handlers;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        /// <summary>
        /// A player earns XP through natural progression, ie. kills and quests completed
        /// </summary>
        /// <param name="amount">The amount of XP being added</param>
        /// <param name="xpType">The source of XP being added</param>
        /// <param name="shareable">True if this XP can be shared with Fellowship</param>
        public void EarnXP(long amount, XpType xpType, int? xpSourceLevel, uint? xpSourceId, uint xpSourceCampValue, double? xpSourceTier, ShareType shareType = ShareType.All, string xpMessage = "", double extraXpMultiplier = 1.0f)
        {
            //Console.WriteLine($"{Name}.EarnXP({amount}, {sharable}, {fixedAmount})");

            bool usesRewardByLevelSystem = false;
            if ((xpType == XpType.Quest || xpType == XpType.Exploration) && amount < 0 && amount > -6000) // this range is used to specify the reward by level system.
            {
                usesRewardByLevelSystem = true;
                int formulaVersion;
                // The following comments are just recommendations and vary from quest to quest, but the larger the value the higher the xp sum awarded.
                if (amount <= -5000) // once per character
                {
                    xpSourceLevel = -((int)amount + 5000);
                    formulaVersion = 5;
                }
                else if (amount <= -4000) // once every 3 weeks or more
                {
                    xpSourceLevel = -((int)amount + 4000);
                    formulaVersion = 4;
                }
                else if (amount <= -3000) // once a week or more
                {
                    xpSourceLevel = -((int)amount + 3000);
                    formulaVersion = 3;
                }
                else if (amount <= -2000) // once a day or more
                {
                    xpSourceLevel = -((int)amount + 2000);
                    formulaVersion = 2;
                }
                else if (amount <= -1000) // more than once per day
                {
                    xpSourceLevel = -((int)amount + 1000);
                    formulaVersion = 1;
                }
                else
                {
                    xpSourceLevel = -(int)amount;
                    formulaVersion = 0;
                }

                int modifiedLevel = Math.Max((int)Level, 5);
                if (Level < 100 && modifiedLevel <= xpSourceLevel / 3)
                {
                    xpSourceLevel = modifiedLevel * 3;
                    Session.Network.EnqueueSend(new GameMessageSystemChat("Your experience reward has been reduced because your level is not high enough!", ChatMessageType.System));
                }

                float totalXP = GetCreatureDeathXP(xpSourceLevel.Value, 0, false, false, formulaVersion);

                if (xpSourceId.HasValue && xpSourceId != 0)
                {
                    var typeCampBonus = xpSourceCampValue / 100f;

                    totalXP = totalXP * typeCampBonus;

                    if(xpSourceCampValue < 100)
                        xpMessage = $"T: {xpSourceCampValue.ToString("0")}%";
                }

                amount = (long)Math.Round(totalXP);
            }
            else if (amount < 0)
            {
                SpendXP(-amount);
                return;
            }
            else if (xpType == XpType.Kill && Common.ConfigManager.Config.Server.WorldRuleset == Common.Ruleset.CustomDM)
            {
                float totalXP = amount;

                if (xpSourceId.HasValue && xpSourceId != 0)
                {
                    CampManager.HandleCampInteraction(xpSourceId.Value, CurrentLandblock, xpSourceCampValue, out var typeCampBonus, out var areaCampBonus, out var restCampBonus);

                    typeCampBonus = (float)Math.Round(typeCampBonus, 2);
                    areaCampBonus = (float)Math.Round(areaCampBonus, 2);
                    restCampBonus = (float)Math.Round(restCampBonus, 2);

                    float thirdXP = totalXP / 3.0f;
                    totalXP = (thirdXP * typeCampBonus) + (thirdXP * areaCampBonus) + (thirdXP * restCampBonus);

                    xpMessage = $"{xpMessage}{(xpMessage.Length > 0 ? " " : "")}T: {(typeCampBonus * 100).ToString("0")}% A: {(areaCampBonus * 100).ToString("0")}% R: {(restCampBonus * 100).ToString("0")}%";
                }

                amount = (long)Math.Round(totalXP);
            }

            // apply xp modifiers.  Quest XP is multiplicative with general XP modification
            var questModifier = PropertyManager.GetDouble("quest_xp_modifier").Item;
            var modifier = PropertyManager.GetDouble("xp_modifier").Item;

            modifier *= extraXpMultiplier;
            if (xpType == XpType.Quest)
                modifier *= questModifier;

            if (GameplayMode == GameplayModes.HardcorePK)
                modifier *= PropertyManager.GetDouble("hardcore_pk_xp_modifier").Item;
            else if (GameplayMode == GameplayModes.HardcoreNPK)
                modifier *= PropertyManager.GetDouble("hardcore_npk_xp_modifier").Item;

            var catchupModifier = GetCatchupXp(Level ?? 1);

            modifier *= catchupModifier;

            if (xpType == XpType.Kill)
            {
                if (xpSourceTier != null)
                {
                    var highTier = Math.Ceiling(xpSourceTier ?? 1);
                    var lowTier = Math.Floor(xpSourceTier ?? 1);

                    double highTierMod;
                    switch (highTier)
                    {
                        case 1: highTierMod = PropertyManager.GetDouble("xp_modifier_kill_tier1").Item; break;
                        case 2: highTierMod = PropertyManager.GetDouble("xp_modifier_kill_tier2").Item; break;
                        case 3: highTierMod = PropertyManager.GetDouble("xp_modifier_kill_tier3").Item; break;
                        case 4: highTierMod = PropertyManager.GetDouble("xp_modifier_kill_tier4").Item; break;
                        case 5: highTierMod = PropertyManager.GetDouble("xp_modifier_kill_tier5").Item; break;
                        case 6: highTierMod = PropertyManager.GetDouble("xp_modifier_kill_tier6").Item; break;
                        default: highTierMod = 1; break;
                    }

                    double lowTierMod;
                    switch (lowTier)
                    {
                        case 1: lowTierMod = PropertyManager.GetDouble("xp_modifier_kill_tier1").Item; break;
                        case 2: lowTierMod = PropertyManager.GetDouble("xp_modifier_kill_tier2").Item; break;
                        case 3: lowTierMod = PropertyManager.GetDouble("xp_modifier_kill_tier3").Item; break;
                        case 4: lowTierMod = PropertyManager.GetDouble("xp_modifier_kill_tier4").Item; break;
                        case 5: lowTierMod = PropertyManager.GetDouble("xp_modifier_kill_tier5").Item; break;
                        case 6: lowTierMod = PropertyManager.GetDouble("xp_modifier_kill_tier6").Item; break;
                        default: lowTierMod = 1; break;
                    }

                    if (highTierMod != 1 || lowTierMod != 1)
                    {
                        var highTierWeight = (xpSourceTier ?? 1) % 1;
                        var lowTierWeight = 1 - highTierWeight;

                        var highTierAmount = (long)(amount * highTierWeight * highTierMod);
                        var lowTierAmount = (long)(amount * lowTierWeight * lowTierMod);

                        amount = lowTierAmount + highTierAmount;
                    }
                }
                else if (xpSourceLevel != null)
                {
                    if (xpSourceLevel < 28)
                        modifier *= PropertyManager.GetDouble("xp_modifier_kill_tier1").Item;
                    else if (xpSourceLevel < 65)
                        modifier *= PropertyManager.GetDouble("xp_modifier_kill_tier2").Item;
                    else if (xpSourceLevel < 95)
                        modifier *= PropertyManager.GetDouble("xp_modifier_kill_tier3").Item;
                    else if (xpSourceLevel < 110)
                        modifier *= PropertyManager.GetDouble("xp_modifier_kill_tier4").Item;
                    else if (xpSourceLevel < 135)
                        modifier *= PropertyManager.GetDouble("xp_modifier_kill_tier5").Item;
                    else
                        modifier *= PropertyManager.GetDouble("xp_modifier_kill_tier6").Item;
                }
            }
            else if (xpType == XpType.Quest && xpSourceLevel != null)
            {
                if (usesRewardByLevelSystem)
                {
                    var tier = CalculateExtendedTier(xpSourceLevel ?? 1);
                    var highTier = Math.Ceiling(tier);
                    var lowTier = Math.Floor(tier);

                    double highTierMod;
                    switch (highTier)
                    {
                        case 1: highTierMod = PropertyManager.GetDouble("xp_modifier_reward_tier1").Item; break;
                        case 2: highTierMod = PropertyManager.GetDouble("xp_modifier_reward_tier2").Item; break;
                        case 3: highTierMod = PropertyManager.GetDouble("xp_modifier_reward_tier3").Item; break;
                        case 4: highTierMod = PropertyManager.GetDouble("xp_modifier_reward_tier4").Item; break;
                        case 5: highTierMod = PropertyManager.GetDouble("xp_modifier_reward_tier5").Item; break;
                        case 6: highTierMod = PropertyManager.GetDouble("xp_modifier_reward_tier6").Item; break;
                        default: highTierMod = 1; break;
                    }

                    double lowTierMod;
                    switch (lowTier)
                    {
                        case 1: lowTierMod = PropertyManager.GetDouble("xp_modifier_reward_tier1").Item; break;
                        case 2: lowTierMod = PropertyManager.GetDouble("xp_modifier_reward_tier2").Item; break;
                        case 3: lowTierMod = PropertyManager.GetDouble("xp_modifier_reward_tier3").Item; break;
                        case 4: lowTierMod = PropertyManager.GetDouble("xp_modifier_reward_tier4").Item; break;
                        case 5: lowTierMod = PropertyManager.GetDouble("xp_modifier_reward_tier5").Item; break;
                        case 6: lowTierMod = PropertyManager.GetDouble("xp_modifier_reward_tier6").Item; break;
                        default: lowTierMod = 1; break;
                    }

                    if (highTierMod != 1 || lowTierMod != 1)
                    {
                        var highTierWeight = (xpSourceTier ?? 1) % 1;
                        var lowTierWeight = 1 - highTierWeight;

                        var highTierAmount = (long)(amount * highTierWeight * highTierMod);
                        var lowTierAmount = (long)(amount * lowTierWeight * lowTierMod);

                        amount = lowTierAmount + highTierAmount;
                    }
                }
                else
                {
                    if (xpSourceLevel < 16)
                        modifier *= PropertyManager.GetDouble("xp_modifier_reward_tier1").Item;
                    else if (xpSourceLevel < 36)
                        modifier *= PropertyManager.GetDouble("xp_modifier_reward_tier2").Item;
                    else if (xpSourceLevel < 56)
                        modifier *= PropertyManager.GetDouble("xp_modifier_reward_tier3").Item;
                    else if (xpSourceLevel < 76)
                        modifier *= PropertyManager.GetDouble("xp_modifier_reward_tier4").Item;
                    else if (xpSourceLevel < 96)
                        modifier *= PropertyManager.GetDouble("xp_modifier_reward_tier5").Item;
                    else
                        modifier *= PropertyManager.GetDouble("xp_modifier_reward_tier6").Item;
                }
            }

            // should this be passed upstream to fellowship / allegiance?
            var enchantment = GetXPAndLuminanceModifier(xpType);

            var m_amount = (long)Math.Round(amount * enchantment * modifier);

            var m_amount_before_extra = m_amount;

            long extraNotSharedAmount = 0;

            if (m_amount < 0)
            {
                log.Warn($"{Name}.EarnXP({amount}, {shareType})");
                log.Warn($"modifier: {modifier}, enchantment: {enchantment}, m_amount: {m_amount}");
                return;
            }
            else
            {
                if (Common.ConfigManager.Config.Server.WorldRuleset == Common.Ruleset.CustomDM)
                {
                    float totalExtraXP = 0;
                    float totalNotSharedExtraXP = 0;
                    if (xpType == XpType.Quest || xpType == XpType.Exploration || (xpType == XpType.Fellowship && PropertyManager.GetBool("relive_bonus_applies_to_received_fellow_xp").Item))
                    {
                        if (Level < (MaxReachedLevel ?? 1) && IsHardcore)
                        {
                            var extraXP = m_amount * (float)PropertyManager.GetDouble("relive_bonus_xp").Item;
                            totalNotSharedExtraXP += extraXP;

                            xpMessage = $"Relive Bonus: +{extraXP:N0}xp {xpMessage}";
                        }
                    }
                    else if (xpType == XpType.Kill)
                    {
                        if (CurrentLandblock != null && EventManager.HotDungeonLandblock == CurrentLandblock.Id.Raw >> 16)
                        {
                            var extraXP = m_amount * (float)PropertyManager.GetDouble("hot_dungeon_bonus_xp").Item;
                            totalExtraXP += extraXP;

                            xpMessage = $"Hot Dungeon Bonus: +{extraXP:N0}xp {xpMessage}";
                        }

                        if (Level < (MaxReachedLevel ?? 1) && IsHardcore)
                        {
                            var extraXP = m_amount * (float)PropertyManager.GetDouble("relive_bonus_xp").Item;
                            totalNotSharedExtraXP += extraXP;

                            xpMessage = $"Relive Bonus: +{extraXP:N0}xp {xpMessage}";
                        }
                    }

                    m_amount += (long)Math.Round(totalExtraXP);
                    extraNotSharedAmount += (long)Math.Round(totalNotSharedExtraXP);
                }
            }

            GrantXP(m_amount, xpType, shareType, xpMessage, extraNotSharedAmount);

            if (xpType == XpType.Kill && CurrentLandblock != null)
            {
                var bonusMod = (PropertyManager.GetDouble("exploration_bonus_xp").Item + 0.5) * PropertyManager.GetDouble("exploration_bonus_xp_kills").Item;
                if (Exploration1LandblockId == CurrentLandblock.Id.Raw >> 16 && Exploration1KillProgressTracker > 0)
                {
                    Exploration1KillProgressTracker--;
                    long explorationXP = (long)(m_amount_before_extra * bonusMod);
                    xpMessage = $"{Exploration1KillProgressTracker:N0} kill{(Exploration1KillProgressTracker != 1 ? "s" : "")} remaining.";
                    GrantXP(explorationXP, XpType.Exploration, ShareType.Fellowship, xpMessage);

                    if (Exploration1KillProgressTracker == 0)
                    {
                        PlayParticleEffect(PlayScript.AugmentationUseSkill, Guid);
                        if (Exploration1LandblockReached && Exploration1MarkerProgressTracker == 0)
                            Session.Network.EnqueueSend(new GameMessageSystemChat("Your exploration assignment is now fulfilled!", ChatMessageType.Broadcast));
                    }
                }
                else if (Exploration2LandblockId == CurrentLandblock.Id.Raw >> 16 && Exploration2KillProgressTracker > 0)
                {
                    Exploration2KillProgressTracker--;
                    long explorationXP = (long)(m_amount_before_extra * bonusMod);
                    xpMessage = $"{Exploration2KillProgressTracker:N0} kill{(Exploration2KillProgressTracker != 1 ? "s" : "")} remaining.";
                    GrantXP(explorationXP, XpType.Exploration, ShareType.Fellowship, xpMessage);

                    if (Exploration2KillProgressTracker == 0)
                    {
                        PlayParticleEffect(PlayScript.AugmentationUseSkill, Guid);
                        if (Exploration2LandblockReached && Exploration2MarkerProgressTracker == 0)
                            Session.Network.EnqueueSend(new GameMessageSystemChat("Your exploration assignment is now fulfilled!", ChatMessageType.Broadcast));
                    }
                }
                else if (Exploration3LandblockId == CurrentLandblock.Id.Raw >> 16 && Exploration3KillProgressTracker > 0)
                {
                    Exploration3KillProgressTracker--;
                    long explorationXP = (long)(m_amount_before_extra * bonusMod);
                    xpMessage = $"{Exploration3KillProgressTracker:N0} kill{(Exploration3KillProgressTracker != 1 ? "s" : "")} remaining.";
                    GrantXP(explorationXP, XpType.Exploration, ShareType.Fellowship, xpMessage);

                    if (Exploration3KillProgressTracker == 0)
                    {
                        PlayParticleEffect(PlayScript.AugmentationUseSkill, Guid);
                        if (Exploration3LandblockReached && Exploration3MarkerProgressTracker == 0)
                            Session.Network.EnqueueSend(new GameMessageSystemChat("Your exploration assignment is now fulfilled!", ChatMessageType.Broadcast));
                    }
                }
            }
        }

        public double GetCatchupXp(int playerLevel)
        {
            var catchupXpModifier = PropertyManager.GetDouble("catchup_xp_modifier").Item;
            var previousMaxLevel = PropertyManager.GetLong("previous_max_level").Item;

            if (catchupXpModifier <= 1 || playerLevel >= previousMaxLevel || previousMaxLevel <= 1)
                return 1;
            else
                return catchupXpModifier - (catchupXpModifier - 1) * (playerLevel - 1) / (previousMaxLevel - 1);
        }

        /// <summary>
        /// Directly grants XP to the player, without the XP modifier
        /// </summary>
        /// <param name="amount">The amount of XP to grant to the player</param>
        /// <param name="xpType">The source of the XP being granted</param>
        /// <param name="shareable">If TRUE, this XP can be shared with fellowship members</param>
        public void GrantXP(long amount, XpType xpType, ShareType shareType = ShareType.All, string xpMessage = "", long extraNotSharedAmount = 0, string sourceString = "")
        {
            if (GameplayMode == GameplayModes.Limbo)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"You are in limbo mode and cannot earn any experience, please select a gameplay mode.", ChatMessageType.Broadcast));
                return;
            }

            if (IsOlthoiPlayer)
            {
                if (HasVitae)
                    UpdateXpVitae(amount);

                return;
            }

            if (Fellowship != null && Fellowship.ShareXP && shareType.HasFlag(ShareType.Fellowship))
            {
                // this will divy up the XP, and re-call this function
                // with ShareType.Fellowship removed
                Fellowship.SplitXp((ulong)amount, xpType, shareType, this, xpMessage, extraNotSharedAmount);
                return;
            }

            if (xpType == XpType.Fellowship && PropertyManager.GetBool("relive_bonus_applies_to_received_fellow_xp").Item)
            {
                if (Level < (MaxReachedLevel ?? 1))
                {
                    var extraXP = (long)(amount * (float)PropertyManager.GetDouble("relive_bonus_xp").Item);
                    extraNotSharedAmount += extraXP;

                    xpMessage = $"Relive Bonus: +{extraXP:N0}xp {xpMessage}";
                }
            }

            // Make sure UpdateXpAndLevel is done on this players thread
            EnqueueAction(new ActionEventDelegate(() => UpdateXpAndLevel(amount + extraNotSharedAmount, xpType, xpMessage, sourceString)));

            //Update XP tracking info
            try
            {
                if (!XpTrackerStartTimestamp.HasValue)
                {
                    XpTrackerStartTimestamp = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
                    XpTrackerTotalXp = 0;
                }

                XpTrackerTotalXp += amount + extraNotSharedAmount;
            }
            catch (Exception ex)
            {
                log.Error($"Exception in Player.GrantXP while updating XP tracking info. Ex: {ex}");
            }

            // for passing XP up the allegiance chain,
            // this function is only called at the very beginning, to start the process.
            if (shareType.HasFlag(ShareType.Allegiance))
                UpdateXpAllegiance(amount);

            // only certain types of XP are granted to items
            if (xpType == XpType.Kill || xpType == XpType.Quest)
                GrantItemXP(amount);
        }

        /// <summary>
        /// Adds XP to a player's total XP, handles triggers (vitae, level up)
        /// </summary>
        private void UpdateXpAndLevel(long amount, XpType xpType, string xpMessage = "", string sourceString = "")
        {
            // until we are max level we must make sure that we send
            var xpTable = DatManager.PortalDat.XpTable;

            var maxLevel = GetMaxLevel();
            var maxLevelXp = xpTable.CharacterLevelXPList[(int)maxLevel];

            bool allowXpAtMaxLevel = PropertyManager.GetBool("allow_xp_at_max_level").Item;
            var totalXpCap = Common.ConfigManager.Config.Server.WorldRuleset == Common.Ruleset.Infiltration ? maxLevelXp : long.MaxValue; // At what value the total xp counter will stop counting.
            var availableXpCap = Common.ConfigManager.Config.Server.WorldRuleset == Common.Ruleset.Infiltration ? uint.MaxValue : long.MaxValue; // Max unassigned xp amount.


            long vitaeRedirectedAmount;
            if (Common.ConfigManager.Config.Server.WorldRuleset == Common.Ruleset.CustomDM)
            {
                long xpLeft = amount;
                if (HasVitae && IsHardcore && xpType == XpType.Kill)
                    xpLeft = UpdateXpVitae(amount); // Only kill xp reduces hardcore vitae penalty.
                else if (HasVitae && xpType != XpType.Allegiance)
                    xpLeft = UpdateXpVitae(amount);

                vitaeRedirectedAmount = amount - xpLeft;
                amount = xpLeft;
            }

            if (Level != maxLevel || allowXpAtMaxLevel)
            {
                var addAmount = amount;

                var amountLeftToEnd = (long)maxLevelXp - TotalExperience ?? 0;
                if (!allowXpAtMaxLevel && amount > amountLeftToEnd)
                    addAmount = amountLeftToEnd;

                TotalExperience += addAmount;
                if (TotalExperience > (long)totalXpCap)
                    TotalExperience = (long)totalXpCap;

                AvailableExperience += addAmount;
                if (AvailableExperience > availableXpCap)
                    AvailableExperience = availableXpCap;

                var xpTotalUpdate = new GameMessagePrivateUpdatePropertyInt64(this, PropertyInt64.TotalExperience, TotalExperience ?? 0);
                var xpAvailUpdate = new GameMessagePrivateUpdatePropertyInt64(this, PropertyInt64.AvailableExperience, AvailableExperience ?? 0);
                Session.Network.EnqueueSend(xpTotalUpdate, xpAvailUpdate);

                CheckForLevelup();
            }

            if (xpMessage != "")
                xpMessage = $" {xpMessage.Trim()}";

            if (xpType == XpType.Quest)
                Session.Network.EnqueueSend(new GameMessageSystemChat($"You've earned {amount:N0} experience.{xpMessage}", ChatMessageType.Broadcast));
            else if (Common.ConfigManager.Config.Server.WorldRuleset == Common.Ruleset.CustomDM)
            {
                if (xpType == XpType.Fellowship && sourceString == "")
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"Your fellowship shared {amount:N0} experience with you!{xpMessage}", ChatMessageType.Broadcast));
                if (xpType == XpType.Fellowship)
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"Your fellow {sourceString} shared {amount:N0} experience with you!{xpMessage}", ChatMessageType.Broadcast));
                else if (xpType == XpType.Kill && xpMessage != "")
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"You've earned {amount:N0} experience!{xpMessage}", ChatMessageType.Broadcast));
                else if (xpType == XpType.Proficiency && xpMessage != "")
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"You've earned {amount:N0}{xpMessage} experience!", ChatMessageType.Broadcast));
                else if (xpType == XpType.Exploration)
                {
                    if (xpMessage != "")
                        Session.Network.EnqueueSend(new GameMessageSystemChat($"You've earned {amount:N0} exploration experience!{xpMessage}", ChatMessageType.Broadcast));
                    else
                        Session.Network.EnqueueSend(new GameMessageSystemChat($"You've earned {amount:N0} exploration experience!", ChatMessageType.Broadcast));
                }
            }

            if (Common.ConfigManager.Config.Server.WorldRuleset != Common.Ruleset.CustomDM)
            {
                if (HasVitae && IsHardcore && xpType != XpType.Kill)
                    return; // Only kill xp reduces hardcore vitae penalty.

                if (HasVitae && xpType != XpType.Allegiance)
                    UpdateXpVitae(amount);
            }
        }

        /// <summary>
        /// Optionally passes XP up the Allegiance tree
        /// </summary>
        private void UpdateXpAllegiance(long amount)
        {
            if (!HasAllegiance) return;

            AllegianceManager.PassXP(AllegianceNode, (ulong)amount, true);
        }

        private void HandleVitaeOnLogin(double currentUnixTime)
        {
            vitaeTickTimestamp = currentUnixTime + vitaeTickInterval + 10;

            var vitae = EnchantmentManager.GetVitae();

            if (vitae == null || !VitaeDecayTimestamp.HasValue)
                return;

            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(10.0f);
            actionChain.AddAction(this, () =>
            {
                var times = (int)Math.Floor((currentUnixTime - VitaeDecayTimestamp).Value / vitaeTickInterval);

                ReduceVitae(times);
            });
            actionChain.EnqueueChain();
        }

        /// <summary>
        /// Lowers vitae penalty directly
        /// </summary>
        /// <param name="amount">The amount of points to lower the vitae penalty by</param>
        public void ReduceVitae(int amount)
        {
            if (amount <= 0)
                return;

            var vitae = EnchantmentManager.GetVitae();

            if (vitae == null)
                return;

            var vitaePenalty = vitae.StatModValue;
            var startPenalty = vitaePenalty;

            for(int i = 0; i < amount; i++)
            {
                vitaePenalty = EnchantmentManager.ReduceVitae();
                if (vitaePenalty == 1.0f)
                {
                    VitaeCpPool = 0;
                    break;
                }
            }

            Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.VitaeCpPool, VitaeCpPool.Value));

            if (vitaePenalty != startPenalty)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"Your Vitae recovers! Your Vitae penalty is now {(int)(100 - (vitaePenalty * 100))}%.", ChatMessageType.Magic));
                EnchantmentManager.SendUpdateVitae();
            }

            if (vitaePenalty.EpsilonEquals(1.0f) || vitaePenalty > 1.0f)
            {
                var actionChain = new ActionChain();
                actionChain.AddDelaySeconds(2.0f);
                actionChain.AddAction(this, () =>
                {
                    var vitae = EnchantmentManager.GetVitae();
                    if (vitae != null)
                    {
                        var curPenalty = vitae.StatModValue;
                        if (curPenalty.EpsilonEquals(1.0f) || curPenalty > 1.0f)
                            EnchantmentManager.RemoveVitae();
                    }
                });
                actionChain.EnqueueChain();
            }

            return;
        }

        /// <summary>
        /// Handles updating the vitae penalty through earned XP
        /// </summary>
        /// <param name="amount">The amount of XP to apply to the vitae penalty</param>
        private long UpdateXpVitae(long amount)
        {
            var vitae = EnchantmentManager.GetVitae();

            long xpLeft = amount;

            if (vitae == null)
            {
                log.Error($"{Name}.UpdateXpVitae({amount}) vitae null, likely due to cross-thread operation or corrupt EnchantmentManager cache. Please report this.");
                log.Error(Environment.StackTrace);
                return xpLeft;
            }

            if (vitae.StatModValue.EpsilonEquals(1.0f) || vitae.StatModValue > 1.0f)
                return xpLeft;

            var vitaePenalty = vitae.StatModValue;
            var startPenalty = vitaePenalty;

            var maxPool = (int)VitaeCPPoolThreshold(vitaePenalty, DeathLevel.Value);
            var curPool = VitaeCpPool + amount;

            xpLeft -= maxPool - (int)VitaeCpPool;
            while (curPool >= maxPool)
            {
                curPool -= maxPool;
                vitaePenalty = EnchantmentManager.ReduceVitae();
                if (vitaePenalty.EpsilonEquals(1.0f) || vitaePenalty > 1.0f)
                    break;
                maxPool = (int)VitaeCPPoolThreshold(vitaePenalty, DeathLevel.Value);

                xpLeft -= maxPool;
            }
            VitaeCpPool = (int)curPool;

            xpLeft = Math.Max(xpLeft, 0);

            if (Common.ConfigManager.Config.Server.WorldRuleset == Common.Ruleset.CustomDM)
            {
                var xpConsumed = amount - xpLeft;
                Session.Network.EnqueueSend(new GameMessageSystemChat($"Your Vitae penalty consumes {xpConsumed:N0} experience!", ChatMessageType.Magic));
            }

            Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.VitaeCpPool, VitaeCpPool.Value));

            if (vitaePenalty != startPenalty)
            {
                if (Common.ConfigManager.Config.Server.WorldRuleset == Common.Ruleset.CustomDM)
                {
                    var newVitaePenaltyPercent = (int)(100 - (vitaePenalty * 100));
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"Your experience has reduced your Vitae penalty! It is now {newVitaePenaltyPercent:N0}%.", ChatMessageType.Magic));
                }
                else
                    Session.Network.EnqueueSend(new GameMessageSystemChat("Your experience has reduced your Vitae penalty!", ChatMessageType.Magic));
                EnchantmentManager.SendUpdateVitae();
            }

            if (vitaePenalty.EpsilonEquals(1.0f) || vitaePenalty > 1.0f)
            {
                var actionChain = new ActionChain();
                actionChain.AddDelaySeconds(2.0f);
                actionChain.AddAction(this, () =>
                {
                    var vitae = EnchantmentManager.GetVitae();
                    if (vitae != null)
                    {
                        var curPenalty = vitae.StatModValue;
                        if (curPenalty.EpsilonEquals(1.0f) || curPenalty > 1.0f)
                            EnchantmentManager.RemoveVitae();
                    }
                });
                actionChain.EnqueueChain();
            }

            return xpLeft;
        }

        /// <summary>
        /// Returns the maximum possible character level
        /// </summary>
        public static uint GetMaxLevel()
        {
            uint maxPossibleLevel = (uint)DatManager.PortalDat.XpTable.CharacterLevelXPList.Count - 1;
            uint maxSettingLevel = (uint)PropertyManager.GetLong("max_level").Item;
            return (Math.Min(maxPossibleLevel, maxSettingLevel));
        }

        /// <summary>
        /// Returns TRUE if player >= MaxLevel
        /// </summary>
        public bool IsMaxLevel => Level >= GetMaxLevel();

        /// <summary>
        /// Returns the remaining XP required to reach a level
        /// </summary>
        public long? GetRemainingXP(uint level)
        {
            var maxLevel = GetMaxLevel();
            if (level < 1 || level > maxLevel)
                return null;

            var levelTotalXP = DatManager.PortalDat.XpTable.CharacterLevelXPList[(int)level];

            return (long)levelTotalXP - TotalExperience.Value;
        }

        /// <summary>
        /// Returns the remaining XP required to the next level
        /// </summary>
        public ulong GetRemainingXP()
        {
            var maxLevel = GetMaxLevel();
            if (Level >= maxLevel)
                return 0;

            var nextLevelTotalXP = DatManager.PortalDat.XpTable.CharacterLevelXPList[Level.Value + 1];
            return nextLevelTotalXP - (ulong)TotalExperience.Value;
        }

        /// <summary>
        /// Returns the total XP required to reach a level
        /// </summary>
        public static ulong GetTotalXP(int level)
        {
            var maxLevel = GetMaxLevel();
            if (level < 0 || level > maxLevel)
                return 0;

            return DatManager.PortalDat.XpTable.CharacterLevelXPList[level];
        }

        /// <summary>
        /// Returns the total amount of XP required for a player reach max level
        /// </summary>
        public static long MaxLevelXP
        {
            get
            {
                var xpTable = DatManager.PortalDat.XpTable.CharacterLevelXPList;

                return (long)xpTable[xpTable.Count - 1];
            }
        }

        /// <summary>
        /// Returns the XP required to go from level A to level B
        /// </summary>
        public ulong GetXPBetweenLevels(int levelA, int levelB)
        {
            // special case for max level
            var maxLevel = (int)GetMaxLevel();

            levelA = Math.Clamp(levelA, 1, maxLevel - 1);
            levelB = Math.Clamp(levelB, 1, maxLevel);

            var levelA_totalXP = DatManager.PortalDat.XpTable.CharacterLevelXPList[levelA];
            var levelB_totalXP = DatManager.PortalDat.XpTable.CharacterLevelXPList[levelB];

            return levelB_totalXP - levelA_totalXP;
        }

        public ulong GetXPToNextLevel(int level)
        {
            return GetXPBetweenLevels(level, level + 1);
        }

        /// <summary>
        /// Determines if the player has advanced a level
        /// </summary>
        private void CheckForLevelup()
        {
            var xpTable = DatManager.PortalDat.XpTable;

            var maxLevel = GetMaxLevel();

            if (Level >= maxLevel) return;

            var startingLevel = Level;
            bool creditEarned = false;

            // increases until the correct level is found
            while ((ulong)(TotalExperience ?? 0) >= xpTable.CharacterLevelXPList[(Level ?? 0) + 1])
            {
                Level++;

                // increase the skill credits if the chart allows this level to grant a credit
                if (xpTable.CharacterLevelSkillCreditList[Level ?? 0] > 0)
                {
                    AvailableSkillCredits += (int)xpTable.CharacterLevelSkillCreditList[Level ?? 0];
                    TotalSkillCredits += (int)xpTable.CharacterLevelSkillCreditList[Level ?? 0];
                    creditEarned = true;
                }

                if (Level <= maxLevel && Level > (MaxReachedLevel ?? 1))
                    MaxReachedLevel = Level;

                // break if we reach max
                if (Level == maxLevel)
                {
                    PlayParticleEffect(PlayScript.WeddingBliss, Guid);
                    break;
                }
            }

            if (Level > startingLevel)
            {
                var message = (Level == maxLevel) ? $"You have reached the maximum level of {Level}!" : $"You are now level {Level}!";

                message += (AvailableSkillCredits > 0) ? $"\nYou have {AvailableExperience:#,###0} experience points and {AvailableSkillCredits} skill credits available to raise skills and attributes." : $"\nYou have {AvailableExperience:#,###0} experience points available to raise skills and attributes.";

                var levelUp = new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.Level, Level ?? 1);
                var currentCredits = new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.AvailableSkillCredits, AvailableSkillCredits ?? 0);

                if (Level != maxLevel && !creditEarned)
                {
                    var nextLevelWithCredits = 0;

                    for (int i = (Level ?? 0) + 1; i <= maxLevel; i++)
                    {
                        if (xpTable.CharacterLevelSkillCreditList[i] > 0)
                        {
                            nextLevelWithCredits = i;
                            break;
                        }
                    }
                    message += $"\nYou will earn another skill credit at level {nextLevelWithCredits}.";
                }

                ReachedLevelTimestamp = (int)Common.Time.GetUnixTime();

                if (Fellowship != null)
                    Fellowship.OnFellowLevelUp(this);

                if (AllegianceNode != null)
                    AllegianceNode.OnLevelUp();

                Session.Network.EnqueueSend(levelUp);

                SetMaxVitals();

                // play level up effect
                PlayParticleEffect(PlayScript.LevelUp, Guid);

                VerifyWieldedLevelRequirements();

                Session.Network.EnqueueSend(new GameMessageSystemChat(message, ChatMessageType.Advancement), currentCredits);

                if (Common.ConfigManager.Config.Server.WorldRuleset == Common.Ruleset.CustomDM)
                    UpdateCustomSkillFormulae();

                // Let's take the opportinity to send an activity recommendation to the player.
                var recommendationChain = new ActionChain();
                recommendationChain.AddDelaySeconds(5.0f);
                recommendationChain.AddAction(this, () =>
                {
                    PlayerCommands.SingleRecommendation(Session, true);
                });
                recommendationChain.EnqueueChain();
            }
        }

        /// <summary>
        /// Spends the amount of XP specified, deducting it from available experience
        /// </summary>
        public bool SpendXP(long amount, bool sendNetworkUpdate = true)
        {
            if (amount > AvailableExperience)
                return false;

            AvailableExperience -= amount;

            if (sendNetworkUpdate)
                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt64(this, PropertyInt64.AvailableExperience, AvailableExperience ?? 0));

            return true;
        }

        /// <summary>
        /// Tries to spend all of the players Xp into Attributes, Vitals and Skills
        /// </summary>
        public void SpendAllXp(bool sendNetworkUpdate = true)
        {
            SpendAllAvailableAttributeXp(Strength, sendNetworkUpdate);
            SpendAllAvailableAttributeXp(Endurance, sendNetworkUpdate);
            SpendAllAvailableAttributeXp(Coordination, sendNetworkUpdate);
            SpendAllAvailableAttributeXp(Quickness, sendNetworkUpdate);
            SpendAllAvailableAttributeXp(Focus, sendNetworkUpdate);
            SpendAllAvailableAttributeXp(Self, sendNetworkUpdate);

            SpendAllAvailableVitalXp(Health, sendNetworkUpdate);
            SpendAllAvailableVitalXp(Stamina, sendNetworkUpdate);
            SpendAllAvailableVitalXp(Mana, sendNetworkUpdate);

            foreach (var skill in Skills)
            {
                if (skill.Value.AdvancementClass >= SkillAdvancementClass.Trained)
                    SpendAllAvailableSkillXp(skill.Value, sendNetworkUpdate);
            }
        }

        /// <summary>
        /// Gives available XP of the amount specified, without increasing total XP
        /// </summary>
        public void RefundXP(long amount)
        {
            AvailableExperience += amount;

            var xpUpdate = new GameMessagePrivateUpdatePropertyInt64(this, PropertyInt64.AvailableExperience, AvailableExperience ?? 0);
            Session.Network.EnqueueSend(xpUpdate);
        }

        public void HandleMissingXp()
        {
            var verifyXp = GetProperty(PropertyInt64.VerifyXp) ?? 0;
            if (verifyXp == 0) return;

            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(5.0f);
            actionChain.AddAction(this, () =>
            {
                var xpType = verifyXp > 0 ? "unassigned experience" : "experience points";

                var msg = $"This character was missing some {xpType} --\nYou have gained an additional {Math.Abs(verifyXp).ToString("N0")} {xpType}!";

                Session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));

                if (verifyXp < 0)
                {
                    // add to character's total XP
                    TotalExperience -= verifyXp;

                    CheckForLevelup();
                }

                RemoveProperty(PropertyInt64.VerifyXp);
            });

            actionChain.EnqueueChain();
        }

        /// <summary>
        /// Returns the total amount of XP required to go from vitae to vitae + 0.01
        /// </summary>
        /// <param name="vitae">The current player life force, ie. 0.95f vitae = 5% penalty</param>
        /// <param name="level">The player DeathLevel, their level on last death</param>
        private double VitaeCPPoolThreshold(float vitae, int level)
        {
            if (Common.ConfigManager.Config.Server.WorldRuleset == Common.Ruleset.EoR)
                return (Math.Pow(level, 2.5) * 2.5 + 20.0) * Math.Pow(vitae, 5.0) + 0.5;
            else
            {
                // http://acpedia.org/wiki/Announcements_-_2005/07_-_Throne_of_Destiny_(expansion)#FAQ_-_AC:TD_Level_Cap_Update
                // "The vitae system has not changed substantially since Asheron's Call launched in 1999.
                // Since that time, the experience awarded by killing creatures has increased considerably.
                // This means that a 5% vitae loss currently is much easier to work off now than it was in the past.
                // In addition, the maximum cost to work off a point of vitae was capped at 12,500 experience points."
                return Math.Min((Math.Pow(level, 2) * 5 + 20) * Math.Pow(vitae, 5.0) + 0.5, 12500);
            }
        }

        

        public void GrantLevelProportionalXpForArena(double percent, long min, long max)
        {
            // temporarily give no xp
            return;

            var nextLevelXP = GetXPBetweenLevels(Level.Value, Level.Value + 1);

            var scaledXP = (long)Math.Round(nextLevelXP * percent);

            if (max > 0)
                scaledXP = Math.Min(scaledXP, max);

            if (min > 0)
                scaledXP = Math.Max(scaledXP, min);

            EarnXpForArena(scaledXP, XpType.Quest, ShareType.Allegiance);
        }

        private void EarnXpForArena(long amount, XpType xpType, ShareType shareType = ShareType.All)
        {

            //Console.WriteLine($"{Name}.EarnXP({amount}, {sharable}, {fixedAmount})");

            // apply xp modifiers.  Quest XP is multiplicative with general XP modification
            var questModifier = PropertyManager.GetDouble("quest_xp_modifier").Item;
            var modifier = PropertyManager.GetDouble("xp_modifier").Item;
            if (xpType == XpType.Quest)
                modifier *= questModifier;

            // should this be passed upstream to fellowship / allegiance?
            var enchantment = GetXPAndLuminanceModifier(xpType);

            var m_amount = (long)Math.Round(amount * enchantment * modifier);

            if (m_amount < 0)
            {
                log.Warn($"{Name}.EarnXP({amount}, {shareType})");
                log.Warn($"modifier: {modifier}, enchantment: {enchantment}, m_amount: {m_amount}");
                return;
            }

            GrantXPForArena(m_amount, xpType, shareType);

        }

        public void GrantXPForArena(long amount, XpType xpType, ShareType shareType = ShareType.All)
        {
            // Make sure UpdateXpAndLevel is done on this players thread
            EnqueueAction(new ActionEventDelegate(() => UpdateXpAndLevel(amount, xpType)));

            // only certain types of XP are granted to items
            if (xpType == XpType.Kill || xpType == XpType.Quest)
                GrantItemXP(amount);
        }


        /// <summary>
        /// Raise the available XP by a percentage of the current level XP or a maximum
        /// </summary>
        public void GrantLevelProportionalXp(double percent, long min, long max)
        {
            var nextLevelXP = GetXPBetweenLevels(Level.Value, Level.Value + 1);

            var scaledXP = (long)Math.Round(nextLevelXP * percent);

            if (max > 0)
                scaledXP = Math.Min(scaledXP, max);

            if (min > 0)
                scaledXP = Math.Max(scaledXP, min);

            // apply xp modifiers?
            EarnXP(scaledXP, XpType.Quest, Level, null, 0, null, ShareType.Allegiance);
        }

        /// <summary>
        /// The player earns XP for items that can be leveled up
        /// by killing creatures and completing quests,
        /// while those items are equipped.
        /// </summary>
        public void GrantItemXP(long amount)
        {
            foreach (var item in EquippedObjects.Values.Where(i => i.HasItemLevel))
                GrantItemXP(item, amount);
        }

        public void GrantItemXP(WorldObject item, long amount)
        {
            var prevItemLevel = item.ItemLevel.Value;
            var addItemXP = item.AddItemXP(amount);

            if (addItemXP > 0)
                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt64(item, PropertyInt64.ItemTotalXp, item.ItemTotalXp.Value));

            // handle item leveling up
            var newItemLevel = item.ItemLevel.Value;
            if (newItemLevel > prevItemLevel)
            {
                OnItemLevelUp(item, prevItemLevel);

                var actionChain = new ActionChain();
                actionChain.AddAction(this, () =>
                {
                    var msg = $"Your {item.Name} has increased in power to level {newItemLevel}!";
                    Session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));

                    EnqueueBroadcast(new GameMessageScript(Guid, PlayScript.AetheriaLevelUp));
                });
                actionChain.EnqueueChain();
            }
        }

        /// <summary>
        /// Returns the multiplier to XP and Luminance from Trinkets and Augmentations
        /// </summary>
        public float GetXPAndLuminanceModifier(XpType xpType)
        {
            var enchantmentBonus = EnchantmentManager.GetXPBonus();

            var augBonus = 0.0f;
            if (xpType == XpType.Kill && AugmentationBonusXp > 0)
                augBonus = AugmentationBonusXp * 0.05f;

            var modifier = 1.0f + enchantmentBonus + augBonus;
            //Console.WriteLine($"XPAndLuminanceModifier: {modifier}");

            return modifier;
        }

        public void RevertToBrandNewCharacter(bool keepFellowship, bool keepAllegiance, bool keepHousing, bool keepBondedEquipment, bool keepSpells, bool setToLimboGameplayMode = false, long startingXP = 0)
        {
            if(!keepFellowship)
                FellowshipQuit(false);

            if (!keepAllegiance)
                AllegianceManager.HandlePlayerDelete(Guid.Full);

            // Reset Gameplay Mode
            PlayerKillerStatus = PlayerKillerStatus.NPK;
            PkLevel = PKLevel.NPK;
            GameplayMode = GameplayModes.Regular;
            GameplayModeExtraIdentifier = 0;
            GameplayModeIdentifierString = null;

            // Reset T.A.R.
            CampManager.EraseAll();

            // Reset Food
            ExtraHealthRegenPool = 0;
            ExtraStaminaRegenPool = 0;
            ExtraManaRegenPool = 0;

            // Reset Titles
            RemoveAllTitles();
            if(setToLimboGameplayMode)
                AddTitle((uint)CharacterTitle.DeadMeat, true, true, true); // This title was replaced with the "In Limbo" title.
            else if (ChargenTitleId > 0)
                AddTitle((uint)ChargenTitleId, true, true, true);

            // Reset buffs and vitae
            EnchantmentManager.RemoveVitae();
            EnchantmentManager.RemoveAllEnchantments();

            // Reset trackers
            XpTrackerStartTimestamp = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
            XpTrackerTotalXp = 0;
            NumDeaths = 0;
            PlayerKillsPkl = 0;
            PlayerKillsPk = 0;
            DeathLevel = 0;
            Age = 0;
            initialAgeTime = DateTime.MinValue;
            ImbueAttempts = 0;
            ImbueSuccesses = 0;
            SetProperty(PropertyString.DateOfBirth, $"{DateTime.UtcNow:dd MMMM yyyy}");

            // Reset positions
            RemovePosition(PositionType.LastOutsideDeath);
            RemovePosition(PositionType.LastPortal);
            RemovePosition(PositionType.LinkedLifestone);
            RemovePosition(PositionType.LinkedPortalOne);
            RemovePosition(PositionType.LinkedPortalTwo);

            if (!keepSpells)
            {
                RemoveAllSpells();
                LeyLineSeed = null;
            }

            // Reset Quest Timers
            QuestManager.EraseAll();

            // Reset Contracts
            ContractManager.EraseAll();

            // Reset Exploration Contracts
            Exploration1LandblockId = 0;
            Exploration1KillProgressTracker = 0;
            Exploration1MarkerProgressTracker = 0;
            Exploration1Description = "";
            Exploration1LandblockReached = false;
            Exploration2LandblockId = 0;
            Exploration2KillProgressTracker = 0;
            Exploration2MarkerProgressTracker = 0;
            Exploration2Description = "";
            Exploration2LandblockReached = false;
            Exploration3LandblockId = 0;
            Exploration3KillProgressTracker = 0;
            Exploration3MarkerProgressTracker = 0;
            Exploration3Description = "";
            Exploration3LandblockReached = false;

            // Reset Attributes
            var propertyCount = Enum.GetNames(typeof(PropertyAttribute)).Length;
            for (var i = 1; i < propertyCount; i++)
            {
                var attribute = (PropertyAttribute)i;

                Attributes[attribute].Ranks = 0;
                Attributes[attribute].ExperienceSpent = 0;
            }

            propertyCount = Enum.GetNames(typeof(PropertyAttribute2nd)).Length;
            for (var i = 1; i < propertyCount; i += 2)
            {
                var attribute = (PropertyAttribute2nd)i;

                Vitals[attribute].Ranks = 0;
                Vitals[attribute].ExperienceSpent = 0;
            }

            // Reset Skills
            propertyCount = Enum.GetNames(typeof(Skill)).Length;
            for (var i = 1; i < propertyCount; i++)
            {
                var skill = (Skill)i;

                ResetSkill(skill, false, true);
            }

            AvailableExperience = 0;

            var heritageGroup = DatManager.PortalDat.CharGen.HeritageGroups[(uint)(Heritage ?? 1)];
            AvailableSkillCredits = (int)heritageGroup.SkillCredits;

            // Reset Luminance
            LumAugDamageRating = 0;
            LumAugDamageReductionRating = 0;
            LumAugCritDamageRating = 0;
            LumAugCritReductionRating = 0;
            //LumAugSurgeEffectRating = 0;
            LumAugSurgeChanceRating = 0;
            LumAugItemManaUsage = 0;
            LumAugItemManaGain = 0;
            LumAugVitality = 0;
            LumAugHealingRating = 0;
            LumAugSkilledCraft = 0;
            LumAugSkilledSpec = 0;
            LumAugAllSkills = 0;

            AvailableLuminance = null;
            MaximumLuminance = null;

            // Reset Society
            Faction1Bits = null;
            SocietyRankCelhan = null;
            SocietyRankEldweb = null;
            SocietyRankRadblo = null;

            // Reset Aetheria
            AetheriaFlags = AetheriaBitfield.None;

            // Remove Level
            TotalExperience = 0;
            Level = 1;

            // Add starter skills
            if (ChargenSkillsTrained != null)
            {
                var skillsToTrain = ChargenSkillsTrained.Split("|");
                foreach (var skillString in skillsToTrain)
                {
                    if (int.TryParse(skillString, out var skillId))
                    {
                        var skill = DatManager.PortalDat.SkillTable.SkillBaseHash[(uint)skillId];
                        var trainedCost = skill.TrainedCost;

                        foreach (var skillGroup in heritageGroup.Skills)
                        {
                            if (skillGroup.SkillNum == skillId)
                            {
                                trainedCost = skillGroup.NormalCost;
                                break;
                            }
                        }

                        TrainSkill((Skill)skillId, trainedCost, true);
                    }
                }
            }

            if (ChargenSkillsSpecialized != null)
            {
                var skillsToSpecialize = ChargenSkillsSpecialized.Split("|");
                foreach (var skillString in skillsToSpecialize)
                {
                    if (int.TryParse(skillString, out var skillId))
                    {
                        var skill = DatManager.PortalDat.SkillTable.SkillBaseHash[(uint)skillId];
                        var trainedCost = skill.TrainedCost;
                        var specializedCost = skill.UpgradeCostFromTrainedToSpecialized;

                        foreach (var skillGroup in heritageGroup.Skills)
                        {
                            if (skillGroup.SkillNum == skillId)
                            {
                                trainedCost = skillGroup.NormalCost;
                                specializedCost = skillGroup.PrimaryCost;
                                break;
                            }
                        }

                        TrainSkill((Skill)skillId, trainedCost);
                        SpecializeSkill((Skill)skillId, specializedCost);
                    }
                }
            }

            if (ChargenSkillsSecondary != null)
            {
                var skillsToSetAsSecondary = ChargenSkillsSecondary.Split("|");
                foreach (var skillString in skillsToSetAsSecondary)
                {
                    var skillAndPrimarySkill = skillString.Split(":");
                    if(skillAndPrimarySkill.Length == 2)
                    {
                        if (int.TryParse(skillAndPrimarySkill[0], out var secondarySkillId) && int.TryParse(skillAndPrimarySkill[1], out var primarySkillId))
                        {
                            var primarySkill = GetCreatureSkill((Skill)primarySkillId);
                            var secondarySkill = GetCreatureSkill((Skill)secondarySkillId);

                            if(primarySkill.AdvancementClass > SkillAdvancementClass.Untrained && secondarySkill.AdvancementClass > SkillAdvancementClass.Untrained)
                            {
                                secondarySkill.SecondaryTo = (Skill)primarySkillId;
                            }
                        }
                    }
                }
            }

            if (Common.ConfigManager.Config.Server.WorldRuleset == Common.Ruleset.EoR)
            {
                // Set Heritage based Melee and Ranged Masteries
                PlayerFactory.GetMasteries(HeritageGroup, out WeaponType meleeMastery, out WeaponType rangedMastery);

                SetProperty(PropertyInt.MeleeMastery, (int)meleeMastery);
                SetProperty(PropertyInt.RangedMastery, (int)rangedMastery);

                // Set innate augs
                PlayerFactory.SetInnateAugmentations(this);
            }

            if (startingXP > 0)
                UpdateXpAndLevel(startingXP, XpType.Admin);

            // Leave this for last as it could potentially block some actions during the reset process.
            if (setToLimboGameplayMode)
            {
                GameplayMode = GameplayModes.Limbo;
                SetProperty(PropertyBool.RecallsDisabled, true);
            }

            RevertToBrandNewCharacterEquipment(keepHousing, keepBondedEquipment);

            Session.Network.EnqueueSend(new GameEventPlayerDescription(Session));
            EnqueueBroadcast(new GameMessagePublicUpdatePropertyInt(this, PropertyInt.PlayerKillerStatus, (int)PlayerKillerStatus), new GameMessagePublicUpdatePropertyInt(this, PropertyInt.PkLevelModifier, PkLevelModifier));
        }

        HashSet<uint> BondedItemBlacklist = new HashSet<uint>()
        {
            23045, // Coordination To Endurance Gem
            23046, // Coordination To Focus Gem
            23047, // Coordination To Quickness Gem
            23048, // Coordination To Self Gem
            23049, // Coordination To Strength Gem
            23050, // Endurance To Coordination Gem
            23051, // Endurance To Focus Gem
            23052, // Endurance To Quickness Gem
            23053, // Endurance To Self Gem
            23054, // Endurance To Strength Gem
            23055, // Focus To Coordination Gem
            23056, // Focus To Endurance Gem
            23057, // Focus To Quickness Gem
            23058, // Focus To Self Gem
            23059, // Focus To Strength Gem
            23060, // Quickness To Coordination Gem
            23061, // Quickness To Endurance Gem
            23062, // Quickness To Focus Gem
            23063, // Quickness To Self Gem
            23064, // Quickness To Strength Gem
            23065, // Self To Coordination Gem
            23066, // Self To Endurance Gem
            23067, // Self To Focus Gem
            23068, // Self To Quickness Gem
            23069, // Self To Strength Gem
            23070, // Strength To Coordination Gem
            23071, // Strength To Endurance Gem
            23072, // Strength To Focus Gem
            23073, // Strength To Quickness Gem
            23074, // Strength To Self Gem
            22937, // Gem of Lowering Coordination
            22938, // Gem of Lowering Endurance
            22939, // Gem of Lowering Focus
            22940, // Gem of Lowering Quickness
            22941, // Gem of Lowering Self
            22942, // Gem of Lowering Strength
            22943, // Gem of Raising Coordination
            22944, // Gem of Raising Endurance
            22945, // Gem of Raising Focus
            22946, // Gem of Raising Quickness
            22947, // Gem of Raising Self
            22948, // Gem of Raising Strength
            50180, // Exploration Contract
            50189, //Ring of Impermanency
            22315, // Alchemy Gem of Forgetfulness
            22316, // Arcane Lore Gem of Forgetfulness
            22317, // Armor Tinkering Gem of Forgetfulness
            22318, // Axe and Mace Gem of Forgetfulness
            22319, // Bow and Crossbow Gem of Forgetfulness
            22320, // Cooking Gem of Forgetfulness
            22321, // Assess Gem of Forgetfulness
            22322, // Creature Enchantment Gem of Forgetfulness
            22323, // Crossbow Gem of Forgetfulness
            22324, // Dagger Gem of Forgetfulness
            22325, // Deception Gem of Forgetfulness
            22326, // Fletching Gem of Forgetfulness
            22327, // Healing Gem of Forgetfulness
            22328, // Item Tinkering Gem of Forgetfulness
            22329, // Item Enchantment Gem of Forgetfulness
            22330, // Jump Gem of Forgetfulness
            22331, // Leadership Gem of Forgetfulness
            22332, // Life Magic Gem of Forgetfulness
            22333, // Lockpick Gem of Forgetfulness
            22334, // Loyalty Gem of Forgetfulness
            22335, // Mace Gem of Forgetfulness
            22336, // Magic Defense Gem of Forgetfulness
            22337, // Magic Item Tinkering Gem of Forgetfulness
            22338, // Mana Conversion Gem of Forgetfulness
            22339, // Melee Defense Gem of Forgetfulness
            22340, // Missile Defense Gem of Forgetfulness
            22341, // Personal Appraisal Gem of Forgetfulness
            22342, // Run Gem of Forgetfulness
            22343, // Spear and Staff Gem of Forgetfulness
            22344, // Staff Gem of Forgetfulness
            22345, // Sword Gem of Forgetfulness
            22346, // Thrown Weapon Gem of Forgetfulness
            22347, // Unarmed Combat Gem of Forgetfulness
            22348, // War Magic Gem of Forgetfulness
            22349, // Weapon Tinkering Gem of Forgetfulness
            22350, // Alchemy Gem of Enlightenment
            22351, // Arcane Lore Gem of Enlightenment
            22352, // Armor Tinkering Gem of Enlightenment
            22353, // Axe and Mace Gem of Enlightenment
            22354, // Bow and Crossbow Gem of Enlightenment
            22355, // Cooking Gem of Enlightenment
            22356, // Assess Gem of Enlightenment
            22357, // Creature Enchantment Gem of Enlightenment
            22358, // Crossbow Gem of Enlightenment
            22359, // Dagger Gem of Enlightenment
            22360, // Deception Gem of Enlightenment
            22361, // Fletching Gem of Enlightenment
            22362, // Healing Gem of Enlightenment
            22363, // Item Tinkering Gem of Enlightenment
            22364, // Item Enchantment Gem of Enlightenment
            22365, // Jump Gem of Enlightenment
            22366, // Leadership Gem of Enlightenment
            22367, // Life Magic Gem of Enlightenment
            22368, // Lockpick Gem of Enlightenment
            22369, // Loyalty Gem of Enlightenment
            22370, // Mace Gem of Enlightenment
            22371, // Magic Defense Gem of Enlightenment
            22372, // Magic Item Tinkering Gem of Enlightenment
            22373, // Mana Conversion Gem of Enlightenment
            22374, // Melee Defense Gem of Enlightenment
            22375, // Missile Defense Gem of Enlightenment
            22376, // Personal Appraisal Gem of Enlightenment
            22377, // Run Gem of Enlightenment
            22378, // Spear and Staff Gem of Enlightenment
            22379, // Staff Gem of Enlightenment
            22380, // Sword Gem of Enlightenment
            22381, // Thrown Weapon Gem of Enlightenment
            22382, // Unarmed Combat Gem of Enlightenment
            22383, // War Magic Gem of Enlightenment
            22384, // Weapon Tinkering Gem of Enlightenment
            28926, // Salvaging Gem of Forgetfulness
            45378, // Shield Gem of Forgetfulness
            45383, // Shield Gem of Enlightenment
            50095, // Armor Gem of Enlightenment
            50096, // Appraise Gem of Enlightenment
            50097, // Awareness Gem of Enlightenment
            50098, // Sneaking Gem of Enlightenment
            50099, // Appraise Gem of Forgetfulness
            50100, // Armor Gem of Forgetfulness
            50101, // Awareness Gem of Forgetfulness
            50102 // Sneaking Gem of Forgetfulness
        };

        public void RevertToBrandNewCharacterEquipment(bool keepHousing, bool keepBondedEquipment)
        {
            // Destroy all items
            var inventory = GetAllPossessions();
            var inventoryToDelete = new List<WorldObject>();

            var keepNonEquippable = PropertyManager.GetBool("dekaru_hc_keep_non_equippable_bonded_on_death").Item;
            foreach (var item in inventory)
            {
                if (keepHousing && item.WeenieType == WeenieType.Deed) // Keep houses
                {
                    if (item.CurrentWieldedLocation != null || item.Container != this)
                        HandleActionPutItemInContainer(item.Guid.Full, Guid.Full);
                    continue;
                }

                if (item.WeenieType == WeenieType.Gem && item.Container.WeenieClassId == 60001) // Gem pouch
                    continue;

                if (item.WeenieType == WeenieType.SpellComponent && item.Container.WeenieClassId == 50009) // Spell Component Pouch
                    continue;

                if (keepBondedEquipment
                    && (keepNonEquippable || (item.ValidLocations ?? EquipMask.None) != EquipMask.None) && item.Bonded == BondedStatus.Bonded
                    && !BondedItemBlacklist.Contains(item.WeenieClassId))
                {
                    if(item.CurrentWieldedLocation != null || item.Container != this && item.Container.Bonded != BondedStatus.Bonded)
                        HandleActionPutItemInContainer(item.Guid.Full, Guid.Full);
                    continue;
                }

                inventoryToDelete.Add(item);
            }

            foreach (var item in inventoryToDelete)
            {
                item.DeleteObject(this);
            }

            if (ChargenClothing != null)
            {
                var chargenClothingList = ChargenClothing.Split("|");
                if (chargenClothingList.Length == 12)
                {
                    bool a = uint.TryParse(chargenClothingList[0], out var hatWcid);
                    bool b = uint.TryParse(chargenClothingList[1], out var hatPalette);
                    bool c = double.TryParse(chargenClothingList[2], out var hatShade);

                    bool d = uint.TryParse(chargenClothingList[3], out var shirtWcid);
                    bool e = uint.TryParse(chargenClothingList[4], out var shirtPalette);
                    bool f = double.TryParse(chargenClothingList[5], out var shirtShade);

                    bool g = uint.TryParse(chargenClothingList[6], out var pantsWcid);
                    bool h = uint.TryParse(chargenClothingList[7], out var pantsPalette);
                    bool i = double.TryParse(chargenClothingList[8], out var pantsShade);

                    bool j = uint.TryParse(chargenClothingList[9], out var footwearWcid);
                    bool k = uint.TryParse(chargenClothingList[10], out var footwearPalette);
                    bool l = double.TryParse(chargenClothingList[11], out var footwearShade);

                    if (a && b && c)
                    {
                        var hat = PlayerFactory.GetClothingObject(hatWcid, hatPalette, hatShade);
                        if (hat != null)
                            TryEquipObjectWithNetworking(hat, hat.ValidLocations ?? 0);
                    }

                    if (d && e && f)
                    {
                        var shirt = PlayerFactory.GetClothingObject(shirtWcid, shirtPalette, shirtShade);
                        if (shirt != null)
                            TryEquipObjectWithNetworking(shirt, shirt.ValidLocations ?? 0);
                    }

                    if (g && h && i)
                    {
                        var pants = PlayerFactory.GetClothingObject(pantsWcid, pantsPalette, pantsShade);
                        if (pants != null)
                            TryEquipObjectWithNetworking(pants, pants.ValidLocations ?? 0);
                    }

                    if (j && k && l)
                    {
                        var footwear = PlayerFactory.GetClothingObject(footwearWcid, footwearPalette, footwearShade);
                        if (footwear != null)
                            TryEquipObjectWithNetworking(footwear, footwear.ValidLocations ?? 0);
                    }
                }
            }

            PlayerFactory.GrantStarterItems(this);

            SendInventoryAndWieldedItems();
            UpdateCoinValue();
            UpdateTradeNoteValue();
        }

        public CreatureAttribute GetCostToRaiseViaAttribute(Skill skill, PropertyAttribute2nd vital, out uint xpToSkillUpViaAttribute, out int ranksForSkillPoint)
        {
            var skillTable = DatManager.PortalDat.SkillTable;
            var vitalTable = DatManager.PortalDat.SecondaryAttributeTable;

            var attributeXpTable = DatManager.PortalDat.XpTable.AttributeXpList;

            var maxRank = attributeXpTable.Count - 1;

            DatLoader.Entity.SkillFormula formula;

            if (skill != Skill.None)
            {
                if (!skillTable.SkillBaseHash.TryGetValue((uint)skill, out SkillBase skillBase))
                {
                    xpToSkillUpViaAttribute = uint.MaxValue;
                    ranksForSkillPoint = 0;
                    return null;
                }
                formula = skillBase.Formula;
            }
            else
            {
                switch (vital)
                {
                    case PropertyAttribute2nd.MaxHealth:  formula = vitalTable.MaxHealth.Formula; break;
                    case PropertyAttribute2nd.MaxStamina: formula = vitalTable.MaxStamina.Formula; break;
                    case PropertyAttribute2nd.MaxMana:    formula = vitalTable.MaxMana.Formula; break;
                    default:
                        xpToSkillUpViaAttribute = uint.MaxValue;
                        ranksForSkillPoint = 0;
                        return null;
                }
            }

            var ranksForSkillPointAttr1 = (int)Math.Ceiling((float)formula.Z / formula.X);
            var xpToSkillUpAttr1 = uint.MaxValue;
            CreatureAttribute attribute1 = null;
            if (formula.Attr1 != 0)
            {
                attribute1 = Attributes[(PropertyAttribute)formula.Attr1];
                if (!attribute1.IsMaxRank)
                {
                    var startRank = (int)(attribute1.Base - attribute1.StartingValue);
                    var destRank = Math.Min(startRank + ranksForSkillPointAttr1, maxRank);
                    ranksForSkillPointAttr1 = destRank - startRank;

                    xpToSkillUpAttr1 = (uint)GetXPBetweenAttributeLevels(startRank, destRank);
                }
            }

            var ranksForSkillPointAttr2 = (int)Math.Ceiling((float)formula.Z / formula.Y);
            var xpToSkillUpAttr2 = uint.MaxValue;
            CreatureAttribute attribute2 = null;
            if (formula.Attr2 != 0)
            {
                attribute2 = Attributes[(PropertyAttribute)formula.Attr2];
                if (!attribute2.IsMaxRank)
                {
                    var startRank = (int)(attribute2.Base - attribute2.StartingValue);
                    var destRank = Math.Min(startRank + ranksForSkillPointAttr2, maxRank);
                    ranksForSkillPointAttr2 = destRank - startRank;

                    xpToSkillUpAttr2 = (uint)GetXPBetweenAttributeLevels(startRank, destRank);
                }
            }

            if (xpToSkillUpAttr1 <= xpToSkillUpAttr2)
            {
                xpToSkillUpViaAttribute = xpToSkillUpAttr1;
                ranksForSkillPoint = ranksForSkillPointAttr1;
                return attribute1;
            }
            else
            {
                xpToSkillUpViaAttribute = xpToSkillUpAttr2;
                ranksForSkillPoint = ranksForSkillPointAttr2;
                return attribute2;
            }
        }

        private struct CreatureSkillVitalOrAttribute
        {
            public CreatureSkill Skill;
            public CreatureVital Vital;
            public CreatureAttribute Attribute;

            public CreatureSkillVitalOrAttribute(CreatureSkill skill)
            {
                Skill = skill;
                Vital = null;
                Attribute = null;
            }

            public CreatureSkillVitalOrAttribute(CreatureVital vital)
            {
                Skill = null;
                Vital = vital;
                Attribute = null;
            }

            public CreatureSkillVitalOrAttribute(CreatureAttribute attribute)
            {
                Skill = null;
                Vital = null;
                Attribute = attribute;
            }

            public bool IsVital()
            {
                return Vital != null;
            }

            public bool IsSkill()
            {
                return Skill != null;
            }

            public bool IsAttribute()
            {
                return Attribute != null;
            }
        }
        public void AutoSpendXp(Dictionary<Skill, float> prioritiesPreset)
        {
            if (prioritiesPreset == null || prioritiesPreset.Count == 0)
                return;

            List<Tuple<long, CreatureSkillVitalOrAttribute>> currentPriorityList = new List<Tuple<long, CreatureSkillVitalOrAttribute>>();

            SortedDictionary<PropertyAttribute, int> spentMapAttribute = new SortedDictionary<PropertyAttribute, int>();
            SortedDictionary<PropertyAttribute2nd, int> spentMapVital = new SortedDictionary<PropertyAttribute2nd, int>();
            SortedDictionary<Skill, int> spentMapSkill = new SortedDictionary<Skill, int>();

            long totalExperienceSpent = 0;

            for (var attemptCount = 0; attemptCount < 10000; attemptCount++)
            {
                currentPriorityList.Clear();

                foreach (var skill in Skills)
                {
                    if (skill.Value.AdvancementClass < SkillAdvancementClass.Trained)
                        continue;

                    if (prioritiesPreset.TryGetValue(skill.Key, out var priority))
                    {
                        if (priority == 0)
                            continue;

                        var experienceSpent = (long)Math.Max(skill.Value.ExperienceSpent / priority, 1);

                        currentPriorityList.Add(new Tuple<long, CreatureSkillVitalOrAttribute>(experienceSpent, new CreatureSkillVitalOrAttribute(skill.Value)));
                    }
                }

                foreach (var vital in Vitals)
                {
                    var skill = Skill.None;
                    switch(vital.Key)
                    {
                        case PropertyAttribute2nd.MaxHealth: skill = (Skill)100; break;
                        case PropertyAttribute2nd.MaxStamina: skill = (Skill)101; break;
                        case PropertyAttribute2nd.MaxMana: skill = (Skill)102; break;
                        default:
                            continue;
                    }
                    if (prioritiesPreset.TryGetValue(skill, out var priority))
                    {
                        if (priority == 0)
                            continue;

                        var experienceSpent = (long)Math.Max(vital.Value.ExperienceSpent / priority, 1);
                        currentPriorityList.Add(new Tuple<long, CreatureSkillVitalOrAttribute>(experienceSpent, new CreatureSkillVitalOrAttribute(vital.Value)));
                    }
                }

                foreach (var attribute in Attributes)
                {
                    var skill = Skill.None;
                    switch (attribute.Key)
                    {
                        case PropertyAttribute.Strength: skill = (Skill)200; break;
                        case PropertyAttribute.Endurance: skill = (Skill)201; break;
                        case PropertyAttribute.Coordination: skill = (Skill)202; break;
                        case PropertyAttribute.Quickness: skill = (Skill)203; break;
                        case PropertyAttribute.Focus: skill = (Skill)204; break;
                        case PropertyAttribute.Self: skill = (Skill)205; break;
                        default:
                            continue;
                    }
                    if (prioritiesPreset.TryGetValue(skill, out var priority))
                    {
                        if (priority == 0)
                            continue;

                        var experienceSpent = (long)Math.Max(attribute.Value.ExperienceSpent / priority, 1);
                        currentPriorityList.Add(new Tuple<long, CreatureSkillVitalOrAttribute>(experienceSpent, new CreatureSkillVitalOrAttribute(attribute.Value)));
                    }
                }

                var skillIncreased = false;
                currentPriorityList.Sort((x, y) => x.Item1.CompareTo(y.Item1));
                foreach (var skillPriority in currentPriorityList)
                {
                    if (skillPriority.Item2.IsSkill())
                    {
                        var skill = skillPriority.Item2.Skill;
                        var skillXPTable = GetSkillXPTable(skill.AdvancementClass);

                        if (skill.AdvancementClass <= SkillAdvancementClass.Untrained)
                            continue;

                        var xpToNextRank = GetXpToNextRank(skill) ?? uint.MaxValue;

                        var attribute = GetCostToRaiseViaAttribute(skill.Skill, PropertyAttribute2nd.Undef, out var xpToSkillUpViaAttribute, out var ranksForSkillPoint);

                        if (!skill.IsMaxRank && xpToSkillUpViaAttribute > xpToNextRank)
                        {
                            if (AvailableExperience < xpToNextRank)
                                continue;

                            SpendSkillXp(skill, xpToNextRank, false);
                            totalExperienceSpent += xpToNextRank;

                            spentMapSkill.TryGetValue(skill.Skill, out var timesRaised);
                            spentMapSkill[skill.Skill] = timesRaised + 1;

                        }
                        else if (attribute != null)
                        {
                            if (attribute.IsMaxRank || AvailableExperience < xpToSkillUpViaAttribute)
                                continue;

                            SpendAttributeXp(attribute, xpToSkillUpViaAttribute, false);
                            totalExperienceSpent += xpToSkillUpViaAttribute;

                            spentMapAttribute.TryGetValue(attribute.Attribute, out var timesRaised);
                            spentMapAttribute[attribute.Attribute] = timesRaised + ranksForSkillPoint;
                        }
                        else
                            continue;
                    }
                    else if (skillPriority.Item2.IsVital())
                    {
                        var vital = skillPriority.Item2.Vital;
                        var vitalXPTable = DatManager.PortalDat.XpTable.VitalXpList;

                        var xpToNextRank = GetXpToNextRank(vital) ?? uint.MaxValue;

                        var attribute = GetCostToRaiseViaAttribute(Skill.None, vital.Vital, out var xpToSkillUpViaAttribute, out var ranksForSkillPoint);

                        if (!vital.IsMaxRank && xpToSkillUpViaAttribute > xpToNextRank)
                        {
                            if (AvailableExperience < xpToNextRank)
                                continue;

                            SpendVitalXp(vital, xpToNextRank, false);
                            totalExperienceSpent += xpToNextRank;

                            spentMapVital.TryGetValue(vital.Vital, out var timesRaised);
                            spentMapVital[vital.Vital] = timesRaised + 1;

                        }
                        else if (attribute != null)
                        {
                            if (attribute.IsMaxRank || AvailableExperience < xpToSkillUpViaAttribute)
                                continue;

                            SpendAttributeXp(attribute, xpToSkillUpViaAttribute, false);
                            totalExperienceSpent += xpToSkillUpViaAttribute;

                            spentMapAttribute.TryGetValue(attribute.Attribute, out var timesRaised);
                            spentMapAttribute[attribute.Attribute] = timesRaised + ranksForSkillPoint;
                        }
                        else
                            continue;
                    }
                    else if (skillPriority.Item2.IsAttribute())
                    {
                        var attribute = skillPriority.Item2.Attribute;
                        var attributeXPTable = DatManager.PortalDat.XpTable.AttributeXpList;

                        var xpToNextRank = GetXpToNextRank(attribute) ?? uint.MaxValue;

                        if (!attribute.IsMaxRank)
                        {
                            if (AvailableExperience < xpToNextRank)
                                continue;

                            SpendAttributeXp(attribute, xpToNextRank);
                            totalExperienceSpent += xpToNextRank;

                            spentMapAttribute.TryGetValue(attribute.Attribute, out var timesRaised);
                            spentMapAttribute[attribute.Attribute] = timesRaised + 1;
                        }
                        else
                            continue;
                    }
                    else
                        continue;

                    skillIncreased = true;
                    break;
                }

                if (!skillIncreased)
                    break;
            }

            Session.Network.EnqueueSend(new GameEventPlayerDescription(Session));

            foreach (var entry in spentMapAttribute)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"Your {entry.Key} has been raised {entry.Value} times.", ChatMessageType.Broadcast));
            }

            foreach (var entry in spentMapVital)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"Your {entry.Key.ToSentence()} has been raised {entry.Value} times.", ChatMessageType.Broadcast));
            }

            foreach (var entry in spentMapSkill)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"Your {entry.Key.ToSentence()} skill has been raised {entry.Value} times.", ChatMessageType.Broadcast));
            }

            Session.Network.EnqueueSend(new GameMessageSystemChat($"Total experience spent: {totalExperienceSpent:N0}.", ChatMessageType.Broadcast));
        }
    }
}
