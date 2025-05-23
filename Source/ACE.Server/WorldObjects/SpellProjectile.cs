using ACE.Common;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories.Tables;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Structure;
using ACE.Server.WorldObjects.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ACE.Server.WorldObjects
{
    public class SpellProjectile : WorldObject
    {
        public Spell Spell;
        public ProjectileSpellType SpellType { get; set; }

        public Position SpawnPos { get; set; }
        public float DistanceToTarget { get; set; }
        public uint LifeProjectileDamage { get; set; }

        public SpellProjectileInfo Info { get; set; }

        /// <summary>
        /// Only set to true when this spell was launched by using the built-in spell on a caster
        /// </summary>
        public bool IsWeaponSpell { get; set; }

        /// <summary>
        /// If a spell projectile is from a proc source,
        /// make sure there is no attempt to re-proc again when the spell projectile hits
        /// </summary>
        public bool FromProc { get; set; }

        public int DebugVelocity;

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public SpellProjectile(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public SpellProjectile(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            // Override weenie description defaults
            ValidLocations = null;
            DefaultScriptId = null;
        }

        /// <summary>
        /// Perfroms additional set up of the spell projectile based on the spell id or its derived type.
        /// </summary>
        public void Setup(Spell spell, ProjectileSpellType spellType)
        {
            Spell = spell;
            SpellType = spellType;

            InitPhysicsObj();

            // Runtime changes to default state
            ReportCollisions = true;
            Missile = true;
            AlignPath = true;
            PathClipped = true;
            IgnoreCollisions = false;

            // FIXME: use data here
            if (!Spell.Name.Equals("Rolling Death"))
                Ethereal = false;

            if (SpellType == ProjectileSpellType.Bolt || SpellType == ProjectileSpellType.Streak
                || SpellType == ProjectileSpellType.Arc || SpellType == ProjectileSpellType.Volley || SpellType == ProjectileSpellType.Blast
                || WeenieClassId == 7276 || WeenieClassId == 7277 || WeenieClassId == 7279 || WeenieClassId == 7280)
            {
                DefaultScriptId = (uint)PlayScript.ProjectileCollision;
                DefaultScriptIntensity = 1.0f;
            }

            // Some wall spells don't have scripted collisions
            if (WeenieClassId == 7278 || WeenieClassId == 7281 || WeenieClassId == 7282 || WeenieClassId == 23144)
            {
                ScriptedCollision = false;
            }

            AllowEdgeSlide = false;

            // No need to send an ObjScale of 1.0f over the wire since that is the default value
            if (ObjScale == 1.0f)
                ObjScale = null;

            if (SpellType == ProjectileSpellType.Ring)
            {
                if (spell.Id == 3818)
                {
                    DefaultScriptId = (uint)PlayScript.Explode;
                    DefaultScriptIntensity = 1.0f;
                    ScriptedCollision = true;
                }
                else
                {
                    ScriptedCollision = false;
                }
            }

            // Projectiles with RotationSpeed get omega values and "align path" turned off which
            // creates the nice swirling animation
            if ((RotationSpeed ?? 0) != 0)
            {
                AlignPath = false;
                PhysicsObj.Omega = new Vector3((float)(Math.PI * 2 * RotationSpeed), 0, 0);
            }
        }

        public static ProjectileSpellType GetProjectileSpellType(uint spellID)
        {
            var spell = new Spell(spellID);

            if (spell.Wcid == 0)
                return ProjectileSpellType.Undef;

            if (spell.NumProjectiles == 1)
            {
                if (spell.Category >= SpellCategory.AcidStreak && spell.Category <= SpellCategory.SlashingStreak ||
                         spell.Category == SpellCategory.NetherStreak || spell.Category == SpellCategory.Fireworks)
                    return ProjectileSpellType.Streak;

                else if (spell.NonTracking)
                    return ProjectileSpellType.Arc;

                else
                    return ProjectileSpellType.Bolt;
            }

            if (spell.Category >= SpellCategory.AcidRing && spell.Category <= SpellCategory.SlashingRing || spell.SpreadAngle == 360)
                return ProjectileSpellType.Ring;

            if (spell.Category >= SpellCategory.AcidBurst && spell.Category <= SpellCategory.SlashingBurst ||
                spell.Category == SpellCategory.NetherDamageOverTimeRaising3)
                return ProjectileSpellType.Blast;

            // 1481 - Flaming Missile Volley
            if (spell.Category >= SpellCategory.AcidVolley && spell.Category <= SpellCategory.BladeVolley || spell.Name.Contains("Volley"))
                return ProjectileSpellType.Volley;

            if (spell.Category >= SpellCategory.AcidWall && spell.Category <= SpellCategory.SlashingWall)
                return ProjectileSpellType.Wall;

            if (spell.Category >= SpellCategory.AcidStrike && spell.Category <= SpellCategory.SlashingStrike)
                return ProjectileSpellType.Strike;

            return ProjectileSpellType.Undef;
        }

        public float GetProjectileScriptIntensity(ProjectileSpellType spellType)
        {
            if (spellType == ProjectileSpellType.Wall)
            {
                return 0.4f;
            }
            if (spellType == ProjectileSpellType.Ring)
            {
                if (Spell.Level == 6 || Spell.Id == 3818)
                    return 0.4f;
                if (Spell.Level == 7)
                    return 1.0f;
            }

            // Bolt, Blast, Volley, Streak and Arc all seem to use this scale
            // TODO: should this be based on spell level, or power of first scarab?
            // ie. can this use Spell.Formula.ScarabScale?
            switch (Spell.Level)
            {
                case 1:
                    return 0f;
                case 2:
                    return 0.2f;
                case 3:
                    return 0.4f;
                case 4:
                    return 0.6f;
                case 5:
                    return 0.8f;
                case 6:
                case 7:
                case 8:
                    return 1.0f;
                default:
                    return 0f;
            }
        }

        public bool WorldEntryCollision { get; set; }

        public void ProjectileImpact()
        {
            //Console.WriteLine($"{Name}.ProjectileImpact()");

            ReportCollisions = false;
            Ethereal = true;
            IgnoreCollisions = true;
            NoDraw = true;
            Cloaked = true;
            LightsStatus = false;

            PhysicsObj.set_active(false);

            if (PhysicsObj.entering_world)
            {
                // this path should only happen if spell_projectile_ethereal = false
                EnqueueBroadcast(new GameMessageScript(Guid, PlayScript.Launch, GetProjectileScriptIntensity(SpellType)));
                WorldEntryCollision = true;
            }

            EnqueueBroadcast(new GameMessageSetState(this, PhysicsObj.State));
            EnqueueBroadcast(new GameMessageScript(Guid, PlayScript.Explode, GetProjectileScriptIntensity(SpellType)));

            // this should only be needed for spell_projectile_ethereal = true,
            // however it can also fix a display issue on client in default mode,
            // where GameMessageSetState updates projectile to ethereal before it has actually collided on client,
            // causing a 'ghost' projectile to continue to sail through the target

            PhysicsObj.Velocity = Vector3.Zero;
            EnqueueBroadcast(new GameMessageVectorUpdate(this));

            ActionChain selfDestructChain = new ActionChain();
            selfDestructChain.AddDelaySeconds(5.0);
            selfDestructChain.AddAction(this, () => Destroy());
            selfDestructChain.EnqueueChain();
        }

        /// <summary>
        /// Handles collision with scenery or other static objects that would block a projectile from reaching its target,
        /// in which case the projectile should be removed with no further processing.
        /// </summary>
        public override void OnCollideEnvironment()
        {
            //Console.WriteLine($"{Name}.OnCollideEnvironment()");

            if (Info != null && ProjectileSource is Player player && player.DebugSpell)
            {
                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name}.OnCollideEnvironment()", ChatMessageType.Broadcast));
                player.Session.Network.EnqueueSend(new GameMessageSystemChat(Info.ToString(), ChatMessageType.Broadcast));
            }

            ProjectileImpact();
        }

        public override void OnCollideObject(WorldObject target)
        {
            //Console.WriteLine($"{Name}.OnCollideObject({target.Name})");

            var player = ProjectileSource as Player;

            if (Info != null && player != null && player.DebugSpell)
            {
                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name}.OnCollideObject({target?.Name} ({target?.Guid}))", ChatMessageType.Broadcast));
                player.Session.Network.EnqueueSend(new GameMessageSystemChat(Info.ToString(), ChatMessageType.Broadcast));
            }

            ProjectileImpact();

            // ensure valid creature target
            var creatureTarget = target as Creature;
            if (creatureTarget == null || target == ProjectileSource)
                return;

            if (Spell.IsDudProjectile)
                return;

            if (player != null)
            {
                player.EndSneaking();
                player.LastHitSpellProjectile = Spell;
            }

            // ensure caster can damage target
            var sourceCreature = ProjectileSource as Creature;
            if (sourceCreature != null && !sourceCreature.CanDamage(creatureTarget))
                return;

            // if player target, ensure matching PK status
            var targetPlayer = creatureTarget as Player;

            bool isPvP = player != null && targetPlayer != null;

            var pkError = ProjectileSource?.CheckPKStatusVsTarget(creatureTarget, Spell);
            if (pkError != null)
            {
                if (player != null)
                    player.Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(player.Session, pkError[0], creatureTarget.Name));

                if (targetPlayer != null)
                    targetPlayer.Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(targetPlayer.Session, pkError[1], ProjectileSource.Name));

                return;
            }

            var critical = false;
            var critDefended = false;
            var overpower = false;
            var resisted = false;
            var blocked = false;
            var isPerfectBlock = false;
            var damageBlocked = 0f;

            var damage = CalculateDamage(ProjectileSource, creatureTarget, ref critical, ref critDefended, ref overpower, ref resisted, ref blocked, ref isPerfectBlock, ref damageBlocked);

            creatureTarget.OnAttackReceived(ProjectileSource, CombatType.Magic, critical, resisted);

            if (damage != null)
            {
                if (Spell.MetaSpellType == ACE.Entity.Enum.SpellType.EnchantmentProjectile)
                {
                    // handle EnchantmentProjectile successfully landing on target
                    ProjectileSource.CreateEnchantment(creatureTarget, ProjectileSource, ProjectileLauncher, Spell, false, FromProc);
                }
                else
                {
                    if (Spell.IsExtendedSpell)
                        creatureTarget.ApplyDoT((int)(damage.Value / 4), (int)(damage.Value * 2), critical, CombatType.Magic, Spell.DamageType, ProjectileSource, ProjectileLauncher, sourceCreature?.GetCreatureSkill(Spell.School), Spell.NameWithMetaspellAdjectives);
                    else
                        DamageTarget(creatureTarget, damage.Value, critical, critDefended, overpower, blocked, isPerfectBlock, damageBlocked);

                    if (player != null && player != creatureTarget)
                    {
                        var currentTime = Time.GetUnixTime();
                        if (player.NextTechniqueNegativeActivationTime <= currentTime)
                        {
                            var techniqueTrinket = player.GetEquippedTrinket();
                            if (techniqueTrinket != null && techniqueTrinket.TacticAndTechniqueId == (int)TacticAndTechniqueType.Opportunist)
                            {
                                var chance = 0.3f;
                                if (chance > ThreadSafeRandom.Next(0.0f, 1.0f))
                                {
                                    // Chance of inflicting self damage while using the Opportunist technique.
                                    var criticalSelf = false;
                                    var critDefendedSelf = false;
                                    var overpowerSelf = false;
                                    var resistedSelf = false;
                                    var blockedSelf = false;
                                    var isPerfectBlockSelf = false;
                                    var damageBlockedSelf = 0f;

                                    var damage2 = CalculateDamage(ProjectileSource, player, ref criticalSelf, ref critDefendedSelf, ref overpowerSelf, ref resistedSelf, ref blockedSelf, ref isPerfectBlockSelf, ref damageBlockedSelf);
                                    if (damage2 != null)
                                    {
                                        if (Spell.IsExtendedSpell)
                                            creatureTarget.ApplyDoT((int)(damage2.Value / 4), (int)(damage2.Value * 2), criticalSelf, CombatType.Magic, Spell.DamageType, ProjectileSource, ProjectileLauncher, sourceCreature?.GetCreatureSkill(Spell.School), Spell.NameWithMetaspellAdjectives);
                                        else
                                            DamageTarget(player, damage2.Value, criticalSelf, critDefendedSelf, overpowerSelf, blocked, isPerfectBlock, damageBlocked);

                                        player.NextTechniqueNegativeActivationTime = currentTime + Player.TechniqueNegativeActivationInterval;
                                    }
                                }
                            }
                        }
                    }
                }

                // if this SpellProjectile has a TargetEffect, play it on successful hit
                DoSpellEffects(Spell, ProjectileSource, creatureTarget, true);

                if (player != null)
                    Proficiency.OnSuccessUse(player, player.GetCreatureSkill(Spell.School), Spell.PowerMod);

                // handle target procs
                // note that for untargeted multi-projectile spells,
                // ProjectileTarget will be null here, so procs will not apply

                // TODO: instead of ProjectileLauncher is Caster, perhaps a SpellProjectile.CanProc bool that defaults to true,
                // but is set to false if the source of a spell is from a proc, to prevent multi procs?

                if (ProjectileSource != null && ProjectileTarget != null && !FromProc)
                {
                    // TODO figure out why cross-landblock group operations are happening here. We shouldn't need this code Mag-nus 2021-02-09
                    bool threadSafe = true;

                    if (LandblockManager.CurrentlyTickingLandblockGroupsMultiThreaded)
                    {
                        // Ok... if we got here, we're likely in the parallel landblock physics processing.
                        if (ProjectileSource.CurrentLandblock == null || ProjectileSource.CurrentLandblock == null || ProjectileSource.CurrentLandblock.CurrentLandblockGroup != target.CurrentLandblock.CurrentLandblockGroup)
                            threadSafe = false;
                    }

                    if (threadSafe)
                    {
                        if (sourceCreature != null && creatureTarget != null)
                        {
                            // This can result in spell projectiles being added to either sourceCreature or creatureTargets landblock.
                            sourceCreature.TryProcEquippedItems(sourceCreature, creatureTarget, false, ProjectileLauncher);
                        }

                        HandleEnchainSpell(Spell, ProjectileSource, ProjectileLauncher, creatureTarget);
                    }
                    else
                    {
                        // ProjectileSource and target are now in different landblock groups.
                        // What has likely happened is that sourceCreature sent a projectile toward creatureTarget. Before impact, ProjectileSource was teleported away.
                        // To perform this fully thread safe, we would enqueue the work onto worldManager.
                        // WorldManager.EnqueueAction(new ActionEventDelegate(() => sourceCreature.TryProcEquippedItems(creatureTarget, false)));
                        // But, to keep it simple, we will just ignore it and not bother with TryProcEquippedItems for this particular impact.
                    }
                }

                // also called on resist
                if (player != null && targetPlayer == null)
                    player.OnAttackMonster(creatureTarget);

                if (player == null && targetPlayer == null)
                {
                    // check for faction combat
                    if (sourceCreature != null && creatureTarget != null && (sourceCreature.AllowFactionCombat(creatureTarget) || sourceCreature.PotentialFoe(creatureTarget)))
                        sourceCreature.MonsterOnAttackMonster(creatureTarget);
                }
            }
        }

        /// <summary>
        /// Calculates the damage for a spell projectile
        /// Used by war magic, void magic, and life magic projectiles
        /// </summary>
        public float? CalculateDamage(WorldObject source, Creature target, ref bool criticalHit, ref bool critDefended, ref bool overpower, ref bool resisted, ref bool blocked, ref bool isPerfectBlock, ref float damageBlocked)
        {
            var sourcePlayer = source as Player;
            var targetPlayer = target as Player;
            var isPvP = sourcePlayer != null && targetPlayer != null;

            if (source == null || target == null || target.IsDead || target.Invincible || target.IsOnNoDamageLandblock)
                return null;

            // check lifestone protection
            if (targetPlayer != null && targetPlayer.UnderLifestoneProtection)
            {
                if (sourcePlayer != null)
                    sourcePlayer.Session.Network.EnqueueSend(new GameMessageSystemChat($"The Lifestone's magic protects {targetPlayer.Name} from the attack!", ChatMessageType.Magic));

                targetPlayer.HandleLifestoneProtection();
                return null;
            }

            //Arenas - If this is an arena landblock
            //don't allow any dmg except while the event is in a started status and between non-eliminated players            
            if (targetPlayer != null && ArenaLocation.IsArenaLandblock(targetPlayer.Location.Landblock))
            {
                var arenaEvent = ArenaManager.GetArenaEventByLandblock(targetPlayer.Location.Landblock);
                if (arenaEvent == null || arenaEvent.Status != 4)
                {
                    return 0.0f;
                }

                if (sourcePlayer != null && sourcePlayer.IsArenaObserver)
                    return 0.0f;
            }

            var critDamageBonus = 0.0f;
            var weaponCritDamageMod = 1.0f;
            var weaponResistanceMod = 1.0f;
            var resistanceMod = 1.0f;

            // life magic
            var lifeMagicDamage = 0.0f;

            // war/void magic
            var baseDamage = 0;
            var skillBonus = 0.0f;
            var finalDamage = 0.0f;

            var resistanceType = Creature.GetResistanceType(Spell.DamageType);

            var sourceCreature = source as Creature;
            if (sourceCreature?.Overpower != null)
                overpower = Creature.GetOverpower(sourceCreature, target);

            var weapon = ProjectileLauncher;

            var resistSource = IsWeaponSpell ? weapon : source;

            resisted = source.TryResistSpell(target, Spell, resistSource, true);
            if (resisted && !overpower)
                return null;

            var shieldMod = 1.0f;
            if (Common.ConfigManager.Config.Server.WorldRuleset == Common.Ruleset.CustomDM && !Spell.IsExtendedSpell)
            {
                var shield = target.GetEquippedShield();
                if (shield != null && target != source)
                {
                    uint magicSkill;
                    if (sourceCreature != null)
                        magicSkill = sourceCreature.GetCreatureSkill(Spell.School).Current;
                    else
                        magicSkill = (uint)(source.ItemSpellcraft ?? 100);

                    var effectiveBlockSkill = target.GetEffectiveShieldSkill(CombatType.Magic);
                    var blockChance = Creature.GetBlockChance(shield, target, magicSkill, effectiveBlockSkill, isPvP);
                    if (blockChance > ThreadSafeRandom.Next(0.0f, 1.0f))
                    {
                        blocked = true;

                        var shieldSkill = target.GetShieldSkill();
                        var perfectBlockChance = GetWeaponCriticalChance(shield, target, shieldSkill, sourceCreature, isPvP);
                        if (perfectBlockChance > ThreadSafeRandom.Next(0.0f, 1.0f))
                        {
                            isPerfectBlock = true;
                            if (weapon != null)
                            {
                                if (weapon.HasImbuedEffect(ImbuedEffectType.IgnoreAllArmor))
                                    shieldMod = 1.0f;
                                else
                                    shieldMod = sourceCreature.GetIgnoreShieldMod(sourceCreature, weapon, isPvP);
                            }
                            else
                                shieldMod = 0.0f;
                        }
                        else
                            shieldMod = target.GetShieldMod(source, Spell.DamageType, weapon, isPvP, 0.25f);
                    }
                    else
                    {
                        blocked = false;
                        shieldMod = target.GetShieldMod(source, Spell.DamageType, weapon, isPvP, 0.025f);
                    }
                }
                else
                {
                    blocked = false;
                    shieldMod = 1.0f;
                }
            }

            CreatureSkill attackSkill = null;
            if (sourceCreature != null)
                attackSkill = sourceCreature.GetCreatureSkill(Spell.School);

            TacticAndTechniqueType techniqueId = TacticAndTechniqueType.None;
            WorldObject techniqueTrinket = null;

            if (sourcePlayer != null && Common.ConfigManager.Config.Server.WorldRuleset == Common.Ruleset.CustomDM)
            {
                techniqueTrinket = sourcePlayer.GetEquippedTrinket();
                if (techniqueTrinket != null)
                    techniqueId = (TacticAndTechniqueType)techniqueTrinket.TacticAndTechniqueId;
            }

            // critical hit
            var criticalChance = GetWeaponMagicCritFrequency(weapon, sourceCreature, attackSkill, target, isPvP);

            if (sourcePlayer != null && source != target)
            {
                if (techniqueId == TacticAndTechniqueType.Opportunist)
                    criticalChance += 0.10f;
            }

            if (source == target)
                criticalChance = 0.0f; // Self-damage never crits.

            if (ThreadSafeRandom.Next(0.0f, 1.0f) < criticalChance)
            {
                if (targetPlayer != null && targetPlayer.AugmentationCriticalDefense > 0)
                {
                    var criticalDefenseMod = sourcePlayer != null ? 0.05f : 0.25f;
                    var criticalDefenseChance = targetPlayer.AugmentationCriticalDefense * criticalDefenseMod;

                    if (criticalDefenseChance > ThreadSafeRandom.Next(0.0f, 1.0f))
                        critDefended = true;
                }

                if (!critDefended)
                    criticalHit = true;
            }

            var absorbMod = GetAbsorbMod(this, target);

            //http://acpedia.org/wiki/Announcements_-_2014/01_-_Forces_of_Nature - Aegis is 72% effective in PvP
            if (isPvP && (target.CombatMode == CombatMode.Melee || target.CombatMode == CombatMode.Missile) && Common.ConfigManager.Config.Server.WorldRuleset != Common.Ruleset.CustomDM)
            {
                absorbMod = 1 - absorbMod;
                absorbMod *= 0.72f;
                absorbMod = 1 - absorbMod;
            }

            if (isPvP && Spell.IsHarmful)
                Player.UpdatePKTimers(sourcePlayer, targetPlayer);

            var elementalDamageMod = GetCasterElementalDamageModifier(weapon, sourceCreature, target, Spell.DamageType);

            // Possible 2x + damage bonus for the slayer property
            var slayerMod = GetWeaponCreatureSlayerModifier(weapon, sourceCreature, target);

            // life magic projectiles: ie., martyr's hecatomb
            if (Spell.MetaSpellType == ACE.Entity.Enum.SpellType.LifeProjectile)
            {
                lifeMagicDamage = LifeProjectileDamage * Spell.DamageRatio;

                if (!isPvP || Common.ConfigManager.Config.Server.WorldRuleset != Common.Ruleset.CustomDM)
                {
                    // could life magic projectiles crit?
                    // if so, did they use the same 1.5x formula as war magic, instead of 2.0x?
                    if (criticalHit)
                    {
                        // verify: CriticalMultiplier only applied to the additional crit damage,
                        // whereas CD/CDR applied to the total damage (base damage + additional crit damage)
                        weaponCritDamageMod = GetWeaponCritDamageMod(weapon, sourceCreature, attackSkill, target, isPvP);

                        critDamageBonus = lifeMagicDamage * 0.5f * weaponCritDamageMod;
                    }
                }

                weaponResistanceMod = GetWeaponResistanceModifier(weapon, sourceCreature, attackSkill, Spell.DamageType);

                // if attacker/weapon has IgnoreMagicResist directly, do not transfer to spell projectile
                // only pass if SpellProjectile has it directly, such as 2637 - Invoking Aun Tanua

                resistanceMod = (float)Math.Max(0.0f, target.GetResistanceMod(resistanceType, this, null, weaponResistanceMod));

                finalDamage = (lifeMagicDamage + critDamageBonus) * elementalDamageMod * slayerMod * resistanceMod * absorbMod;
            }
            // war/void magic projectiles
            else
            {
                if (criticalHit)
                {
                    // Original:
                    // http://acpedia.org/wiki/Announcements_-_2002/08_-_Atonement#Letter_to_the_Players

                    // Critical Strikes: In addition to the skill-based damage bonus, each projectile spell has a 2% chance of causing a critical hit on the target and doing increased damage.
                    // A magical critical hit is similar in some respects to melee critical hits (although the damage calculation is handled differently).
                    // While a melee critical hit automatically does twice the maximum damage of the weapon, a magical critical hit will do an additional half the minimum damage of the spell.
                    // For instance, a magical critical hit from a level 7 spell, which does 110-180 points of damage, would add an additional 55 points of damage to the spell.

                    // Later updated for PvE only:

                    // http://acpedia.org/wiki/Announcements_-_2004/07_-_Treaties_in_Stone#Letter_to_the_Players

                    // Currently when a War Magic spell scores a critical hit, it adds a multiple of the base damage of the spell to a normal damage roll.
                    // Starting in July, War Magic critical hits will instead add a multiple of the maximum damage of the spell.
                    // No more crits that do less damage than non-crits!

                    if (isPvP) // PvP: 50% of the MIN damage added to normal damage roll
                        critDamageBonus = Spell.MinDamage * 0.5f;
                    else   // PvE: 50% of the MAX damage added to normal damage roll
                        critDamageBonus = Spell.MaxDamage * 0.5f;

                    // verify: CriticalMultiplier only applied to the additional crit damage,
                    // whereas CD/CDR applied to the total damage (base damage + additional crit damage)
                    weaponCritDamageMod = GetWeaponCritDamageMod(weapon, sourceCreature, attackSkill, target, isPvP);

                    critDamageBonus *= weaponCritDamageMod;
                }

                /* War Magic skill-based damage bonus
                 * http://acpedia.org/wiki/Announcements_-_2002/08_-_Atonement#Letter_to_the_Players
                 */
                if (sourcePlayer != null)
                {
                    var magicSkill = sourcePlayer.GetCreatureSkill(Spell.School).Current;

                    if (magicSkill > Spell.Power)
                    {
                        var percentageBonus = (magicSkill - Spell.Power) / 1000.0f;

                        skillBonus = Spell.MinDamage * percentageBonus;
                    }
                }

                if (Spell.IsMaximizedSpell)
                    baseDamage = Spell.MaxDamage;
                else
                    baseDamage = ThreadSafeRandom.Next(Spell.MinDamage, Spell.MaxDamage);

                if (Common.ConfigManager.Config.Server.WorldRuleset == Common.Ruleset.CustomDM)
                {
                    if (sourcePlayer == null && sourceCreature != null)
                    {
                        var mobWarDmgScale = PropertyManager.GetDouble("customdm_mob_war_damage_scale", 1.0).Item;
                        if (mobWarDmgScale != 1.0)
                            baseDamage = (int)Math.Round(baseDamage * mobWarDmgScale);
                    }
                    else if (sourcePlayer != null && !isPvP)
                    {
                        var playerWarDmgScale = PropertyManager.GetDouble("customdm_player_war_damage_scale_pve", 1.0).Item;
                        if (playerWarDmgScale != 1.0)
                            baseDamage = (int)Math.Round(baseDamage * playerWarDmgScale);
                    }
                }
                else if (Common.ConfigManager.Config.Server.WorldRuleset <= Common.Ruleset.Infiltration)
                {
                    if (sourcePlayer == null)
                        baseDamage /= 2; // Monsters do half projectile spell damage.
                }

                weaponResistanceMod = GetWeaponResistanceModifier(weapon, sourceCreature, attackSkill, Spell.DamageType);

                // if attacker/weapon has IgnoreMagicResist directly, do not transfer to spell projectile
                // only pass if SpellProjectile has it directly, such as 2637 - Invoking Aun Tanua

                resistanceMod = (float)Math.Max(0.0f, target.GetResistanceMod(resistanceType, this, null, weaponResistanceMod));

                if (isPvP && Spell.DamageType == DamageType.Nether)
                {
                    // for direct damage from void spells in pvp,
                    // apply void_pvp_modifier *on top of* the player's natural resistance to nether

                    // this supposedly brings the direct damage from void spells in pvp closer to retail
                    resistanceMod *= (float)PropertyManager.GetDouble("void_pvp_modifier").Item;
                }

                if (Common.ConfigManager.Config.Server.WorldRuleset == Common.Ruleset.CustomDM && FromProc && sourcePlayer != null)
                {
                    var dmgScale = 0.5f + sourcePlayer.ScaleWithPowerAccuracyBar(0.5f);
                    baseDamage = (int)(baseDamage * dmgScale);
                    critDamageBonus = critDamageBonus * dmgScale;

                    skillBonus *= 2;
                }

                finalDamage = baseDamage + critDamageBonus + skillBonus;

                finalDamage *= elementalDamageMod * slayerMod * resistanceMod * absorbMod;
            }

            if (shieldMod != 1.0f && Common.ConfigManager.Config.Server.WorldRuleset == Common.Ruleset.CustomDM)
            {
                var damageBeforeShieldMod = finalDamage;
                finalDamage *= shieldMod;

                damageBlocked = damageBeforeShieldMod - finalDamage;
            }

            //Apply pvp dmg mods for war and void (not including DOTs which are in EnchantmentManager.ApplyDamageTick)
            if (isPvP)
            {
                float pvpMod = (float)PropertyManager.GetInterpolatedDouble(Level ?? 1, "pvp_dmg_mod_low", "pvp_dmg_mod_high", "pvp_dmg_mod_low_level", "pvp_dmg_mod_high_level");

                if (Spell.School == MagicSchool.WarMagic)
                {
                    if (FromProc)
                        pvpMod *= (float)PropertyManager.GetInterpolatedDouble(Level ?? 1, "pvp_dmg_mod_low_unarmed_war", "pvp_dmg_mod_high_unarmed_war", "pvp_dmg_mod_low_level", "pvp_dmg_mod_high_level");
                    else if (SpellType == ProjectileSpellType.Streak)
                        pvpMod *= (float)PropertyManager.GetInterpolatedDouble(Level ?? 1, "pvp_dmg_mod_low_war_streak", "pvp_dmg_mod_high_war_streak", "pvp_dmg_mod_low_level", "pvp_dmg_mod_high_level");
                    else
                        pvpMod *= (float)PropertyManager.GetInterpolatedDouble(Level ?? 1, "pvp_dmg_mod_low_war", "pvp_dmg_mod_high_war", "pvp_dmg_mod_low_level", "pvp_dmg_mod_high_level");

                }
                else if (Spell.DamageType == DamageType.Nether)
                {
                    if (SpellType == ProjectileSpellType.Streak)
                        pvpMod *= (float)PropertyManager.GetInterpolatedDouble(Level ?? 1, "pvp_dmg_mod_low_void_streak", "pvp_dmg_mod_high_void_streak", "pvp_dmg_mod_low_level", "pvp_dmg_mod_high_level");
                    else
                        pvpMod *= (float)PropertyManager.GetInterpolatedDouble(Level ?? 1, "pvp_dmg_mod_low_void", "pvp_dmg_mod_high_void", "pvp_dmg_mod_low_level", "pvp_dmg_mod_high_level");
                }

                finalDamage = finalDamage * pvpMod;
            }

            // show debug info
            if (sourceCreature != null && sourceCreature.DebugDamage.HasFlag(Creature.DebugDamageType.Attacker))
            {
                ShowInfo(sourceCreature, Spell, attackSkill, criticalChance, criticalHit, critDefended, overpower, weaponCritDamageMod, skillBonus, baseDamage, critDamageBonus, elementalDamageMod, slayerMod, weaponResistanceMod, resistanceMod, absorbMod, LifeProjectileDamage, lifeMagicDamage, finalDamage);
            }
            if (target.DebugDamage.HasFlag(Creature.DebugDamageType.Defender))
            {
                ShowInfo(target, Spell, attackSkill, criticalChance, criticalHit, critDefended, overpower, weaponCritDamageMod, skillBonus, baseDamage, critDamageBonus, elementalDamageMod, slayerMod, weaponResistanceMod, resistanceMod, absorbMod, LifeProjectileDamage, lifeMagicDamage, finalDamage);
            }

            //Arenas - If this is an arena landblock
            //track dmg dealt and received
            if (targetPlayer != null && ArenaLocation.IsArenaLandblock(targetPlayer.Location.Landblock))
            {
                var arenaEvent = ArenaManager.GetArenaEventByLandblock(targetPlayer.Location.Landblock);
                if (arenaEvent != null && arenaEvent.Status == 4 && sourcePlayer != null)
                {
                    var attackerArenaPlayer = arenaEvent.Players.FirstOrDefault(x => x.CharacterId == sourcePlayer.Character.Id);
                    var defenderArenaPlayer = arenaEvent.Players.FirstOrDefault(x => x.CharacterId == targetPlayer.Character.Id);

                    if (attackerArenaPlayer != null && defenderArenaPlayer != null)
                    {
                        attackerArenaPlayer.TotalDmgDealt += (uint)Math.Round(finalDamage);
                        defenderArenaPlayer.TotalDmgReceived += (uint)Math.Round(finalDamage);
                    }
                }
            }

            return finalDamage;
        }

        public static float GetAbsorbMod(WorldObject source, Creature target)
        {
            if (Common.ConfigManager.Config.Server.WorldRuleset != Common.Ruleset.CustomDM)
            {
                switch (target.CombatMode)
                {
                    case CombatMode.Melee:

                        // does target have shield equipped?
                        var shield = target.GetEquippedShield();
                        if (shield != null && shield.GetAbsorbMagicDamage() != null)
                        {
                            if (Common.ConfigManager.Config.Server.WorldRuleset != Common.Ruleset.Infiltration)
                                return GetShieldMod(source, target, shield);
                            else
                                return AbsorbMagic(target, shield);
                        }

                        break;

                    case CombatMode.Missile:

                        var missileLauncherOrShield = target.GetEquippedMissileLauncher() ?? target.GetEquippedShield();
                        if (missileLauncherOrShield != null && missileLauncherOrShield.GetAbsorbMagicDamage() != null)
                            return AbsorbMagic(target, missileLauncherOrShield);

                        break;

                    case CombatMode.Magic:

                        var caster = target.GetEquippedWand();
                        if (caster != null && caster.GetAbsorbMagicDamage() != null)
                            return AbsorbMagic(target, caster);

                        break;
                }
                return 1.0f;
            }
            else
            {
                float shieldMagicAbsorb = 1.0f;
                float casterMagicAbsorb = 1.0f;
                float missileLauncherMagicAbsorb = 1.0f;

                var shield = target.GetEquippedShield();
                if (shield != null && shield.GetAbsorbMagicDamage() != null)
                    shieldMagicAbsorb = GetShieldMod(source, target, shield);

                var caster = target.GetEquippedWand();
                if (caster != null && caster.GetAbsorbMagicDamage() != null)
                    casterMagicAbsorb = AbsorbMagic(target, caster);

                var missileLauncher = target.GetEquippedMissileLauncher();
                if (missileLauncher != null && missileLauncher.GetAbsorbMagicDamage() != null)
                    missileLauncherMagicAbsorb = AbsorbMagic(target, missileLauncher);

                var casterOrMissileAbsorb = Math.Min(casterMagicAbsorb, missileLauncherMagicAbsorb);
                return Math.Min(shieldMagicAbsorb, casterOrMissileAbsorb);
            }
        }

        /// <summary>
        /// Calculates the amount of damage a shield absorbs from magic projectile
        /// </summary>
        public static float GetShieldMod(WorldObject source, Creature target, WorldObject shield)
        {
            bool bypassShieldAngleCheck = false;
            if (Common.ConfigManager.Config.Server.WorldRuleset == Common.Ruleset.CustomDM)
            {
                var techniqueTrinket = target.GetEquippedTrinket();
                if (techniqueTrinket != null && techniqueTrinket.TacticAndTechniqueId == (int)TacticAndTechniqueType.Defensive)
                    bypassShieldAngleCheck = true; // Shields cover all angles while using the Defensive technique.
            }

            if (!bypassShieldAngleCheck)
            {
                // is spell projectile in front of creature target,
                // within shield effectiveness area?
                var effectiveAngle = 180.0f;
                var angle = target.GetAngle(source);
                if (Math.Abs(angle) > effectiveAngle / 2.0f)
                    return 1.0f;
            }

            if (Common.ConfigManager.Config.Server.WorldRuleset != Common.Ruleset.CustomDM)
            {
                // https://asheron.fandom.com/wiki/Shield
                // The formula to determine magic absorption for shields is:
                // Reduction Percent = (cap * specMod * baseSkill * 0.003f) - (cap * specMod * 0.3f)
                // Cap = Maximum reduction
                // SpecMod = 1.0 for spec, 0.8 for trained
                // BaseSkill = 100 to 433 (above 433 base shield you always achieve the maximum %)

                var shieldSkill = target.GetCreatureSkill(Skill.Shield);
                // ensure trained?
                if (shieldSkill.AdvancementClass < SkillAdvancementClass.Trained || shieldSkill.Base < 100)
                    return 1.0f;

                var baseSkill = Math.Min(shieldSkill.Base, 433);
                var specMod = shieldSkill.AdvancementClass == SkillAdvancementClass.Specialized ? 1.0f : 0.8f;
                var cap = (float)(shield.GetAbsorbMagicDamage() ?? 0.0f);

                // speced, 100 skill = 0%
                // trained, 100 skill = 0%
                // speced, 200 skill = 30%
                // trained, 200 skill = 24%
                // speced, 300 skill = 60%
                // trained, 300 skill = 48%
                // speced, 433 skill = 100%
                // trained, 433 skill = 80%

                var reduction = (cap * specMod * baseSkill * 0.003f) - (cap * specMod * 0.3f);

                var shieldMod = Math.Min(1.0f, 1.0f - reduction);
                return shieldMod;
            }
            else
                return Math.Min(1.0f, 1.0f - (float)(shield.GetAbsorbMagicDamage() ?? 0.0f));
        }

        /// <summary>
        /// Calculates the damage reduction modifier for bows and casters
        /// with 'Magic Absorbing' property
        /// </summary>
        public static float AbsorbMagic(Creature target, WorldObject item)
        {
            // https://asheron.fandom.com/wiki/Category:Magic_Absorbing

            // Tomes and Bows
            // The formula to determine magic absorption for Tomes and the Fetish of the Dark Idols:
            // - For a 25% maximum item: (magic absorbing %) = 25 - (0.1 * (319 - base magic defense))
            // - For a 10% maximum item: (magic absorbing %) = 10 - (0.04 * (319 - base magic defense))

            // wiki currently has what is likely a typo for the 10% formula,
            // where it has a factor of 0.4 instead of 0.04
            // with 0.4, the 10% items would not start to become effective until base magic defense 294
            // with 0.04, both formulas start to become effective at base magic defense 69

            // using an equivalent formula that produces the correct results for 10% and 25%,
            // and also produces the correct results for any %

            var absorbMagicDamage = item.GetAbsorbMagicDamage();

            if (absorbMagicDamage == null)
                return 1.0f;

            var maxPercent = absorbMagicDamage.Value;

            var baseCap = 319;
            var magicDefBase = target.GetCreatureSkill(Skill.MagicDefense).Base;
            var diff = Math.Max(0, baseCap - magicDefBase);

            var percent = maxPercent - maxPercent * diff * 0.004f;

            return Math.Min(1.0f, 1.0f - (float)percent);
        }

        /// <summary>
        /// Called for a spell projectile to damage its target
        /// </summary>
        public void DamageTarget(Creature target, float damage, bool critical, bool critDefended, bool overpower, bool blocked, bool isPerfectBlock, float damageBlocked)
        {
            var targetPlayer = target as Player;

            if (target == null || target.Invincible || target.IsDead || target.IsOnNoDamageLandblock)
                return;

            var sourceCreature = ProjectileSource as Creature;
            var sourcePlayer = ProjectileSource as Player;

            var pkBattle = sourcePlayer != null && targetPlayer != null;

            var amount = 0u;
            var percent = 0.0f;

            var damageRatingMod = 1.0f;
            var heritageMod = 1.0f;
            var sneakAttackMod = 1.0f;
            var critDamageRatingMod = 1.0f;
            var pkDamageRatingMod = 1.0f;

            var damageResistRatingMod = 1.0f;
            var critDamageResistRatingMod = 1.0f;
            var pkDamageResistRatingMod = 1.0f;

            WorldObject equippedCloak = null;

            // handle life projectiles for stamina / mana
            if (Spell.Category == SpellCategory.StaminaLowering)
            {
                percent = damage / target.Stamina.MaxValue;
                amount = (uint)-target.UpdateVitalDelta(target.Stamina, (int)-Math.Round(damage));
            }
            else if (Spell.Category == SpellCategory.ManaLowering)
            {
                percent = damage / target.Mana.MaxValue;
                amount = (uint)-target.UpdateVitalDelta(target.Mana, (int)-Math.Round(damage));
            }
            else
            {
                // for possibly applying sneak attack to magic projectiles,
                // only do this for health-damaging projectiles?
                if (sourcePlayer != null)
                {
                    // TODO: use target direction vs. projectile position, instead of player position
                    // could sneak attack be applied to void DoTs?
                    sneakAttackMod = sourcePlayer.GetSneakAttackMod(target);
                    //Console.WriteLine("Magic sneak attack:  + sneakAttackMod);
                    heritageMod = sourcePlayer.GetHeritageBonus(sourcePlayer.GetEquippedWand()) ? 1.05f : 1.0f;
                }

                var damageRating = sourceCreature?.GetDamageRating() ?? 0;
                damageRatingMod = Creature.AdditiveCombine(Creature.GetPositiveRatingMod(damageRating), heritageMod, sneakAttackMod);

                damageResistRatingMod = target.GetDamageResistRatingMod(CombatType.Magic);

                if (critical)
                {
                    critDamageRatingMod = Creature.GetPositiveRatingMod(sourceCreature?.GetCritDamageRating() ?? 0);
                    critDamageResistRatingMod = Creature.GetNegativeRatingMod(target.GetCritDamageResistRating());

                    damageRatingMod = Creature.AdditiveCombine(damageRatingMod, critDamageRatingMod);
                    damageResistRatingMod = Creature.AdditiveCombine(damageResistRatingMod, critDamageResistRatingMod);
                }

                if (pkBattle)
                {
                    pkDamageRatingMod = Creature.GetPositiveRatingMod(sourceCreature?.GetPKDamageRating() ?? 0);
                    pkDamageResistRatingMod = Creature.GetNegativeRatingMod(target.GetPKDamageResistRating());

                    damageRatingMod = Creature.AdditiveCombine(damageRatingMod, pkDamageRatingMod);
                    damageResistRatingMod = Creature.AdditiveCombine(damageResistRatingMod, pkDamageResistRatingMod);
                }

                damage *= damageRatingMod * damageResistRatingMod;

                percent = damage / target.Health.MaxValue;

                //Console.WriteLine($"Damage rating: " + Creature.ModToRating(damageRatingMod));

                equippedCloak = target.EquippedCloak;

                if (equippedCloak != null && Cloak.HasDamageProc(equippedCloak) && Cloak.RollProc(equippedCloak, percent))
                {
                    var reducedDamage = Cloak.GetReducedAmount(ProjectileSource, damage);

                    Cloak.ShowMessage(target, ProjectileSource, damage, reducedDamage);

                    damage = reducedDamage;
                    percent = damage / target.Health.MaxValue;
                }

                amount = (uint)-target.UpdateVitalDelta(target.Health, (int)-Math.Round(damage));
                target.DamageHistory.Add(ProjectileSource, Spell.DamageType, amount);

                //if (targetPlayer != null && targetPlayer.Fellowship != null)
                    //targetPlayer.Fellowship.OnVitalUpdate(targetPlayer);
            }

            amount = (uint)Math.Round(damage);    // full amount for debugging

            // show debug info
            if (sourceCreature != null && sourceCreature.DebugDamage.HasFlag(Creature.DebugDamageType.Attacker))
            {
                ShowInfo(sourceCreature, heritageMod, sneakAttackMod, damageRatingMod, damageResistRatingMod, critDamageRatingMod, critDamageResistRatingMod, pkDamageRatingMod, pkDamageResistRatingMod, damage);
            }
            if (target.DebugDamage.HasFlag(Creature.DebugDamageType.Defender))
            {
                ShowInfo(target, heritageMod, sneakAttackMod, damageRatingMod, damageResistRatingMod, critDamageRatingMod, critDamageResistRatingMod, pkDamageRatingMod, pkDamageResistRatingMod, damage);
            }

            if (target.IsAlive)
            {
                string verb = null, plural = null;
                Strings.GetAttackVerb(Spell.DamageType, percent, ref verb, ref plural);
                var type = Spell.DamageType.GetName().ToLower();

                var critMsg = critical ? "Critical hit! " : "";
                var sneakMsg = sneakAttackMod > 1.0f ? "Sneak Attack! " : "";
                var overpowerMsg = overpower ? "Overpower! " : "";

                var nonHealth = Spell.Category == SpellCategory.StaminaLowering || Spell.Category == SpellCategory.ManaLowering;

                if (sourcePlayer != null)
                {
                    var critProt = critDefended ? " Your critical hit was avoided with their augmentation!" : "";

                    var yourselfOrTargetName = target.Name;

                    if (sourcePlayer == target)
                        yourselfOrTargetName = "yourself";

                    var attackerMsg = $"{critMsg}{overpowerMsg}{sneakMsg}You {verb} {yourselfOrTargetName} for {amount} points with {Spell.NameWithMetaspellAdjectives}.{critProt}";

                    // could these crit / sneak attack?
                    if (nonHealth)
                    {
                        var vital = Spell.Category == SpellCategory.StaminaLowering ? "stamina" : "mana";
                        attackerMsg = $"With {Spell.NameWithMetaspellAdjectives} you drain {amount} points of {vital} from {yourselfOrTargetName}.";
                    }

                    if (!sourcePlayer.SquelchManager.Squelches.Contains(target, ChatMessageType.Magic))
                        sourcePlayer.Session.Network.EnqueueSend(new GameMessageSystemChat(attackerMsg, ChatMessageType.Magic));
                }

                if (targetPlayer != null && sourcePlayer != targetPlayer)
                {
                    var critProt = critDefended ? " Your augmentation allows you to avoid a critical hit!" : "";

                    var defenderMsg = $"{critMsg}{overpowerMsg}{sneakMsg}{ProjectileSource.Name} {plural} you for {amount} points with {Spell.NameWithMetaspellAdjectives}.{critProt}";

                    if (nonHealth)
                    {
                        var vital = Spell.Category == SpellCategory.StaminaLowering ? "stamina" : "mana";
                        defenderMsg = $"{ProjectileSource.Name} {(Spell.IsQuickcastSpell ? "quickcasts" : "casts")} {Spell.NameWithMetaspellAdjectivesWithoutQuickcast} and drains {amount} points of your {vital}.";
                    }

                    if (!targetPlayer.SquelchManager.Squelches.Contains(ProjectileSource, ChatMessageType.Magic))
                    {
                        targetPlayer.Session.Network.EnqueueSend(new GameMessageSystemChat(defenderMsg, ChatMessageType.Magic));

                        if (damageBlocked > 0)
                        {
                            if (blocked)
                            {
                                targetPlayer.Session.Network.EnqueueSend(new GameMessageSystemChat($"{(isPerfectBlock ? "Perfect Block! " : "")}Your shield blocks {damageBlocked:N0} points of {Spell.DamageType.GetName()} damage!", ChatMessageType.Magic));

                                var blockSound = new GameMessageSound(Guid, Sound.HitLeather1, 1.0f);
                                EnqueueBroadcast(blockSound);
                            }
                            else
                                targetPlayer.Session.Network.EnqueueSend(new GameMessageSystemChat($"Your shield obstructs {damageBlocked:N0} points of {Spell.DamageType.GetName()} damage!", ChatMessageType.Magic));
                        }
                    }

                    if (sourceCreature != null)
                        targetPlayer.SetCurrentAttacker(sourceCreature);
                }

                if (!nonHealth)
                {
                    if (equippedCloak != null && Cloak.HasProcSpell(equippedCloak))
                        Cloak.TryProcSpell(target, ProjectileSource, equippedCloak, percent);

                    target.EmoteManager.OnDamage(sourcePlayer);

                    if (critical)
                        target.EmoteManager.OnReceiveCritical(sourcePlayer);
                }
            }
            else
            {
                var lastDamager = ProjectileSource != null ? new DamageHistoryInfo(ProjectileSource) : null;
                target.OnDeath(lastDamager, Spell.DamageType, critical);
                target.Die();
            }
        }

        /// <summary>
        /// Sets the physics state for a launched projectile
        /// </summary>
        public void SetProjectilePhysicsState(WorldObject target, bool useGravity)
        {
            if (useGravity)
                GravityStatus = true;

            CurrentMotionState = null;
            Placement = null;

            // TODO: Physics description timestamps (sequence numbers) don't seem to be getting updated

            //Console.WriteLine("SpellProjectile PhysicsState: " + PhysicsObj.State);

            var pos = Location.Pos;
            var rotation = Location.Rotation;
            PhysicsObj.Position.Frame.Origin = pos;
            PhysicsObj.Position.Frame.Orientation = rotation;

            var velocity = Velocity;
            //velocity = Vector3.Transform(velocity, Matrix4x4.Transpose(Matrix4x4.CreateFromQuaternion(rotation)));
            PhysicsObj.Velocity = velocity;

            if (target != null)
                PhysicsObj.ProjectileTarget = target.PhysicsObj;

            PhysicsObj.set_active(true);
        }

        public static void ShowInfo(Creature observed, Spell spell, CreatureSkill skill, float criticalChance, bool criticalHit, bool critDefended, bool overpower, float weaponCritDamageMod,
            float magicSkillBonus, int baseDamage, float critDamageBonus, float elementalDamageMod, float slayerMod,
            float weaponResistanceMod, float resistanceMod, float absorbMod,
            float lifeProjectileDamage, float lifeMagicDamage, float finalDamage)
        {
            var observer = PlayerManager.GetOnlinePlayer(observed.DebugDamageTarget);
            if (observer == null)
            {
                observed.DebugDamage = Creature.DebugDamageType.None;
                return;
            }

            var info = $"Skill: {skill.Skill.ToSentence()}\n";
            info += $"CriticalChance: {criticalChance}\n";
            info += $"CriticalHit: {criticalHit}\n";

            if (critDefended)
                info += $"CriticalDefended: {critDefended}\n";

            info += $"Overpower: {overpower}\n";
        
            if (spell.MetaSpellType == ACE.Entity.Enum.SpellType.LifeProjectile)
            {
                // life magic projectile
                info += $"LifeProjectileDamage: {lifeProjectileDamage}\n";
                info += $"DamageRatio: {spell.DamageRatio}\n";
                info += $"LifeMagicDamage: {lifeMagicDamage}\n";
            }
            else
            {
                // war/void projectile
                var difficulty = Math.Min(spell.Power, 350);
                info += $"Difficulty: {difficulty}\n";

                if (magicSkillBonus != 0.0f)
                    info += $"SkillBonus: {magicSkillBonus}\n";

                info += $"BaseDamageRange: {spell.MinDamage} - {spell.MaxDamage}\n";
                info += $"BaseDamage: {baseDamage}\n";
                info += $"DamageType: {spell.DamageType}\n";
            }

            if (weaponCritDamageMod != 1.0f)
                info += $"WeaponCritDamageMod: {weaponCritDamageMod}\n";

            if (critDamageBonus != 0)
                info += $"CritDamageBonus: {critDamageBonus}\n";

            if (elementalDamageMod != 1.0f)
                info += $"ElementalDamageMod: {elementalDamageMod}\n";

            if (slayerMod != 1.0f)
                info += $"SlayerMod: {slayerMod}\n";

            if (weaponResistanceMod != 1.0f)
                info += $"WeaponResistanceMod: {weaponResistanceMod}\n";

            if (resistanceMod != 1.0f)
                info += $"ResistanceMod: {resistanceMod}\n";

            if (absorbMod != 1.0f)
                info += $"AbsorbMod: {absorbMod}\n";

            //observer.Session.Network.EnqueueSend(new GameMessageSystemChat(info, ChatMessageType.Broadcast));
            observer.DebugDamageBuffer += info;
        }

        public static void ShowInfo(Creature observed, float heritageMod, float sneakAttackMod, float damageRatingMod, float damageResistRatingMod,
            float critDamageRatingMod, float critDamageResistRatingMod, float pkDamageRatingMod, float pkDamageResistRatingMod, float damage)
        {
            var observer = PlayerManager.GetOnlinePlayer(observed.DebugDamageTarget);
            if (observer == null)
            {
                observed.DebugDamage = Creature.DebugDamageType.None;
                return;
            }
            var info = "";

            if (heritageMod != 1.0f)
                info += $"HeritageMod: {heritageMod}\n";

            if (sneakAttackMod != 1.0f)
                info += $"SneakAttackMod: {sneakAttackMod}\n";

            if (critDamageRatingMod != 1.0f)
                info += $"CritDamageRatingMod: {critDamageRatingMod}\n";

            if (pkDamageRatingMod != 1.0f)
                info += $"PkDamageRatingMod: {pkDamageRatingMod}\n";

            if (damageRatingMod != 1.0f)
                info += $"DamageRatingMod: {damageRatingMod}\n";

            if (critDamageResistRatingMod != 1.0f)
                 info += $"CritDamageResistRatingMod: {critDamageResistRatingMod}\n";

            if (pkDamageResistRatingMod != 1.0f)
                info += $"PkDamageResistRatingMod: {pkDamageResistRatingMod}\n";

            if (damageResistRatingMod != 1.0f)
                info += $"DamageResistRatingMod: {damageResistRatingMod}\n";

            info += $"Final damage: {damage}";

            observer.Session.Network.EnqueueSend(new GameMessageSystemChat(observer.DebugDamageBuffer + info, ChatMessageType.Broadcast));

            observer.DebugDamageBuffer = null;
        }
    }
}
