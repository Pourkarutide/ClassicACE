DELETE FROM `weenie` WHERE `class_Id` = 60007;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (60007, 'portalcandethcustom', 7, '2024-03-02 18:26:00') /* Portal */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (60007,   1,      65536) /* ItemType - Portal */
     , (60007,  16,         32) /* ItemUseable - Remote */
     , (60007,  86,         40) /* MinLevel */
     , (60007,  93,       3084) /* PhysicsState - Ethereal, ReportCollisions, Gravity, LightingOn */
     , (60007, 111,          1) /* PortalBitmask - Unrestricted */
     , (60007, 133,          4) /* ShowableOnRadar - ShowAlways */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (60007,   1, True ) /* Stuck */
     , (60007,  11, False) /* IgnoreCollisions */
     , (60007,  12, True ) /* ReportCollisions */
     , (60007,  13, True ) /* Ethereal */
     , (60007,  15, True ) /* LightsStatus */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (60007,  54,    -0.1) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (60007,   1, 'Candeth Keep Shortcut') /* Name */
     , (60007,  16, 'Tier 4 town with a prime outdoor leveling zone for skilled adventurers 40+') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (60007,   1, 0x020006F4) /* Setup */
     , (60007,   2, 0x09000003) /* MotionTable */
     , (60007,   8, 0x0600106B) /* Icon */;

INSERT INTO `weenie_properties_position` (`object_Id`, `position_Type`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (60007, 2, 0x2B120029, 120.642, 1.549, 48.005, 0.087156, 0, 0, -0.996195) /* Destination */
/* @teleloc 0x2B120029 [120.641998 1.549000 48.005001] 0.087156 0.000000 0.000000 -0.996195 */;
