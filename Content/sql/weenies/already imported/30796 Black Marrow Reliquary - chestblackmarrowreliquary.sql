DELETE FROM `weenie` WHERE `class_Id` = 30796;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (30796, 'chestblackmarrowreliquary', 20, '2019-04-09 00:13:01') /* Chest */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (30796,   1,        512) /* ItemType - Container */
     , (30796,   5,       9000) /* EncumbranceVal */
     , (30796,   6,         -1) /* ItemsCapacity */
     , (30796,   7,         -1) /* ContainersCapacity */
     , (30796,   8,       3000) /* Mass */
     , (30796,  16,         48) /* ItemUseable - ViewedRemote */
     , (30796,  19,       2500) /* Value */
     , (30796,  38,       9999) /* ResistLockpick */
     , (30796,  81,          2) /* MaxGeneratedObjects */
     , (30796,  82,          2) /* InitGeneratedObjects */
     , (30796,  83,          2) /* ActivationResponse - Use */
     , (30796,  93,       1048) /* PhysicsState - ReportCollisions, IgnoreCollisions, Gravity */
     , (30796, 100,          1) /* GeneratorType - Relative */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (30796,   1, True ) /* Stuck */
     , (30796,   2, False) /* Open */
     , (30796,   3, True ) /* Locked */
     , (30796,  12, True ) /* ReportCollisions */
     , (30796,  13, False) /* Ethereal */
     , (30796,  33, False) /* ResetMessagePending */
     , (30796,  34, False) /* DefaultOpen */
     , (30796,  35, True ) /* DefaultLocked */
     , (30796,  86, True ) /* ChestRegenOnClose */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (30796,  43,       1) /* GeneratorRadius */
     , (30796,  54,       1) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (30796,   1, 'Black Marrow Reliquary') /* Name */
     , (30796,  12, 'KeyBlackMarrow') /* LockCode */
     , (30796,  14, 'Use a Black Marrow Key to unlock this cache.') /* Use */
     , (30796,  16, 'A disturbing reliquary, charred black by the devastation of the Singularity Caul.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (30796,   1, 0x020012E4) /* Setup */
     , (30796,   2, 0x09000185) /* MotionTable */
     , (30796,   3, 0x20000026) /* SoundTable */
     , (30796,   8, 0x06003774) /* Icon */
     , (30796,  22, 0x3400002B) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (30796, -1, 32, 0, 1, 1, 2, 72, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Random Loot: Tier 6 - DeathTreasureType: 32(T6_Boss_HighAmount) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: ContainTreasure */
     , (30796, 0.002, 30801, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Blood Fang Jewel(30801/gemportalobsidianrim) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (30796, 0.004, 30800, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Black Glass Array(30800/gemportalobsidianplains) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (30796, 0.006, 30802, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Soul Chalice(30802/gemportalpanopticon) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (30796, 0.008, 30803, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Desolate Seed(30803/gemportalsingularitycaul) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (30796, 0.01, 30809, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Warrior's Emblem(30809/gemportalayntayn) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (30796, 0.012, 30811, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Burning Veil(30811/gemportalcaulcano) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (30796, 0.014, 30810, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Shadow Cursed Totem(30810/gemportalburningtower) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (30796, 0.016, 30799, 0, 1, 1, 2, 8, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Corrupted Skull(30799/gemportalfloatingtower) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (30796, 0.018, 30808, 0, 1, 1, 2, 8, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Seething Skull(30808/gemportafloatingbridge) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (30796, 0.02, 30812, 0, 1, 1, 2, 8, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Antiquated Compass(30812/gemportalcauloasis) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (30796, 0.022, 30813, 0, 1, 1, 2, 8, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Black Luster Pearl(30813/pearlblackluster) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (30796, 0.024, 30807, 0, 1, 1, 2, 8, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate The Orphanage(30807/gemquestorphanage) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (30796, 0.026, 30805, 0, 1, 1, 2, 8, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Ancient Temple(30805/gemquestlivingtome) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (30796, 0.028, 30804, 0, 1, 1, 2, 8, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Singularity Caul Asylum(30804/gemquestasylum) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (30796, 0.03, 30806, 0, 1, 1, 2, 8, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Cavernous Olthoi Chasm(30806/gemquestolthoichasm) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */;
