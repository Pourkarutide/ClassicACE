using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HarmonyLib.Code;

namespace ACE.Server.Entity
{
    public static class QuestItemMutations
    {
        private static HashSet<uint> _mutationWeenieBlackList = null;
        public static HashSet<uint> MutationWeenieBlackList
        {
            get
            {
                if (_mutationWeenieBlackList == null)
                {
                    _mutationWeenieBlackList = new HashSet<uint>
                    {
                        5186, // Bai Den's Gem
                        5190, // Lou Ka's Yaoji
                        5768, // Poofy Snowball
                        9094, // Unkindled Thaumaturgic Plate Coat
                        9095, // Unkindled Thaumaturgic Plate Girth
                        9096, // Unkindled Thaumaturgic Plate Leggings
                        10995, // Ebon Spine Harpoon
                        10996, // Ebon Spine Harpoon
                        10997, // Ebon Spine Harpoon
                        12750, // Academy Dagger
                        12751, // Academy Hand Axe
                        12752, // Academy Atlatl
                        12753, // Academy Cestus
                        12754, // Academy Shortbow
                        12755, // Academy Mace
                        12756, // Academy Spear
                        12757, // Academy Staff
                        12758, // Academy Short Sword
                        12759, // Academy Wand
                        12760, // Academy Light Crossbow
                        13210, // Academy Coat
                        13211, // Academy Coat
                        13212, // Academy Coat
                        13213, // Academy Coat
                        13214, // Academy Coat
                        13215, // Academy Coat
                        13216, // Academy Coat
                        13217, // Academy Coat
                        13218, // Academy Coat
                        13219, // Academy Coat
                        13239, // Leather Cap
                        13240, // Leather Gauntlets
                        13241, // Leather Leggings
                        23881, // Fish Boots
                        24176, // Jaleh's Wedding Ring
                        24177, // Jaleh's Silk Shirt
                        24178, // Jaleh's Slippers
                        24180, // Jaleh's Turban
                        24615, // Fine Olthoi Bracers
                        24616, // Good Olthoi Bracers
                        24617, // Fine Olthoi Breastplate
                        24618, // Good Olthoi Breastplate
                        24619, // Fine Olthoi Gauntlets
                        24620, // Good Olthoi Gauntlets
                        24621, // Fine Olthoi Girth
                        24622, // Good Olthoi Girth
                        24623, // Fine Olthoi Greaves
                        24624, // Good Olthoi Greaves
                        24625, // Fine Olthoi Brood Queen Helm
                        24626, // Good Olthoi Brood Queen Helm
                        24627, // Fine Olthoi Pauldrons
                        24628, // Good Olthoi Pauldrons
                        24629, // Fine Olthoi Sollerets
                        24630, // Good Olthoi Sollerets
                        24631, // Fine Olthoi Tassets
                        24632, // Good Olthoi Tassets
                        24889, // Greater Olthoi Bracers
                        24890, // Lesser Olthoi Bracers
                        24891, // Greater Olthoi Breastplate
                        24892, // Lesser Olthoi Breastplate
                        24893, // Greater Olthoi Gauntlets
                        24894, // Lesser Olthoi Gauntlets
                        24895, // Greater Olthoi Girth
                        24896, // Lesser Olthoi Girth
                        24897, // Greater Olthoi Greaves
                        24898, // Lesser Olthoi Greaves
                        24899, // Greater Olthoi Brood Queen Helm
                        24900, // Lesser Olthoi Brood Queen Helm
                        24901, // Greater Olthoi Pauldrons
                        24902, // Lesser Olthoi Pauldrons
                        24903, // Greater Olthoi Sollerets
                        24904, // Lesser Olthoi Sollerets
                        24905, // Greater Olthoi Tassets
                        24906, // Lesser Olthoi Tassets
                        25547, // Greater Olthoi Shield
                        25548, // Fine Olthoi Shield
                        25549, // Lesser Olthoi Shield
                        25550, // Good Olthoi Shield
                        25702, // Bandit Mask
                        27592, // Ebon Spine Harpoon
                        30493, // Bai Den's Ring
                        30494, // Bai Den's Bracelet
                        30495, // Bai Den's Necklace
                        30496, // Lou Ka's Trident
                        30497, // Lou Ka's Katar
                        30498, // Lou Ka's Shouken
                        6128, // Peerless Shivering Atlan Staff
                        6129, // Peerless Smoldering Atlan Staff
                        6130, // Peerless Sparking Atlan Staff
                        6131, // Peerless Stinging Atlan Staff
                        6132, // Peerless Atlan Staff
                        6133, // Superior Shivering Atlan Staff
                        6134, // Superior Smoldering Atlan Staff
                        6135, // Superior Sparking Atlan Staff
                        6136, // Superior Stinging Atlan Staff
                        6137, // Superior Atlan Staff
                        6138, // Fine Shivering Atlan Staff
                        6139, // Fine Smoldering Atlan Staff
                        6140, // Fine Sparking Atlan Staff
                        6141, // Fine Stinging Atlan Staff
                        6142, // Fine Atlan Staff
                        6144, // Peerless Atlan Axe
                        6145, // Peerless Shivering Atlan Axe
                        6146, // Peerless Shivering Atlan Axe
                        6147, // Peerless Smoldering Atlan Axe
                        6148, // Peerless Smoldering Atlan Axe
                        6149, // Peerless Sparking Atlan Axe
                        6150, // Peerless Sparking Atlan Axe
                        6151, // Peerless Stinging Atlan Axe
                        6152, // Peerless Stinging Atlan Axe
                        6153, // Superior Atlan Axe
                        6154, // Superior Shivering Atlan Axe
                        6155, // Superior Shivering Atlan Axe
                        6156, // Superior Smoldering Atlan Axe
                        6157, // Superior Smoldering Atlan Axe
                        6158, // Superior Sparking Atlan Axe
                        6159, // Superior Sparking Atlan Axe
                        6160, // Superior Stinging Atlan Axe
                        6161, // Superior Stinging Atlan Axe
                        6162, // Fine Atlan Axe
                        6163, // Fine Shivering Atlan Axe
                        6164, // Fine Shivering Atlan Axe
                        6165, // Fine Smoldering Atlan Axe
                        6166, // Fine Smoldering Atlan Axe
                        6167, // Fine Sparking Atlan Axe
                        6168, // Fine Sparking Atlan Axe
                        6169, // Fine Stinging Atlan Axe
                        6170, // Fine Stinging Atlan Axe
                        6171, // Peerless Atlan Claw
                        6172, // Peerless Shivering Atlan Claw
                        6173, // Peerless Shivering Atlan Claw
                        6174, // Peerless Smoldering Atlan Claw
                        6175, // Peerless Smoldering Atlan Claw
                        6176, // Peerless Sparking Atlan Claw
                        6177, // Peerless Sparking Atlan Claw
                        6178, // Peerless Stinging Atlan Claw
                        6179, // Peerless Stinging Atlan Claw
                        6180, // Superior Atlan Claw
                        6181, // Superior Shivering Atlan Claw
                        6182, // Superior Shivering Atlan Claw
                        6183, // Superior Smoldering Atlan Claw
                        6184, // Superior Smoldering Atlan Claw
                        6185, // Superior Sparking Atlan Claw
                        6186, // Superior Sparking Atlan Claw
                        6187, // Superior Stinging Atlan Claw
                        6188, // Superior Stinging Atlan Claw
                        6189, // Fine Atlan Claw
                        6190, // Fine Shivering Atlan Claw
                        6191, // Fine Shivering Atlan Claw
                        6192, // Fine Smoldering Atlan Claw
                        6193, // Fine Smoldering Atlan Claw
                        6194, // Fine Sparking Atlan Claw
                        6195, // Fine Sparking Atlan Claw
                        6196, // Fine Stinging Atlan Claw
                        6197, // Fine Stinging Atlan Claw
                        6198, // Fine Stinging Atlan Staff
                        6199, // Peerless Atlan Dagger
                        6200, // Peerless Shivering Atlan Dagger
                        6201, // Peerless Shivering Atlan Dagger
                        6202, // Peerless Smoldering Atlan Dagger
                        6203, // Peerless Smoldering Atlan Dagger
                        6204, // Peerless Sparking Atlan Dagger
                        6205, // Peerless Sparking Atlan Dagger
                        6206, // Peerless Stinging Atlan Dagger
                        6207, // Peerless Stinging Atlan Dagger
                        6208, // Superior Atlan Dagger
                        6209, // Superior Shivering Atlan Dagger
                        6210, // Superior Shivering Atlan Dagger
                        6211, // Superior Smoldering Atlan Dagger
                        6212, // Superior Smoldering Atlan Dagger
                        6213, // Superior Sparking Atlan Dagger
                        6214, // Superior Sparking Atlan Dagger
                        6215, // Superior Stinging Atlan Dagger
                        6216, // Superior Stinging Atlan Dagger
                        6217, // Fine Atlan Dagger
                        6218, // Fine Shivering Atlan Dagger
                        6219, // Fine Shivering Atlan Dagger
                        6220, // Fine Smoldering Atlan Dagger
                        6221, // Fine Smoldering Atlan Dagger
                        6222, // Fine Sparking Atlan Dagger
                        6223, // Fine Sparking Atlan Dagger
                        6224, // Fine Stinging Atlan Dagger
                        6225, // Fine Stinging Atlan Dagger
                        6226, // Peerless Atlan Mace
                        6227, // Peerless Shivering Atlan Mace
                        6228, // Peerless Shivering Atlan Mace
                        6229, // Peerless Smoldering Atlan Mace
                        6230, // Peerless Smoldering Atlan Mace
                        6231, // Peerless Sparking Atlan Mace
                        6232, // Peerless Sparking Atlan Mace
                        6233, // Peerless Stinging Atlan Mace
                        6234, // Peerless Stinging Atlan Mace
                        6235, // Superior Atlan Mace
                        6236, // Superior Shivering Atlan Mace
                        6237, // Superior Shivering Atlan Mace
                        6238, // Superior Smoldering Atlan Mace
                        6239, // Superior Smoldering Atlan Mace
                        6240, // Superior Sparking Atlan Mace
                        6241, // Superior Sparking Atlan Mace
                        6242, // Superior Stinging Atlan Mace
                        6243, // Superior Stinging Atlan Mace
                        6244, // Fine Atlan Mace
                        6245, // Fine Shivering Atlan Mace
                        6246, // Fine Shivering Atlan Mace
                        6247, // Fine Smoldering Atlan Mace
                        6248, // Fine Smoldering Atlan Mace
                        6249, // Fine Sparking Atlan Mace
                        6250, // Fine Sparking Atlan Mace
                        6251, // Fine Stinging Atlan Mace
                        6252, // Fine Stinging Atlan Mace
                        6253, // Peerless Atlan Spear
                        6254, // Peerless Shivering Atlan Spear
                        6255, // Peerless Shivering Atlan Spear
                        6256, // Peerless Smoldering Atlan Spear
                        6257, // Peerless Smoldering Atlan Spear
                        6258, // Peerless Sparking Atlan Spear
                        6259, // Peerless Sparking Atlan Spear
                        6260, // Peerless Stinging Atlan Spear
                        6261, // Peerless Stinging Atlan Spear
                        6262, // Superior Atlan Spear
                        6263, // Superior Shivering Atlan Spear
                        6264, // Superior Shivering Atlan Spear
                        6265, // Superior Smoldering Atlan Spear
                        6266, // Superior Smoldering Atlan Spear
                        6267, // Superior Sparking Atlan Spear
                        6268, // Superior Sparking Atlan Spear
                        6269, // Superior Stinging Atlan Spear
                        6270, // Superior Stinging Atlan Spear
                        6271, // Fine Atlan Spear
                        6272, // Fine Shivering Atlan Spear
                        6273, // Fine Shivering Atlan Spear
                        6274, // Fine Smoldering Atlan Spear
                        6275, // Fine Smoldering Atlan Spear
                        6276, // Fine Sparking Atlan Spear
                        6277, // Fine Sparking Atlan Spear
                        6278, // Fine Stinging Atlan Spear
                        6279, // Fine Stinging Atlan Spear
                        6280, // Peerless Shivering Atlan Staff
                        6281, // Peerless Smoldering Atlan Staff
                        6282, // Peerless Sparking Atlan Staff
                        6283, // Peerless Stinging Atlan Staff
                        6284, // Superior Shivering Atlan Staff
                        6285, // Superior Smoldering Atlan Staff
                        6286, // Superior Sparking Atlan Staff
                        6287, // Superior Stinging Atlan Staff
                        6288, // Fine Shivering Atlan Staff
                        6289, // Fine Smoldering Atlan Staff
                        6290, // Fine Sparking Atlan Staff
                        6291, // Peerless Atlan Sword
                        6292, // Peerless Shivering Atlan Sword
                        6293, // Peerless Shivering Atlan Sword
                        6294, // Peerless Smoldering Atlan Sword
                        6295, // Peerless Smoldering Atlan Sword
                        6296, // Peerless Sparking Atlan Sword
                        6297, // Peerless Sparking Atlan Sword
                        6298, // Peerless Stinging Atlan Sword
                        6299, // Peerless Stinging Atlan Sword
                        6300, // Superior Atlan Sword
                        6301, // Superior Shivering Atlan Sword
                        6302, // Superior Shivering Atlan Sword
                        6303, // Superior Smoldering Atlan Sword
                        6304, // Superior Smoldering Atlan Sword
                        6305, // Superior Sparking Atlan Sword
                        6306, // Superior Sparking Atlan Sword
                        6307, // Superior Stinging Atlan Sword
                        6308, // Superior Stinging Atlan Sword
                        6309, // Fine Atlan Sword
                        6310, // Fine Shivering Atlan Sword
                        6311, // Fine Shivering Atlan Sword
                        6312, // Fine Smoldering Atlan Sword
                        6313, // Fine Smoldering Atlan Sword
                        6314, // Fine Sparking Atlan Sword
                        6315, // Fine Sparking Atlan Sword
                        6316, // Fine Stinging Atlan Sword
                        6317, // Fine Stinging Atlan Sword
                        6358, // Peerless Shadow Atlan Axe
                        6359, // Superior Shadow Atlan Axe
                        6360, // Fine Shadow Atlan Axe
                        6361, // Peerless Shadow Atlan Claw
                        6362, // Superior Shadow Atlan Claw
                        6363, // Fine Shadow Atlan Claw
                        6364, // Peerless Shadow Atlan Dagger
                        6365, // Superior Shadow Atlan Dagger
                        6366, // Fine Shadow Atlan Dagger
                        6367, // Peerless Shadow Atlan Mace
                        6368, // Superior Shadow Atlan Mace
                        6369, // Fine Shadow Atlan Mace
                        6370, // Peerless Shadow Atlan Spear
                        6371, // Superior Shadow Atlan Spear
                        6372, // Fine Shadow Atlan Spear
                        6373, // Peerless Shadow Atlan Staff
                        6374, // Superior Shadow Atlan Staff
                        6375, // Fine Shadow Atlan Staff
                        6376, // Peerless Shadow Atlan Sword
                        6377, // Superior Shadow Atlan Sword
                        6378, // Fine Shadow Atlan Sword
                        7448, // Peerless Atlan Axe of Black Fire
                        7449, // Superior Atlan Axe of Black Fire
                        7450, // Fine Atlan Axe of Black Fire
                        7451, // Peerless Atlan Claw of Black Fire
                        7452, // Superior Atlan Claw of Black Fire
                        7453, // Fine Atlan Claw of Black Fire
                        7454, // Peerless Atlan Dagger of Black Fire
                        7455, // Superior Atlan Dagger of Black Fire
                        7456, // Fine Atlan Dagger of Black Fire
                        7457, // Peerless Atlan Mace of Black Fire
                        7458, // Superior Atlan Mace of Black Fire
                        7459, // Fine Atlan Mace of Black Fire
                        7460, // Peerless Atlan Spear of Black Fire
                        7461, // Superior Atlan Spear of Black Fire
                        7462, // Fine Atlan Spear of Black Fire
                        7463, // Peerless Atlan Staff of Black Fire
                        7464, // Superior Atlan Staff of Black Fire
                        7465, // Fine Atlan Staff of Black Fire
                        7466, // Peerless Atlan Sword of Black Fire
                        7467, // Superior Atlan Sword of Black Fire
                        7468, // Fine Atlan Sword of Black Fire
                        12746,//Training Atlatl
                        12740,//Training Hand Axe
                        12741,//Training Shortbow
                        12742,//Training Cestus
                        12739,//Training Dagger
                        12744,//Training Mace
                        12745,//Training Spear
                        12743,//Training Staff
                        12747,//Training Short Sword
                        12748,//Training Wand
                        12749,//Light Training Crossbow

                        46,   // Metal Cap
                        56,   // Leather Gauntlets,
                        13240,// Leather Gauntlets
                        25642,// Leather Gauntlets
                        1435, // Ice Tachi
                        4792, // Celcynd's Ring
                    };
                }

                return _mutationWeenieBlackList;
            }
        }

        public static bool IsQuestItemMutationDisallowed(uint weenieID)
        {
            return MutationWeenieBlackList.Contains(weenieID);
        }

        private static Dictionary<uint, uint> _mutationTierOverrides = null;
        public static Dictionary<uint, uint> MutationTierOverrides
        {
            get
            {
                if (_mutationTierOverrides == null)
                {
                    _mutationTierOverrides = new Dictionary<uint, uint>();
                    // add your list of WeenieIDs and the mutation tier you want to apply for quest items
                    // ex: to hard code the quest item with weenieID = 12345 to use mutation tier 5
                    //     _mutationTierOverrides.Add(12345, 5);
                }

                return _mutationTierOverrides;
            }
        }

        public static uint? GetMutationTierOverride(uint weenieID)
        {
            return MutationTierOverrides.ContainsKey(weenieID) ? MutationTierOverrides[weenieID] : null;
        }


        // Morosity - adding check for tool valid.  Blocks all stone tool type recipes and such
        private static List<uint> _mutationToolBlackList = null;

        private static List<uint> MutationToolBlackList =>
            _mutationToolBlackList ?? (_mutationToolBlackList = new List<uint>
            {
                // add your list of WeenieIDs for tools items you don't want to allow to mutate
                6127, // Stone tool
                20023 // Isparian Weapons Modifying Tool
            });

        public static bool IsToolValidForQuestMutation(uint wcid)
        {
            return !MutationToolBlackList.Contains(wcid);
        }
    }    
}
