DELETE FROM `weenie` WHERE `class_Id` = 3801;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (3801, 'javelinfrost', 4, '2005-02-09 10:00:00') /* Missile */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (3801,   1,        256) /* ItemType - MissileWeapon */
     , (3801,   5,         10) /* EncumbranceVal */
     , (3801,   8,         10) /* Mass */
     , (3801,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (3801,  11,        250) /* MaxStackSize */
     , (3801,  12,          1) /* StackSize */
     , (3801,  13,         10) /* StackUnitEncumbrance */
     , (3801,  14,         10) /* StackUnitMass */
     , (3801,  15,         20) /* StackUnitValue */
     , (3801,  16,          1) /* ItemUseable - No */
     , (3801,  18,        128) /* UiEffects - Frost */
     , (3801,  19,         20) /* Value */
     , (3801,  44,          9) /* Damage */
     , (3801,  45,          8) /* DamageType - Cold */
     , (3801,  46,        128) /* DefaultCombatStyle - ThrownWeapon */
     , (3801,  48,         12) /* WeaponSkill - ThrownWeapon */
     , (3801,  49,         20) /* WeaponTime */
     , (3801,  51,          2) /* CombatUse - Missile */
     , (3801,  93,     132116) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, Inelastic */
     , (3801, 150,        103) /* HookPlacement - Hook */
     , (3801, 151,          2) /* HookType - Wall */
     , (3801, 169,  101188618) /* TsysMutationData */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (3801,  17, True ) /* Inelastic */
     , (3801,  69, False ) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (3801,  22,    0.25) /* DamageVariance */
     , (3801,  27,       0) /* RotationSpeed */
     , (3801,  29,       1) /* WeaponDefense */
     , (3801,  62,       1) /* WeaponOffense */
     , (3801,  78,       1) /* Friction */
     , (3801,  79,       0) /* Elasticity */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (3801,   1, 'Frost Javelin') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (3801,   1, 0x02000519) /* Setup */
     , (3801,   3, 0x20000014) /* SoundTable */
     , (3801,   8, 0x060010C9) /* Icon */
     , (3801,  22, 0x3400002B) /* PhysicsEffectTable */;
