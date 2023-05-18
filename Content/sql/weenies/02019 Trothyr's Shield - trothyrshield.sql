DELETE FROM `weenie` WHERE `class_Id` = 2019;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (2019, 'trothyrshield', 1, '2019-05-03 00:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (2019,   1,          2) /* ItemType - Armor */
     , (2019,   3,         14) /* PaletteTemplate - Red */
     , (2019,   5,        800) /* EncumbranceVal */
     , (2019,   8,        400) /* Mass */
     , (2019,   9,    2097152) /* ValidLocations - Shield */
     , (2019,  16,          1) /* ItemUseable - No */
     , (2019,  18,          1) /* UiEffects - Magical */
     , (2019,  19,       1000) /* Value */
     , (2019,  27,          2) /* ArmorType - Leather */
     , (2019,  28,         60) /* ArmorLevel */
     , (2019,  51,          4) /* CombatUse - Shield */
     , (2019,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (2019, 106,         60) /* ItemSpellcraft */
     , (2019, 107,        400) /* ItemCurMana */
     , (2019, 108,        400) /* ItemMaxMana */
     , (2019, 109,         45) /* ItemDifficulty */
     , (2019, 150,        103) /* HookPlacement - Hook */
     , (2019, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (2019,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (2019,  13,       1) /* ArmorModVsSlash */
     , (2019,  14,     0.8) /* ArmorModVsPierce */
     , (2019,  15,       1) /* ArmorModVsBludgeon */
     , (2019,  16,     0.5) /* ArmorModVsCold */
     , (2019,  17,     0.5) /* ArmorModVsFire */
     , (2019,  18,     0.5) /* ArmorModVsAcid */
     , (2019,  19,     0.6) /* ArmorModVsElectric */
     , (2019,  39,    1.25) /* DefaultScale */
     , (2019, 110,       1) /* BulkMod */
     , (2019, 111,       1) /* SizeMod */
     , (2019, 10003,      -5) /* MeleeDefenseCap */
     , (2019, 10004,      -5) /* MissileDefenseCap */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (2019,   1, 'Trothyr''s Shield') /* Name */
     , (2019,  33, 'pickup_mutated_trothyrshield') /* Quest string */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (2019,   1, 0x02000162) /* Setup */
     , (2019,   3, 0x20000014) /* SoundTable */
     , (2019,   6, 0x04000BEF) /* PaletteBase */
     , (2019,   7, 0x10000092) /* ClothingBase */
     , (2019,   8, 0x06000FE1) /* Icon */
     , (2019,  22, 0x3400002B) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (2019,  5844,      2)  /* Shield Mastery Other II */
     , (2019,  1357,      2)  /* Endurance Other III */;
