

using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class WaterStateProcessor : StateMachineProcessor<WaterComponent>
    {
        public WaterStateProcessor() : base(true)
        {
        }
        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var waterComp = kvp.Value.Component1;
                var stateMachineComp = kvp.Value.Component2;
                var isPlayWater = waterComp.Timer > 0 && waterComp.IsWatering;
                if(isPlayWater)
                {
                    stateMachineComp.TrySwitchState(StateFlag.Watering, 0, GameConstant.WaterStateLevel, stateDuration : waterComp.Duration);
                }
            }
        }
    }
}
