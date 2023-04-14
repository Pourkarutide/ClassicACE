using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
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

        public string MutateQuestItem()
        {
            //Validate this is not a loot gen item, and that its a caster, weapon, armor or clothing
            if
            (
               (this.ItemType != ItemType.Caster &&
                this.ItemType != ItemType.MeleeWeapon &&
                this.ItemType != ItemType.MissileWeapon &&
                this.ItemType != ItemType.Armor &&
                this.ItemType != ItemType.Clothing) ||
               (this.Workmanship.HasValue &&
                this.Workmanship.Value > 0)
            )
            {
                return "";
            }

            //Make sure the item is not on the mutation blacklist
            if (QuestItemMutations.IsQuestItemMutationDisallowed(this.WeenieClassId))
                return "This item cannot be mutated!";

            StringBuilder resultMessage = new StringBuilder();
            var mutationTier = QuestItemMutations.GetMutationTierOverride(this.WeenieClassId) ?? GetMutationTier();

            Random rand = new Random();
            double roll = 0;

            //Get the number of mutations to apply
            //50% chance for first mutation
            //15% chance for a second mutation
            //1% chance for a third mutation
            //TODO should this be dictated by mutation tier?
            //TODO should make the chances configurable?
            roll = rand.NextDouble();
            int mutationCount = 0;
            if(roll < 0.3)
            {
                mutationCount = 3;
            }
            else if (roll < 0.5)
            {
                mutationCount = 2;
            }
            else if (roll < .85)
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
                int mutationType = rand.Next(1, 6);

                //For Slayers, Rends, Sets and Steel Tinks, don't allow those to be added more than once, reroll if already added
                int rerollAttempts = 0; //just a fail-safe to avoid an infinite loop, even tho it shouldn't be possible
                while ((mutationType == 1 || mutationType == 5) && mutationTypes.Contains(mutationType) && rerollAttempts < 20)
                {
                    mutationType = rand.Next(1, 6);
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
                            resultMessage.Append(QuestItem_ApplySlayerMutation() + "\n");
                            break;
                        case 2: //AttributeCantrip
                            resultMessage.Append(QuestItem_ApplyAttributeCantripMutation() + "\n");
                            break;
                        case 3: //SkillCantrip
                            resultMessage.Append(QuestItem_ApplySkillCantripMutation() + "\n");
                            break;
                        case 4: //ItemCantrip
                            resultMessage.Append(QuestItem_ApplyItemCantripMutation() + "\n");
                            break;
                        case 5: //Rend
                            resultMessage.Append(QuestItem_ApplyRendMutation() + "\n");
                            break;
                    }
                }
                else if (this.ItemType == ItemType.Armor || this.ItemType == ItemType.Clothing)
                {
                    switch (mutationType)
                    {
                        case 1: //ArmorLevel
                            resultMessage.Append(QuestItem_ApplyArmorLevelMutation() + "\n");
                            break;
                        case 2: //AttributeCantrip
                            resultMessage.Append(QuestItem_ApplyAttributeCantripMutation() + "\n");
                            break;
                        case 3: //SkillCantrip
                            resultMessage.Append(QuestItem_ApplySkillCantripMutation() + "\n");
                            break;
                        case 4: //Rating
                            resultMessage.Append(QuestItem_ApplyRatingMutation() + "\n");
                            break;
                        case 5: //EquipmentSet
                            resultMessage.Append(QuestItem_ApplyEquipmentSetMutation() + "\n");
                            break;
                    }
                }                
            }

            return resultMessage.ToString();
        }


        private uint GetMutationTier()
        {
            //TODO
            // If this is a quest item where we've defined a mutation tier override, return that override value
            // else if there's no override, then determine the mutation tier based on some properties of the item, such as the skill wield requirements

            return 1;
        }

        private void SetMagicItemCommonProperties()
        {
            if (!this.ItemMaxMana.HasValue || this.ItemMaxMana < 1)
            {
                int difficulty = 25;
                this.ItemSpellcraft = (this.ItemSpellcraft ?? 0) + difficulty;
                this.ItemDifficulty = (this.ItemDifficulty ?? 0) + difficulty;
                int newMaxMana = (this.ItemMaxMana ?? 0);
                this.ItemMaxMana = newMaxMana + difficulty * 4;
                this.ItemCurMana = this.ItemMaxMana;
                this.ItemManaCost = (this.ItemManaCost ?? 0) + difficulty;
            }
        }


        private string QuestItem_ApplySlayerMutation()
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

        private string QuestItem_ApplyAttributeCantripMutation()
        {
            string resultMsg = "";

            var selectAttribute = ThreadSafeRandom.Next(1, 6);

            switch (selectAttribute)
            {
                case 1: // strength
                    this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPSTRENGTH2, this.BiotaDatabaseLock, out _);
                    resultMsg = $"Added Major Strength to the quest item!";
                    break;

                case 2: // endurance
                    this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPENDURANCE2, this.BiotaDatabaseLock, out _);
                    resultMsg = $"Added Major Endurance to the quest item!";
                    break;

                case 3:// coordination
                    this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPCOORDINATION2, this.BiotaDatabaseLock, out _);
                    resultMsg = $"Added Major Coordination to the quest item!";
                    break;

                case 4: // quickness
                    this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPQUICKNESS2, this.BiotaDatabaseLock, out _);
                    resultMsg = $"Added Major Quickness to the quest item!";
                    break;

                case 5: // focus
                    this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPFOCUS2, this.BiotaDatabaseLock, out _);
                    resultMsg = $"Added Major Focus to the quest item!";
                    break;

                case 6: // self
                    this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPWILLPOWER2, this.BiotaDatabaseLock, out _);
                    resultMsg = $"Added Major Willpower to the quest item!";
                    break;
            }

            SetMagicItemCommonProperties();

            return resultMsg;
        }

        private string QuestItem_ApplySkillCantripMutation()
        {
            string resultMsg = "";

            var selectSkill = ThreadSafeRandom.Next(1, 18);            

            switch (selectSkill)
            {
                case 1:
                    this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPINVULNERABILITY2, this.BiotaDatabaseLock, out _);
                    resultMsg = "Added Major Invulnerability to the quest item!";
                    break;

                case 2:
                    this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPMAGICRESISTANCE2, this.BiotaDatabaseLock, out _);
                    resultMsg = "Added Major Magic Resistance to the quest item!";
                    break;

                case 3:
                    this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPLIFEMAGICAPTITUDE2, this.BiotaDatabaseLock, out _);
                    resultMsg = "Added Major Life Magic to the quest item!";
                    break;

                case 4:
                    this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPARCANEPROWESS2, this.BiotaDatabaseLock, out _);
                    resultMsg = "Added Major Arcane Lore to the quest item!";
                    break;

                case 5:
                    this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPMANACONVERSIONPROWESS2, this.BiotaDatabaseLock, out _);
                    resultMsg = "Added Major Mana Conversion to the quest item!";
                    break;

                case 6:
                    this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPWARMAGICAPTITUDE2, this.BiotaDatabaseLock, out _);
                    resultMsg = "Added Major War Magic to the quest item!";
                    break;

                case 7:
                    this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPHEALINGPROWESS2, this.BiotaDatabaseLock, out _);
                    resultMsg = "Added Major Healing to the quest item!";
                    break;

                case 8:
                    this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPJUMPINGPROWESS2, this.BiotaDatabaseLock, out _);
                    resultMsg = "Added Major Jump to the quest item!";
                    break;

                case 9:
                    this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPSPRINT2, this.BiotaDatabaseLock, out _);
                    resultMsg = "Added Major Run to the quest item!";
                    break;

                case 10:
                    this.Biota.GetOrAddKnownSpell((int)SpellId.CantripDualWieldAptitude2, this.BiotaDatabaseLock, out _);
                    resultMsg = "Added Major Dual Wield to the quest item!";
                    break;

                case 11:
                    this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPDECEPTIONPROWESS2, this.BiotaDatabaseLock, out _);
                    resultMsg = "Added Major Deception to the quest item!";
                    break;

                case 12:
                    this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPLEADERSHIP2, this.BiotaDatabaseLock, out _);
                    resultMsg = "Added Major Leadership to the quest item!";
                    break;

                case 13:
                    this.Biota.GetOrAddKnownSpell((int)SpellId.CantripSneakingAptitude2, this.BiotaDatabaseLock, out _);
                    resultMsg = "Added Major Sneaking Aptitude to the quest item!";
                    break;

                case 14:
                    this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPFLETCHINGPROWESS2, this.BiotaDatabaseLock, out _);
                    resultMsg = "Added Major Fletching to the quest item!";
                    break;

                case 15:
                    this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPLIGHTWEAPONSAPTITUDE1, this.BiotaDatabaseLock, out _);
                    resultMsg = "Added Major Axe Aptitude to the quest item!";
                    break;

                case 16:
                    this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPHEAVYWEAPONSAPTITUDE2, this.BiotaDatabaseLock, out _);
                    resultMsg = "Added Major Sword Aptitude to the quest item!";
                    break;

                case 17:
                    this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPFINESSEWEAPONSAPTITUDE2, this.BiotaDatabaseLock, out _);
                    resultMsg = "Added Major Dagger Aptitude to the quest item!";
                    break;

                case 18:
                    this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPMISSILEWEAPONSAPTITUDE2, this.BiotaDatabaseLock, out _);
                    resultMsg = "Added Major Bow Aptitude to the quest item!";
                    break;                
            }

            SetMagicItemCommonProperties();

            return resultMsg;
        }

        private string QuestItem_ApplyItemCantripMutation()
        {
            string resultMsg = "";

            var selectItemSpell = ThreadSafeRandom.Next(1, 3);

            switch (selectItemSpell)
            {
                case 1: // Blood Drinker / Spirit Drinker

                    if (this.ItemType != ItemType.Caster)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPBLOODTHIRST2, this.BiotaDatabaseLock, out _);
                        resultMsg = $"Added Major Blood Drinker to the quest item!";
                    }
                    else
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CantripSpiritThirst2, this.BiotaDatabaseLock, out _);
                        resultMsg = $"Added Major Spirit Drinker to the quest item!";
                    }
                    break;

                case 2: // Defender / HeartSeeker                                

                    if (new Random().NextDouble() < 0.5f)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPDEFENDER2, this.BiotaDatabaseLock, out _);
                        resultMsg = $"Added Major Defender to the quest item!";
                    }
                    else
                    {
                        if (this.ItemType == ItemType.Caster)
                        {
                            this.Biota.GetOrAddKnownSpell((int)SpellId.CantripSpiritThirst2, this.BiotaDatabaseLock, out _);
                            resultMsg = $"Added Major Spirit Drinker to the quest item!";
                        }
                        else
                        {
                            this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPHEARTTHIRST2, this.BiotaDatabaseLock, out _);
                            resultMsg = $"Added Major Heart Thirst to the quest item!";
                        }
                    }
                    break;

                case 3:// Swift Killer / Hermetic Link

                    if (this.ItemType == ItemType.Caster)
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CantripHermeticLink2, this.BiotaDatabaseLock, out _);
                        resultMsg = $"Added Major Hermetic Link to the quest item!";
                    }
                    else
                    {
                        this.Biota.GetOrAddKnownSpell((int)SpellId.CANTRIPSWIFTHUNTER2, this.BiotaDatabaseLock, out _);
                        resultMsg = $"Added Major Swift Hunter to the quest item!";
                    }
                    break;
            }

            SetMagicItemCommonProperties();

            return resultMsg;
        }

        private string QuestItem_ApplyRendMutation()
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

        private string QuestItem_ApplyArmorLevelMutation()
        {
            var numSteelTinksAdded = new Random().Next(1, 11);
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

        private string QuestItem_ApplyEquipmentSetMutation()
        {
            var roll = new Random().Next(13, 30);
            int tries = 0;
            while (roll == 17 && tries < 20)
            {
                roll = new Random().Next(13, 30);
                tries++;
            }

            this.EquipmentSetId = (EquipmentSet)roll;
            return $"Added {Enum.GetName((EquipmentSet)roll)} Set to quest item!";
        }

        private string QuestItem_ApplyRatingMutation()
        {
            string resultMsg = "";

            var selectRating = ThreadSafeRandom.Next(1, 6);
            var ratingAmount = ThreadSafeRandom.Next(1, 3);

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
