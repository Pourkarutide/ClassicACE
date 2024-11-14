DELETE FROM `weenie` WHERE `class_Id` = 50129;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (50129, 'spellextractionscroll7', 74, '2022-12-08 08:07:09') /* SpellTransferScroll */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (50129,   1,        128) /* ItemType - Misc */
     , (50129,   5,          1) /* EncumbranceVal */
     , (50129,   8,          1) /* Mass */
     , (50129,   9,          0) /* ValidLocations - None */
     , (50129,  11,         50) /* MaxStackSize */
     , (50129,  12,          1) /* StackSize */
     , (50129,  13,          1) /* StackUnitEncumbrance */
     , (50129,  14,          1) /* StackUnitMass */
     , (50129,  15,      64000) /* StackUnitValue */
     , (50129,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (50129,  19,      64000) /* Value */
     , (50129,  25,          7) /* Level */
     , (50129,  33,          0) /* Bonded - Normal */
     , (50129,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (50129,  94,      35215) /* TargetType - Jewelry, Misc, Gem, RedirectableItemEnchantmentTarget */
     , (50129, 114,          0) /* Attuned - Normal */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (50129,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (50129,  39,       1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (50129,   1, 'Spell Extraction Scroll VII') /* Name */
     , (50129,  14, 'Use this item to extract a level VII spell from a treasure-generated item.') /* Use */
     , (50129,  16, 'Once extracted, the spell can be transferred to another treasure-generated item.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (50129,   1, 0x0200018A) /* Setup */
     , (50129,   8, 0x06006920) /* Icon */
     , (50129,  22, 0x3400002B) /* PhysicsEffectTable */
     , (50129,  27, 0x4000001C) /* UseUserAnimation - Reading */
     , (50129,  52, 0x06001F63) /* IconUnderlay */;
