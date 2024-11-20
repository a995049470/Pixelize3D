using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    [DefaultEntityComponentProcessor(typeof(AnimationPositionResetEventProcessor))]
    public class AnimationPositionResetEventComponent : EntityComponent, IAnimationClipEvent
    {
        
    }
    
}
