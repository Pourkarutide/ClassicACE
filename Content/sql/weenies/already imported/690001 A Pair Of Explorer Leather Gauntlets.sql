DELETE FROM `weenie` WHERE `class_Id` = 690001;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (690001, 'gauntletsleatherrarenewbiequest', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (690001,   1,          2) /* ItemType - Armor */
     , (690001,   3,         27) /* PaletteTemplate - DarkGreenMetal */
     , (690001,   4,      32768) 
     , (690001,   5,         50)
     , (690001,   8,         50)
     , (690001,   9,         32)
     , (690001,  16,          1) /* ItemUseable - No */
     , (690001,  18,          1) /* UiEffects - Magical */
     , (690001,  19,          1) /* Value */
     , (690001,  27,          2) /* ArmorType - Leather */
     , (690001,  28,         80) /* ArmorLevel */
     , (690001,  33,          1) /* Bonded - Bonded */
     , (690001,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (690001, 106,        150) /* ItemSpellcraft */
     , (690001, 107,        400) /* ItemCurMana */
     , (690001, 108,        400) /* ItemMaxMana */
     , (690001, 109,         30) /* ItemDifficulty */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (690001,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (690001,   5,  -0.025) /* ManaRate */
     , (690001,  12,    0.66) /* Shade */
     , (690001,  13,       1) /* ArmorModVsSlash */
     , (690001,  14,       1) /* ArmorModVsPierce */
     , (690001,  15,       1) /* ArmorModVsBludgeon */
     , (690001,  16,     0.6) /* ArmorModVsCold */
     , (690001,  17,     0.6) /* ArmorModVsFire */
     , (690001,  18,     0.6) /* ArmorModVsAcid */
     , (690001,  19,     0.6) /* ArmorModVsElectric */
     , (690001, 110,       1) /* BulkMod */
     , (690001, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (690001,   1, 'A Pair Of Explorer Leather Gauntlets') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (690001,   1, 0x020000D8)  /* Setup */
     , (690001,   3, 0x20000014) /* SoundTable */
     , (690001,   6, 0x0400007E) /* PaletteBase */
     , (690001,   7, 0x100004E4) /* ClothingBase */
     , (690001,   8, 0x06002E8E) /* Icon */
     , (690001,  22, 0x3400002B) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (690001,   881,      2)  /* Healing Other II */
     , (690001,  1482,      2)  /* Impenetrability II */;
