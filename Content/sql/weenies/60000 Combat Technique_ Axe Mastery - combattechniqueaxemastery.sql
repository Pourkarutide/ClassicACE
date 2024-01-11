DELETE FROM `weenie` WHERE `class_Id` = 60000;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (60000, 'combattechniqueaxemastery', 1, '2023-11-11 02:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (60000,   1,        128) /* ItemType - Misc */
     , (60000,   5,         30) /* EncumbranceVal */
     , (60000,   8,         90) /* Mass */
     , (60000,   9,   67108864) /* ValidLocations - TrinketOne */
     , (60000,  16,          1) /* ItemUseable - No */
     , (60000,  18,         32) /* UiEffects - Fire */
     , (60000,  19,       2500) /* Value */
     , (60000,  33,          1) /* Bonded - Bonded */
     , (60000,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (60000, 10000,          9) /* TacticAndTechniqueId */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (60000,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (60000,  39,       1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (60000,   1, 'Combat Technique: Axe Mastery') /* Name */
     , (60000,  16, 'Mastery of the Axe

Adds an additional 150% multiplier for critical hits which strike any extremity (hands, feet, head).

This technique only works in melee combat with strikes using an axe.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (60000,   1, 0x02000155) /* Setup */
     , (60000,   8, 0x06001EF1) /* Icon */
     , (60000,  22, 0x3400002B) /* PhysicsEffectTable */
     , (60000,  50, 0x06006B4F) /* IconOverlay */;
