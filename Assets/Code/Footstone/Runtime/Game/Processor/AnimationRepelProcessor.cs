using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class AnimationRepelProcessor : SimpleGameEntityProcessor<AnimationRepelEventComponent, AnimationDamageEventComponent, AttackComponent, VelocityComponent>
    {
        public AnimationRepelProcessor() : base()
        {
            Order = ProcessorOrder.Repel;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var repelEventComp = kvp.Value.Component1;
                var damageEventComp = kvp.Value.Component2;
                if(damageEventComp.IsDamageSuccess )
                {
                    var attackComp = kvp.Value.Component3;
                    var velocityComp = kvp.Value.Component4;
                    var dir = attackComp.AttackDir;
                    var uidList = damageEventComp.TargetUIDList;
                    foreach (var uid in uidList)
                    {
                        if(sceneSystem.SceneInstance.TryGetEntity(uid, out var target))
                        {
                            cmd.AddEntityComponent<RepulsedComponent>(target, comp =>
                            {
                                comp.StartRepulsed(repelEventComp.RepelSpeed, repelEventComp.RepelDistance, dir);
                            });
                        }
                    }
                    
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
