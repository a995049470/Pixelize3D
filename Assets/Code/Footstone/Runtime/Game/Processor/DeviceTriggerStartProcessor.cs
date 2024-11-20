using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class DeviceTriggerStartProcessor : SimpleGameEntityProcessor<TriggerDeviceComponent, TriggerDeviceLabelComponent>
    {
        public DeviceTriggerStartProcessor() : base()
        {
            Order = ProcessorOrder.TriggerStart;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var deviceComp = kvp.Value.Component1;
                var ownerComp = kvp.Value.Component2;
                
                if(ownerComp.Target != null && deviceComp.IsWaitTrigger)
                {
                    deviceComp.TriggerFlag = TriggerFlag.Triggering;
                }
            }
        }
    }

}
