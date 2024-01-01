DELETE FROM `weenie` WHERE `class_Id` = 690000;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (690000, 'gauntletsleathernewbiequest', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (690000,   1,          2) /* ItemType - Armor */
     , (690000,   3,          1) /* PaletteTemplate - AquaBlue */
     , (690000,   4,      32768)
     , (690000,   5,         50) /* EncumbranceVal */
     , (690000,   8,         50) /* Mass */
     , (690000,   9,         32) 
     , (690000,  16,          1) /* ItemUseable - No */
     , (690000,  18,          1) /* UiEffects - Magical */
     , (690000,  19,          1) /* Value */
     , (690000,  27,          2) /* ArmorType - Leather */
     , (690000,  28,         80) /* ArmorLevel */
     , (690000,  33,          1) /* Bonded - Bonded */
     , (690000,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (690000, 106,        100) /* ItemSpellcraft */
     , (690000, 107,        400) /* ItemCurMana */
     , (690000, 108,        400) /* ItemMaxMana */
     , (690000, 109,         15) /* ItemDifficulty */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (690000,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (690000,   5,  -0.025) /* ManaRate */
     , (690000,  12,     0.3) /* Shade */
     , (690000,  13,       1) /* ArmorModVsSlash */
     , (690000,  14,       1) /* ArmorModVsPierce */
     , (690000,  15,       1) /* ArmorModVsBludgeon */
     , (690000,  16,     0.6) /* ArmorModVsCold */
     , (690000,  17,     0.6) /* ArmorModVsFire */
     , (690000,  18,     0.6) /* ArmorModVsAcid */
     , (690000,  19,     0.6) /* ArmorModVsElectric */
     , (690000, 110,       1) /* BulkMod */
     , (690000, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (690000,   1, 'A Pair Of Society Leather Gauntlets') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (690000,   1, 0x020000D8) /* Setup */
     , (690000,   3, 0x20000014) /* SoundTable */
     , (690000,   6, 0x0400007E) /* PaletteBase */
     , (690000,   7, 0x10000008) /* ClothingBase */
     , (690000,   8, 0x060013FC) /* Icon */
     , (690000,  22, 0x3400002B) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (690000,   880,      2)  
     , (690000,    51,      2)  /* Impenetrability I */;
