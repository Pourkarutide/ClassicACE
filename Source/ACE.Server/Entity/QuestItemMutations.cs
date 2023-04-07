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
                if(_mutationWeenieBlackList == null)
                {
                    _mutationWeenieBlackList = new List<uint>();
                    // add your list of WeenieIDs for quest items you don't want to allow to mutate
                    // ex: _mutationWeenieBlackList.Add(12345);
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

    }    
}
