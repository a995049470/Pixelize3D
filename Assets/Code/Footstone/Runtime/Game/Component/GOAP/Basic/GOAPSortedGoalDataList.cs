using System.Collections.Generic;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    [System.Serializable]
    public class GOAPSortedGoalDataList
    {
        [SerializeField]
        private List<GOAPGoalReference> goalReferences = new();
        private bool isDitry = true;
        private List<GOAPGoalData> goals = new();
        public List<GOAPGoalData> Goals
        {
            get
            {
                if(isDitry)
                {
                    isDitry = false;
                    goals.Clear();
                    foreach (var goalReference in goalReferences)
                    {
                        goals.Add(goalReference.Asset);
                    }
                    goals.Sort((x, y) => y.Order - x.Order);

                }
                return goals;
            }
        }
        
        public void SetDitry()
        {
            isDitry = true;
        }
    }

}



