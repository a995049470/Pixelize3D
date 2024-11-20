using Lost.Runtime.Footstone.Core;
using UnityEngine;
namespace Lost.Runtime.Footstone.Game
{
    public class TriggerDeviceProcessor : SimpleGameEntityProcessor<TriggerDeviceComponent>
    {
        private TriggerManProcessor triggerManProcessor;

        public TriggerDeviceProcessor() : base()
        {
            Order = ProcessorOrder.CheckTriggerDevice;
        }

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            triggerManProcessor = GetProcessor<TriggerManProcessor>();
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var triggerDeviceComp = kvp.Value.Component1;
                if(triggerDeviceComp.IsUnknow) 
                {
                    var uid = triggerDeviceComp.TirggerManUID;
                    bool isGet = triggerManProcessor.TryGetTriggerMan(uid, out var triggerMan);
                    var dis = float.MaxValue;
                    if(isGet)
                    {
                        dis = Vector3.Distance(triggerMan.Entity.Transform.Position, triggerDeviceComp.Entity.Transform.Position);
                    }
                    if(dis > 0.9f)
                    {
                        triggerDeviceComp.TriggerFlag = TriggerFlag.WaitTrigger;
                        triggerDeviceComp.TirggerManUID = uniqueIdManager.InvalidId;
                    }
                    
                }
            }
        }
    }
}
