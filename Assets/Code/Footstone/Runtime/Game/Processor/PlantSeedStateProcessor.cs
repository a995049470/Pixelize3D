using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class PlantSeedStateProcessor : SimpleGameEntityProcessor<PlantSeedStateComponent, LandPlantComponent>
    {
        public PlantSeedStateProcessor() : base()
        {
            Order = ProcessorOrder.UpdatePlantState + 0;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var plantComp = kvp.Value.Component2;
                var seedComp = kvp.Value.Component1;
                if(plantComp.CurrentPlantFlag == PlantFlag.Seed)
                {
                    bool isGrowthSuccess = seedComp.Growth(ref plantComp.RemainGrowthPower);
                    if(isGrowthSuccess)
                    {
                        plantComp.CurrentPlantFlag = PlantFlag.Seedling;
                    }
                    else
                    {
                        plantComp.NextPlantKey = seedComp.Key;
                    }
                }
                
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
            
        }
    }
}
