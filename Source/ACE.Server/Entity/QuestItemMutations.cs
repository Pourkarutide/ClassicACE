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
                    12754, 12760, 12759, 12758, 12757, 12756, 12755,
                    12750, 12753, 12751, 12752, 10995, 10996, 10997,
                    27592, 24890, 24892, 24894, 24896, 24898, 24900,
                    24902, 24904, 24906, 25550, 24616, 24618, 24620,
                    24622, 24624, 24626, 24628, 24630, 24632, 25549,
                    24615, 24617, 24619, 24621, 24623, 24625, 24627,
                    24629, 24631, 25548, 24889, 24891, 24893, 24895,
                    24897, 24899, 24901, 24903, 24905, 25547, 13241,
                    13239, 13240, 13210, 13211, 13212, 13213, 13214,
                    13215, 13216, 13217, 13218, 13219

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
