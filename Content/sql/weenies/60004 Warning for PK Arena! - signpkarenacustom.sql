DELETE FROM `weenie` WHERE `class_Id` = 60004;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (60004, 'signpkarenacustom', 8, '2005-02-09 10:00:00') /* Book */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (60004,   1,       8192) /* ItemType - Writable */
     , (60004,   5,       9000) /* EncumbranceVal */
     , (60004,   8,       1800) /* Mass */
     , (60004,  16,         48) /* ItemUseable - ViewedRemote */
     , (60004,  19,        125) /* Value */
     , (60004,  93,       1048) /* PhysicsState - ReportCollisions, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (60004,   1, True ) /* Stuck */
     , (60004,  12, True ) /* ReportCollisions */
     , (60004,  13, False) /* Ethereal */
     , (60004,  22, False) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (60004,  54,       5) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (60004,   1, 'Warning for PK Arena!') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (60004,   1, 0x02000290) /* Setup */
     , (60004,   8, 0x060012D3) /* Icon */;

INSERT INTO `weenie_properties_book` (`object_Id`, `max_Num_Pages`, `max_Num_Chars_Per_Page`)
VALUES (60004, 1, 1000);

INSERT INTO `weenie_properties_book_page_data` (`object_Id`, `page_Id`, `author_Id`, `author_Name`, `author_Account`, `ignore_Author`, `page_Text`)
VALUES (60004, 0, 0xFFFFFFFF, ' ', 'prewritten', False, '
WARNING!

These are the designated PK Arenas. 

There is no death penalty if you die INSIDE these dungeons, even on Hardcore mode. 
');
