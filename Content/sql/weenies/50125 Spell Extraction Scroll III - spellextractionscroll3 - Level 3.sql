DELETE FROM `weenie` WHERE `class_Id` = 50125;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (50125, 'spellextractionscroll3', 74, '2022-12-08 08:07:09') /* SpellTransferScroll */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (50125,   1,        128) /* ItemType - Misc */
     , (50125,   5,          1) /* EncumbranceVal */
     , (50125,   8,          1) /* Mass */
     , (50125,   9,          0) /* ValidLocations - None */
     , (50125,  11,         50) /* MaxStackSize */
     , (50125,  12,          1) /* StackSize */
     , (50125,  13,          1) /* StackUnitEncumbrance */
     , (50125,  14,          1) /* StackUnitMass */
     , (50125,  15,       4000) /* StackUnitValue */
     , (50125,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (50125,  19,       4000) /* Value */
     , (50125,  25,          3) /* Level */
     , (50125,  33,          0) /* Bonded - Normal */
     , (50125,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (50125,  94,      35215) /* TargetType - Jewelry, Misc, Gem, RedirectableItemEnchantmentTarget */
     , (50125, 114,          0) /* Attuned - Normal */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (50125,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (50125,  39,       1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (50125,   1, 'Spell Extraction Scroll III') /* Name */
     , (50125,  14, 'Use this item to extract a level III spell or minor cantrip from a treasure-generated item.') /* Use */
     , (50125,  16, 'Once extracted, the spell can be transferred to another treasure-generated item.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (50125,   1, 0x0200018A) /* Setup */
     , (50125,   8, 0x06006920) /* Icon */
     , (50125,  22, 0x3400002B) /* PhysicsEffectTable */
     , (50125,  27, 0x4000001C) /* UseUserAnimation - Reading */
     , (50125,  52, 0x060013F6) /* IconUnderlay */;
