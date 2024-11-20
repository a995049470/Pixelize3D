using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    [DefaultEntityComponentProcessor(typeof(PlantAdultStateProcessor))]
    [DefaultEntityComponentProcessor(typeof(PlantAdultStateComponentCopyProcessor))]
    public class PlantAdultStateComponent : EntityComponent
    {
        public string Key;
        //达成会切换到下个阶段
        public int TargetPower = 5;
        public int SubStateCount = 1;
        
        public int CurrentPower;

        private int cacheSubStateIndex = -1;
        private string cacheKey;

        public string GetCurrentKey()
        {
            return cacheKey;
        }

        
        public bool Growth(ref int remainPower, ref float speedUpProgress)
        {
            bool isSuccess = false;
            if(speedUpProgress > 0)
            {
                CurrentPower += Mathf.FloorToInt(TargetPower * speedUpProgress);
                CurrentPower = Mathf.Min(TargetPower, CurrentPower);
                speedUpProgress = 0;
            }
            if(CurrentPower + remainPower >= TargetPower)
            {
                isSuccess = true;
                if(TargetPower > CurrentPower)
                {
                    remainPower -= TargetPower - CurrentPower;
                }
                CurrentPower = 0;
            }
            else
            {
                CurrentPower += remainPower;
                remainPower = 0;
            }
            int subStateIndex = Mathf.FloorToInt((float)CurrentPower / TargetPower * SubStateCount);
            if(cacheSubStateIndex != subStateIndex)
            {
                cacheSubStateIndex = subStateIndex;
                cacheKey = subStateIndex == 0 ? Key : $"{Key}{subStateIndex + 1}";
            }
            return isSuccess;
        }
    }
}
