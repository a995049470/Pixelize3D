using System.Diagnostics;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class InteractiveCooldownProcessor : SimpleGameEntityProcessor<InteractiveComponent, CooldownComponent, InteractiveLabelComponent>
    {
        private TimeProcessor timeProcessor;
        public InteractiveCooldownProcessor() : base()
        {
            Order = ProcessorOrder.InteractiveCooldown;
        }

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            timeProcessor = GetProcessor<TimeProcessor>();
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var interactiveComp = kvp.Value.Component1;
                var cooldownComp = kvp.Value.Component2;
                if(interactiveComp.IsTriggerEffect)
                {
                    var currentDay = timeProcessor.SingleComponent.Day;
                    if(currentDay <= cooldownComp.LastDay)
                    {
                        interactiveComp.TriggerFlag = TriggerFlag.WaitTrigger;
                        UnityEngine.Debug.Log("改天再来");
                    }
                    else
                    {
                        cooldownComp.LastDay = currentDay;
                    }
                }
            }
        }
    }

}
