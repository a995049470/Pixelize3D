using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class RemoveVelocityComponentOnDeadProcessor : SimpleGameEntityProcessor<DeadComponent, VelocityComponent>
    {
        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var deadComp = kvp.Value.Component1;
                var velocityComp = kvp.Value.Component2;
                if(deadComp.IsDeading())
                {
                    cmd.RemoveEntityComponent(velocityComp);
                }                   
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
