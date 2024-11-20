using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class PlantSeedlingStateProcessor : SimpleGameEntityProcessor<PlantSeedlingStateComponent, LandPlantComponent>
    {
        public PlantSeedlingStateProcessor() : base()
        {
            Order = ProcessorOrder.UpdatePlantState + 1;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var plantComp = kvp.Value.Component2;
                var seedlingComp = kvp.Value.Component1;
                if (plantComp.CurrentPlantFlag == PlantFlag.Seedling)
                {
                    bool isGrowthSuccess = seedlingComp.Growth(ref plantComp.RemainGrowthPower);
                    if (isGrowthSuccess)
                    {
                        plantComp.CurrentPlantFlag = PlantFlag.Adult;
                    }
                    else
                    {
                        plantComp.NextPlantKey = seedlingComp.Key;
                    }
                }
                

            }

            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
