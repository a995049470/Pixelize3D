using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class AnimationOneShot3DAudioPlayProcessor : SimpleGameEntityProcessor<AnimationOneShot3DAudioPlayComponent, AudioPlayerAgentComponent>
    {
        public AnimationOneShot3DAudioPlayProcessor() : base()
        {
            Order = ProcessorOrder.AnimationAuidoPlay;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var animationAudioComp = kvp.Value.Component1;
                var agentComp = kvp.Value.Component2;
                var audioKey = animationAudioComp.AudioKey;
                agentComp.PlayOneShot3D(audioKey);      
            }
        }
    }
}
