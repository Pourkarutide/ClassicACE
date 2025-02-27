DELETE FROM `weenie` WHERE `class_Id` = 27553;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (27553, 'boltdeadlyarmorpiercingtest3', 5, '2019-12-25 00:00:00') /* Ammunition */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (27553,   1,        256) /* ItemType - MissileWeapon */
     , (27553,   3,         61) /* PaletteTemplate - White */
     , (27553,   5,         10) /* EncumbranceVal */
     , (27553,   8,          2) /* Mass */
     , (27553,   9,    8388608) /* ValidLocations - MissileAmmo */
     , (27553,  11,        250) /* MaxStackSize */
     , (27553,  12,          1) /* StackSize */
     , (27553,  13,         10) /* StackUnitEncumbrance */
     , (27553,  14,          2) /* StackUnitMass */
     , (27553,  15,          9) /* StackUnitValue */
     , (27553,  16,          1) /* ItemUseable - No */
     , (27553,  19,          9) /* Value */
     , (27553,  44,         32) /* Damage */
     , (27553,  45,          2) /* DamageType - Pierce */
     , (27553,  50,          2) /* AmmoType - Bolt */
     , (27553,  51,          3) /* CombatUse - Ammo */
     , (27553,  93,     132116) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, Inelastic */
     , (27553, 150,        103) /* HookPlacement - Hook */
     , (27553, 151,          2) /* HookType - Wall */
     , (27553, 158,          2) /* WieldRequirements - RawSkill */
     , (27553, 159,          3) /* WieldSkillType - Crossbow */
     , (27553, 160,        235) /* WieldDifficulty */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (27553,  17, True ) /* Inelastic */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (27553,  22,    0.18) /* DamageVariance */
     , (27553,  29,       1) /* WeaponDefense */
     , (27553,  39,     1.1) /* DefaultScale */
     , (27553,  62,       1) /* WeaponOffense */
     , (27553,  78,       1) /* Friction */
     , (27553,  79,       0) /* Elasticity */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (27553,   1, 'Deadly Armor Piercing Quarrel') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (27553,   1, 0x0200012A) /* Setup */
     , (27553,   3, 0x20000014) /* SoundTable */
     , (27553,   6, 0x04000BEF) /* PaletteBase */
     , (27553,   7, 0x10000352) /* ClothingBase */
     , (27553,   8, 0x06002489) /* Icon */
     , (27553,  22, 0x3400002B) /* PhysicsEffectTable */;
