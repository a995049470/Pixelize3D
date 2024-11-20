using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    //可交互物体开始触发
    public class InteractiveTriggerStartProcessor : SimpleGameEntityProcessor<InteractiveComponent, InteractiveLabelComponent>
    {
        public InteractiveTriggerStartProcessor() : base()
        {
            Order = ProcessorOrder.InteractiveTriggerStart;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var interactiveComp = kvp.Value.Component1;
                var ownerComp = kvp.Value.Component2;
                
                if(ownerComp.Target != null && !interactiveComp.IsWaitPlayerRespond)
                {
                    interactiveComp.TriggerFlag = TriggerFlag.Triggering;
                }
            }
        }
    }

}
