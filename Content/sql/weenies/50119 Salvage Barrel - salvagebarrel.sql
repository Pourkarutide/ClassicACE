DELETE FROM `weenie` WHERE `class_Id` = 50119;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (50119, 'salvagebarrel', 21, '2022-11-28 10:00:00') /* Container */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (50119,   1,        512) /* ItemType - Container */
     , (50119,   5,         50) /* EncumbranceVal */
     , (50119,   6,        102) /* ItemsCapacity */
     , (50119,   8,         50) /* Mass */
     , (50119,   9,          0) /* ValidLocations - None */
     , (50119,  16,         56) /* ItemUseable - ContainedViewedRemote */
     , (50119,  19,        100) /* Value */
     , (50119,  33,          1) /* Bonded - Bonded */
     , (50119,  74, 1073741824) /* MerchandiseItemTypes - TinkeringMaterial */
     , (50119,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (50119,  13, True ) /* Ethereal */
     , (50119,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (50119,   1, 'Salvage Barrel') /* Name */
     , (50119,  15, 'Can only hold salvage.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (50119,   1, 0x020000A5) /* Setup */
     , (50119,   3, 0x20000014) /* SoundTable */
     , (50119,   8, 0x06002FE0) /* Icon */
     , (50119,  22, 0x3400002B) /* PhysicsEffectTable */;
