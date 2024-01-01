DELETE FROM `landblock_instance` WHERE `landblock` = 0xC6A9;

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9000,  1088, 0xC6A9002B, 135.053, 58.373, 41.937, -0.687853, 0, 0, -0.72585, False, '2005-02-09 10:00:00'); /* Arwic Mines Portal(1088/portalarwicmines) */
/* @teleloc 0xC6A9002B [135.052994 58.373001 41.937000] -0.687853 0.000000 0.000000 -0.725850 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9001,  4568, 0xC6A90007, 12.6156, 155.602, 41.937, 0.201147, 0, 0, -0.979561, False, '2005-02-09 10:00:00'); /* Portal to Tou-Tou(4568/portaltoutou) */
/* @teleloc 0xC6A90007 [12.615600 155.602005 41.937000] 0.201147 0.000000 0.000000 -0.979561 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9026,   695, 0xC6A9015D, 103.939, 80.2309, 42.005, 0.407587, 0, 0, 0.913167, False, '2005-02-09 10:00:00'); /* Cateril Carsmad the Armorer(695/arwicblacksmith) - Level: 10 - Generates - Hand Axe(303/axehand) / Jerkin(124/jerkin) / Breeches(117/breeches) / Leather Boots(115/bootsleather) / Apron(10696/apron) / Trade Note (100)(2621/tradenote100) / Trade Note (500)(2622/tradenote500) / Trade Note (1,000)(2623/tradenote1000) / Trade Note (5,000)(2624/tradenote5000) / Trade Note (10,000)(2625/tradenote10000) / Trade Note (50,000)(2626/tradenote50000) / Trade Note (100,000)(2627/tradenote100000) / Trade Note (150,000)(20628/tradenote150000) / Trade Note (200,000)(20629/tradenote200000) / Trade Note (250,000)(20630/tradenote250000) / Combat Tactic: Taunt(50046/combattactictaunt) / Combat Tactic: Sneaking(50078/combattacticsneaking) / Combat Technique: Reckless(50047/combattechniquereckless) / Combat Technique: Defensive(50048/combattechniquedefensive) / Combat Technique: Opportunist(50049/combattechniqueopportunist) / Combat Technique: Riposte(50050/combattechniqueriposte) / Combat Technique: Power Shot(50111/combattechniquepowershot) */
/* @teleloc 0xC6A9015D [103.939003 80.230904 42.005001] 0.407587 0.000000 0.000000 0.913167 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9027,   400, 0xC6A90159, 106.198, 87.2021, 42.005, 0.683474, 0, 0, 0.729975, False, '2005-02-09 10:00:00'); /* Carsith the Weaponsmith(400/arwicblacksmith2) - Level: 10 - Generates - War Hammer(359/warhammer) / Jerkin(124/jerkin) / Breeches(117/breeches) / Shoes(132/shoes) / Apron(10696/apron) / Left-hand Tether(45683/ace45683-lefthandtether) / Left-hand Tether Remover(45684/ace45684-lefthandtetherremover) / Trade Note (100)(2621/tradenote100) / Trade Note (500)(2622/tradenote500) / Trade Note (1,000)(2623/tradenote1000) / Trade Note (5,000)(2624/tradenote5000) / Trade Note (10,000)(2625/tradenote10000) / Trade Note (50,000)(2626/tradenote50000) / Trade Note (100,000)(2627/tradenote100000) / Trade Note (150,000)(20628/tradenote150000) / Trade Note (200,000)(20629/tradenote200000) / Trade Note (250,000)(20630/tradenote250000) / Combat Tactic: Taunt(50046/combattactictaunt) / Combat Tactic: Sneaking(50078/combattacticsneaking) / Combat Technique: Reckless(50047/combattechniquereckless) / Combat Technique: Defensive(50048/combattechniquedefensive) / Combat Technique: Opportunist(50049/combattechniqueopportunist) / Combat Technique: Riposte(50050/combattechniqueriposte) / Combat Technique: Power Shot(50111/combattechniquepowershot) */
/* @teleloc 0xC6A90159 [106.197998 87.202103 42.005001] 0.683474 0.000000 0.000000 0.729975 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A902E,  3951, 0xC6A9000A, 40.7643, 41.8, 42, 0.707107, 0, 0, -0.707107, False, '2005-02-09 10:00:00'); /* Linkable Monster Gen (1 hour)(3951/linkmonstergen1hour) - Generates - Place Holder Object(3666/placeholder) */
/* @teleloc 0xC6A9000A [40.764301 41.799999 42.000000] 0.707107 0.000000 0.000000 -0.707107 */

INSERT INTO `landblock_instance_link` (`parent_GUID`, `child_GUID`, `last_Modified`)
VALUES (0x7C6A902E, 0x7C6A9036, '2005-02-09 10:00:00')/* Leather Crafter (4213/leathercrafteraluvian) - Level: 6 - Generates - Shirt(2591/shirtpuffy) / Breeches(117/breeches) / Boots(2606/boots) */;

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A902F,   463, 0xC6A90012, 59.773, 39.2189, 42, 0.707107, 0, 0, -0.707107, False, '2005-02-09 10:00:00'); /* Arwic(463/sign-arwic) - Generates - Town Crier(5773/towncrieraluvianmale) - Level: 15 */
/* @teleloc 0xC6A90012 [59.772999 39.218899 42.000000] 0.707107 0.000000 0.000000 -0.707107 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9030,   698, 0xC6A90173, 81.7814, 15.9958, 42.005, 0.809307, 0, 0, -0.587386, False, '2005-02-09 10:00:00'); /* Belthew Shearstone the Jeweler(698/arwicjeweler) - Level: 4 - Generates - Tunic(134/tunic) / Pants(127/pants) / Shoes(132/shoes) / Apron(10696/apron) / Trade Note (100)(2621/tradenote100) / Trade Note (500)(2622/tradenote500) / Trade Note (1,000)(2623/tradenote1000) / Trade Note (5,000)(2624/tradenote5000) / Trade Note (10,000)(2625/tradenote10000) / Trade Note (50,000)(2626/tradenote50000) / Trade Note (100,000)(2627/tradenote100000) / Trade Note (150,000)(20628/tradenote150000) / Trade Note (200,000)(20629/tradenote200000) / Trade Note (250,000)(20630/tradenote250000) / Portal Recall Gem(50000/gemportalrecall) / Primary Portal Recall Gem(50001/gemprimaryportalrecall) / Secondary Portal Recall Gem(50002/gemsecondaryportalrecall) / Lifestone Recall Gem(50007/gemlifestonerecall) / Primary Portal Summon Gem(50005/gemprimaryportalsummon) / Secondary Portal Summon Gem(50006/gemsecondaryportalsummon) / Primary Portal Tie Gem(50003/gemprimaryportaltie) / Secondary Portal Tie Gem(50004/gemsecondaryportaltie) / Lifestone Tie Gem(50008/gemlifestonetie) / Xarabydun Portal Gem(26639/gemportalxarabydun) / Al-Arqas Portal Gem(8973/gemportalalarqas) / Yaraq Portal Gem(8984/gemportalyaraq) / Samsur Portal Gem(8980/gemportalsamsur) / Yanshi Portal Gem(8983/gemportalyanshi) / Shoushi Portal Gem(8981/gemportalshoushi) / Nanto Portal Gem(8978/gemportalnanto) / Holtburg Portal Gem(8976/gemportalholtburg) / Lytelthorpe Portal Gem(8977/gemportallytelthorpe) / Rithwic Portal Gem(8979/gemportalrithwic) */
/* @teleloc 0xC6A90173 [81.781403 15.995800 42.005001] 0.809307 0.000000 0.000000 -0.587386 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9031,   702, 0xC6A9012B, 27.64, 78.0078, 42.005, -0.0092, 0, 0, 0.999958, False, '2005-02-09 10:00:00'); /* Shopkeep Mirinda(702/arwicshopkeep) - Level: 3 - Generates - Tunic(2592/tunicpuffy) / Pants(2598/pantsbaggy) / Shoes(132/shoes) / Apron(10696/apron) / Bundle of Arrowshafts(4585/arrowshaft) / Bundle of Quarrelshafts(5339/quarrelshaft) / Apple(258/apple) / Flour(4761/flour) / Water(4746/water) / Baking Pan(4754/bakingpan) / Whittling Knife(5778/whittlingknife) / Empty Flask(151/flask) / Parchment(365/parchment) / Torch(293/torch) / Pack(136/backpack) / Small Belt Pouch(139/beltpouchsmall) / Spell Component Pouch(50009/componentpouch) / Salvage Barrel(50119/salvagebarrel) */
/* @teleloc 0xC6A9012B [27.639999 78.007797 42.005001] -0.009200 0.000000 0.000000 0.999958 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9032,   703, 0xC6A90121, 59.1166, 127.648, 42.005, -0.999094, 0, 0, 0.04256, False, '2005-02-09 10:00:00'); /* Davis the Tailor(703/arwictailor) - Level: 4 - Generates - Tunic(134/tunic) / Pants(127/pants) / Leather Boots(115/bootsleather) / Cowl(119/cowlcloth) / Apron(10696/apron) / Pants(127/pants) / Pants(127/pants) / Pants(127/pants) / Shirt(130/shirt) / Shirt(130/shirt) / Shirt(130/shirt) / Cap(118/capcloth) / Cap(118/capcloth) / Cap(118/capcloth) / Boots(2606/boots) / Boots(2606/boots) / Boots(2606/boots) / Faran Robe with Hood(5851/robealuvianhood) / Faran Robe(5850/robealuviannohood) / Kireth Gown with Band(8371/dressaluvian) / Plain Lockpick(513/lockpickplain) / Reliable Lockpick(545/lockpickreliable) / Good Lockpick(512/lockpickgood) */
/* @teleloc 0xC6A90121 [59.116600 127.648003 42.005001] -0.999094 0.000000 0.000000 0.042560 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9033,   696, 0xC6A9010B, 102.671, 130.88, 42.005, -0.167467, 0, 0, -0.985878, False, '2005-02-09 10:00:00'); /* Grocer Rodega Tyning(696/arwicgrocer) - Level: 5 - Generates - Shirt(130/shirt) / Breeches(117/breeches) / Leather Boots(115/bootsleather) / Cap(118/capcloth) / Apron(10696/apron) / Baking Pan(4754/bakingpan) / Simple Dried Rations(23327/rationsdriedsimple) / Elaborate Dried Rations(23326/rationsdriedelaborate) / Apple(258/apple) / Grapes(264/grapes) / Bunch of Nanners(22578/nannerbunch) / Carrot(5758/carrot) / Cabbage(260/cabbage) / Pumpkin(8232/pumpkin) / Brimstone-cap Mushroom(547/mushroom) / Water(4746/water) / Milk(2463/milk) / Egg(546/egg) / Flour(4761/flour) / Uncooked Rice(4768/uncookedrice) / Chicken(262/chicken) / Fish(263/fish) / Meat(265/meat) / Side of Beef(4753/beefside) / Rabbit Carcass(5633/rabbitcarcass) / Rennet(4766/rennet) / Brine(4755/brine) / Cinnamon Bark(5780/cinnamonbark) / Honey(4763/honey) / Oregano(5803/oregano) / Hot Pepper(5794/hotpepper) / Nutmeg(14795/nutmeg) / Ginger(14789/ginger) / Brown Beans(7825/cacaobeans) / Peppermint Stick(13222/peppermintstick) */
/* @teleloc 0xC6A9010B [102.670998 130.880005 42.005001] -0.167467 0.000000 0.000000 -0.985878 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9034,   697, 0xC6A90102, 59.7999, 13.1228, 42.005, 0.998285, 0, 0, 0.058541, False, '2005-02-09 10:00:00'); /* Healer Dragando the Leech(697/arwichealer) - Level: 7 - Generates - Jerkin(124/jerkin) / Breeches(117/breeches) / Leather Boots(115/bootsleather) / Cap(118/capcloth) / Apron(10696/apron) / Health Draught(2457/healthdraught) / Health Potion(377/healthpotion) / Health Tincture(27319/healthtincture) / Mana Draught(2460/manadraught) / Mana Potion(379/manapotion) / Mana Tincture(27322/manatincture) / Stamina Draught(5634/staminadraught) / Stamina Potion(378/staminapotion) / Stamina Tincture(27326/staminatincture) / Handy Healing Kit(628/healingkitcrude) / Adept Healing Kit(629/healingkitplain) / Gifted Healing Kit(630/healingkitgood) / Trade Note (100)(2621/tradenote100) / Trade Note (500)(2622/tradenote500) / Trade Note (1,000)(2623/tradenote1000) / Trade Note (5,000)(2624/tradenote5000) / Trade Note (10,000)(2625/tradenote10000) / Trade Note (50,000)(2626/tradenote50000) / Trade Note (100,000)(2627/tradenote100000) / Trade Note (150,000)(20628/tradenote150000) / Trade Note (200,000)(20629/tradenote200000) / Trade Note (250,000)(20630/tradenote250000) / Heal Other III(4588/servicehealother3) / Revitalize Other III(4591/servicerevitalizeother3) / Mana Boost Other III(4594/servicemanaboost3) / Regeneration Other III(50051/serviceregenerateother3) / Rejuvenation Other III(50052/servicerejuvenationother3) / Mana Renewal Other III(50053/servicemanarenewal3) / Cleanse All Magic Other(8182/servicedispelother3) / Nullify All Magic Other(8185/servicedispelother6) */
/* @teleloc 0xC6A90102 [59.799900 13.122800 42.005001] 0.998285 0.000000 0.000000 0.058541 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9035,   693, 0xC6A9013F, 36.7994, 35.0502, 42.005, -0.719481, 0, 0, 0.694512, False, '2005-02-09 10:00:00'); /* Barkeep Mae Lilidag(693/arwicbarkeeper) - Level: 7 - Generates - Tunic(134/tunic) / Pants(127/pants) / Leather Boots(115/bootsleather) / Apron(10696/apron) / Ale(2451/ale) / Stout(2471/stout) / Mead(2462/mead) / Milk(2463/milk) / Water(4746/water) / Cheese(261/cheese) / Cake(620/cake) / Fried Steak(4732/friedsteak) / Meat Pie(4734/meatpie) / The Obsidian Span(6420/rumorempbridge) / A Call To Arms(11929/writingwar) / The Lost Wish Lovers(24034/rumorlostwishlovers) / A Shivering Stone(6416/rumoratlancrag) / Rumor Color Codes(50054/rumorcolorcodes) */
/* @teleloc 0xC6A9013F [36.799400 35.050201 42.005001] -0.719481 0.000000 0.000000 0.694512 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9036,  4213, 0xC6A90125, 60.0735, 129.665, 45.005, 0.987307, 0, 0, 0.158823,  True, '2005-02-09 10:00:00'); /* Leather Crafter(4213/leathercrafteraluvian) - Level: 6 - Generates - Shirt(2591/shirtpuffy) / Breeches(117/breeches) / Boots(2606/boots) */
/* @teleloc 0xC6A90125 [60.073502 129.664993 45.005001] 0.987307 0.000000 0.000000 0.158823 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A903D,   694, 0xC6A9014D, 41.4102, 33.9591, 42.005, 0.705405, 0, 0, -0.708804, False, '2005-02-09 10:00:00'); /* Barkeep Lienne(694/arwicbarkeeper2) - Level: 7 - Generates - Jerkin(124/jerkin) / Breeches(117/breeches) / Shoes(132/shoes) / Cowl(119/cowlcloth) / Apron(10696/apron) / Bowl of Stew(549/stew) / Apple(258/apple) / Bread(259/bread) / Cabbage(260/cabbage) / Cake(620/cake) / Cheese(261/cheese) / Apple Juice(2452/applejuice) / Cider(2453/cider) / Coffee(2454/coffee) / Grape Juice(2455/grapejuice) / Green Tea(2456/greentea) / Kumiss(2459/kumiss) / Mead(2462/mead) / Milk(2463/milk) / Orange Juice(2464/orangejuice) / Palm Wine(2465/palmwine) / Rumor Color Codes(50054/rumorcolorcodes) */
/* @teleloc 0xC6A9014D [41.410198 33.959099 42.005001] 0.705405 0.000000 0.000000 -0.708804 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A903F,  7923, 0xC6A9000A, 40.7628, 42.7992, 42, 0.707107, 0, 0, -0.707107, False, '2005-02-09 10:00:00'); /* Linkable Monster Generator ( 3 Min.)(7923/linkmonstergen3minutes) - Generates - Place Holder Object(3666/placeholder) */
/* @teleloc 0xC6A9000A [40.762798 42.799198 42.000000] 0.707107 0.000000 0.000000 -0.707107 */

INSERT INTO `landblock_instance_link` (`parent_GUID`, `child_GUID`, `last_Modified`)
VALUES (0x7C6A903F, 0x7C6A9040, '2005-02-09 10:00:00')/* Anasha (22934/studentnuhmudirapermgiftquest) - Level: 9 - Generates - Tunic(2594/tunicflared) / Pants(2598/pantsbaggy) / Boots(2606/boots) */;

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9040, 22934, 0xC6A90154, 43.1023, 106.412, 42.005, -0.38242, 0, 0, 0.923989,  True, '2005-02-09 10:00:00'); /* Anasha(22934/studentnuhmudirapermgiftquest) - Level: 9 - Generates - Tunic(2594/tunicflared) / Pants(2598/pantsbaggy) / Boots(2606/boots) */
/* @teleloc 0xC6A90154 [43.102299 106.412003 42.005001] -0.382420 0.000000 0.000000 0.923989 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9041, 23631, 0xC6A90000, 55.741, 58.5781, 103.046, 0.69713, 0, 0, -0.716945, False, '2005-02-09 10:00:00'); /* April 2003 Raining Mad Cows Gen(23631/eventmadcowgen) - Generates - Mad Cow(23623/cowmad) - Level: 7 / Mad Cow(23623/cowmad) - Level: 7 / Mad Cow(23623/cowmad) - Level: 7 / Mad Cow(23623/cowmad) - Level: 7 / Mad Cow(23623/cowmad) - Level: 7 / Mad Cow(23623/cowmad) - Level: 7 / Mad Cow(23623/cowmad) - Level: 7 / Mad Cow(23623/cowmad) - Level: 7 / Mad Cow(23623/cowmad) - Level: 7 / Mad Cow(23623/cowmad) - Level: 7 */
/* @teleloc 0xC6A90000 [55.741001 58.578098 103.045998] 0.697130 0.000000 0.000000 -0.716945 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9042, 25486, 0xC6A90153, 36.7432, 25.998, 42.005, -0.968216, 0, 0, 0.250115,  True, '2005-02-09 10:00:00'); /* Hiyp the Toad(25486/hiypthetoadrot2) - Level: 15 - Generates - Shirt(2590/shirtbaggy) / Pants(2597/pantsflared) / Boots(2606/boots) */
/* @teleloc 0xC6A90153 [36.743198 25.997999 42.005001] -0.968216 0.000000 0.000000 0.250115 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9043,  7923, 0xC6A9000A, 40.7851, 40.9047, 42, 0.707107, 0, 0, -0.707107, False, '2005-02-09 10:00:00'); /* Linkable Monster Generator ( 3 Min.)(7923/linkmonstergen3minutes) - Generates - Place Holder Object(3666/placeholder) */
/* @teleloc 0xC6A9000A [40.785099 40.904701 42.000000] 0.707107 0.000000 0.000000 -0.707107 */

INSERT INTO `landblock_instance_link` (`parent_GUID`, `child_GUID`, `last_Modified`)
VALUES (0x7C6A9043, 0x7C6A9042, '2005-02-09 10:00:00')/* Hiyp the Toad (25486/hiypthetoadrot2) - Level: 15 - Generates - Shirt(2590/shirtbaggy) / Pants(2597/pantsflared) / Boots(2606/boots) */
     , (0x7C6A9043, 0x7C6A9044, '2005-02-09 10:00:00')/* Translator Aun Laokhe (27117/translatoraunlaokhe) - Level: 50 - Generates - Buadren(11971/shieldtumerokdrummonsteronly) */
     , (0x7C6A9043, 0x7C6A9046, '2005-02-09 10:00:00')/* Apprentice Alchemist (28182/collectoralchemyalulow) - Level: 5 - Generates - Shirt(130/shirt) / Pants(127/pants) / Boots(2606/boots) */;

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9044, 27117, 0xC6A90036, 146.904, 137.281, 42.005, 0.407439, 0, 0, 0.913233,  True, '2005-02-09 10:00:00'); /* Translator Aun Laokhe(27117/translatoraunlaokhe) - Level: 50 - Generates - Buadren(11971/shieldtumerokdrummonsteronly) */
/* @teleloc 0xC6A90036 [146.904007 137.281006 42.005001] 0.407439 0.000000 0.000000 0.913233 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9045,   509, 0xC6A90000, 80.4637, 81.6297, 42.005, -0.999985, 0, 0, 0.005435, False, '2005-02-09 10:00:00'); /* Life Stone(509/lifestone) */
/* @teleloc 0xC6A90000 [80.463699 81.629700 42.005001] -0.999985 0.000000 0.000000 0.005435 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9046, 28182, 0xC6A90102, 53.3937, 10.9117, 42.005, 0.951646, 0, 0, -0.307196,  True, '2005-02-09 10:00:00'); /* Apprentice Alchemist(28182/collectoralchemyalulow) - Level: 5 - Generates - Shirt(130/shirt) / Pants(127/pants) / Boots(2606/boots) */
/* @teleloc 0xC6A90102 [53.393700 10.911700 42.005001] 0.951646 0.000000 0.000000 -0.307196 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9047, 28929, 0xC6A90144, 26.2151, 42.683, 42, -0.212638, 0, 0, 0.977131, False, '2005-02-09 10:00:00'); /* Generator Antius Roads(28929/generatorantiusroads) - Generates - Antius Blackmoor(28961/antiusblackmoorroads) - Level: 126 */
/* @teleloc 0xC6A90144 [26.215099 42.682999 42.000000] -0.212638 0.000000 0.000000 0.977131 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9048, 28930, 0xC6A90144, 26.2151, 42.683, 42, -0.125494, 0, 0, 0.992094, False, '2005-02-09 10:00:00'); /* Generator Audrey Roads Gen(28930/generatoraudreyroads) - Generates - Guard Audrey(28968/guardaudrey) - Level: 115 */
/* @teleloc 0xC6A90144 [26.215099 42.682999 42.000000] -0.125494 0.000000 0.000000 0.992094 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9049,   412, 0xC6A9000A, 45.2887, 35.0374, 42.082, 0.707107, 0, 0, 0.707107, False, '2021-01-14 08:07:27'); /* Door(412/door-aluvian-house) */
/* @teleloc 0xC6A9000A [45.288700 35.037399 42.082001] 0.707107 0.000000 0.000000 0.707107 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A904A,   412, 0xC6A9000A, 24.2699, 35.0416, 42.082, 0.707107, 0, 0, -0.707107, False, '2021-01-14 08:17:02'); /* Door(412/door-aluvian-house) */
/* @teleloc 0xC6A9000A [24.269899 35.041599 42.082001] 0.707107 0.000000 0.000000 -0.707107 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A904B,   412, 0xC6A9000A, 35.2812, 30.026, 45.582, -0.999995, 0, 0, 0.003089, False, '2021-01-14 08:24:03'); /* Door(412/door-aluvian-house) */
/* @teleloc 0xC6A9000A [35.281200 30.025999 45.582001] -0.999995 0.000000 0.000000 0.003089 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A904C,   412, 0xC6A9013C, 31.8336, 40.0393, 42.082, 0, 0, 0, -1, False, '2021-01-14 08:30:51'); /* Door(412/door-aluvian-house) */
/* @teleloc 0xC6A9013C [31.833599 40.039299 42.082001] 0.000000 0.000000 0.000000 -1.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A904D,   412, 0xC6A9013E, 38.7256, 40.0455, 42.082, 0, 0, 0, -1, False, '2021-01-14 08:38:01'); /* Door(412/door-aluvian-house) */
/* @teleloc 0xC6A9013E [38.725601 40.045502 42.082001] 0.000000 0.000000 0.000000 -1.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A904E,   412, 0xC6A9013F, 35.2777, 39.5343, 42.082, 1, 0, 0, 0, False, '2021-01-14 08:41:53'); /* Door(412/door-aluvian-house) */
/* @teleloc 0xC6A9013F [35.277699 39.534302 42.082001] 1.000000 0.000000 0.000000 0.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A904F,  5486, 0xC6A90021, 100.634, 15.463, 41.937, -0.941981, 0, 0, -0.335665, False, '2021-01-14 09:49:41'); /* Al-Jalima Portal(5486/portalaljalima) */
/* @teleloc 0xC6A90021 [100.634003 15.463000 41.937000] -0.941981 0.000000 0.000000 -0.335665 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9050,   692, 0xC6A90177, 84.2171, 20.0199, 45.005, -0.993925, 0, 0, -0.11006, False, '2021-01-14 10:30:47'); /* Magus Guthima the Wise(692/arwicarchmage) - Level: 7 - Generates - Tunic(134/tunic) / Pants(127/pants) / Shoes(132/shoes) / Apron(10696/apron) / Lead Scarab(691/scarablead) / Iron Scarab(689/scarabiron) / Copper Scarab(686/scarabcopper) / Silver Scarab(688/scarabsilver) / Amaranth(765/amaranth) / Bistort(766/bistort) / Comfrey(767/comfrey) / Damiana(768/damiana) / Dragonsblood(769/dragonsblood) / Eyebright(770/eyebright) / Frankincense(771/frankincense) / Ginseng(625/ginseng) / Hawthorn(772/hawthorn) / Henbane(773/henbane) / Hyssop(774/hyssop) / Mandrake(775/mandrake) / Mugwort(776/mugwort) / Myrrh(777/myrrh) / Saffron(778/saffron) / Vervain(779/vervain) / Wormwood(780/wormwood) / Yarrow(781/yarrow) / Powdered Agate(782/agate) / Powdered Amber(783/amber) / Powdered Azurite(784/azurite) / Powdered Bloodstone(785/bloodstone) / Powdered Carnelian(786/carnelian) / Powdered Hematite(626/hematite) / Powdered Lapis Lazuli(787/lapislazul) / Powdered Malachite(788/malachite) / Powdered Moonstone(789/moonstone) / Powdered Onyx(790/onyx) / Powdered Quartz(791/quartz) / Powdered Turquoise(792/turquoise) / Brimstone(753/alchembrimstone) / Cadmia(754/alchemcadmia) / Cinnabar(755/alchemcinnabar) / Cobalt(756/alchemcobalt) / Colcothar(757/alchemcolcothar) / Gypsum(758/alchemgypsum) / Quicksilver(759/alchemquicksilver) / Realgar(760/alchemrealgar) / Stibnite(761/alchemstibnite) / Turpeth(762/alchemturpeth) / Verdigris(763/alchemverdigris) / Vitriol(764/alchemvitriol) / Poplar Talisman(749/poplartalisman) / Blackthorn Talisman(742/blackthorntalisman) / Yew Talisman(752/yewtalisman) / Hemlock Talisman(747/hemlocktalisman) / Alder Talisman(627/aldertalisman) / Ebony Talisman(744/ebonytalisman) / Birch Talisman(741/birchtalisman) / Ashwood Talisman(740/ashwoodtalisman) / Elder Talisman(745/eldertalisman) / Rowan Talisman(750/rowantalisman) / Willow Talisman(751/willowtalisman) / Cedar Talisman(743/cedartalisman) / Oak Talisman(748/oaktalisman) / Hazel Talisman(746/hazeltalisman) / Red Taper(1650/taperred) / Pink Taper(1649/taperpink) / Orange Taper(1648/taperorange) / Yellow Taper(1653/taperyellow) / Green Taper(1645/tapergreen) / Turquoise Taper(1654/taperturquoise) / Blue Taper(1643/taperblue) / Indigo Taper(1647/taperindigo) / Violet Taper(1651/taperviolet) / Brown Taper(1644/taperbrown) / White Taper(1652/taperwhite) / Grey Taper(1646/tapergrey) / Minor Mana Stone(27331/manastoneminor) / Lesser Mana Stone(2434/manastonelesser) / Mana Stone(2435/manastone) / Tiny Mana Charge(4612/manastonetiny) / Small Mana Charge(4613/manastonesmall) / Moderate Mana Charge(4614/manastonemoderate) / High Mana Charge(4615/manastonehigh) / Great Mana Charge(4616/manastonegreat) / Faran Life Apprentice Robe(6068/robesucklifealuvian) / Faran War Apprentice Robe(6071/robesuckwaraluvian) / Alembic(4747/alembic) / Mortar and Pestle(4751/mortarandpestle) / Aqua Incanta(4748/aquaincanta) / Neutral Balm(5338/neutralbalm) / Pack(136/backpack) / Small Belt Pouch(139/beltpouchsmall) / Wand(5539/wandaluvian) / Spell Component Pouch(50009/componentpouch) / Trade Note (100)(2621/tradenote100) / Trade Note (500)(2622/tradenote500) / Trade Note (1,000)(2623/tradenote1000) / Trade Note (5,000)(2624/tradenote5000) / Trade Note (10,000)(2625/tradenote10000) / Trade Note (50,000)(2626/tradenote50000) / Trade Note (100,000)(2627/tradenote100000) / Trade Note (150,000)(20628/tradenote150000) / Trade Note (200,000)(20629/tradenote200000) / Trade Note (250,000)(20630/tradenote250000) / Strength Other III(30664/servicestrengthother3) / Endurance Other III(30670/serviceenduranceother3) / Coordination Other III(30668/servicecoordinationother3) / Quickness Other III(30674/servicequicknessother3) / Focus Other III(30672/servicefocusother3) / Willpower Other III(30666/servicewillpowerother3) / Ley Line Amulet: War Magic(50056/leylineamuletwar) / Ley Line Amulet: Life Magic(50057/leylineamuletlife) / Spell Conduit I(50104/spellconduit1) - Level: 1 / Spell Conduit II(50105/spellconduit2) - Level: 2 / Spell Conduit III(50106/spellconduit3) - Level: 3 / Spell Conduit IV(50107/spellconduit4) - Level: 4 / Salvage Barrel(50119/salvagebarrel) / Spell Extraction Scroll I(50123/spellextractionscroll1) - Level: 1 / Spell Extraction Scroll II(50124/spellextractionscroll2) - Level: 2 / Spell Extraction Scroll III(50125/spellextractionscroll3) - Level: 3 / Spell Extraction Scroll IV(50126/spellextractionscroll4) - Level: 4 */
/* @teleloc 0xC6A90177 [84.217102 20.019899 45.005001] -0.993925 0.000000 0.000000 -0.110060 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9051,   403, 0xC6A9016C, 104.529, 32.7592, 42.005, 0.869088, 0, 0, 0.494657, False, '2021-01-14 10:32:39'); /* Harald the Fletcher(403/arwicbowyer) - Level: 6 - Generates - Longbow(306/bowlong) / Jerkin(124/jerkin) / Pants(127/pants) / Leather Boots(115/bootsleather) / Apron(10696/apron) / Quarrel(305/bolt) / Arrow(300/arrow) / Bundle of Arrowheads(4586/arrowhead) / Bundle of Arrowshafts(4585/arrowshaft) / Bundle of Quarrelshafts(5339/quarrelshaft) / Blunt Arrow(3599/arrowblunt) / Blunt Quarrel(3603/boltblunt) / Frog Crotch Arrow(3601/arrowfrogcrotch) / Frog Crotch Quarrel(3605/boltfrogcrotch) / Armor Piercing Arrow(3598/arrowarmorpiercing) / Armor Piercing Quarrel(3602/boltarmorpiercing) / Wrapped Bundle of Arrowheads(9359/wrappedarrowhead) / Wrapped Bundle of Broad Arrowheads(9363/wrappedarrowheadbroad) / Wrapped Bundle of Blunt Arrowheads(9362/wrappedarrowheadblunt) / Wrapped Bundle of Armor Piercing Arrowheads(9361/wrappedarrowheadarmorpiercing) / Wrapped Bundle of Frog Crotch Arrowheads(9366/wrappedarrowheadfrogcrotch) / Wrapped Bundle of Arrowshafts(9377/wrappedarrowshaft) / Wrapped Bundle of Quarrelshafts(9378/wrappedquarrelshaft) / Atlatl Dart(12464/atlatldart) / Bundle of Atlatl Dart Shafts(15296/atlatldartshaft) / Wrapped Bundle of Atlatl Dartshafts(15298/wrappedatlatldartshaft) / Throwing Dart(316/dart) / Throwing Dagger(315/daggerthrowing) / Javelin(320/javelin) / Throwing Axe(304/axethrowing) / Throwing Club(310/clubthrowing) / Djarid(317/djarid) / Shouken(343/shuriken) / Trade Note (100)(2621/tradenote100) / Trade Note (500)(2622/tradenote500) / Trade Note (1,000)(2623/tradenote1000) / Trade Note (5,000)(2624/tradenote5000) / Trade Note (10,000)(2625/tradenote10000) / Trade Note (50,000)(2626/tradenote50000) / Trade Note (100,000)(2627/tradenote100000) / Trade Note (150,000)(20628/tradenote150000) / Trade Note (200,000)(20629/tradenote200000) / Trade Note (250,000)(20630/tradenote250000) */
/* @teleloc 0xC6A9016C [104.528999 32.759201 42.005001] 0.869088 0.000000 0.000000 0.494657 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9052,   701, 0xC6A90154, 41.9994, 113.885, 42.005, 0.985795, 0, 0, -0.167952, False, '2021-01-14 10:51:45'); /* Laqisha the Scribe(701/arwicscribe) - Level: 5 - Generates - Shirt(130/shirt) / Pants(127/pants) / Shoes(132/shoes) / Turban(135/turban) / Apron(10696/apron) / Book(364/book) / Parchment(365/parchment) / Tome(367/tome) / Bloodshed Rumor(4170/directionsoldtalisman) / The Festival Stones of the Empyrean(5602/directionsfestivalstones) / The Meeting Halls(6419/directionsallegiancehall) / The Reclusive Mage(5677/rumorlethe4) / Altar of Asheron Rumor(5601/rumornpk) / Aluvian Cookbook(5583/cookbookaluvian) / Specialty Cookbook(5856/cookbookspecialty) / Chocolate Cookbook(7884/cookbookchocolate) / Festival Cookbook(14797/cookbookfestival) / Alchemy Guide(5586/guidealchemy) / Fletching Guide(5587/guidefletching) / Combat Manual(50045/combatManual) */
/* @teleloc 0xC6A90154 [41.999401 113.885002 42.005001] 0.985795 0.000000 0.000000 -0.167952 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9053,  4786, 0xC6A90132, 37.7263, 87.2657, 38, 0.122813, 0, 0, -0.99243, False, '2021-01-14 10:52:42'); /* Olthoi Hunter Gen(4786/olthoihuntergen) - Generates - Olthoi Hunter(3930/olthoihunter) - Level: 9 */
/* @teleloc 0xC6A90132 [37.726299 87.265701 38.000000] 0.122813 0.000000 0.000000 -0.992430 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9054,   412, 0xC6A90011, 58.6088, 21.2148, 42.082, 0, 0, 0, -1, False, '2021-01-14 10:56:03'); /* Door(412/door-aluvian-house) */
/* @teleloc 0xC6A90011 [58.608799 21.214800 42.082001] 0.000000 0.000000 0.000000 -1.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9055,   412, 0xC6A90011, 50.8487, 14.6753, 42.082, 0.707107, 0, 0, -0.707107, False, '2021-01-14 10:58:47'); /* Door(412/door-aluvian-house) */
/* @teleloc 0xC6A90011 [50.848701 14.675300 42.082001] 0.707107 0.000000 0.000000 -0.707107 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9056,   412, 0xC6A90019, 87.1842, 20.0035, 42.082, 0, 0, 0, -1, False, '2021-01-14 11:09:11'); /* Door(412/door-aluvian-house) */
/* @teleloc 0xC6A90019 [87.184196 20.003500 42.082001] 0.000000 0.000000 0.000000 -1.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9057,   412, 0xC6A90019, 80.8759, 10.4984, 42.082, 1, 0, 0, 0, False, '2021-01-14 11:12:56'); /* Door(412/door-aluvian-house) */
/* @teleloc 0xC6A90019 [80.875900 10.498400 42.082001] 1.000000 0.000000 0.000000 0.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9058,   412, 0xC6A90022, 98.5905, 33.7481, 42.082, -0.705347, 0, 0, 0.708862, False, '2021-01-14 11:17:40'); /* Door(412/door-aluvian-house) */
/* @teleloc 0xC6A90022 [98.590500 33.748100 42.082001] -0.705347 0.000000 0.000000 0.708862 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9059,   412, 0xC6A90022, 106.366, 40.2874, 42.082, 0, 0, 0, -1, False, '2021-01-14 11:19:38'); /* Door(412/door-aluvian-house) */
/* @teleloc 0xC6A90022 [106.365997 40.287399 42.082001] 0.000000 0.000000 0.000000 -1.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A905A,   412, 0xC6A90024, 107.754, 72.8728, 42.082, 1, 0, 0, 0, False, '2021-01-14 11:21:57'); /* Door(412/door-aluvian-house) */
/* @teleloc 0xC6A90024 [107.753998 72.872803 42.082001] 1.000000 0.000000 0.000000 0.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A905B,   412, 0xC6A90169, 107.766, 83.8794, 42.082, 0, 0, 0, -1, False, '2021-01-14 11:25:13'); /* Door(412/door-aluvian-house) */
/* @teleloc 0xC6A90169 [107.765999 83.879402 42.082001] 0.000000 0.000000 0.000000 -1.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A905C,   412, 0xC6A90026, 104.231, 123.724, 42.082, 1, 0, 0, 0, False, '2021-01-14 11:27:12'); /* Door(412/door-aluvian-house) */
/* @teleloc 0xC6A90026 [104.231003 123.723999 42.082001] 1.000000 0.000000 0.000000 0.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A905D,   412, 0xC6A90026, 99.2455, 128.195, 42.082, 0.707107, 0, 0, -0.707107, False, '2021-01-14 11:28:48'); /* Door(412/door-aluvian-house) */
/* @teleloc 0xC6A90026 [99.245499 128.195007 42.082001] 0.707107 0.000000 0.000000 -0.707107 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A905E,   412, 0xC6A90026, 105.572, 125.154, 45.082, 0.707107, 0, 0, -0.707107, False, '2021-01-14 11:34:23'); /* Door(412/door-aluvian-house) */
/* @teleloc 0xC6A90026 [105.571999 125.153999 45.082001] 0.707107 0.000000 0.000000 -0.707107 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A905F,   412, 0xC6A90026, 101.814, 126.187, 45.082, 0.999991, 0, 0, -0.00433, False, '2021-01-14 11:35:54'); /* Door(412/door-aluvian-house) */
/* @teleloc 0xC6A90026 [101.814003 126.186996 45.082001] 0.999991 0.000000 0.000000 -0.004330 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9060,   412, 0xC6A90016, 54.2794, 125.877, 42.082, 1, 0, 0, 0, False, '2021-01-14 11:38:27'); /* Door(412/door-aluvian-house) */
/* @teleloc 0xC6A90016 [54.279400 125.876999 42.082001] 1.000000 0.000000 0.000000 0.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9061,   412, 0xC6A90016, 60.5887, 135.39, 42.082, 0, 0, 0, -1, False, '2021-01-14 11:39:53'); /* Door(412/door-aluvian-house) */
/* @teleloc 0xC6A90016 [60.588699 135.389999 42.082001] 0.000000 0.000000 0.000000 -1.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9062,   412, 0xC6A9000D, 45.3027, 108.958, 42.082, 0.707107, 0, 0, 0.707107, False, '2021-01-14 11:42:55'); /* Door(412/door-aluvian-house) */
/* @teleloc 0xC6A9000D [45.302700 108.958000 42.082001] 0.707107 0.000000 0.000000 0.707107 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9063,   412, 0xC6A9000D, 41.0478, 104.849, 42.082, 1, 0, 0, 0, False, '2021-01-14 11:44:56'); /* Door(412/door-aluvian-house) */
/* @teleloc 0xC6A9000D [41.047798 104.848999 42.082001] 1.000000 0.000000 0.000000 0.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9064,   412, 0xC6A9012F, 30.7998, 91.2622, 42.082, 0.707107, 0, 0, 0.707107, False, '2021-01-14 11:46:43'); /* Door(412/door-aluvian-house) */
/* @teleloc 0xC6A9012F [30.799801 91.262199 42.082001] 0.707107 0.000000 0.000000 0.707107 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9065,   412, 0xC6A9000C, 34.5477, 79.3138, 42.082, 1, 0, 0, 0, False, '2021-01-14 11:49:20'); /* Door(412/door-aluvian-house) */
/* @teleloc 0xC6A9000C [34.547699 79.313797 42.082001] 1.000000 0.000000 0.000000 0.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9066,   385, 0xC6A9000B, 33.2399, 62.3297, 42.055, 0.891264, 0, 0, -0.453485, False, '2021-01-14 12:03:27'); /* Cow Generator(385/cow-generator) - Generates - Cow(14/cow) - Level: 2 */
/* @teleloc 0xC6A9000B [33.239899 62.329700 42.055000] 0.891264 0.000000 0.000000 -0.453485 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9067,   494, 0xC6A9000B, 34.7328, 58.7591, 42, 1, 0, 0, 0, False, '2021-01-14 12:06:14'); /* Arwic Livestock Pen(494/sign-arwiccowfence) */
/* @teleloc 0xC6A9000B [34.732800 58.759102 42.000000] 1.000000 0.000000 0.000000 0.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9068,   495, 0xC6A9000A, 47.4645, 35.086, 46.665, 0.707107, 0, 0, 0.707107, False, '2021-01-14 12:08:10'); /* Twin Aurock Tavern(495/sign-arwicshoptavern) */
/* @teleloc 0xC6A9000A [47.464500 35.085999 46.665001] 0.707107 0.000000 0.000000 0.707107 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9069,   496, 0xC6A9000C, 27.718, 74.7332, 49.145, 1, 0, 0, 0, False, '2021-01-14 12:12:32'); /* Miranda's Miscellaneous(496/sign-arwicshopmirinda) */
/* @teleloc 0xC6A9000C [27.718000 74.733200 49.145000] 1.000000 0.000000 0.000000 0.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A906A,   497, 0xC6A9000D, 45.7759, 110.776, 42, 0.707107, 0, 0, 0.707107, False, '2021-01-14 12:15:11'); /* Laquishah's Writing Materials(497/sign-arwicshopscribe) */
/* @teleloc 0xC6A9000D [45.775902 110.776001 42.000000] 0.707107 0.000000 0.000000 0.707107 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A906B,   498, 0xC6A90016, 55.4776, 122.735, 47, 1, 0, 0, 0, False, '2021-01-14 12:17:58'); /* Davis the Tailor(498/sign-arwicshoptailor) */
/* @teleloc 0xC6A90016 [55.477600 122.735001 47.000000] 1.000000 0.000000 0.000000 0.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A906C,   499, 0xC6A90022, 98.1192, 32.0097, 42, 0.707107, 0, 0, -0.707107, False, '2021-01-14 12:20:05'); /* The True Shot(499/sign-arwicshopfletcher) */
/* @teleloc 0xC6A90022 [98.119202 32.009701 42.000000] 0.707107 0.000000 0.000000 -0.707107 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A906D,   500, 0xC6A90024, 96.6328, 77.9033, 47.02, 0.707107, 0, 0, -0.707107, False, '2021-01-14 12:21:08'); /* Carsith's Forge(500/sign-arwicshopblacksmith) */
/* @teleloc 0xC6A90024 [96.632797 77.903297 47.020000] 0.707107 0.000000 0.000000 -0.707107 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A906E,   502, 0xC6A90026, 101.385, 120.648, 47.7, 1, 0, 0, 0, False, '2021-01-14 12:27:55'); /* Rodega's Provender(502/sign-arwicshopgrocer) */
/* @teleloc 0xC6A90026 [101.385002 120.648003 47.700001] 1.000000 0.000000 0.000000 0.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A906F,  7241, 0xC6A9016C, 107.492, 36.2807, 42.005, -0.577497, 0, 0, -0.816393, False, '2021-01-14 13:21:32'); /* Yuan Hanzu(7241/bowyermasteryuanhanzu) - Level: 24 - Generates - Shirt(2590/shirtbaggy) / Pants(2597/pantsflared) / Leather Boots(115/bootsleather) / Yumi(363/yumi) */
/* @teleloc 0xC6A9016C [107.491997 36.280701 42.005001] -0.577497 0.000000 0.000000 -0.816393 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9070,   463, 0xC6A90025, 103.272, 108.436, 42.055, 0, 0, 0, -1, False, '2021-01-15 06:34:25'); /* Arwic(463/sign-arwic) - Generates - Town Crier(5773/towncrieraluvianmale) - Level: 15 */
/* @teleloc 0xC6A90025 [103.272003 108.435997 42.055000] 0.000000 0.000000 0.000000 -1.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9071, 21465, 0xC6A90016, 69.5138, 127.248, 41.937, -0.000672, 0, 0, -1, False, '2021-01-23 15:18:44'); /* Ispar Yard
 Arwic (21465/portalisparyard) */
/* @teleloc 0xC6A90016 [69.513802 127.248001 41.937000] -0.000672 0.000000 0.000000 -1.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9072, 21477, 0xC6A90016, 69.5138, 126.248, 42.055, -0.000672, 0, 0, -1, False, '2021-01-23 15:18:48'); /* Ispar Yard(21477/isparyardsign) */
/* @teleloc 0xC6A90016 [69.513802 126.248001 42.055000] -0.000672 0.000000 0.000000 -1.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9073, 35962, 0xC6A9016C, 104.76, 37.8939, 42.005, 0.281666, 0, 0, -0.959513, False, '2021-02-02 13:18:29'); /* Havala bint Mylos(35962/ace35962-havalabintmylos) - Level: 24 - Generates - Atlatl(12463/atlatl) / Smock(2589/smock) / Pantaloons(2600/pantaloons) / Leather Boots(115/bootsleather) */
/* @teleloc 0xC6A9016C [104.760002 37.893902 42.005001] 0.281666 0.000000 0.000000 -0.959513 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C6A9074,   174, 0xC6A9000B, 42.084, 66.5495, 42, 1, 0, 0, 0, False, '2022-09-22 11:39:34'); /* Well(174/well) */
/* @teleloc 0xC6A9000B [42.084000 66.549500 42.000000] 1.000000 0.000000 0.000000 0.000000 */
