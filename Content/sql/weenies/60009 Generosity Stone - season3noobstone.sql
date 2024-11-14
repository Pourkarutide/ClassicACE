DELETE FROM `weenie` WHERE `class_Id` = 60009;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (60009, 'season3noobstone', 37, '2024-11-14 00:00:00') /* ManaStone */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (60009,   1,     524288) /* ItemType - ManaStone */
     , (60009,   5,         50) /* EncumbranceVal */
     , (60009,   8,         50) /* Mass */
     , (60009,  16,     655368) /* ItemUseable - SourceContainedTargetSelfOrContained */
     , (60009,  18,          1) /* UiEffects - Magical */
     , (60009,  19,          0) /* Value */
     , (60009,  33,          1) /* Bonded - Bonded */
     , (60009,  87,         20) /* MaxLevel */
     , (60009,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (60009,  94,      35103) /* TargetType - Jewelry, Creature, Gem, RedirectableItemEnchantmentTarget */
     , (60009, 107,      10000) /* ItemCurMana */
     , (60009, 108,      10000) /* ItemMaxMana */
     , (60009, 114,          1) /* Attuned - Attuned */
     , (60009, 150,        103) /* HookPlacement - Hook */
     , (60009, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (60009,  22, True ) /* Inscribable */
     , (60009,  63, True ) /* UnlimitedUse */
     , (60009,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (60009,  87,       1) /* ItemEfficiency */
     , (60009, 137,       0) /* ManaStoneDestroyChance */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (60009,   1, 'Generosity Stone') /* Name */
     , (60009,  16, 'This mana stone does not run out of charges. It will not be destroyed upon use.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (60009,   1, 0x020004B9) /* Setup */
     , (60009,   8, 0x06005B72) /* Icon */
     , (60009,  52, 0x06005B0C) /* IconUnderlay */;
