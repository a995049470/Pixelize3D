

using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class JumpStateProcessor : SimpleGameEntityProcessor<VelocityComponent, JumperComponent, StateMachineComponent>
    {
        public JumpStateProcessor() : base()
        {
            Order = ProcessorOrder.StateSwitch;
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
            foreach (var kvp in ComponentDatas)
            {
                StateFlag flag = StateFlag.None;
                int level = -1;
                var velocityComp = kvp.Value.Component1;
                var stateMachineComp = kvp.Value.Component3;
                var subIndex = 0;
                var aniSpeed = 1.0f;
                var duration = -1.0f;
                if(velocityComp.IsIdling)
                {
                    flag = StateFlag.Idle;
                    level = GameConstant.IdelStateLevel;
                    subIndex = velocityComp.IdleSubIndex;
                }
                else if(velocityComp.IsMoving)
                {
                    flag = StateFlag.Jump;
                    level = GameConstant.WalkStateLevel;
                    duration = 1.0f / velocityComp.Speed.GetFinalValue(time.FrameCount);
                }
                if(level >= 0)
                {
                    stateMachineComp.TrySwitchState(flag, subIndex, level, speed : aniSpeed, stateDuration : duration);
                }
            }
        }
    }
}
