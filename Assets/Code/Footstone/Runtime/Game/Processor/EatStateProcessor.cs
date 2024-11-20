using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class EatStateProcessor : SimpleGameEntityProcessor<EatComponent, StateMachineComponent>
    {
        public EatStateProcessor() : base()
        {
            Order = ProcessorOrder.StateSwitch;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var eatComp = kvp.Value.Component1;
                var stateMachineComp = kvp.Value.Component2;  
                if(eatComp.IsEatting)
                    stateMachineComp.TrySwitchState(StateFlag.Eat, 0, GameConstant.EatStateLevel, 0, 1, true);
            }
        }
    }
}
