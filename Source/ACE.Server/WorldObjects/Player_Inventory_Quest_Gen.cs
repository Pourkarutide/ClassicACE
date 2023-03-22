using ACE.Common;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Managers;
using ACE.Server.Network.GameMessages.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public WorldObject ModifyQuestItem(WorldObject item, int modSelectMin, int modSelectMax, bool extrachance)
        {
            var selectMod = ThreadSafeRandom.Next(modSelectMin, modSelectMax);
            var selectSlayerType = ThreadSafeRandom.Next(1, 24);
            var selectArmorMod = ThreadSafeRandom.Next(1, 1);
            var extraChance = ThreadSafeRandom.Next(1, 100);
            var splitChance = ThreadSafeRandom.Next(1, 2);
            var splitChance2 = ThreadSafeRandom.Next(1, 2);

            // if an extra chance occurred, we only want it to choose from cantrips and ratings.
            //This means if you want a rend, armor set, or slayer you gotta get lucky on the very first roll from Player_Inventory.cs
            if (extrachance) // 2 , 4 , 5
            {
                var chooseExtraMod = ThreadSafeRandom.Next(1, 3);

                switch (chooseExtraMod)
                {
                    case 1:
                        selectMod = 2;
                        break;
                    case 2:
                        selectMod = 4;
                        break;
                    case 3:
                        selectMod = 5;
                        break;
                }
            }

            switch (selectMod)
            {
                case 1: // Slayer Weapons & Steel Tinks
                    if (item.ItemType == ItemType.MeleeWeapon || item.ItemType == ItemType.MissileWeapon || item.ItemType == ItemType.Caster)
                    {

                        switch (selectSlayerType)
                        {
                            case 1:
                                item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Banderling;
                                item.SlayerDamageBonus = 1.20f;
                                item.SlayerAdded = 1;
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 2, 3, true);

                                return item;
                            case 2:
                                item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Drudge;
                                item.SlayerDamageBonus = 1.20f;
                                item.SlayerAdded = 1;
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 2, 3, true);

                                return item;
                            case 3:
                                item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Gromnie;
                                item.SlayerDamageBonus = 1.20f;
                                item.SlayerAdded = 1;
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 2, 3, true);

                                return item;
                            case 4:
                                item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Lugian;
                                item.SlayerDamageBonus = 1.20f;
                                item.SlayerAdded = 1;
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 2, 3, true);

                                return item;
                            case 5:
                                item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Grievver;
                                item.SlayerDamageBonus = 1.20f;
                                item.SlayerAdded = 1;
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 2, 3, true);

                                return item;
                            case 6:
                                item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Human;
                                item.SlayerDamageBonus = 1.20f;
                                item.SlayerAdded = 1;
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 2, 3, true);

                                return item;
                            case 7:
                                item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Mattekar;
                                item.SlayerDamageBonus = 1.20f;
                                item.SlayerAdded = 1;
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 2, 3, true);

                                return item;
                            case 8:
                                item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Mite;
                                item.SlayerDamageBonus = 1.20f;
                                item.SlayerAdded = 1;
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 2, 3, true);

                                return item;
                            case 9:
                                item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Mosswart;
                                item.SlayerDamageBonus = 1.20f;
                                item.SlayerAdded = 1;
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 2, 3, true);

                                return item;
                            case 10:
                                item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Mumiyah;
                                item.SlayerDamageBonus = 1.20f;
                                item.SlayerAdded = 1;
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 2, 3, true);

                                return item;
                            case 11:
                                item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Olthoi;
                                item.SlayerDamageBonus = 1.20f;
                                item.SlayerAdded = 1;
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 2, 3, true);

                                return item;
                            case 12:
                                item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.PhyntosWasp;
                                item.SlayerDamageBonus = 1.20f;
                                item.SlayerAdded = 1;
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 2, 3, true);

                                return item;
                            case 13:
                                item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Shadow;
                                item.SlayerDamageBonus = 1.20f;
                                item.SlayerAdded = 1;
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 2, 3, true);

                                return item;
                            case 14:
                                item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Shreth;
                                item.SlayerDamageBonus = 1.20f;
                                item.SlayerAdded = 1;
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 2, 3, true);

                                return item;
                            case 15:
                                item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Skeleton;
                                item.SlayerDamageBonus = 1.20f;
                                item.SlayerAdded = 1;
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 2, 3, true);

                                return item;
                            case 16:
                                item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Tumerok;
                                item.SlayerDamageBonus = 1.20f;
                                item.SlayerAdded = 1;
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 2, 3, true);

                                return item;
                            case 17:
                                item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Tusker;
                                item.SlayerDamageBonus = 1.20f;
                                item.SlayerAdded = 1;
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 2, 3, true);

                                return item;
                            case 18:
                                item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Virindi;
                                item.SlayerDamageBonus = 1.20f;
                                item.SlayerAdded = 1;
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 2, 3, true);

                                return item;
                            case 19:
                                item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Wisp;
                                item.SlayerDamageBonus = 1.20f;
                                item.SlayerAdded = 1;
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 2, 3, true);

                                return item;
                            case 20:
                                item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Zefir;
                                item.SlayerDamageBonus = 1.20f;
                                item.SlayerAdded = 1;
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 2, 3, true);

                                return item;
                        }
                    }
                    else if (item.ItemType == ItemType.Armor || item.ItemType == ItemType.Clothing)
                    {
                        switch (selectArmorMod)
                        {
                            case 1: // Steel Tinker

                                var tinkamount = ThreadSafeRandom.Next(1, 10);

                                item.ArmorLevel += 20 * tinkamount;
                                item.NumTimesTinkered += tinkamount;


                                Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {tinkamount} Steel tinks! (+{20 * tinkamount}AL)", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 2, 3, true);

                                return item;

                            default:
                                return item;
                        }
                    }
                    break;
                case 2: // Weapon Attribute Cantrips & Armor Skill Cantrips
                    if (item.ItemType == ItemType.MeleeWeapon || item.ItemType == ItemType.MissileWeapon || item.ItemType == ItemType.Caster)
                    {
                        var selectAttribute = ThreadSafeRandom.Next(1, 6);

                        switch (selectAttribute)
                        {
                            case 1: // strength

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPSTRENGTH2);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Strength to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 3, 3, true);

                                return item;
                            case 2: // endurance

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPENDURANCE2);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Endurance to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 3, 3, true);

                                return item;
                            case 3:// coordination

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPCOORDINATION2);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Coordination to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 3, 3, true);

                                return item;
                            case 4: // quickness

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPQUICKNESS2);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Quickness to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 3, 3, true);

                                return item;
                            case 5: // focus

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPFOCUS2);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Focus to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 3, 3, true);

                                return item;
                            case 6: // self

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPWILLPOWER2);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Willpower to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 3, 3, true);

                                return item;
                        }
                    }
                    else if (item.ItemType == ItemType.Armor || item.ItemType == ItemType.Clothing)
                    {
                        var selectSkill = ThreadSafeRandom.Next(1, 21);

                        switch (selectSkill)
                        {
                            case 1:

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPINVULNERABILITY2);

                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Invulnerability to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 3, 3, true);

                                return item;
                            case 2:

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPMAGICRESISTANCE2);

                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Magic Resistance to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 3, 3, true);

                                return item;
                            case 3:

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPLIFEMAGICAPTITUDE2);

                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Life Magic to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 3, 3, true);

                                return item;
                            case 4:

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPARCANEPROWESS2);

                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Arcane Lore to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 3, 3, true);

                                return item;
                            case 5:

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPMANACONVERSIONPROWESS2);

                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Mana Conversion to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 3, 3, true);

                                return item;
                            case 6:

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPWARMAGICAPTITUDE2);

                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major War Magic to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 3, 3, true);

                                return item;
                            case 7:

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPHEALINGPROWESS2);

                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Healing to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 3, 3, true);

                                return item;
                            case 8:

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPJUMPINGPROWESS2);

                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Jump to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 3, 3, true);

                                return item;
                            case 9:

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPSPRINT2);

                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Run to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 3, 3, true);

                                return item;
                            case 10:

                                RecipeManager.AddSpell(this, item, SpellId.CantripDualWieldAptitude2);

                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Dual Wield to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 3, 3, true);

                                return item;
                            case 11:

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPDECEPTIONPROWESS2);

                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Deception to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 3, 3, true);

                                return item;
                            case 12:

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPLEADERSHIP2);

                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Leadership to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 3, 3, true);

                                return item;
                            case 13:

                                RecipeManager.AddSpell(this, item, SpellId.CantripSneakingAptitude2);

                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Sneaking Aptitude to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 3, 3, true);

                                return item;
                            case 14:

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPFLETCHINGPROWESS2);

                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Fletching to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 3, 3, true);

                                return item;
                        }
                    }
                    break;
                case 3: // Weapon Rends & Armor Sets
                    if (item.ItemType == ItemType.MeleeWeapon || item.ItemType == ItemType.MissileWeapon || item.ItemType == ItemType.Caster)
                    {
                        var rendSelect = ThreadSafeRandom.Next(1, 7);

                        switch (rendSelect)
                        {
                            case 1: // acid

                                item.ImbuedEffect = ImbuedEffectType.AcidRending;
                                item.W_DamageType = DamageType.Acid;
                                item.SetProperty(PropertyDataId.IconUnderlay, 0x06003355);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Acid Rending to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 2, true);

                                return item;
                            case 2: // Cold

                                item.ImbuedEffect = ImbuedEffectType.ColdRending;
                                item.W_DamageType = DamageType.Cold;
                                item.SetProperty(PropertyDataId.IconUnderlay, 0x06003353);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Cold Rending to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 2, true);

                                return item;
                            case 3: // Electric

                                item.ImbuedEffect = ImbuedEffectType.ElectricRending;
                                item.W_DamageType = DamageType.Electric;
                                item.SetProperty(PropertyDataId.IconUnderlay, 0x06003354);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Lightning Rending to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 2, true);

                                return item;
                            case 4: // Fire

                                item.ImbuedEffect = ImbuedEffectType.FireRending;
                                item.W_DamageType = DamageType.Fire;
                                item.SetProperty(PropertyDataId.IconUnderlay, 0x06003359);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Fire Rending to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 2, true);

                                return item;
                            case 5: // Pierce

                                item.ImbuedEffect = ImbuedEffectType.PierceRending;
                                item.W_DamageType = DamageType.Pierce;
                                item.SetProperty(PropertyDataId.IconUnderlay, 0x0600335b);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Pierce Rending to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 2, true);

                                return item;
                            case 6: // Slash

                                item.ImbuedEffect = ImbuedEffectType.SlashRending;
                                item.W_DamageType = DamageType.Slash;
                                item.SetProperty(PropertyDataId.IconUnderlay, 0x0600335c);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Slash Rending to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 2, true);

                                return item;
                            case 7: // Bludgeon

                                item.ImbuedEffect = ImbuedEffectType.BludgeonRending;
                                item.W_DamageType = DamageType.Bludgeon;
                                item.SetProperty(PropertyDataId.IconUnderlay, 0x0600335a);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Bludgeon Rending to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 2, true);

                                return item;
                        }

                    }
                    else if (item.ItemType == ItemType.Armor || item.ItemType == ItemType.Clothing)
                    {
                        var armorSetSelect = ThreadSafeRandom.Next(1, 9);

                        switch (armorSetSelect)
                        {
                            case 1:
                                item.EquipmentSetId = EquipmentSet.Adepts;

                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Adept Set to quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 2, true);

                                return item;
                            case 2:
                                item.EquipmentSetId = EquipmentSet.Archers;

                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Archer Set to quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 2, true);

                                return item;
                            case 3:
                                item.EquipmentSetId = EquipmentSet.Crafters;

                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Crafters Set to quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 2, true);

                                return item;
                            case 4:
                                item.EquipmentSetId = EquipmentSet.Defenders;

                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Defender Set to quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 2, true);

                                return item;
                            case 5:
                                item.EquipmentSetId = EquipmentSet.Dexterous;

                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Dexterous Set to quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 2, true);

                                return item;
                            case 6:
                                item.EquipmentSetId = EquipmentSet.Hardened;

                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Hardened Set to quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 2, true);

                                return item;
                            case 7:
                                item.EquipmentSetId = EquipmentSet.Hearty;

                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Hearty Set to quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 2, true);

                                return item;
                            case 8:
                                item.EquipmentSetId = EquipmentSet.Interlocking;

                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Interlocking Set to quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 2, true);

                                return item;
                            case 9:
                                item.EquipmentSetId = EquipmentSet.Reinforced;

                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Reinforced Set to quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 2, true);

                                return item;
                        }
                    }

                    break;
                case 4: // Weapon Item Cantrips & Armor Attribute Cantrips
                    if (item.ItemType == ItemType.MeleeWeapon || item.ItemType == ItemType.MissileWeapon || item.ItemType == ItemType.Caster)
                    {
                        var selectItemSpell = ThreadSafeRandom.Next(1, 6);

                        switch (selectItemSpell)
                        {
                            case 1: // Blood Drinker / Spirit Drinker

                                if (item.ItemType != ItemType.Caster)
                                {
                                    RecipeManager.AddSpell(this, item, SpellId.CANTRIPBLOODTHIRST2);
                                    Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Blood Drinker to the quest item!", ChatMessageType.System));

                                    if (extraChance <= 15)
                                        return ModifyQuestItem(item, 1, 2, true);

                                    return item;
                                }
                                else
                                {
                                    RecipeManager.AddSpell(this, item, SpellId.CantripSpiritThirst2);
                                    Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Spirit Drinker to the quest item!", ChatMessageType.System));

                                    if (extraChance <= 15)
                                        return ModifyQuestItem(item, 1, 2, true);

                                    return item;
                                }
                            case 2: // Defender / HeartSeeker                                

                                if (splitChance == 1)
                                {
                                    RecipeManager.AddSpell(this, item, SpellId.CANTRIPDEFENDER2);
                                    Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Defender to the quest item!", ChatMessageType.System));

                                    if (extraChance <= 15)
                                        return ModifyQuestItem(item, 1, 2, true);

                                    return item;
                                }
                                else
                                {
                                    if (item.ItemType == ItemType.Caster)
                                    {
                                        if (splitChance2 == 1)
                                        {
                                            RecipeManager.AddSpell(this, item, SpellId.CantripSpiritThirst2);
                                            Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Spirit Drinker to the quest item!", ChatMessageType.System));

                                            if (extraChance <= 15)
                                                return ModifyQuestItem(item, 1, 2, true);

                                            return item;
                                        }
                                        else
                                        {
                                            RecipeManager.AddSpell(this, item, SpellId.CANTRIPDEFENDER2);
                                            Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Defender to the quest item!", ChatMessageType.System));

                                            if (extraChance <= 15)
                                                return ModifyQuestItem(item, 1, 2, true);

                                            return item;
                                        }
                                    }
                                    else
                                    {
                                        RecipeManager.AddSpell(this, item, SpellId.CANTRIPHEARTTHIRST2);
                                        Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Heart Thirst to the quest item!", ChatMessageType.System));

                                        if (extraChance <= 15)
                                            return ModifyQuestItem(item, 1, 2, true);

                                        return item;
                                    }
                                }
                            case 3:// Swift Killer / Hermetic Link

                                if (item.ItemType == ItemType.Caster)
                                {

                                    RecipeManager.AddSpell(this, item, SpellId.CantripHermeticLink2);
                                    Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Hermetic Link to the quest item!", ChatMessageType.System));

                                    if (extraChance <= 15)
                                        return ModifyQuestItem(item, 1, 2, true);

                                    return item;
                                }
                                else
                                {
                                    RecipeManager.AddSpell(this, item, SpellId.CANTRIPSWIFTHUNTER2);
                                    Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Swift Hunter to the quest item!", ChatMessageType.System));

                                    if (extraChance <= 15)
                                        return ModifyQuestItem(item, 1, 2, true);

                                    return item;
                                }
                        }
                    }
                    else if (item.ItemType == ItemType.Armor || item.ItemType == ItemType.Clothing)
                    {
                        var selectAttribute = ThreadSafeRandom.Next(1, 6);

                        switch (selectAttribute)
                        {
                            case 1: // strength

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPSTRENGTH2);

                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Strength to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 2, true);

                                return item;
                            case 2: // endurance

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPENDURANCE2);

                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Endurance to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 2, true);

                                return item;
                            case 3:// coordination

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPCOORDINATION2);

                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Coordination to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 2, true);

                                return item;
                            case 4: // quickness

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPQUICKNESS2);

                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Quickness to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 2, true);

                                return item;
                            case 5: // focus

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPFOCUS2);

                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Focus to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 2, true);

                                return item;
                            case 6: // self

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPWILLPOWER2);

                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Willpower to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 2, true);

                                return item;
                        }
                    }
                    break;
                case 5: // Weapon Skill Cantrips & Armor Ratings
                    if (item.ItemType == ItemType.MeleeWeapon || item.ItemType == ItemType.MissileWeapon || item.ItemType == ItemType.Caster)
                    {
                        var selectSkill = ThreadSafeRandom.Next(1, 21);

                        switch (selectSkill)
                        {
                            case 1:

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPINVULNERABILITY2);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Invulnerability to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 5, true);

                                return item;
                            case 2:

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPMAGICRESISTANCE2);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Magic Resistance to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 5, true);

                                return item;
                            case 4:

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPLIFEMAGICAPTITUDE2);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Life Magic to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 5, true);

                                return item;
                            case 6:

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPARCANEPROWESS2);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Arcane Lore to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 5, true);

                                return item;
                            case 7:

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPMANACONVERSIONPROWESS2);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Mana Conversion to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 5, true);

                                return item;
                            case 10:

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPWARMAGICAPTITUDE2);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major War Magic to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 5, true);

                                return item;
                            case 11:

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPHEALINGPROWESS2);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Healing to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 5, true);

                                return item;
                            case 12:

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPJUMPINGPROWESS2);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Jump to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 5, true);

                                return item;
                            case 13:

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPSPRINT2);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Run to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 5, true);

                                return item;
                            case 14:

                                RecipeManager.AddSpell(this, item, SpellId.CantripDualWieldAptitude2);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Dual Wield to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 5, true);

                                return item;
                            case 18:

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPDECEPTIONPROWESS2);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Deception to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 5, true);

                                return item;
                            case 19:

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPLEADERSHIP2);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Leadership to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 5, true);

                                return item;
                            case 20:

                                RecipeManager.AddSpell(this, item, SpellId.CantripSneakingAptitude2);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Sneaking Aptitude to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 5, true);

                                return item;
                            case 21:

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPFLETCHINGPROWESS2);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Fletching to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 5, true);

                                return item;
                            case 22:

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPLIGHTWEAPONSAPTITUDE1);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Axe Aptitude to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 5, true);

                                return item;
                            case 23:

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPHEAVYWEAPONSAPTITUDE2);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Sword Aptitude to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 5, true);

                                return item;
                            case 24:

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPFINESSEWEAPONSAPTITUDE2);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Dagger Aptitude to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 5, true);

                                return item;
                            case 25:

                                RecipeManager.AddSpell(this, item, SpellId.CANTRIPMISSILEWEAPONSAPTITUDE2);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Major Bow Aptitude to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 5, true);

                                return item;
                        }
                    }
                    else if (item.ItemType == ItemType.Armor || item.ItemType == ItemType.Clothing)
                    {
                        var selectRating = ThreadSafeRandom.Next(1, 6);
                        var ratingAmount = ThreadSafeRandom.Next(1, 3);

                        switch (selectRating)
                        {
                            case 1:

                                if (item.GearDamage == null)
                                    item.GearDamage = 0;

                                item.SetProperty(PropertyInt.GearDamage, (int)item.GearDamage + ratingAmount);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added {ratingAmount} Damage Rating to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 5, true);

                                return item;
                            case 2:

                                if (item.GearDamageResist == null)
                                    item.GearDamageResist = 0;

                                item.SetProperty(PropertyInt.GearDamageResist, (int)item.GearDamageResist + ratingAmount);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added {ratingAmount} Damage Resist Rating to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 5, true);

                                return item;
                            case 3:

                                if (item.GearCritDamageResist == null)
                                    item.GearCritDamageResist = 0;

                                item.SetProperty(PropertyInt.GearCritDamageResist, (int)item.GearCritDamageResist + ratingAmount);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added {ratingAmount} Crit Damage Resist Rating to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 5, true);

                                return item;
                            case 4:

                                if (item.GearCritDamage == null)
                                    item.GearCritDamage = 0;

                                item.SetProperty(PropertyInt.GearCritDamage, (int)item.GearCritDamage + ratingAmount);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added {ratingAmount} Crit Damage Rating to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 5, true);

                                return item;
                            case 5:

                                if (item.GearCritResist == null)
                                    item.GearCritResist = 0;

                                item.SetProperty(PropertyInt.GearCritResist, (int)item.GearCritResist + ratingAmount);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added {ratingAmount} Crit Resist Rating to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 5, true);

                                return item;
                            case 6:

                                if (item.GearCrit == null)
                                    item.GearCrit = 0;

                                item.SetProperty(PropertyInt.GearCrit, (int)item.GearCrit + ratingAmount);
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Added {ratingAmount} Crit Rating to the quest item!", ChatMessageType.System));

                                if (extraChance <= 15)
                                    return ModifyQuestItem(item, 1, 5, true);

                                return item;
                        }
                    }
                    break;
                default:
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"{selectMod}", ChatMessageType.System));
                    return item;
            }
            return item;
        }


        public bool RerollForRend(WorldObject item)
        {
            var rendcount = 0;

            if (item.HasImbuedEffect(ImbuedEffectType.AcidRending))
                rendcount++;
            if (item.HasImbuedEffect(ImbuedEffectType.ColdRending))
                rendcount++;
            if (item.HasImbuedEffect(ImbuedEffectType.ElectricRending))
                rendcount++;
            if (item.HasImbuedEffect(ImbuedEffectType.FireRending))
                rendcount++;
            if (item.HasImbuedEffect(ImbuedEffectType.SlashRending))
                rendcount++;
            if (item.HasImbuedEffect(ImbuedEffectType.PierceRending))
                rendcount++;
            if (item.HasImbuedEffect(ImbuedEffectType.BludgeonRending))
                rendcount++;

            if (rendcount > 0)
                return false;
            else
                return true;
        }

        public bool RerollForSlayer(WorldObject item)
        {
            if (item.SlayerAdded == 0 || item.SlayerAdded == null)
                return true;
            else
                return false;
        }

        public bool RerollForSet(WorldObject item)
        {
            if (item.EquipmentSetId == null)
                return true;
            else
                return false;
        }

        public WorldObject GiveSlayer(WorldObject item)
        {
            var selectSlayerType = ThreadSafeRandom.Next(1, 24);

            switch (selectSlayerType)
            {
                case 1:
                    item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Banderling;
                    item.SlayerDamageBonus = 1.20f;
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                    return item;
                case 2:
                    item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Drudge;
                    item.SlayerDamageBonus = 1.20f;
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                    return item;
                case 3:
                    item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Gromnie;
                    item.SlayerDamageBonus = 1.20f;
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                    return item;
                case 4:
                    item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Lugian;
                    item.SlayerDamageBonus = 1.20f;
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                    return item;
                case 5:
                    item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Grievver;
                    item.SlayerDamageBonus = 1.20f;
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                    return item;
                case 6:
                    item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Human;
                    item.SlayerDamageBonus = 1.20f;
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                    return item;
                case 7:
                    item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Mattekar;
                    item.SlayerDamageBonus = 1.20f;
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                    return item;
                case 8:
                    item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Mite;
                    item.SlayerDamageBonus = 1.20f;
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                    return item;
                case 9:
                    item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Mosswart;
                    item.SlayerDamageBonus = 1.20f;
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                    return item;
                case 10:
                    item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Mumiyah;
                    item.SlayerDamageBonus = 1.20f;
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                    return item;
                case 11:
                    item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Olthoi;
                    item.SlayerDamageBonus = 1.20f;
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                    return item;
                case 12:
                    item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.PhyntosWasp;
                    item.SlayerDamageBonus = 1.20f;
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                    return item;
                case 13:
                    item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Shadow;
                    item.SlayerDamageBonus = 1.20f;
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                    return item;
                case 14:
                    item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Shreth;
                    item.SlayerDamageBonus = 1.20f;
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                    return item;
                case 15:
                    item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Skeleton;
                    item.SlayerDamageBonus = 1.20f;
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                    return item;
                case 16:
                    item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Tumerok;
                    item.SlayerDamageBonus = 1.20f;
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                    return item;
                case 17:
                    item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Tusker;
                    item.SlayerDamageBonus = 1.20f;
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                    return item;
                case 18:
                    item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Virindi;
                    item.SlayerDamageBonus = 1.20f;
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                    return item;
                case 19:
                    item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Wisp;
                    item.SlayerDamageBonus = 1.20f;
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                    return item;
                case 20:
                    item.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Zefir;
                    item.SlayerDamageBonus = 1.20f;
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"This quest item was granted {item.SlayerCreatureType} slayer!", ChatMessageType.System));

                    return item;
            }
            return item;
        }

        public WorldObject GiveRend(WorldObject item)
        {
            var rendSelect = ThreadSafeRandom.Next(1, 7);

            switch (rendSelect)
            {
                case 1: // acid

                    item.ImbuedEffect = ImbuedEffectType.AcidRending;
                    item.W_DamageType = DamageType.Acid;
                    item.SetProperty(PropertyDataId.IconUnderlay, 0x06003355);
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Acid Rending to the quest item!", ChatMessageType.System));

                    return item;
                case 2: // Cold

                    item.ImbuedEffect = ImbuedEffectType.ColdRending;
                    item.W_DamageType = DamageType.Cold;
                    item.SetProperty(PropertyDataId.IconUnderlay, 0x06003353);
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Cold Rending to the quest item!", ChatMessageType.System));

                    return item;
                case 3: // Electric

                    item.ImbuedEffect = ImbuedEffectType.ElectricRending;
                    item.W_DamageType = DamageType.Electric;
                    item.SetProperty(PropertyDataId.IconUnderlay, 0x06003354);
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Lightning Rending to the quest item!", ChatMessageType.System));

                    return item;
                case 4: // Fire

                    item.ImbuedEffect = ImbuedEffectType.FireRending;
                    item.W_DamageType = DamageType.Fire;
                    item.SetProperty(PropertyDataId.IconUnderlay, 0x06003359);
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Fire Rending to the quest item!", ChatMessageType.System));

                    return item;
                case 5: // Pierce

                    item.ImbuedEffect = ImbuedEffectType.PierceRending;
                    item.W_DamageType = DamageType.Pierce;
                    item.SetProperty(PropertyDataId.IconUnderlay, 0x0600335b);
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Pierce Rend to the quest item!", ChatMessageType.System));

                    return item;
                case 6: // Slash

                    item.ImbuedEffect = ImbuedEffectType.SlashRending;
                    item.W_DamageType = DamageType.Slash;
                    item.SetProperty(PropertyDataId.IconUnderlay, 0x0600335c);
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Slash Rending to the quest item!", ChatMessageType.System));

                    return item;
                case 7: // Bludgeon

                    item.ImbuedEffect = ImbuedEffectType.BludgeonRending;
                    item.W_DamageType = DamageType.Bludgeon;
                    item.SetProperty(PropertyDataId.IconUnderlay, 0x0600335a);
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Bludgeon Rending to the quest item!", ChatMessageType.System));

                    return item;
            }

            return item;
        }
        public WorldObject GiveSet(WorldObject item)
        {
            var armorSetSelect = ThreadSafeRandom.Next(1, 9);

            switch (armorSetSelect)
            {
                case 1:
                    item.EquipmentSetId = EquipmentSet.Adepts;
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Adept Set to quest item!", ChatMessageType.System));         

                    return item;
                case 2:
                    item.EquipmentSetId = EquipmentSet.Archers;
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Archer Set to quest item!", ChatMessageType.System));

                    return item;
                case 3:
                    item.EquipmentSetId = EquipmentSet.Crafters;
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Crafters Set to quest item!", ChatMessageType.System));

                    return item;
                case 4:
                    item.EquipmentSetId = EquipmentSet.Defenders;
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Defender Set to quest item!", ChatMessageType.System));

                    return item;
                case 5:
                    item.EquipmentSetId = EquipmentSet.Dexterous;
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Dexterous Set to quest item!", ChatMessageType.System));

                    return item;
                case 6:
                    item.EquipmentSetId = EquipmentSet.Hardened;
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Hardened Set to quest item!", ChatMessageType.System));

                    return item;
                case 7:
                    item.EquipmentSetId = EquipmentSet.Hearty;
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Hearty Set to quest item!", ChatMessageType.System));

                    return item;
                case 8:
                    item.EquipmentSetId = EquipmentSet.Interlocking;
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Interlocking Set to quest item!", ChatMessageType.System));

                    return item;
                case 9:
                    item.EquipmentSetId = EquipmentSet.Reinforced;
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"Added Reinforced Set to quest item!", ChatMessageType.System));

                    return item;
            }

            return item;
        }
    }
}
