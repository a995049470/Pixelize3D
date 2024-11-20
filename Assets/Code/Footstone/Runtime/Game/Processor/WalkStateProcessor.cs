

using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class WalkStateProcessor : SimpleGameEntityProcessor<VelocityComponent, WalkerComponent, StateMachineComponent>
    {
        public WalkStateProcessor() : base()
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
                string key = null;
                if(velocityComp.IsIdling)
                {
                    flag = StateFlag.Idle;
                    level = GameConstant.IdelStateLevel;
                    subIndex = velocityComp.IdleSubIndex;
                }
                else if(velocityComp.IsMoving)
                {
                    flag = StateFlag.Walk;
                    level = GameConstant.WalkStateLevel;
                    aniSpeed = velocityComp.Speed.GetFinalValue(time.FrameCount) / velocityComp.Speed.BaseVale;
                    var walkComp = kvp.Value.Component2;
                    key = walkComp.WalkClipEventControllerReference.Key;
                }
                if(level >= 0)
                {
                    stateMachineComp.TrySwitchState(flag, subIndex, level, speed : aniSpeed, eventControllerKey : key);
                }
            }
        }
    }
}
