

using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class AttackStateProcessor : StateMachineProcessor<AttackComponent>
    {
        public AttackStateProcessor() : base(true)
        {
            
        }
        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var data = kvp.Value;
                StateFlag flag = StateFlag.None;
                int level = -1;
                int subIndex = 0;
                var attackComp = data.Component1;
                var stateMachineComp = data.Component2;
                var isPlayAttack = attackComp.IsPlayAttack;
                if(isPlayAttack)
                {
                    flag = StateFlag.Attack;
                    level = GameConstant.AttackStateLevel;
                    var speed = attackComp.AttackSpeed.GetFinalValue(time.FrameCount);
                    subIndex = attackComp.Data.AttackSubIndex;
                    var duration = attackComp.Data.AttackDuration;
                    var key = attackComp.Data.ClipEventControllerReference.Key;
                    stateMachineComp.TrySwitchState(flag, subIndex, level, stateDuration : duration, eventControllerKey : key, _loop : true, speed : speed);
                }
            }
        }
    }
}
