using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    [DefaultEntityComponentProcessor(typeof(PlantSeedStateProcessor))]
    [DefaultEntityComponentProcessor(typeof(PlantSeedStateComponentCopyProcessor))]
    public class PlantSeedStateComponent : EntityComponent
    {
        public string Key;
        //达成会切换到下个阶段
        public int TargetPower = 5;
        public int CurrentPower;

        public bool Growth(ref int remainPower)
        {
            bool isSuccess = false;
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
            return isSuccess;
        }
    }
}
