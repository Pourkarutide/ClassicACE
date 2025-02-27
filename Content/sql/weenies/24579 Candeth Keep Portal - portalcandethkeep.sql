DELETE FROM `weenie` WHERE `class_Id` = 24579;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (24579, 'portalcandethkeep', 7, '2005-02-09 10:00:00') /* Portal */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (24579,   1,      65536) /* ItemType - Portal */
     , (24579,  16,         32) /* ItemUseable - Remote */
     , (24579,  93,       3084) /* PhysicsState - Ethereal, ReportCollisions, Gravity, LightingOn */
     , (24579, 111,          1) /* PortalBitmask - Unrestricted */
     , (24579, 133,          4) /* ShowableOnRadar - ShowAlways */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (24579,   1, True ) /* Stuck */
     , (24579,  11, False) /* IgnoreCollisions */
     , (24579,  12, True ) /* ReportCollisions */
     , (24579,  13, True ) /* Ethereal */
     , (24579,  15, True ) /* LightsStatus */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (24579,  54,    -0.1) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (24579,   1, 'Candeth Keep Portal') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (24579,   1, 0x020001B3) /* Setup */
     , (24579,   2, 0x09000003) /* MotionTable */
     , (24579,   8, 0x0600106B) /* Icon */;

INSERT INTO `weenie_properties_position` (`object_Id`, `position_Type`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (24579, 2, 0x2B120029, 120.642, 1.549, 48.005, 0.087156, 0, 0, -0.996195) /* Destination */
/* @teleloc 0x2B120029 [120.641998 1.549000 48.005001] 0.087156 0.000000 0.000000 -0.996195 */;
