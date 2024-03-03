DELETE FROM `weenie` WHERE `class_Id` = 60003;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (60003, 'portalmountainretreatcustom', 7, '2024-03-02 18:26:00') /* Portal */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (60003,   1,      65536) /* ItemType - Portal */
     , (60003,  16,         32) /* ItemUseable - Remote */
     , (60003,  86,         50) /* MinLevel */
     , (60003,  93,       3084) /* PhysicsState - Ethereal, ReportCollisions, Gravity, LightingOn */
     , (60003, 111,          1) /* PortalBitmask - Unrestricted */
     , (60003, 133,          4) /* ShowableOnRadar - ShowAlways */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (60003,   1, True ) /* Stuck */
     , (60003,  11, False) /* IgnoreCollisions */
     , (60003,  12, True ) /* ReportCollisions */
     , (60003,  13, True ) /* Ethereal */
     , (60003,  15, True ) /* LightsStatus */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (60003,  54,    -0.1) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (60003,   1, 'Mountain Market (PK Risk!)') /* Name */
     , (60003,  16, 'High tier vendor and lifestone. Potentially contested area for PKs.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (60003,   1, 0x020006F4) /* Setup */
     , (60003,   2, 0x09000003) /* MotionTable */
     , (60003,   8, 0x0600106B) /* Icon */;

INSERT INTO `weenie_properties_position` (`object_Id`, `position_Type`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (60003, 2, 0x7ACA0135, 62.8242, 9.73467, 200.005, 0.999967, 0, 0, -0.008065) /* Destination */
/* @teleloc 0x7ACA0135 [62.824200 9.734670 200.005005] 0.999967 0.000000 0.000000 -0.008065 */;
