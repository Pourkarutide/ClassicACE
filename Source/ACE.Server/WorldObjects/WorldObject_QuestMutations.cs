using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using System.Linq;
using ACE.Common;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Factories.Tables;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects.Entity;
using Org.BouncyCastle.Asn1.X509;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        private enum QuestItemWeaponMutationType
        {
            None = 0,
            Slayer = 1,
            AttributeCantrip = 2,
            SkillCantrip = 3,
            ItemCantrip = 4,
            Rend = 5
        }

        private enum QuestItemClothingMutationType
        {
            None = 0,
            ArmorLevel = 1,
            AttributeCantrip = 2,
            SkillCantrip = 3,
            Rating = 4,
            EquipmentSet = 5
        }

        private enum QuestItemJewelryMutationType
        {
            None = 0,
            AttributeCantrip = 1,
            SkillCantrip = 2,
        }

        public string MutateQuestItem()
        {
            //Validate this is not a loot gen item, and that its a caster, weapon, armor or clothing
            // Morosity - adding Ammo check here
            if
            (
               (this.ItemType != ItemType.Caster &&
                this.ItemType != ItemType.MeleeWeapon &&
                this.ItemType != ItemType.MissileWeapon &&
                this.ItemType != ItemType.Armor &&
                this.ItemType != ItemType.Clothing &&
                this.ItemType != ItemType.Jewelry) ||
               (this.Workmanship.HasValue &&
                this.Workmanship.Value > 0) ||
                this is Ammunition 
            )
            {
                return "";
            }

            //Make sure the item is not on the mutation blacklist
            if (QuestItemMutations.IsQuestItemMutationDisallowed(this.WeenieClassId))
                return "This item cannot be mutated!";

            StringBuilder resultMessage = new StringBuilder();
            var mutationTier = GetMutationTier();

            double roll = 0;

            //Get the number of mutations to apply
            //50% chance for first mutation
            //15% chance for a second mutation
            //1% chance for a third mutation
            //TODO should this be dictated by mutation tier?
            //TODO should make the chances configurable?
            roll = ThreadSafeRandom.Next(0f, 1f);
            int mutationCount = 0;
            if(roll < 0.10)
            {
                mutationCount = 3;
            }
            else if (roll < 0.35)
            {
                mutationCount = 2;
            }
            else if (roll < .50)
            {
                mutationCount = 1;
            }

            if(mutationCount < 1)
            {
                //TODO - do we return any message?
                return "";
            }

            //Roll which types of mutations to apply            
            //  Slayer / ArmorLevel = 1
            //  AttributeCantrip = 2
            //  SkillCantrip = 3         
            //  ItemCantrip / EquipmentSet = 4
            //  Rend / Rating = 5
            List<int> mutationTypes = new List<int>();

            for(int i = 0; i < mutationCount; i++)
            {
                int mutationType = ThreadSafeRandom.Next(1, 5);

                //For Slayers, Rends, Sets and Steel Tinks, don't allow those to be added more than once, reroll if already added
                int rerollAttempts = 0; //just a fail-safe to avoid an infinite loop, even tho it shouldn't be possible
                while ((mutationType == 1 || mutationType == 5) && mutationTypes.Contains(mutationType) && rerollAttempts < 20)
                {
                    mutationType = ThreadSafeRandom.Next(1, 5);
                    rerollAttempts++;
                }

                mutationTypes.Add(mutationType);
            }

            //Apply the mutations
            foreach (int mutationType in mutationTypes)
            {
                if (this.ItemType == ItemType.MeleeWeapon || this.ItemType == ItemType.MissileWeapon || this.ItemType == ItemType.Caster)
                {
                    switch (mutationType)
                    {
                        case 1: //Slayer
                            resultMessage.Append(QuestItem_ApplySlayerMutation(mutationTier) + "\n");
                            break;
                        case 2: //AttributeCantrip
                            resultMessage.Append(QuestItem_ApplyAttributeCantripMutation(mutationTier) + "\n");
                            break;
                        case 3: //SkillCantrip
                            resultMessage.Append(QuestItem_ApplySkillCantripMutation(mutationTier) + "\n");
                            break;
                        case 4: //ItemCantrip
                            resultMessage.Append(QuestItem_ApplyItemCantripMutation(mutationTier) + "\n");
                            break;
                        case 5: //Rend
                            resultMessage.Append(QuestItem_ApplyRendMutation(mutationTier) + "\n");
                            break;
                    }
                }
                else if (this.ItemType == ItemType.Armor || this.ItemType == ItemType.Clothing)
                {
                    switch (mutationType)
                    {
                        case 1: //ArmorLevel
                            resultMessage.Append(QuestItem_ApplyArmorLevelMutation(mutationTier) + "\n");
                            break;
                        case 2: //AttributeCantrip
                            resultMessage.Append(QuestItem_ApplyAttributeCantripMutation(mutationTier) + "\n");
                            break;
                        case 3: //SkillCantrip
                            resultMessage.Append(QuestItem_ApplySkillCantripMutation(mutationTier) + "\n");
                            break;
                        case 4: //Rating
                            resultMessage.Append(QuestItem_ApplyRatingMutation(mutationTier) + "\n");
                            break;
                        case 5: //EquipmentSet
                            resultMessage.Append(QuestItem_ApplyEquipmentSetMutation(mutationTier) + "\n");
                            break;
                    }
                }
                else if (this.ItemType == ItemType.Jewelry)
                {
                    switch (mutationType)
                    {
                        case 1: //AttributeCantrip
                            resultMessage.Append(QuestItem_ApplyAttributeCantripMutation(mutationTier) + "\n");
                            break;
                        case 2: //SkillCantrip
                            resultMessage.Append(QuestItem_ApplySkillCantripMutation(mutationTier) + "\n");
                            break;
                    }
                }
            }
            return resultMessage.ToString();
        }


        private uint GetMutationTier()
        {
            var mutationTierOverride = QuestItemMutations.GetMutationTierOverride(this.WeenieClassId);
            if (mutationTierOverride != null)
                return mutationTierOverride.Value;

            List<(WieldRequirement, int?)> reqs = new List<(WieldRequirement, int?)>() { (WieldRequirements, WieldDifficulty), (WieldRequirements2, WieldDifficulty2), (WieldRequirements3, WieldDifficulty3), (WieldRequirements4, WieldDifficulty4)}.Where(x => (uint)x.Item1 != 0).ToList();

            var levelReq = reqs.Find(x => x.Item1 == WieldRequirement.Level);
            var attrReq = reqs.Find(x => x.Item1 == WieldRequirement.Attrib);
            var baseAttrReq = reqs.Find(x => x.Item1 == WieldRequirement.RawAttrib);
            var skillReq = reqs.Find(x => x.Item1 == WieldRequirement.Skill);
            var baseSkillReq = reqs.Find(x => x.Item1 == WieldRequirement.RawSkill);

            if (levelReq.Item2 < 30)
                return 1;
            else if (levelReq.Item2 < 50)
                return 2;
            else if (levelReq.Item2 >= 50)
                return 3;

            if (attrReq.Item2 < 150)
                return 1;
            else if (attrReq.Item2 <= 200)
                return 2;
            else if (attrReq.Item2 > 200)
                return 3;

            if (baseAttrReq.Item2 < 150)
                return 1;
            else if (baseAttrReq.Item2 <= 200)
                return 2;
            else if (baseAttrReq.Item2 > 200)
                return 3;

            if (skillReq.Item2 < 250)
                return 1;
            else if (skillReq.Item2 <= 350)
                return 2;
            else if (skillReq.Item2 > 350)
                return 3;

            if (baseSkillReq.Item2 < 250)
                return 1;
            else if (baseSkillReq.Item2 <= 300)
                return 2;
            else if (baseSkillReq.Item2 > 300)
                return 3;

            return 1;
        }

        private void SetMagicItemCommonProperties(uint mutationTier)
        {
            if (!this.ItemMaxMana.HasValue || this.ItemMaxMana < 1)
            {
                int difficulty;
                if (mutationTier == 1)
                    difficulty = ThreadSafeRandom.Next(50, 100);
                else if (mutationTier == 2)
                    difficulty = ThreadSafeRandom.Next(100, 150);
                else
                    difficulty = ThreadSafeRandom.Next(150, 200);
                this.ItemDifficulty = Math.Max(this.ItemDifficulty ?? 0, difficulty);

                int spellcraft;
                if (mutationTier == 1)
                    spellcraft = ThreadSafeRandom.Next(50, 100);
                else if (mutationTier == 2)
                    spellcraft = ThreadSafeRandom.Next(100, 150);
                else
                    spellcraft = ThreadSafeRandom.Next(200, 300);
                this.ItemSpellcraft = Math.Max(this.ItemSpellcraft ?? 0, spellcraft);


                int maxMana;
                if (mutationTier == 1)
                    maxMana = ThreadSafeRandom.Next(250, 500);
                else if (mutationTier == 2)
                    maxMana = ThreadSafeRandom.Next(350, 600);
                else
                    maxMana = ThreadSafeRandom.Next(600, 1500);
                this.ItemMaxMana = Math.Max(this.ItemMaxMana ?? 0, maxMana);
                this.ItemCurMana = this.ItemMaxMana;

                double manaRate;
                if (mutationTier == 1)
                    manaRate = -1 / 60.0;
                else if (mutationTier == 2)
                    manaRate = -1 / 30.0;
                else
                    manaRate = -1 / 20.0;
                this.ManaRate = Math.Min(this.ManaRate ?? 0, manaRate);
            }
        }


        private string QuestItem_ApplySlayerMutation(uint mutationTier)
        {
            var selectSlayerType = ThreadSafeRandom.Next(1, 20);
            this.SlayerDamageBonus = 1.20f;

            switch (selectSlayerType)
            {
                case 1:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Banderling;                    
                    break;

                case 2:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Drudge;
                    break;

                case 3:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Gromnie;
                    break;

                case 4:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Lugian;
                    break;

                case 5:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Grievver;
                    break;

                case 6:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Human;
                    break;

                case 7:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Mattekar;
                    break;

                case 8:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Mite;
                    break;

                case 9:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Mosswart;
                    break;

                case 10:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Mumiyah;
                    break;

                case 11:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Olthoi;
                    break;

                case 12:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.PhyntosWasp;
                    break;

                case 13:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Shadow;
                    break;

                case 14:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Shreth;
                    break;

                case 15:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Skeleton;
                    break;

                case 16:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Tumerok;
                    break;

                case 17:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Tusker;
                    break;

                case 18:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Virindi;
                    break;

                case 19:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Wisp;
                    break;

                case 20:
                    this.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Zefir;                    
                    break;
            }

            return $"This quest item was granted {this.SlayerCreatureType} slayer!";
        }

        private string QuestItem_ApplyAttributeCantripMutation(uint mutationTier)
        {
            mutationTier = ConvertMutationTierToCantripTier(mutationTier);

            string resultMsg = "";

            var selectAttribute = ThreadSafeRandom.Next(1, 6);

            switch (selectAttribute)
            {
                case 1: // strength
                    if (mutationTier == 1)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPSTRENGTH1, this.BiotaDatabaseLock, out _);
                        resultMsg = $"Added Minor Strength to the quest item!";
                    }
                    else if (mutationTier == 2)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPSTRENGTH2, this.BiotaDatabaseLock, out _);
                        resultMsg = $"Added Major Strength to the quest item!";
                    }
                    break;

                case 2: // endurance
                    if (mutationTier == 1)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPENDURANCE1, this.BiotaDatabaseLock, out _);
                        resultMsg = $"Added Minor Endurance to the quest item!";
                    }
                    else if (mutationTier == 2)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPENDURANCE2, this.BiotaDatabaseLock, out _);
                        resultMsg = $"Added Major Endurance to the quest item!";
                    }
                    break;

                case 3:// coordination
                    if (mutationTier == 1)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPCOORDINATION1, this.BiotaDatabaseLock, out _);
                        resultMsg = $"Added Minor Coordination to the quest item!";
                    }
                    else if (mutationTier == 2)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPCOORDINATION2, this.BiotaDatabaseLock, out _);
                        resultMsg = $"Added Major Coordination to the quest item!";
                    }
                    break;

                case 4: // quickness
                    if (mutationTier == 1)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPQUICKNESS1, this.BiotaDatabaseLock, out _);
                        resultMsg = $"Added Minor Quickness to the quest item!";
                    }
                    else if (mutationTier == 2)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPQUICKNESS2, this.BiotaDatabaseLock, out _);
                        resultMsg = $"Added Major Quickness to the quest item!";
                    }
                    break;

                case 5: // focus
                    if (mutationTier == 1)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPFOCUS1, this.BiotaDatabaseLock, out _);
                        resultMsg = $"Added Minor Focus to the quest item!";
                    }
                    else if (mutationTier == 2)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPFOCUS2, this.BiotaDatabaseLock, out _);
                        resultMsg = $"Added Major Focus to the quest item!";
                    }
                    break;

                case 6: // self
                    if (mutationTier == 1)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPWILLPOWER1, this.BiotaDatabaseLock, out _);
                        resultMsg = $"Added Minor Willpower to the quest item!";
                    }
                    else if (mutationTier == 2)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPWILLPOWER2, this.BiotaDatabaseLock, out _);
                        resultMsg = $"Added Major Willpower to the quest item!";
                    }
                    break;
            }

            SetMagicItemCommonProperties(mutationTier);

            return resultMsg;
        }

        private uint ConvertMutationTierToCantripTier(uint mutationTier)
        {
            var roll = ThreadSafeRandom.Next(0f, 1f);


            if (mutationTier == 1)
            {
                // 90% minor 10% major by default
                if (roll < PropertyManager.GetDouble("quest_mutation_tier_1_major_chance", 0.1).Item)
                    mutationTier = 2;
            }
            else if (mutationTier == 2)
            {
                // 75% minor 25% major by default
                if (roll > PropertyManager.GetDouble("quest_mutation_tier_2_major_chance", 0.25).Item)
                    mutationTier = 1;
            }
            else if (mutationTier == 3)
            {
                // 10% minor 90% major
                if (roll > PropertyManager.GetDouble("quest_mutation_tier_3_major_chance", 0.90).Item)
                    mutationTier = 1;
                else
                    mutationTier = 2;
            }
            return mutationTier;
        }

        private string QuestItem_ApplySkillCantripMutation(uint mutationTier)
        {
            mutationTier = ConvertMutationTierToCantripTier(mutationTier);
            string resultMsg = "";
            var selectSkill = ThreadSafeRandom.Next(1, 18);

            switch (selectSkill)
            {
                case 1:
                    if (mutationTier == 1)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPINVULNERABILITY1, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Minor Invulnerability to the quest item!";
                    }
                    else if (mutationTier == 2)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPINVULNERABILITY2, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Major Invulnerability to the quest item!";
                    }
                    break;

                case 2:
                    if (mutationTier == 1)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPMAGICRESISTANCE1, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Minor Magic Resistance to the quest item!";
                    }
                    else if (mutationTier == 2)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPMAGICRESISTANCE2, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Major Magic Resistance to the quest item!";
                    }
                    break;

                case 3:
                    if (mutationTier == 1)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPLIFEMAGICAPTITUDE1, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Minor Life Magic to the quest item!";
                    }
                    else if (mutationTier == 2)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPLIFEMAGICAPTITUDE2, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Major Life Magic to the quest item!";
                    }
                    break;

                case 4:
                    if (mutationTier == 1)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPARCANEPROWESS1, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Minor Arcane Lore to the quest item!";
                    }
                    else if (mutationTier == 2)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPARCANEPROWESS2, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Major Arcane Lore to the quest item!";
                    }
                    break;

                case 5:
                    if (mutationTier == 1)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPMANACONVERSIONPROWESS1, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Minor Mana Conversion to the quest item!";
                    }
                    else if (mutationTier == 2)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPMANACONVERSIONPROWESS2, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Major Mana Conversion to the quest item!";
                    }
                    break;

                case 6:
                    if (mutationTier == 1)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPWARMAGICAPTITUDE1, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Minor War Magic to the quest item!";
                    }
                    else if (mutationTier == 2)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPWARMAGICAPTITUDE2, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Major War Magic to the quest item!";
                    }
                    break;

                case 7:
                    if (mutationTier == 1)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPHEALINGPROWESS1, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Minor Healing to the quest item!";
                    }
                    else if (mutationTier == 2)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPHEALINGPROWESS2, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Major Healing to the quest item!";
                    }
                    break;

                case 8:
                    if (mutationTier == 1)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPJUMPINGPROWESS1, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Minor Jump to the quest item!";
                    }
                    else if (mutationTier == 2)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPJUMPINGPROWESS2, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Major Jump to the quest item!";
                    }
                    break;

                case 9:
                    if (mutationTier == 1)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPSPRINT1, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Minor Sprint to the quest item!";
                    }
                    else if (mutationTier == 2)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPSPRINT2, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Major Sprint to the quest item!";
                    }
                    break;

                case 10:
                    if (mutationTier == 1)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CantripDualWieldAptitude1, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Minor Dual Wield to the quest item!";
                    }
                    else if (mutationTier == 2)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CantripDualWieldAptitude2, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Major Dual Wield to the quest item!";
                    }
                    break;

                case 11:
                    if (mutationTier == 1)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPDECEPTIONPROWESS1, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Minor Deception to the quest item!";
                    }
                    else if (mutationTier == 2)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPDECEPTIONPROWESS2, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Major Deception to the quest item!";
                    }
                    break;

                case 12:
                    if (mutationTier == 1)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPLEADERSHIP1, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Minor Leadership to the quest item!";
                    }
                    else if (mutationTier == 2)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPLEADERSHIP2, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Major Leadership to the quest item!";
                    }
                    break;

                case 13:
                    if (mutationTier == 1)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CantripSneakingAptitude1, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Minor Sneaking Aptitude to the quest item!";
                    }
                    else if (mutationTier == 2)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CantripSneakingAptitude2, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Major Sneaking Aptitude to the quest item!";
                    }
                    break;

                case 14:
                    if (mutationTier == 1)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPFLETCHINGPROWESS1, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Minor Fletching to the quest item!";
                    }
                    else if (mutationTier == 2)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPFLETCHINGPROWESS2, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Major Fletching to the quest item!";
                    }
                    break;

                case 15:
                    if (mutationTier == 1)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPLIGHTWEAPONSAPTITUDE1, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Minor Axe Aptitude to the quest item!";
                    }
                    else if (mutationTier == 2)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPLIGHTWEAPONSAPTITUDE2, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Major Axe Aptitude to the quest item!";
                    }
                    break;

                case 16:
                    if (mutationTier == 1)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPHEAVYWEAPONSAPTITUDE1, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Minor Sword Aptitude to the quest item!";
                    }
                    else if (mutationTier == 2)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPHEAVYWEAPONSAPTITUDE2, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Major Sword Aptitude to the quest item!";
                    }
                    break;

                case 17:
                    if (mutationTier == 1)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPFINESSEWEAPONSAPTITUDE1, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Minor Dagger Aptitude to the quest item!";
                    }
                    else if (mutationTier == 2)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPFINESSEWEAPONSAPTITUDE2, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Major Dagger Aptitude to the quest item!";
                    }
                    break;

                case 18:
                    if (mutationTier == 1)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPMISSILEWEAPONSAPTITUDE1, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Minor Bow Aptitude to the quest item!";
                    }
                    else if (mutationTier == 2)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPMISSILEWEAPONSAPTITUDE2, this.BiotaDatabaseLock, out _);
                        resultMsg = "Added Major Bow Aptitude to the quest item!";
                    }
                    break;                
            }

            SetMagicItemCommonProperties(mutationTier);

            return resultMsg;
        }

        private string QuestItem_ApplyItemCantripMutation(uint mutationTier)
        {
            mutationTier = ConvertMutationTierToCantripTier(mutationTier);

            string resultMsg = "";

            var selectItemSpell = ThreadSafeRandom.Next(1, 3);

            switch (selectItemSpell)
            {
                case 1: // Blood Drinker / Spirit Drinker

                    if (this.ItemType != ItemType.Caster)
                    {
                        if (mutationTier == 1)
                        {
                            this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPBLOODTHIRST1, this.BiotaDatabaseLock, out _);
                            resultMsg = $"Added Minor Blood Thirst to the quest item!";
                        }
                        else if (mutationTier == 2)
                        {
                            this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPBLOODTHIRST2, this.BiotaDatabaseLock, out _);
                            resultMsg = $"Added Major Blood Thirst to the quest item!";
                        }
                    }
                    else
                    {
                        if (mutationTier == 1)
                        {
                            this.Biota.GetOrAddKnownSpell((int)SpellId.CantripSpiritThirst1, this.BiotaDatabaseLock, out _);
                            resultMsg = $"Added Minor Spirit Thirst to the quest item!";
                        }
                        else if (mutationTier == 2)
                        {
                            this.Biota.GetOrAddKnownSpell((int)SpellId.CantripSpiritThirst2, this.BiotaDatabaseLock, out _);
                            resultMsg = $"Added Major Spirit Thirst to the quest item!";
                        }
                    }
                    break;

                case 2: // Defender / HeartSeeker                                

                    if (ThreadSafeRandom.Next(0f, 1f) < 0.5f)
                    {
                        if (mutationTier == 1)
                        {
                            this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPDEFENDER1, this.BiotaDatabaseLock, out _);
                            resultMsg = $"Added Minor Defender to the quest item!";
                        }
                        else if (mutationTier == 2)
                        {
                            this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPDEFENDER2, this.BiotaDatabaseLock, out _);
                            resultMsg = $"Added Major Defender to the quest item!";
                        }
                    }
                    else
                    {
                        if (this.ItemType == ItemType.Caster)
                        {
                            if (mutationTier == 1)
                            {
                                this.Biota.GetOrAddKnownSpell((int)SpellId.CantripSpiritThirst1, this.BiotaDatabaseLock, out _);
                                resultMsg = $"Added Minor Spirit Thirst to the quest item!";
                            }
                            else if (mutationTier == 2)
                            {
                                this.Biota.GetOrAddKnownSpell((int)SpellId.CantripSpiritThirst2, this.BiotaDatabaseLock, out _);
                                resultMsg = $"Added Major Spirit Thirst to the quest item!";
                            }
                        }
                        else
                        {
                            if (mutationTier == 1)
                            {
                                this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPHEARTTHIRST1, this.BiotaDatabaseLock, out _);
                                resultMsg = $"Added Minor Heart Thirst to the quest item!";
                            }
                            else if (mutationTier == 2)
                            {
                                this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPHEARTTHIRST2, this.BiotaDatabaseLock, out _);
                                resultMsg = $"Added Major Heart Thirst to the quest item!";
                            }
                        }
                    }
                    break;

                case 3:// Swift Killer / Hermetic Link

                    if (this.ItemType == ItemType.Caster)
                    {
                        if (mutationTier == 1)
                        {
                            this.Biota.GetOrAddKnownSpell((int)SpellId.CantripHermeticLink1, this.BiotaDatabaseLock, out _);
                            resultMsg = $"Added Minor Hermetic Link to the quest item!";
                        }
                        else if (mutationTier == 2)
                        {
                            this.Biota.GetOrAddKnownSpell((int)SpellId.CantripHermeticLink2, this.BiotaDatabaseLock, out _);
                            resultMsg = $"Added Major Hermetic Link to the quest item!";
                        }
                    }
                    else
                    {
                        if (mutationTier == 1)
                        {
                            this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPSWIFTHUNTER1, this.BiotaDatabaseLock, out _);
                            resultMsg = $"Added Minor Swift Hunter to the quest item!";
                        }
                        else if (mutationTier == 2)
                        {
                            this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPSWIFTHUNTER2, this.BiotaDatabaseLock, out _);
                            resultMsg = $"Added Major Swift Hunter to the quest item!";
                        }
                    }
                    break;
            }

            SetMagicItemCommonProperties(mutationTier);

            return resultMsg;
        }

        private string QuestItem_ApplyRendMutation(uint mutationTier)
        {
            string resultMsg = "";

            var rendSelect = ThreadSafeRandom.Next(1, 7);

            switch (rendSelect)
            {
                case 1: // acid
                    this.ImbuedEffect = ImbuedEffectType.AcidRending;
                    this.W_DamageType = DamageType.Acid;
                    this.SetProperty(PropertyDataId.IconUnderlay, 0x06003355);
                    resultMsg = $"Added Acid Rending to the quest item!";
                    break;

                case 2: // Cold
                    this.ImbuedEffect = ImbuedEffectType.ColdRending;
                    this.W_DamageType = DamageType.Cold;
                    this.SetProperty(PropertyDataId.IconUnderlay, 0x06003353);
                    resultMsg = $"Added Cold Rending to the quest item!";
                    break;

                case 3: // Electric
                    this.ImbuedEffect = ImbuedEffectType.ElectricRending;
                    this.W_DamageType = DamageType.Electric;
                    this.SetProperty(PropertyDataId.IconUnderlay, 0x06003354);
                    resultMsg = $"Added Lightning Rending to the quest item!";
                    break;

                case 4: // Fire
                    this.ImbuedEffect = ImbuedEffectType.FireRending;
                    this.W_DamageType = DamageType.Fire;
                    this.SetProperty(PropertyDataId.IconUnderlay, 0x06003359);
                    resultMsg = $"Added Fire Rending to the quest item!";
                    break;

                case 5: // Pierce
                    this.ImbuedEffect = ImbuedEffectType.PierceRending;
                    this.W_DamageType = DamageType.Pierce;
                    this.SetProperty(PropertyDataId.IconUnderlay, 0x0600335b);
                    resultMsg = $"Added Pierce Rend to the quest item!";
                    break;

                case 6: // Slash
                    this.ImbuedEffect = ImbuedEffectType.SlashRending;
                    this.W_DamageType = DamageType.Slash;
                    this.SetProperty(PropertyDataId.IconUnderlay, 0x0600335c);
                    resultMsg = $"Added Slash Rending to the quest item!";
                    break;

                case 7: // Bludgeon
                    this.ImbuedEffect = ImbuedEffectType.BludgeonRending;
                    this.W_DamageType = DamageType.Bludgeon;
                    this.SetProperty(PropertyDataId.IconUnderlay, 0x0600335a);
                    resultMsg = $"Added Bludgeon Rending to the quest item!";
                    break;
            }

            return resultMsg;
        }

        private string QuestItem_ApplyArmorLevelMutation(uint mutationTier)
        {
            var numSteelTinksAdded = ThreadSafeRandom.Next(1, 2 * (int)mutationTier);
            this.ArmorLevel += 20 * numSteelTinksAdded;
            this.NumTimesTinkered += numSteelTinksAdded;
            string tinkerLog = "64";
            for (int i = 1; i < numSteelTinksAdded; i++)
            {
                tinkerLog = tinkerLog + ",64";
            }
            this.TinkerLog = tinkerLog;

            return $"This quest item was granted {numSteelTinksAdded} Steel tinks! (+{20 * numSteelTinksAdded} AL)";
        }

        private string QuestItem_ApplyEquipmentSetMutation(uint mutationTier)
        {
            var roll = ThreadSafeRandom.Next(13, 29);
            int tries = 0;
            while ((EquipmentSet)roll == EquipmentSet.Tinkers && tries < 20)
            {
                roll = ThreadSafeRandom.Next(13, 29);
                tries++;
            }

            this.EquipmentSetId = (EquipmentSet)roll;
            return $"Added {Enum.GetName((EquipmentSet)roll)} Set to quest item!";
        }

        private string QuestItem_ApplyRatingMutation(uint mutationTier)
        {
            string resultMsg = "";

            var selectRating = ThreadSafeRandom.Next(1, 6);
            var ratingAmount = ThreadSafeRandom.Next(1, (int)mutationTier);

            switch (selectRating)
            {
                case 1:

                    if (this.GearDamage == null)
                        this.GearDamage = 0;

                    this.SetProperty(PropertyInt.GearDamage, (int)this.GearDamage + ratingAmount);
                    resultMsg = $"Added {ratingAmount} Damage Rating to the quest item!";
                    break;

                case 2:

                    if (this.GearDamageResist == null)
                        this.GearDamageResist = 0;

                    this.SetProperty(PropertyInt.GearDamageResist, (int)this.GearDamageResist + ratingAmount);
                    resultMsg = $"Added {ratingAmount} Damage Resist Rating to the quest item!";
                    break;

                case 3:

                    if (this.GearCritDamageResist == null)
                        this.GearCritDamageResist = 0;

                    this.SetProperty(PropertyInt.GearCritDamageResist, (int)this.GearCritDamageResist + ratingAmount);
                    resultMsg = $"Added {ratingAmount} Crit Damage Resist Rating to the quest item!";
                    break;

                case 4:

                    if (this.GearCritDamage == null)
                        this.GearCritDamage = 0;

                    this.SetProperty(PropertyInt.GearCritDamage, (int)this.GearCritDamage + ratingAmount);
                    resultMsg = $"Added {ratingAmount} Crit Damage Rating to the quest item!";
                    break;

                case 5:

                    if (this.GearCritResist == null)
                        this.GearCritResist = 0;

                    this.SetProperty(PropertyInt.GearCritResist, (int)this.GearCritResist + ratingAmount);
                    resultMsg = $"Added {ratingAmount} Crit Resist Rating to the quest item!";
                    break;

                case 6:

                    if (this.GearCrit == null)
                        this.GearCrit = 0;

                    this.SetProperty(PropertyInt.GearCrit, (int)this.GearCrit + ratingAmount);
                    resultMsg = $"Added {ratingAmount} Crit Rating to the quest item!";
                    break;
            }

            return resultMsg;
        }
    }      
}
