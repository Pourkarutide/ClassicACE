DELETE FROM `landblock_instance` WHERE `landblock` = 0x8787;

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x78787000,  1148, 0x87870103, 152.092, 184.985, 108, 1, 0, 0, 0, False, '2005-02-09 10:00:00'); /* Door(1148/gardoubledoor) */
/* @teleloc 0x87870103 [152.091995 184.985001 108.000000] 1.000000 0.000000 0.000000 0.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x78787001,  1148, 0x87870000, 160.092, 178.985, 108, 1, 0, 0, 0, False, '2005-02-09 10:00:00'); /* Door(1148/gardoubledoor) */
/* @teleloc 0x87870000 [160.091995 178.985001 108.000000] 1.000000 0.000000 0.000000 0.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x78787002,  1148, 0x87870000, 157.102, 173.495, 108, -0.707107, 0, 0, -0.707107, False, '2005-02-09 10:00:00'); /* Door(1148/gardoubledoor) */
/* @teleloc 0x87870000 [157.102005 173.494995 108.000000] -0.707107 0.000000 0.000000 -0.707107 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7878700C,  4219, 0x87870000, 159.616, 175.354, 108.005, -0.999907, 0, 0, -0.013658, False, '2005-02-09 10:00:00'); /* Linkable Monster Generator ( 7 Min.)(4219/linkmonstergen7minutes) - Generates - Place Holder Object(3666/placeholder) */
/* @teleloc 0x87870000 [159.615997 175.354004 108.004997] -0.999907 0.000000 0.000000 -0.013658 */

INSERT INTO `landblock_instance_link` (`parent_GUID`, `child_GUID`, `last_Modified`)
VALUES (0x7878700C, 0x7878700D, '2005-02-09 10:00:00')/* Skeleton Lord (1762/skeletonlord) - Level: 23 T2_General - Random Loot: Tier 2 - DeathTreasureType: 451(T2_General) - Generates - Skeleton's Skull(3687/skull) / A Small Mnemosyne(9312/pyramidgreensmall) / Skull Stamp(22100/stampsymbolskull) */
     , (0x7878700C, 0x7878700E, '2005-02-09 10:00:00')/* Skeleton Lord (1762/skeletonlord) - Level: 23 T2_General - Random Loot: Tier 2 - DeathTreasureType: 451(T2_General) - Generates - Skeleton's Skull(3687/skull) / A Small Mnemosyne(9312/pyramidgreensmall) / Skull Stamp(22100/stampsymbolskull) */
     , (0x7878700C, 0x7878700F, '2005-02-09 10:00:00')/* Skeleton Wraith (22208/skeletonwraith) - Level: 28 T2_General - Random Loot: Tier 2 - DeathTreasureType: 451(T2_General) - Generates - Skeleton's Skull(3687/skull) / Skeletal Arm(22027/armskeletal) / Skeletal Leg(22031/legskeletal) / Skeletal Torso(22047/torsoskeletal) */
     , (0x7878700C, 0x78787010, '2005-02-09 10:00:00')/* Skeleton Lord (1762/skeletonlord) - Level: 23 T2_General - Random Loot: Tier 2 - DeathTreasureType: 451(T2_General) - Generates - Skeleton's Skull(3687/skull) / A Small Mnemosyne(9312/pyramidgreensmall) / Skull Stamp(22100/stampsymbolskull) */
     , (0x7878700C, 0x78787011, '2005-02-09 10:00:00')/* Skeleton Wraith (22208/skeletonwraith) - Level: 28 T2_General - Random Loot: Tier 2 - DeathTreasureType: 451(T2_General) - Generates - Skeleton's Skull(3687/skull) / Skeletal Arm(22027/armskeletal) / Skeletal Leg(22031/legskeletal) / Skeletal Torso(22047/torsoskeletal) */
     , (0x7878700C, 0x78787012, '2005-02-09 10:00:00')/* Skeleton Wraith (22208/skeletonwraith) - Level: 28 T2_General - Random Loot: Tier 2 - DeathTreasureType: 451(T2_General) - Generates - Skeleton's Skull(3687/skull) / Skeletal Arm(22027/armskeletal) / Skeletal Leg(22031/legskeletal) / Skeletal Torso(22047/torsoskeletal) */
     , (0x7878700C, 0x78787013, '2005-02-09 10:00:00')/* Skeleton Lord (1762/skeletonlord) - Level: 23 T2_General - Random Loot: Tier 2 - DeathTreasureType: 451(T2_General) - Generates - Skeleton's Skull(3687/skull) / A Small Mnemosyne(9312/pyramidgreensmall) / Skull Stamp(22100/stampsymbolskull) */
     , (0x7878700C, 0x78787014, '2005-02-09 10:00:00')/* Risen Knight (8673/zombierisenknight) - Level: 27 T2_General - Random Loot: Tier 2 - DeathTreasureType: 451(T2_General) - Generates - Sturdy Iron Key(6876/keychesthigh) */
     , (0x7878700C, 0x78787015, '2005-02-09 10:00:00')/* Silver Rat (1626/ratsilver) - Level: 28 T2_Warrior - Random Loot: Tier 2 - DeathTreasureType: 457(T2_Warrior) - Generates - Sturdy Iron Key(6876/keychesthigh) */
     , (0x7878700C, 0x78787016, '2005-02-09 10:00:00')/* Skeleton Lord (1762/skeletonlord) - Level: 23 T2_General - Random Loot: Tier 2 - DeathTreasureType: 451(T2_General) - Generates - Skeleton's Skull(3687/skull) / A Small Mnemosyne(9312/pyramidgreensmall) / Skull Stamp(22100/stampsymbolskull) */;

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7878700D,  1762, 0x87870106, 158.615, 186.831, 108.005, 0.241562, 0, 0, -0.970385,  True, '2005-02-09 10:00:00'); /* Skeleton Lord(1762/skeletonlord) - Level: 23 - T2_General - Random Loot: Tier 2 - DeathTreasureType: 451(T2_General) - Generates - Skeleton's Skull(3687/skull) / A Small Mnemosyne(9312/pyramidgreensmall) / Skull Stamp(22100/stampsymbolskull) */
/* @teleloc 0x87870106 [158.615005 186.830994 108.004997] 0.241562 0.000000 0.000000 -0.970385 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7878700E,  1762, 0x87870000, 153.081, 171.058, 111.605, -0.862409, 0, 0, -0.506212,  True, '2005-02-09 10:00:00'); /* Skeleton Lord(1762/skeletonlord) - Level: 23 - T2_General - Random Loot: Tier 2 - DeathTreasureType: 451(T2_General) - Generates - Skeleton's Skull(3687/skull) / A Small Mnemosyne(9312/pyramidgreensmall) / Skull Stamp(22100/stampsymbolskull) */
/* @teleloc 0x87870000 [153.080994 171.057999 111.605003] -0.862409 0.000000 0.000000 -0.506212 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7878700F, 22208, 0x87870000, 152.862, 173.311, 111.605, -0.727732, 0, 0, -0.685861,  True, '2005-02-09 10:00:00'); /* Skeleton Wraith(22208/skeletonwraith) - Level: 28 - T2_General - Random Loot: Tier 2 - DeathTreasureType: 451(T2_General) - Generates - Skeleton's Skull(3687/skull) / Skeletal Arm(22027/armskeletal) / Skeletal Leg(22031/legskeletal) / Skeletal Torso(22047/torsoskeletal) */
/* @teleloc 0x87870000 [152.862000 173.311005 111.605003] -0.727732 0.000000 0.000000 -0.685861 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x78787010,  1762, 0x87870100, 150.115, 173.818, 108.005, -0.749725, 0, 0, 0.66175,  True, '2005-02-09 10:00:00'); /* Skeleton Lord(1762/skeletonlord) - Level: 23 - T2_General - Random Loot: Tier 2 - DeathTreasureType: 451(T2_General) - Generates - Skeleton's Skull(3687/skull) / A Small Mnemosyne(9312/pyramidgreensmall) / Skull Stamp(22100/stampsymbolskull) */
/* @teleloc 0x87870100 [150.115005 173.817993 108.004997] -0.749725 0.000000 0.000000 0.661750 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x78787011, 22208, 0x87870105, 163.752, 182.846, 108.005, 0.629105, 0, 0, 0.77732,  True, '2005-02-09 10:00:00'); /* Skeleton Wraith(22208/skeletonwraith) - Level: 28 - T2_General - Random Loot: Tier 2 - DeathTreasureType: 451(T2_General) - Generates - Skeleton's Skull(3687/skull) / Skeletal Arm(22027/armskeletal) / Skeletal Leg(22031/legskeletal) / Skeletal Torso(22047/torsoskeletal) */
/* @teleloc 0x87870105 [163.751999 182.845993 108.004997] 0.629105 0.000000 0.000000 0.777320 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x78787012, 22208, 0x87870000, 154.032, 179.108, 116.805, -0.901893, 0, 0, -0.431959,  True, '2005-02-09 10:00:00'); /* Skeleton Wraith(22208/skeletonwraith) - Level: 28 - T2_General - Random Loot: Tier 2 - DeathTreasureType: 451(T2_General) - Generates - Skeleton's Skull(3687/skull) / Skeletal Arm(22027/armskeletal) / Skeletal Leg(22031/legskeletal) / Skeletal Torso(22047/torsoskeletal) */
/* @teleloc 0x87870000 [154.031998 179.108002 116.805000] -0.901893 0.000000 0.000000 -0.431959 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x78787013,  1762, 0x87870000, 150.062, 180.181, 116.805, -0.904134, 0, 0, 0.427249,  True, '2005-02-09 10:00:00'); /* Skeleton Lord(1762/skeletonlord) - Level: 23 - T2_General - Random Loot: Tier 2 - DeathTreasureType: 451(T2_General) - Generates - Skeleton's Skull(3687/skull) / A Small Mnemosyne(9312/pyramidgreensmall) / Skull Stamp(22100/stampsymbolskull) */
/* @teleloc 0x87870000 [150.061996 180.181000 116.805000] -0.904134 0.000000 0.000000 0.427249 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x78787014,  8673, 0x87870000, 152.158, 183.074, 116.808, 0.999925, 0, 0, -0.012231,  True, '2005-02-09 10:00:00'); /* Risen Knight(8673/zombierisenknight) - Level: 27 - T2_General - Random Loot: Tier 2 - DeathTreasureType: 451(T2_General) - Generates - Sturdy Iron Key(6876/keychesthigh) */
/* @teleloc 0x87870000 [152.158005 183.074005 116.807999] 0.999925 0.000000 0.000000 -0.012231 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x78787015,  1626, 0x87870103, 149.875, 181.943, 108.012, -0.457096, 0, 0, 0.889417,  True, '2005-02-09 10:00:00'); /* Silver Rat(1626/ratsilver) - Level: 28 - T2_Warrior - Random Loot: Tier 2 - DeathTreasureType: 457(T2_Warrior) - Generates - Sturdy Iron Key(6876/keychesthigh) */
/* @teleloc 0x87870103 [149.875000 181.942993 108.012001] -0.457096 0.000000 0.000000 0.889417 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x78787016,  1762, 0x87870103, 152.698, 177.197, 108.005, -0.984348, 0, 0, -0.176233,  True, '2005-02-09 10:00:00'); /* Skeleton Lord(1762/skeletonlord) - Level: 23 - T2_General - Random Loot: Tier 2 - DeathTreasureType: 451(T2_General) - Generates - Skeleton's Skull(3687/skull) / A Small Mnemosyne(9312/pyramidgreensmall) / Skull Stamp(22100/stampsymbolskull) */
/* @teleloc 0x87870103 [152.697998 177.197006 108.004997] -0.984348 0.000000 0.000000 -0.176233 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x78787017, 23601, 0x87870000, 152, 177, 116.805, 0, 0, 0, -1, False, '2005-02-09 10:00:00'); /* Runed Chest(23601/chestquestlockedlowpoia) - Locked(100/nokey) - Content - Random Loot: Tier 2 - DeathTreasureType: 410(T2_RunedChest) */
/* @teleloc 0x87870000 [152.000000 177.000000 116.805000] 0.000000 0.000000 0.000000 -1.000000 */
