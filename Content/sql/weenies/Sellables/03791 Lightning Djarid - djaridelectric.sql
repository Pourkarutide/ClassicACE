DELETE FROM `weenie` WHERE `class_Id` = 3791;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (3791, 'djaridelectric', 4, '2005-02-09 10:00:00') /* Missile */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (3791,   1,        256) /* ItemType - MissileWeapon */
     , (3791,   5,         15) /* EncumbranceVal */
     , (3791,   8,         20) /* Mass */
     , (3791,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (3791,  11,        250) /* MaxStackSize */
     , (3791,  12,          1) /* StackSize */
     , (3791,  13,         15) /* StackUnitEncumbrance */
     , (3791,  14,         20) /* StackUnitMass */
     , (3791,  15,         20) /* StackUnitValue */
     , (3791,  16,          1) /* ItemUseable - No */
     , (3791,  18,         64) /* UiEffects - Lightning */
     , (3791,  19,         20) /* Value */
     , (3791,  44,         10) /* Damage */
     , (3791,  45,         64) /* DamageType - Electric */
     , (3791,  46,        128) /* DefaultCombatStyle - ThrownWeapon */
     , (3791,  48,         12) /* WeaponSkill - ThrownWeapon */
     , (3791,  49,         20) /* WeaponTime */
     , (3791,  51,          2) /* CombatUse - Missile */
     , (3791,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (3791, 150,        103) /* HookPlacement - Hook */
     , (3791, 151,          2) /* HookType - Wall */
     , (3791, 169,  101188618) /* TsysMutationData */;

     INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (3791,  69, False ) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (3791,  22,    0.25) /* DamageVariance */
     , (3791,  27,       0) /* RotationSpeed */
     , (3791,  29,       1) /* WeaponDefense */
     , (3791,  39,     1.1) /* DefaultScale */
     , (3791,  62,       1) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (3791,   1, 'Lightning Djarid') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (3791,   1, 0x0200050B) /* Setup */
     , (3791,   3, 0x20000014) /* SoundTable */
     , (3791,   8, 0x060010C9) /* Icon */
     , (3791,  22, 0x3400002B) /* PhysicsEffectTable */;
