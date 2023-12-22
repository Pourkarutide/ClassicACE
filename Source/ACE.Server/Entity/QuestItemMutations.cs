using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Server.Entity
{
    public static class QuestItemMutations
    {
        private static List<uint> _mutationWeenieBlackList = null;
        public static List<uint> MutationWeenieBlackList
        {
            get
            {
                if (_mutationWeenieBlackList == null)
                {
                    _mutationWeenieBlackList = new List<uint>
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
                        30498 // Lou Ka's Shouken
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
