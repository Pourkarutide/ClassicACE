DELETE FROM `weenie` WHERE `class_Id` = 50127;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (50127, 'spellextractionscroll5', 74, '2022-12-08 08:07:09') /* SpellTransferScroll */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (50127,   1,        128) /* ItemType - Misc */
     , (50127,   5,          1) /* EncumbranceVal */
     , (50127,   8,          1) /* Mass */
     , (50127,   9,          0) /* ValidLocations - None */
     , (50127,  11,         50) /* MaxStackSize */
     , (50127,  12,          1) /* StackSize */
     , (50127,  13,          1) /* StackUnitEncumbrance */
     , (50127,  14,          1) /* StackUnitMass */
     , (50127,  15,      16000) /* StackUnitValue */
     , (50127,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (50127,  19,      16000) /* Value */
     , (50127,  25,          5) /* Level */
     , (50127,  33,          0) /* Bonded - Normal */
     , (50127,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (50127,  94,      35215) /* TargetType - Jewelry, Misc, Gem, RedirectableItemEnchantmentTarget */
     , (50127, 114,          0) /* Attuned - Normal */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (50127,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (50127,  39,       1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (50127,   1, 'Spell Extraction Scroll V') /* Name */
     , (50127,  14, 'Use this item to extract a level V spell from a treasure-generated item.') /* Use */
     , (50127,  16, 'Once extracted, the spell can be transferred to another treasure-generated item.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (50127,   1, 0x0200018A) /* Setup */
     , (50127,   8, 0x06006920) /* Icon */
     , (50127,  22, 0x3400002B) /* PhysicsEffectTable */
     , (50127,  27, 0x4000001C) /* UseUserAnimation - Reading */
     , (50127,  52, 0x060013F8) /* IconUnderlay */;
