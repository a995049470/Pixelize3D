using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    [DefaultEntityComponentProcessor(typeof(AnimationOneShot3DAudioPlayProcessor))]
    public class AnimationOneShot3DAudioPlayComponent : EntityComponent, IAnimationClipEvent
    {
        public string AudioKey;
    }
    
}
