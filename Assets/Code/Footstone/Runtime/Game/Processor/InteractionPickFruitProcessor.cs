using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class InteractionPickFruitProcessor : SimpleGameEntityProcessor<PlantFruitStateComponent, LandPlantComponent, InteractiveComponent, InteractiveLabelComponent>
    {
        public InteractionPickFruitProcessor() : base()
        {
            Order = ProcessorOrder.PickFruit;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var fruitComp = kvp.Value.Component1;
                var plantComp = kvp.Value.Component2;
                var interactionComp = kvp.Value.Component3;
                if(plantComp.CurrentPlantFlag == PlantFlag.Fruit && interactionComp.IsTriggerEffect)
                {
                    if(!string.IsNullOrEmpty(fruitComp.Fruit))
                    {
                        bagData.ReceiveItem(fruitComp.Fruit, fruitComp.Count);
                    }
                    if(fruitComp.IsDieAfterPick())
                    {
                        plantComp.CurrentPlantFlag = PlantFlag.Death;
                    }
                    else
                    {
                        //重新返回成熟期等待结果
                        plantComp.Refruit(0.55f);
                    }
                }
            }
        }
    }

    
}
