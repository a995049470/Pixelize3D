using System.Diagnostics.Tracing;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class InteractionCostProcessor : SimpleGameEntityProcessor<InteractiveComponent, CostComponent, InteractiveLabelComponent>
    {
        public InteractionCostProcessor() : base()
        {
            Order = ProcessorOrder.InteractiveCostCheck;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var costComp = kvp.Value.Component2;
                var interactionComp = kvp.Value.Component1;
                if(interactionComp.IsTriggerEffect)
                {
                    bool isSuccessCost = costComp.TryCost(bagData);
                    if(!isSuccessCost)
                    {
                        interactionComp.TriggerFlag = TriggerFlag.WaitTrigger;
                    }
                }
                
            }
        }
    }

}
