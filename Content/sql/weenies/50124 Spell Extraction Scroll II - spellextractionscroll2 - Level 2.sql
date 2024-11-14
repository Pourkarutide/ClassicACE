DELETE FROM `weenie` WHERE `class_Id` = 50124;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (50124, 'spellextractionscroll2', 74, '2022-12-08 08:07:09') /* SpellTransferScroll */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (50124,   1,        128) /* ItemType - Misc */
     , (50124,   5,          1) /* EncumbranceVal */
     , (50124,   8,          1) /* Mass */
     , (50124,   9,          0) /* ValidLocations - None */
     , (50124,  11,         50) /* MaxStackSize */
     , (50124,  12,          1) /* StackSize */
     , (50124,  13,          1) /* StackUnitEncumbrance */
     , (50124,  14,          1) /* StackUnitMass */
     , (50124,  15,       2000) /* StackUnitValue */
     , (50124,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (50124,  19,       2000) /* Value */
     , (50124,  25,          2) /* Level */
     , (50124,  33,          0) /* Bonded - Normal */
     , (50124,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (50124,  94,      35215) /* TargetType - Jewelry, Misc, Gem, RedirectableItemEnchantmentTarget */
     , (50124, 114,          0) /* Attuned - Normal */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (50124,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (50124,  39,       1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (50124,   1, 'Spell Extraction Scroll II') /* Name */
     , (50124,  14, 'Use this item to extract a level II spell from a treasure-generated item.') /* Use */
     , (50124,  16, 'Once extracted, the spell can be transferred to another treasure-generated item.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (50124,   1, 0x0200018A) /* Setup */
     , (50124,   8, 0x06006920) /* Icon */
     , (50124,  22, 0x3400002B) /* PhysicsEffectTable */
     , (50124,  27, 0x4000001C) /* UseUserAnimation - Reading */
     , (50124,  52, 0x060013F5) /* IconUnderlay */;
