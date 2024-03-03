DELETE FROM `weenie` WHERE `class_Id` = 60006;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (60006, 'marketplacesignhigh', 8, '2024-03-02 18:26:00') /* Book */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (60006,   1,       8192) /* ItemType - Writable */
     , (60006,   5,       9000) /* EncumbranceVal */
     , (60006,   8,       1800) /* Mass */
     , (60006,  16,         48) /* ItemUseable - ViewedRemote */
     , (60006,  19,        125) /* Value */
     , (60006,  93,       1048) /* PhysicsState - ReportCollisions, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (60006,   1, True ) /* Stuck */
     , (60006,  12, True ) /* ReportCollisions */
     , (60006,  13, False) /* Ethereal */
     , (60006,  22, False) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (60006,  54,       5) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (60006,   1, 'Contested Areas') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (60006,   1, 0x02000290) /* Setup */
     , (60006,   8, 0x060012D3) /* Icon */;

INSERT INTO `weenie_properties_book` (`object_Id`, `max_Num_Pages`, `max_Num_Chars_Per_Page`)
VALUES (60006, 1, 1000);

INSERT INTO `weenie_properties_book_page_data` (`object_Id`, `page_Id`, `author_Id`, `author_Name`, `author_Account`, `ignore_Author`, `page_Text`)
VALUES (60006, 0, 0xFFFFFFFF, ' ', 'prewritten', False, '
Contested areas can be found in this room. 

Warning! Not for the weak. 
');
