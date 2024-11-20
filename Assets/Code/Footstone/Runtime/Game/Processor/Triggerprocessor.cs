using Lost.Runtime.Footstone.Core;
using UnityEngine;
namespace Lost.Runtime.Footstone.Game
{

    //不动的时候触发机关
    public class TriggerProcessor : SimpleGameEntityProcessor<TriggerManComponent, VelocityComponent>
    {
        public TriggerProcessor() : base()
        {
            Order = ProcessorOrder.Trigger;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var triggerManComp = kvp.Value.Component1;
                var velocityComp = kvp.Value.Component2;
                if(!velocityComp.IsMoving)
                {
                    var pos = velocityComp.TargetPos;
                    //跳过该次触发 用来防止第一帧触发陷阱或者事件？
                    //TODO:需要用这种字段避免一些情况还是从设计上避免？
                    if(triggerManComp.LastTriggerPosition == pos)
                    {
                        triggerManComp.LastTriggerPosition = pos;
                    }
                    else
                    {
                        var castCount = physicsSystem.SphereCastNonAlloc(pos, 0.4f, castColliders, GameConstant.GameEventLayer, QueryTriggerInteraction.Collide);
                        for (int i = 0; i < castCount; i++)
                        {
                            var collider = castColliders[i];
                            var targetEntityComp = collider.GetComponent<TargetEntity>();
                            var deviceComp = targetEntityComp?.Target?.Get<TriggerDeviceComponent>();
                            if(deviceComp != null && deviceComp.IsWaitTrigger && !uniqueIdManager.IsVaild(deviceComp.TirggerManUID))
                            {
                                //无论是否能触发都要对uid赋值，可以用来防止一开场就踩在陷阱上而触发陷阱
                                deviceComp.TirggerManUID = triggerManComp.Id;
                                cmd.AddEntityComponent<TriggerDeviceLabelComponent>(targetEntityComp.Target, comp =>
                                {
                                    comp.Target = kvp.Key.Entity;
                                });
                            }
                        }
                    }
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    } 
}
