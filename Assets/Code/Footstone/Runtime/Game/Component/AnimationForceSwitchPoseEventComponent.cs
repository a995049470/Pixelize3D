using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    [DefaultEntityComponentProcessor(typeof(AnimationForceSwitchPoseEventProcessor))]
    public class AnimationForceSwitchPoseEventComponent : EntityComponent, IAnimationClipEvent
    {
        public StateFlag TargetState;
        public int SubIndex;
    }
    
}
