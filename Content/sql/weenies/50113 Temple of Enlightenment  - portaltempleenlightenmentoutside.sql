DELETE FROM `weenie` WHERE `class_Id` = 50113;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (50113, 'portaltempleenlightenmentoutside', 7, '2005-02-09 10:00:00') /* Portal */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (50113,   1,      65536) /* ItemType - Portal */
     , (50113,  16,         32) /* ItemUseable - Remote */
     , (50113,  86,         10) /* MinLevel */
     , (50113,  93,       3084) /* PhysicsState - Ethereal, ReportCollisions, Gravity, LightingOn */
     , (50113, 111,         49) /* PortalBitmask - Unrestricted, NoSummon, NoRecall */
     , (50113, 133,          4) /* ShowableOnRadar - ShowAlways */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (50113,   1, True ) /* Stuck */
     , (50113,  11, False) /* IgnoreCollisions */
     , (50113,  12, True ) /* ReportCollisions */
     , (50113,  13, True ) /* Ethereal */
     , (50113,  15, True ) /* LightsStatus */
     , (50113,  88, False) /* PortalShowDestination */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (50113,  54,    -0.1) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (50113,   1, 'Temple of Enlightenment ') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (50113,   1, 0x020001B3) /* Setup */
     , (50113,   2, 0x09000003) /* MotionTable */
     , (50113,   8, 0x0600106B) /* Icon */;

INSERT INTO `weenie_properties_position` (`object_Id`, `position_Type`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (50113, 2, 0x5D470183, 60.638302, -20.423599, 18.004999, 0.953153, 0, 0, 0.302488) /* Destination */
/* @teleloc 0x7F15002E [136.000000 127.300003 12.000000] -0.933893 0.000000 0.000000 -0.357553 */;
