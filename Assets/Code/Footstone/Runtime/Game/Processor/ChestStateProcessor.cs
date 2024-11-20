using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class ChestStateProcessor : SimpleGameEntityProcessor<ChestComponent, StateMachineComponent>
    {
        public ChestStateProcessor() : base()
        {
            Order = ProcessorOrder.StateSwitch;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var stateComp = kvp.Value.Component2;
                var chestComp = kvp.Value.Component1;
                if(chestComp.IsOpen)
                {
                    stateComp.TrySwitchState(StateFlag.Open, 0, GameConstant.MaxStateLevel, _loop : false);
                }
                
            }
        }
    }

}
