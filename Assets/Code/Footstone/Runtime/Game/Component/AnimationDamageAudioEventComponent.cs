using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    [DefaultEntityComponentProcessor(typeof(AnimationDamageAudioProcessor))]
    public class AnimationDamageAudioEventComponent : EntityComponent, IAnimationClipEvent
    {
        public string SuccessAudioKey = "";
        public string FailAudioKey = "";
    }
    
}
