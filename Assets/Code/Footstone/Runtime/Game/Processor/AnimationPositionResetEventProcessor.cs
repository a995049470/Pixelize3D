using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class AnimationPositionResetEventProcessor : SimpleGameEntityProcessor<AnimationPositionResetEventComponent, ModelRootComponent, VelocityComponent, PoseComponent>
    {
        public AnimationPositionResetEventProcessor() : base()
        {
            Order = ProcessorOrder.PositionReset;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var modelRootComp = kvp.Value.Component2;
                var velocityComp = kvp.Value.Component3;
                var poseComp = kvp.Value.Component4;
                velocityComp.BackToTargetPos();
                var modelRoot = modelRootComp.ModelRootReference.Component;
                modelRoot.localPosition = Vector3.zero;
            }
        }
    }
}
