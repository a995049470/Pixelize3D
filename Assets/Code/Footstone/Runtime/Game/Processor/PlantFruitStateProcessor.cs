using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class PlantFruitStateProcessor : SimpleGameEntityProcessor<PlantFruitStateComponent, LandPlantComponent, InteractiveComponent>
    {
        public PlantFruitStateProcessor() : base()
        {
            Order = ProcessorOrder.UpdatePlantState + 3;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var plantComp = kvp.Value.Component2;
                var fruitComp = kvp.Value.Component1;
                var interactionComp = kvp.Value.Component3;
                

                if (plantComp.CurrentPlantFlag == PlantFlag.Fruit)
                {
                    plantComp.NextPlantKey = fruitComp.Key;
                    interactionComp.TriggerFlag = TriggerFlag.WaitTrigger;
                    //bool isGrowthSuccess = fruitComp.Growth(ref plantComp.RemainGrowthPower);
                    // if (isGrowthSuccess)
                    // {
                    //     plantComp.CurrentPlantFlag = PlantFlag.Adult;
                    // }
                    // else
                    // { 
                    //     plantComp.NextPlantKey = fruitComp.Key;
                    // }
                }
                
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }

    }
}
