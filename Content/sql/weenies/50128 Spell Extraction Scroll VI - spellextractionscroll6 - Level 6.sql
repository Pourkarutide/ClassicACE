DELETE FROM `weenie` WHERE `class_Id` = 50128;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (50128, 'spellextractionscroll6', 74, '2022-12-08 08:07:09') /* SpellTransferScroll */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (50128,   1,        128) /* ItemType - Misc */
     , (50128,   5,          1) /* EncumbranceVal */
     , (50128,   8,          1) /* Mass */
     , (50128,   9,          0) /* ValidLocations - None */
     , (50128,  11,         50) /* MaxStackSize */
     , (50128,  12,          1) /* StackSize */
     , (50128,  13,          1) /* StackUnitEncumbrance */
     , (50128,  14,          1) /* StackUnitMass */
     , (50128,  15,      32000) /* StackUnitValue */
     , (50128,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (50128,  19,      32000) /* Value */
     , (50128,  25,          6) /* Level */
     , (50128,  33,          0) /* Bonded - Normal */
     , (50128,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (50128,  94,      35215) /* TargetType - Jewelry, Misc, Gem, RedirectableItemEnchantmentTarget */
     , (50128, 114,          0) /* Attuned - Normal */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (50128,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (50128,  39,       1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (50128,   1, 'Spell Extraction Scroll VI') /* Name */
     , (50128,  14, 'Use this item to extract a level VI spell or major cantrip from a treasure-generated item.') /* Use */
     , (50128,  16, 'Once extracted, the spell can be transferred to another treasure-generated item.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (50128,   1, 0x0200018A) /* Setup */
     , (50128,   8, 0x06006920) /* Icon */
     , (50128,  22, 0x3400002B) /* PhysicsEffectTable */
     , (50128,  27, 0x4000001C) /* UseUserAnimation - Reading */
     , (50128,  52, 0x060013F9) /* IconUnderlay */;
