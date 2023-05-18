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
                    5186, 5190, 5768, 9094, 9095, 9096, 10995, 10996, 10997,
                    12750, 12751, 12752, 12753, 12754, 12755, 12756, 12757,
                    12758, 12759, 12760, 13210, 13211, 13212, 13213, 13214,
                    13215, 13216, 13217, 13218, 13219, 13239, 13240, 13241,
                    23881, 24176, 24177, 24178, 24180, 24615, 24616, 24617, 24618,
                    24619, 24620, 24621, 24622, 24623, 24624, 24625, 24626,
                    24627, 24628, 24629, 24630, 24631, 24632, 24889, 24890,
                    24891, 24892, 24893, 24894, 24895, 24896, 24897, 24898,
                    24899, 24900, 24901, 24902, 24903, 24904, 24905, 24906,
                    25547, 25548, 25549, 25550, 25702, 27592, 30493, 30494,
                    30495, 30496, 30497, 30498
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
