using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class DeadStateProcessor : StateMachineProcessor<HitPointComponent>
    {
        public DeadStateProcessor() : base(true)
        {

        }
        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var hpComp = kvp.Value.Component1;
                var currentState = kvp.Value.Component2;
                if(hpComp.IsDead)
                    currentState.TrySwitchState(StateFlag.Death, 0, GameConstant.DeadStateLevel, -1, 1, false);
            }
        }
    }
}
