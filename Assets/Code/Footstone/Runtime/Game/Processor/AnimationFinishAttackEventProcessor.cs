using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class AnimationFinishAttackEventProcessor : SimpleGameEntityProcessor<AnimationFinishAttackEventComponent, AttackComponent>
    {
        public AnimationFinishAttackEventProcessor() : base()
        {
            Order = ProcessorOrder.FinishAttack;
        }
        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var eventComp = kvp.Value.Component1;
                var attackComp = kvp.Value.Component2;
                attackComp.ForceCompleteAttack();
            }
        }
    }
}
