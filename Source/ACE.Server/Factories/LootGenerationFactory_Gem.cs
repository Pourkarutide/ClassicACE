using ACE.Common;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Tables;
using ACE.Server.Managers;
using ACE.Server.WorldObjects;
using System;
using System.Collections.Generic;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        private static void MutateGem(WorldObject wo, TreasureDeath profile, bool isMagical, TreasureRoll roll)
        {
            // workmanship
            wo.ItemWorkmanship = WorkmanshipChance.Roll(profile.Tier, profile.LootQualityMod);

            // item color
            MutateColor(wo);

            if (!isMagical)
            {
                // TODO: verify if this is needed
                wo.ItemUseable = Usable.No;
                wo.SpellDID = null;
                wo.ItemManaCost = null;
                wo.ItemMaxMana = null;
                wo.ItemCurMana = null;
                wo.ItemSpellcraft = null;
                wo.ItemDifficulty = null;
                wo.ItemSkillLevelLimit = null;
                wo.ManaRate = null;
            }
            else
            {
                AssignMagic_Gem(wo, profile, roll);

                wo.UiEffects = UiEffects.Magical;

                if (PropertyManager.GetBool("useable_gems").Item)
                    wo.ItemUseable = Usable.Contained;
                else
                    wo.ItemUseable = Usable.No;
            }

            // item value
            if (wo.HasMutateFilter(MutateFilter.Value))
                MutateValue(wo, profile.Tier, roll);

            // long desc
            wo.LongDesc = GetLongDesc(wo);
            if (Common.ConfigManager.Config.Server.WorldRuleset == Common.Ruleset.CustomDM && isMagical && wo.LongDesc != wo.Name)
                wo.Name = wo.LongDesc;
        }

        private static bool AssignMagic_Gem(WorldObject wo, TreasureDeath profile, TreasureRoll roll)
        {
            // TODO: move to standard AssignMagic() pipeline

            var level1SpellId = SpellSelectionTable.Roll(1);

            var spellLevel = SpellLevelChance.Roll(profile.Tier);

            var spellId = SpellLevelProgression.GetSpellAtLevel(level1SpellId, spellLevel);

            if (spellId == SpellId.Undef)
            {
                log.Error($"AssignMagic_Gem({wo.Name}, {profile.TreasureType}, {roll.ItemType}) - Failed to generate level {spellLevel} version of {level1SpellId}");
                return false;
            }

            wo.SpellDID = (uint)spellId;

            var _spell = new Server.Entity.Spell(spellId);
            bool useableGem = PropertyManager.GetBool("useable_gems").Item;

            if (useableGem)
            {
                // retail spellcraft was capped at 370
                wo.ItemSpellcraft = Math.Min(GetSpellPower(_spell), 370);
            }

            var castableMana = (int)_spell.BaseMana * 5;

            wo.ItemMaxMana = RollItemMaxMana(wo, roll, castableMana);
            wo.ItemCurMana = wo.ItemMaxMana;

            if (Common.ConfigManager.Config.Server.WorldRuleset != Common.Ruleset.CustomDM)
            {
                if (useableGem)
                {
                    // verified
                    wo.ItemManaCost = castableMana;
                }
            }
            else
            {
                if (useableGem)
                {
                    if (PropertyManager.GetBool("usable_gems_generated_with_1_mana_cost").Item)
                        wo.ItemManaCost = 1;
                    else
                        wo.ItemManaCost = (int)_spell.BaseMana;
                }
                else
                    wo.Use = "Use a Spell Extraction Scroll to extract this gem's spell without chance of failure.\n";
            }

            roll.AllSpells = new List<SpellId>();
            roll.AllSpells.Add(spellId);

            roll.LifeCreatureEnchantments = new List<SpellId>();
            roll.LifeCreatureEnchantments.Add(spellId);

            CalculateSpellcraft(wo, roll.AllSpells, true, out roll.MinEffectiveSpellcraft, out roll.MaxEffectiveSpellcraft, out roll.RolledEffectiveSpellcraft, out roll.RealSpellcraft);
            AddActivationRequirements(wo, profile, roll);

            return true;
        }

        public static ushort RollItemMaxStructure(WorldObject wo)
        {
            var maxStructure = (ushort)ThreadSafeRandom.Next(10, 20 + (int)(wo.ItemWorkmanship * 2));

            return maxStructure;
        }

        private static void MutateValue_Gem(WorldObject wo)
        {
            if (Common.ConfigManager.Config.Server.WorldRuleset != Common.Ruleset.CustomDM)
            {
                var materialMod = MaterialTable.GetValueMod(wo.MaterialType);

                var workmanshipMod = WorkmanshipChance.GetModifier(wo.ItemWorkmanship);

                wo.Value = (int)(wo.Value * materialMod * workmanshipMod);
            }
            else
            {
                var gemValue = GemMaterialChance.GemValue(wo.MaterialType);
                var materialMod = MaterialTable.GetValueMod(wo.MaterialType);

                var workmanshipMod = WorkmanshipChance.GetModifier(wo.ItemWorkmanship);

                wo.Value = (int)(gemValue * materialMod * workmanshipMod);
            }
        }
    }
}
