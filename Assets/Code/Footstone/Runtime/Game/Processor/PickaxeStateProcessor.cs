

using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class PickaxeStateProcessor : StateMachineProcessor<PickaxeComponent>
    {
        public PickaxeStateProcessor() : base(true)
        {
        }
        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var pickaxeComp = kvp.Value.Component1;
                var stateMachineComp = kvp.Value.Component2;
                var isPlaying = pickaxeComp.Timer > 0 && pickaxeComp.IsPickaxing;
                if(isPlaying)
                {
                    stateMachineComp.TrySwitchState(StateFlag.Pickaxing, 0, GameConstant.PickaxeStateLevel, stateDuration : pickaxeComp.Duration);
                }
            }
        }
    }
}
