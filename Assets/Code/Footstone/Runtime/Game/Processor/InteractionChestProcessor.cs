using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class InteractionChestProcessor : SimpleGameEntityProcessor<ChestComponent, InteractiveComponent, CostComponent, InteractiveLabelComponent>
    {
        public InteractionChestProcessor() : base()
        {
            Order = ProcessorOrder.InteractiveChest;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var interactionComp = kvp.Value.Component2;
                var costComp = kvp.Value.Component3;
                if(costComp.IsSuccessCost && interactionComp.IsTriggering)
                {
                    interactionComp.TriggerFlag = TriggerFlag.TriggeringNotRecyle;
                }
            }
        }
    }

}
