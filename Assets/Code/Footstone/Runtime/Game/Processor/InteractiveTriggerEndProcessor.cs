using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class InteractiveTriggerEndProcessor : SimpleGameEntityProcessor<InteractiveComponent, InteractiveLabelComponent>
    {
        public InteractiveTriggerEndProcessor() : base()
        {
            Order = ProcessorOrder.InteractiveTriggerEnd;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var interactiveComp = kvp.Value.Component1;
                var ownerComp = kvp.Value.Component2;
                //默认解锁完成要回收
                if (interactiveComp.NextTriggerFlag != TriggerFlag.Invalid)
                {
                    interactiveComp.TriggerFlag = interactiveComp.NextTriggerFlag;
                    interactiveComp.NextTriggerFlag = TriggerFlag.Invalid;
                }
                else if(interactiveComp.IsTriggering)
                {
                    if(interactiveComp.IsRepeatInteractive)
                    {
                        interactiveComp.TriggerFlag = interactiveComp.OriginTriggerFlag;
                    }
                    else
                    {
                        interactiveComp.TriggerFlag = TriggerFlag.WaitRecycle;
                    }
                }
                if(!interactiveComp.IsWaitPlayerRespond)
                {
                    cmd.RemoveEntityComponent(ownerComp);
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }

}
