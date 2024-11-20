using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class DeviceTriggerEndProcessor : SimpleGameEntityProcessor<TriggerDeviceComponent, TriggerDeviceLabelComponent>
    {
        public DeviceTriggerEndProcessor() : base()
        {
            Order = ProcessorOrder.TriggerEnd;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var interactiveComp = kvp.Value.Component1;
                var ownerComp = kvp.Value.Component2;
                cmd.RemoveEntityComponent(ownerComp);
                //默认解锁成功， 如果需要是被则触发过程中将TriggerFlag设置为其他标签
                if(interactiveComp.TriggerFlag == TriggerFlag.Triggering)
                {
                    if(interactiveComp.IsRepeatTrigger)
                        interactiveComp.TriggerFlag =  TriggerFlag.WaitCheck;
                    else
                        interactiveComp.TriggerFlag =  TriggerFlag.Triggered;
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }

}
