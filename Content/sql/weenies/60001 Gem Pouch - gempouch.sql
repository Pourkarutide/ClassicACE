DELETE FROM `weenie` WHERE `class_Id` = 60001;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (60001, 'gempouch', 21, '2021-01-27 16:00:00') /* Container */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (60001,   1,        512) /* ItemType - Container */
     , (60001,   3,         84) /* PaletteTemplate - DyeDarkGreen */
     , (60001,   5,         15) /* EncumbranceVal */
     , (60001,   6,         24) /* ItemsCapacity */
     , (60001,   8,        200) /* Mass */
     , (60001,   9,          0) /* ValidLocations - None */
     , (60001,  16,         56) /* ItemUseable - ContainedViewedRemote */
     , (60001,  19,         65) /* Value */
     , (60001,  33,          1) /* Bonded - Bonded */
     , (60001,  74,       2048) /* MerchandiseItemTypes - Gem */
     , (60001,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (60001,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (60001,  39,    1.75) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (60001,   1, 'Gem Pouch') /* Name */
     , (60001,  15, 'Can only hold gems.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (60001,   1, 0x02000152) /* Setup */
     , (60001,   3, 0x20000014) /* SoundTable */
     , (60001,   6, 0x04000BEF) /* PaletteBase */
     , (60001,   7, 0x100004F5) /* ClothingBase */
     , (60001,   8, 0x06001011) /* Icon */
     , (60001,  22, 0x3400002B) /* PhysicsEffectTable */;
