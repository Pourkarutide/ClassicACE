DELETE FROM `weenie` WHERE `class_Id` = 50112;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (50112, 'portaltempleforgetfulnessoutside', 7, '2005-02-09 10:00:00') /* Portal */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (50112,   1,      65536) /* ItemType - Portal */
     , (50112,  16,         32) /* ItemUseable - Remote */
     , (50112,  86,         10) /* MinLevel */
     , (50112,  93,       3084) /* PhysicsState - Ethereal, ReportCollisions, Gravity, LightingOn */
     , (50112, 111,         49) /* PortalBitmask - Unrestricted, NoSummon, NoRecall */
     , (50112, 133,          4) /* ShowableOnRadar - ShowAlways */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (50112,   1, True ) /* Stuck */
     , (50112,  11, False) /* IgnoreCollisions */
     , (50112,  12, True ) /* ReportCollisions */
     , (50112,  13, True ) /* Ethereal */
     , (50112,  15, True ) /* LightsStatus */
     , (50112,  88, False) /* PortalShowDestination */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (50112,  54,    -0.1) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (50112,   1, 'Temple of Forgetfulness') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (50112,   1, 0x020001B3) /* Setup */
     , (50112,   2, 0x09000003) /* MotionTable */
     , (50112,   8, 0x0600106B) /* Icon */;

INSERT INTO `weenie_properties_position` (`object_Id`, `position_Type`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (50112, 2, 0x5D480183, 60.803398, -20.170601, 18.004999, 0.923880, 0, 0, -0.382683) /* Destination */
/* @teleloc 0x7FEE0024 [102.300003 73.500000 108.000000] -0.958820 0.000000 0.000000 -0.284015 */;
