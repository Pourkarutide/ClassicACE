DELETE FROM `weenie` WHERE `class_Id` = 25906;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (25906, 'macemite', 6, '2020-01-21 05:10:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (25906,   1,          1) /* ItemType - MeleeWeapon */
     , (25906,   5,        850) /* EncumbranceVal */
     , (25906,   8,        360) /* Mass */
     , (25906,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (25906,  16,          1) /* ItemUseable - No */
     , (25906,  18,          1) /* UiEffects - Magical */
     , (25906,  19,       4500) /* Value */
     , (25906,  33,          1) /* Bonded - Bonded */
     , (25906,  44,         46) /* Damage */
     , (25906,  45,          4) /* DamageType - Bludgeon */
     , (25906,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (25906,  47,          4) /* AttackType - Slash */
     , (25906,  48,          5) /* WeaponSkill - Mace */
     , (25906,  49,         40) /* WeaponTime */
     , (25906,  51,          1) /* CombatUse - Melee */
     , (25906,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (25906, 106,        400) /* ItemSpellcraft */
     , (25906, 107,        800) /* ItemCurMana */
     , (25906, 108,        800) /* ItemMaxMana */
     , (25906, 109,        125) /* ItemDifficulty */
     , (25906, 115,        425) /* ItemSkillLevelLimit */
     , (25906, 150,        103) /* HookPlacement - Hook */
     , (25906, 151,          2) /* HookType - Wall */
     , (25906, 158,          7) /* WieldRequirements - Level */
     , (25906, 159,          1) /* WieldSkillType - Axe */
     , (25906, 160,        100) /* WieldDifficulty */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (25906,  22, True ) /* Inscribable */
     , (25906,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (25906,   5,   -0.33) /* ManaRate */
     , (25906,  21,    0.62) /* WeaponLength */
     , (25906,  22,     0.5) /* DamageVariance */
     , (25906,  29,    1.06) /* WeaponDefense */
     , (25906,  62,     1.1) /* WeaponOffense */
     , (25906, 136,       2) /* CriticalMultiplier */
     , (25906, 147,    0.15) /* CriticalFrequency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (25906,   1, 'Mace of Dissonance') /* Name */
     , (25906,  16, 'A mace carved and decorated from the leg bone of a fearsome Colossal Mite.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (25906,   1, 0x0200101F) /* Setup */
     , (25906,   3, 0x20000014) /* SoundTable */
     , (25906,   8, 0x06003034) /* Icon */
     , (25906,  22, 0x3400002B) /* PhysicsEffectTable */
     , (25906,  36, 0x0E000014) /* MutateFilter */
     , (25906,  37,          1) /* ItemSkillLimit - Axe */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (25906,  1592,      2)  /* Heart Seeker VI */
     , (25906,  1605,      2)  /* Defender VI */
     , (25906,  1616,      2)  /* Blood Drinker VI */
     , (25906,  1626,      2)  /* Swift Killer V */
     , (25906,  2598,      2)  /* Minor Blood Thirst */;
