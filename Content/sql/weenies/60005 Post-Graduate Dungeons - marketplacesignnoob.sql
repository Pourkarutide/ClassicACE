DELETE FROM `weenie` WHERE `class_Id` = 60005;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (60005, 'marketplacesignnoob', 8, '2005-02-09 10:00:00') /* Book */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (60005,   1,       8192) /* ItemType - Writable */
     , (60005,   5,       9000) /* EncumbranceVal */
     , (60005,   8,       1800) /* Mass */
     , (60005,  16,         48) /* ItemUseable - ViewedRemote */
     , (60005,  19,        125) /* Value */
     , (60005,  93,       1048) /* PhysicsState - ReportCollisions, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (60005,   1, True ) /* Stuck */
     , (60005,  12, True ) /* ReportCollisions */
     , (60005,  13, False) /* Ethereal */
     , (60005,  22, False) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (60005,  54,       5) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (60005,   1, 'Post-Graduate Dungeons') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (60005,   1, 0x02000290) /* Setup */
     , (60005,   8, 0x060012D3) /* Icon */;

INSERT INTO `weenie_properties_book` (`object_Id`, `max_Num_Pages`, `max_Num_Chars_Per_Page`)
VALUES (60005, 1, 1000);

INSERT INTO `weenie_properties_book_page_data` (`object_Id`, `page_Id`, `author_Id`, `author_Name`, `author_Account`, `ignore_Author`, `page_Text`)
VALUES (60005, 0, 0xFFFFFFFF, ' ', 'prewritten', False, '
Within this room can be found some free portals for level 20-30 characters who have finished the early game and need a nudge in the right direction.

If they are too difficult, don''t despair! Consider adjusting your spec, bringing a group, or obtaining more gear from quests until you get the hang of it. Asking for help is encouraged!

Once you''ve mastered these dungeons, Exploration Contracts are a good stepping stone to the higher tiers.
');
