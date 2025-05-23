DELETE FROM `weenie` WHERE `class_Id` = 1439;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1439, 'axeacid', 6, '2019-04-19 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1439,   1,          1) /* ItemType - MeleeWeapon */
     , (1439,   5,        800) /* EncumbranceVal */
     , (1439,   8,        320) /* Mass */
     , (1439,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1439,  16,          1) /* ItemUseable - No */
     , (1439,  18,        256) /* UiEffects - Acid */
     , (1439,  19,       2800) /* Value */
     , (1439,  33,          1) /* Bonded - Bonded */
     , (1439,  44,         16) /* Damage */
     , (1439,  45,         32) /* DamageType - Acid */
     , (1439,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1439,  47,          4) /* AttackType - Slash */
     , (1439,  48,          1) /* WeaponSkill - Axe */
     , (1439,  49,         60) /* WeaponTime */
     , (1439,  51,          1) /* CombatUse - Melee */
     , (1439,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1439, 106,         60) /* ItemSpellcraft */
     , (1439, 107,       1000) /* ItemCurMana */
     , (1439, 108,       1000) /* ItemMaxMana */
     , (1439, 109,         45) /* ItemDifficulty */
     , (1439, 115,        175) /* ItemSkillLevelLimit */
     , (1439, 150,        103) /* HookPlacement - Hook */
     , (1439, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1439,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1439,   5,  -0.033) /* ManaRate */
     , (1439,  21,    0.75) /* WeaponLength */
     , (1439,  22,     0.5) /* DamageVariance */
     , (1439,  29,    1.05) /* WeaponDefense */
     , (1439,  62,    1.05) /* WeaponOffense */
     , (1439, 136,       2) /* CriticalMultiplier */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1439,   1, 'Acid Axe') /* Name */
     , (1439,  16, 'A double-headed battle axe, dripping with vitriol. The handle appears to be made of ivory inlaid with tourmaline.') /* LongDesc */
     , (1439,  33, 'pickup_mutated_axeacid') /* Quest string */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1439,   1, 0x020003D6) /* Setup */
     , (1439,   8, 0x0600254C) /* Icon */
     , (1439,  22, 0x3400002B) /* PhysicsEffectTable */
     , (1439,  37,          1) /* ItemSkillLimit - Axe */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (1439,    49,      2)  /* Swift Killer I */
     , (1439,   517,      2)  /* Acid Protection Self III */
     , (1439,  1020,      2)  /* Bludgeoning Protection Self III */
     , (1439,  1589,      2)  /* Heart Seeker III */
     , (1439,  1601,      2)  /* Defender II */
     , (1439,  1613,      2)  /* Blood Drinker III */;
