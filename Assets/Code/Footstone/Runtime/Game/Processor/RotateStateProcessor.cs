

using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class RotateStateProcessor : StateMachineProcessor<RotateComponent>
    {
        public RotateStateProcessor() : base(true)
        {
        }   

        public override void Update(GameTime time)
        {
            base.Update(time);
            foreach (var kvp in ComponentDatas)
            {
                StateFlag flag = StateFlag.None;
                int level = -1;
                var rotateComp = kvp.Value.Component1;
                var stateMachineComp = kvp.Value.Component2;
                var subIndex = 0;
                if(rotateComp.IsRotating())
                {
                    flag = StateFlag.Walk;
                    level = GameConstant.IdelStateLevel;
                }
                if(level >= 0)
                {
                    stateMachineComp.TrySwitchState(flag, subIndex, level);
                }
            }
        }
    }
}
