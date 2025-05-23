DELETE FROM `weenie` WHERE `class_Id` = 1951;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1951, 'daggerassassin', 6, '2005-02-09 10:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1951,   1,          1) /* ItemType - MeleeWeapon */
     , (1951,   3,         39) /* PaletteTemplate - Black */
     , (1951,   5,        135) /* EncumbranceVal */
     , (1951,   8,         90) /* Mass */
     , (1951,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1951,  16,          1) /* ItemUseable - No */
     , (1951,  19,        300) /* Value */
     , (1951,  44,          7) /* Damage */
     , (1951,  45,          3) /* DamageType - Slash, Pierce */
     , (1951,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1951,  47,        486) /* AttackType - Thrust, Slash, DoubleSlash, TripleSlash, DoubleThrust, TripleThrust */
     , (1951,  48,          4) /* WeaponSkill - Dagger */
     , (1951,  49,         20) /* WeaponTime */
     , (1951,  51,          1) /* CombatUse - Melee */
     , (1951,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1951, 150,        103) /* HookPlacement - Hook */
     , (1951, 151,          2) /* HookType - Wall */
     , (1951, 158,          2) /* WieldRequirements - RawSkill */
     , (1951, 159,          4) /* WieldSkillType - Dagger */
     , (1951, 160,        250) /* WieldDifficulty */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1951,  22, True ) /* Inscribable */
     , (1951,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1951,  21,     0.5) /* WeaponLength */
     , (1951,  22,    0.15) /* DamageVariance */
     , (1951,  29,       1) /* WeaponDefense */
     , (1951,  62,       1) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1951,   1, 'Gertarh''s Dagger') /* Name */
     , (1951,   7, 'Sometimes, a good dagger is your only friend.') /* Inscription */
     , (1951,   8, 'Gertarh') /* ScribeName */
     , (1951,  33, 'pickup_mutated_gertarhdagger') /* Quest string */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1951,   1, 0x0200012F) /* Setup */
     , (1951,   3, 0x20000014) /* SoundTable */
     , (1951,   6, 0x04000BEF) /* PaletteBase */
     , (1951,   7, 0x10000147) /* ClothingBase */
     , (1951,   8, 0x060010C5) /* Icon */
     , (1951,  22, 0x3400002B) /* PhysicsEffectTable */;
