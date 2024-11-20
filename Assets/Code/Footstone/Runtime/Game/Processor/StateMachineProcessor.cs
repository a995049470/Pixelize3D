using Lost.Runtime.Footstone.Core;
namespace Lost.Runtime.Footstone.Game
{

    public class StateMachineProcessor<T> : SimpleGameEntityProcessor<T, StateMachineComponent> where T : EntityComponent
    {
        public StateMachineProcessor(bool isSwitchState) : base()
        {
            Order = isSwitchState ? ProcessorOrder.StateSwitch : ProcessorOrder.PoseUpdate;
        }
    }
}
