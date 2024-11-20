using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    [DefaultEntityComponentProcessor(typeof(AnimationFinishAttackEventProcessor))]
    public class AnimationFinishAttackEventComponent : EntityComponent, IAnimationClipEvent
    {

    }
    
}
