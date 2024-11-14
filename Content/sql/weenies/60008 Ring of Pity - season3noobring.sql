DELETE FROM `weenie` WHERE `class_Id` = 60008;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (60008, 'season3noobring', 1, '2024-11-14 00:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (60008,   1,          8) /* ItemType - Jewelry */
     , (60008,   3,          4) /* PaletteTemplate - Brown */
     , (60008,   5,         15) /* EncumbranceVal */
     , (60008,   8,         90) /* Mass */
     , (60008,   9,     786432) /* ValidLocations - FingerWear */
     , (60008,  16,          1) /* ItemUseable - No */
     , (60008,  18,         32) /* UiEffects - Fire */
     , (60008,  19,          0) /* Value */
     , (60008,  33,          1) /* Bonded - Bonded */
     , (60008,  87,         20) /* MaxLevel */
     , (60008,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (60008, 106,        350) /* ItemSpellcraft */
     , (60008, 107,         20) /* ItemCurMana */
     , (60008, 108,       3000) /* ItemMaxMana */
     , (60008, 109,          0) /* ItemDifficulty */
     , (60008, 114,          1) /* Attuned - Attuned */
     , (60008, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (60008,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (60008,   5,  -0.033) /* ManaRate */
     , (60008,  12,    0.66) /* Shade */
     , (60008,  39,     0.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (60008,   1, 'Ring of Pity') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (60008,   1, 0x02000103) /* Setup */
     , (60008,   3, 0x20000014) /* SoundTable */
     , (60008,   6, 0x04000BEF) /* PaletteBase */
     , (60008,   8, 0x06005BEA) /* Icon */
     , (60008,  22, 0x3400002B) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (60008,  1997,      2)  /* Life Giver */
     , (60008,   993,      2)  /* Sprint Other VI */
     , (60008,  1314,      2)  /* Armor Other III */
     , (60008,     1,      2)  /* Strength Other I */
     , (60008,  1403,      2)  /* Quickness Other I */
     , (60008,   684,      2)  /* Arcane Enlightenment Other I */
     , (60008,    17,      2)  /* Invulnerability Other I */
     , (60008,  6377,      2)  /* Awareness Mastery Other I */;
