

using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class SowStateProcessor : StateMachineProcessor<SowComponent>
    {
        public SowStateProcessor() : base(true)
        {
        }
        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var sowComp = kvp.Value.Component1;
                var stateMachineComp = kvp.Value.Component2;
                var isPlaySow = sowComp.Timer > 0 && sowComp.IsSowing;
                if(isPlaySow)
                {
                    stateMachineComp.TrySwitchState(StateFlag.Sowing, 0, GameConstant.SowStateLevel, stateDuration : sowComp.Duration);
                }
            }
        }
    }
}
