DELETE FROM `weenie` WHERE `class_Id` = 50123;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (50123, 'spellextractionscroll1', 74, '2022-12-08 08:07:09') /* SpellTransferScroll */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (50123,   1,        128) /* ItemType - Misc */
     , (50123,   5,          1) /* EncumbranceVal */
     , (50123,   8,          1) /* Mass */
     , (50123,   9,          0) /* ValidLocations - None */
     , (50123,  11,         50) /* MaxStackSize */
     , (50123,  12,          1) /* StackSize */
     , (50123,  13,          1) /* StackUnitEncumbrance */
     , (50123,  14,          1) /* StackUnitMass */
     , (50123,  15,        500) /* StackUnitValue */
     , (50123,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (50123,  19,        500) /* Value */
     , (50123,  25,          1) /* Level */
     , (50123,  33,          0) /* Bonded - Normal */
     , (50123,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (50123,  94,      35215) /* TargetType - Jewelry, Misc, Gem, RedirectableItemEnchantmentTarget */
     , (50123, 114,          0) /* Attuned - Normal */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (50123,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (50123,  39,       1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (50123,   1, 'Spell Extraction Scroll I') /* Name */
     , (50123,  14, 'Use this item to extract a level I spell from a treasure-generated item.') /* Use */
     , (50123,  16, 'Once extracted, the spell can be transferred to another treasure-generated item.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (50123,   1, 0x0200018A) /* Setup */
     , (50123,   8, 0x06006920) /* Icon */
     , (50123,  22, 0x3400002B) /* PhysicsEffectTable */
     , (50123,  27, 0x4000001C) /* UseUserAnimation - Reading */
     , (50123,  52, 0x060013F4) /* IconUnderlay */;
