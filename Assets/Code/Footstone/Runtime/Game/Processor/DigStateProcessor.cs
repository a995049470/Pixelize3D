

using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{


    public class DigStateProcessor : StateMachineProcessor<DigComponent>
    {
        public DigStateProcessor() : base(true)
        {
        }
        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var digComp = kvp.Value.Component1;
                var stateMachineComp = kvp.Value.Component2;
                var isPlayDig = digComp.Timer > 0 && digComp.IsDiging;
                if(isPlayDig)
                {
                    stateMachineComp.TrySwitchState(StateFlag.Hoeing, 0, GameConstant.DigStateLevel, stateDuration : digComp.Duration);
                }
            }
        }
    }
}
