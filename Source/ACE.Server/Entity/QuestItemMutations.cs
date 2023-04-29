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
                    _mutationWeenieBlackList.Add(12754);
                    _mutationWeenieBlackList.Add(12760);
                    _mutationWeenieBlackList.Add(12759);
                    _mutationWeenieBlackList.Add(12758);
                    _mutationWeenieBlackList.Add(12757);
                    _mutationWeenieBlackList.Add(12756);
                    _mutationWeenieBlackList.Add(12755);
                    _mutationWeenieBlackList.Add(12750);
                    _mutationWeenieBlackList.Add(12753);
                    _mutationWeenieBlackList.Add(12751);
                    _mutationWeenieBlackList.Add(12752);
                    _mutationWeenieBlackList.Add(10995);
                    _mutationWeenieBlackList.Add(10996);
                    _mutationWeenieBlackList.Add(10997);
                    _mutationWeenieBlackList.Add(27592);
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
