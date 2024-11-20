using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class AnimationForceSwitchPoseEventProcessor : SimpleGameEntityProcessor<AnimationForceSwitchPoseEventComponent, StateMachineComponent>
    {
        public AnimationForceSwitchPoseEventProcessor() : base()
        {
            Order = ProcessorOrder.PoseUpdate;
        }
        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var eventComp = kvp.Value.Component1;
                var machineComp = kvp.Value.Component2;
                machineComp.TrySwitchState(eventComp.TargetState, eventComp.SubIndex, GameConstant.MaxStateLevel);
            }
        }

    }
}
