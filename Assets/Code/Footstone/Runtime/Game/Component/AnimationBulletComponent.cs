using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    [DefaultEntityComponentProcessor(typeof(AnimationBulletProcessor))]
    public class AnimationBulletComponent : EntityComponent, IAnimationClipEvent
    {
        public BulletEntityAssetReference BulletRefence = new();
    }
    
}
