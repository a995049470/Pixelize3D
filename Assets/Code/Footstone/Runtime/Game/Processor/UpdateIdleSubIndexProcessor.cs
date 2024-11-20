

using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class UpdateIdleSubIndexProcessor : SimpleGameEntityProcessor<VelocityComponent, AttackComponent>
    {
        public UpdateIdleSubIndexProcessor() : base()
        {
            Order = ProcessorOrder.UpdateIdleSubIndex;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var velocityComp = kvp.Value.Component1;
                var attackComp = kvp.Value.Component2;
                velocityComp.IdleSubIndex = attackComp.Data.IdleSubIndex;
            }
        }
    }
}
