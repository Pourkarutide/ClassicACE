DELETE FROM `weenie` WHERE `class_Id` = 15443;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (15443, 'boltdeadlyelectric', 5, '2019-12-25 00:00:00') /* Ammunition */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (15443,   1,        256) /* ItemType - MissileWeapon */
     , (15443,   3,         82) /* PaletteTemplate - PinkPurple */
     , (15443,   5,         10) /* EncumbranceVal */
     , (15443,   8,          2) /* Mass */
     , (15443,   9,    8388608) /* ValidLocations - MissileAmmo */
     , (15443,  11,        250) /* MaxStackSize */
     , (15443,  12,          1) /* StackSize */
     , (15443,  13,         10) /* StackUnitEncumbrance */
     , (15443,  14,          2) /* StackUnitMass */
     , (15443,  15,         11) /* StackUnitValue */
     , (15443,  16,          1) /* ItemUseable - No */
     , (15443,  18,         64) /* UiEffects - Lightning */
     , (15443,  19,         11) /* Value */
     , (15443,  44,         28) /* Damage */
     , (15443,  45,         64) /* DamageType - Electric */
     , (15443,  50,          2) /* AmmoType - Bolt */
     , (15443,  51,          3) /* CombatUse - Ammo */
     , (15443,  93,     132116) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, Inelastic */
     , (15443, 150,        103) /* HookPlacement - Hook */
     , (15443, 151,          2) /* HookType - Wall */
     , (15443, 158,          2) /* WieldRequirements - RawSkill */
     , (15443, 159,          3) /* WieldSkillType - Crossbow */
     , (15443, 160,        235) /* WieldDifficulty */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (15443,  17, True ) /* Inelastic */
     , (15443,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (15443,  22,    0.28) /* DamageVariance */
     , (15443,  29,       1) /* WeaponDefense */
     , (15443,  39,     1.1) /* DefaultScale */
     , (15443,  62,       1) /* WeaponOffense */
     , (15443,  78,       1) /* Friction */
     , (15443,  79,       0) /* Elasticity */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (15443,   1, 'Deadly Lightning Quarrel') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (15443,   1, 0x020004EF) /* Setup */
     , (15443,   3, 0x20000014) /* SoundTable */
     , (15443,   6, 0x04000BEF) /* PaletteBase */
     , (15443,   7, 0x10000352) /* ClothingBase */
     , (15443,   8, 0x06002490) /* Icon */
     , (15443,  22, 0x3400002B) /* PhysicsEffectTable */;
