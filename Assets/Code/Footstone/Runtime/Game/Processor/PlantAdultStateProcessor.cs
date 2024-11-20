using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class PlantAdultStateProcessor : SimpleGameEntityProcessor<PlantAdultStateComponent, LandPlantComponent>
    {
        public PlantAdultStateProcessor() : base()
        {
            Order = ProcessorOrder.UpdatePlantState + 2;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var plantComp = kvp.Value.Component2;
                var adultComp = kvp.Value.Component1;
                if (plantComp.CurrentPlantFlag == PlantFlag.Adult)
                {
                    bool isGrowthSuccess = adultComp.Growth(ref plantComp.RemainGrowthPower, ref plantComp.AdultStateSpeedUpProgress);
                    if (isGrowthSuccess)
                    {
                        plantComp.CurrentPlantFlag = PlantFlag.Fruit;
                    }
                    else
                    {
                        plantComp.NextPlantKey = adultComp.GetCurrentKey();
                    }
                }
                
                
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
