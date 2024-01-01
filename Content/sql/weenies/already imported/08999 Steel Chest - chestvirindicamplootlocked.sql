DELETE FROM `weenie` WHERE `class_Id` = 8999;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (8999, 'chestvirindicamplootlocked', 20, '2019-08-19 00:00:00') /* Chest */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (8999,   1,        512) /* ItemType - Container */
     , (8999,   5,       9000) /* EncumbranceVal */
     , (8999,   6,         -1) /* ItemsCapacity */
     , (8999,   7,         -1) /* ContainersCapacity */
     , (8999,   8,       3000) /* Mass */
     , (8999,  16,         48) /* ItemUseable - ViewedRemote */
     , (8999,  19,       2500) /* Value */
     , (8999,  38,       5000) /* ResistLockpick */
     , (8999,  81,          3) /* MaxGeneratedObjects */
     , (8999,  82,          3) /* InitGeneratedObjects */
     , (8999,  83,          2) /* ActivationResponse - Use */
     , (8999,  93,       1048) /* PhysicsState - ReportCollisions, IgnoreCollisions, Gravity */
     , (8999,  96,        500) /* EncumbranceCapacity */
     , (8999, 100,          1) /* GeneratorType - Relative */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (8999,   1, True ) /* Stuck */
     , (8999,   2, False) /* Open */
     , (8999,   3, True ) /* Locked */
     , (8999,  12, True ) /* ReportCollisions */
     , (8999,  13, False) /* Ethereal */
     , (8999,  33, False) /* ResetMessagePending */
     , (8999,  34, False) /* DefaultOpen */
     , (8999,  35, True ) /* DefaultLocked */
     , (8999,  86, True ) /* ChestRegenOnClose */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (8999,  43,       1) /* GeneratorRadius */
     , (8999,  54,       1) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (8999,   1, 'Steel Chest') /* Name */
     , (8999,  12, 'keychesthigh') /* LockCode */
     , (8999,  14, 'Use this item to open it and see its contents.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (8999,   1, 0x0200007C) /* Setup */
     , (8999,   2, 0x09000004) /* MotionTable */
     , (8999,   3, 0x20000021) /* SoundTable */
     , (8999,   8, 0x06001020) /* Icon */
     , (8999,  22, 0x3400002B) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (8999, -1, 338, 0, 1, 1, 2, 72, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Random Loot: Tier 6 - DeathTreasureType: 338(T4_ScrollSteelChest) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: ContainTreasure */
     , (8999, -1, 20179, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Superb Mana Charge(20179/manastonesuperb) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (8999, 0.005, 7509, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Scroll of Searing Disc(7509/scrollacidring) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (8999, 0.01, 7510, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Scroll of Horizon's Blades(7510/scrollbladering) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (8999, 0.015, 7511, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Scroll of Cassius' Ring of Fire(7511/scrollflamering) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (8999, 0.02, 7512, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Scroll of Nuhmudira's Spines(7512/scrollforcering) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (8999, 0.025, 7513, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Scroll of Halo of Frost(7513/scrollfrostring) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (8999, 0.03, 7514, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Scroll of Eye of the Storm(7514/scrolllightningring) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (8999, 0.035, 7515, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Scroll of Tectonic Rifts(7515/scrollshockwavering) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (8999, 0.04, 7516, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Scroll of Blistering Creeper(7516/scrollacidwall) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (8999, 0.045, 7517, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Scroll of Bed of Blades(7517/scrollbladewall) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (8999, 0.05, 7518, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Scroll of Slithering Flames(7518/scrollflamewall) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (8999, 0.055, 7519, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Scroll of Spike Strafe(7519/scrollforcewall) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (8999, 0.06, 7520, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Scroll of Foon-Ki's Glacial Floe(7520/scrollfrostwall) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (8999, 0.065, 7521, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Scroll of Os' Wall(7521/scrolllightningwall) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (8999, 0.07, 7522, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Scroll of Hammering Crawler(7522/scrollshockwavewall) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (8999, 0.075, 20430, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Scroll of Dissolving Vortex(20430/scrollacidblast7) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (8999, 0.08, 20435, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Scroll of Sau Kolin's Sword(20435/scrollbladeblast7) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (8999, 0.085, 20439, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Scroll of Silencia's Scorn(20439/scrollflameblast7) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (8999, 0.09, 20444, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Scroll of Stinging Needles(20444/scrollforceblast7) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (8999, 0.095, 20449, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Scroll of Winter's Embrace(20449/scrollfrostblast7) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (8999, 0.1, 20454, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Scroll of Luminous Wrath(20454/scrolllightningblast7) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (8999, 0.105, 20459, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Scroll of Pummeling Storm(20459/scrollshockblast7) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (8999, 0.11, 20434, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Scroll of Celdiseth's Searing(20434/scrollacidvolley7) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (8999, 0.115, 20437, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Scroll of Flensing Wings(20437/scrollbladevolley7) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (8999, 0.12, 20443, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Scroll of Infernae(20443/scrollflamevolley7) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (8999, 0.125, 20448, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Scroll of Fusillade(20448/scrollforcevolley7) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (8999, 0.13, 20453, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Scroll of Blizzard(20453/scrollfrostvolley7) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (8999, 0.135, 20458, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Scroll of Tempest(20458/scrolllightningvolley7) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (8999, 0.14, 20438, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Scroll of Thousand Fists(20438/scrollbludgeoningvolley7) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (8999, 0.84, 20179, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Superb Mana Charge(20179/manastonesuperb) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (8999, 1, 9060, 0, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Titan Mana Charge(9060/manachargetitan) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */;
