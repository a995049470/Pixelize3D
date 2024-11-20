using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    [DefaultEntityComponentProcessor(typeof(AnimationRepelProcessor))]
    public class AnimationRepelEventComponent : EntityComponent, IAnimationClipEvent
    {
        //击退距离
        public float RepelDistance = 1;
        public float RepelSpeed = 10;
        
    }
    
}
