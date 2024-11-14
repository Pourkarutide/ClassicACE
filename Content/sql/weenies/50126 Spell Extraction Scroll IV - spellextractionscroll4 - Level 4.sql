DELETE FROM `weenie` WHERE `class_Id` = 50126;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (50126, 'spellextractionscroll4', 74, '2022-12-08 08:07:09') /* SpellTransferScroll */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (50126,   1,        128) /* ItemType - Misc */
     , (50126,   5,          1) /* EncumbranceVal */
     , (50126,   8,          1) /* Mass */
     , (50126,   9,          0) /* ValidLocations - None */
     , (50126,  11,         50) /* MaxStackSize */
     , (50126,  12,          1) /* StackSize */
     , (50126,  13,          1) /* StackUnitEncumbrance */
     , (50126,  14,          1) /* StackUnitMass */
     , (50126,  15,       4000) /* StackUnitValue */
     , (50126,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (50126,  19,       4000) /* Value */
     , (50126,  25,          4) /* Level */
     , (50126,  33,          0) /* Bonded - Normal */
     , (50126,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (50126,  94,      35215) /* TargetType - Jewelry, Misc, Gem, RedirectableItemEnchantmentTarget */
     , (50126, 114,          0) /* Attuned - Normal */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (50126,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (50126,  39,       1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (50126,   1, 'Spell Extraction Scroll IV') /* Name */
     , (50126,  14, 'Use this item to extract a level IV spell from a treasure-generated item.') /* Use */
     , (50126,  16, 'Once extracted, the spell can be transferred to another treasure-generated item.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (50126,   1, 0x0200018A) /* Setup */
     , (50126,   8, 0x06006920) /* Icon */
     , (50126,  22, 0x3400002B) /* PhysicsEffectTable */
     , (50126,  27, 0x4000001C) /* UseUserAnimation - Reading */
     , (50126,  52, 0x060013F7) /* IconUnderlay */;
