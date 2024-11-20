using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class AnimationDamageAudioProcessor : SimpleGameEntityProcessor<AnimationDamageAudioEventComponent, AnimationDamageEventComponent, AudioPlayerAgentComponent>
    {
        public AnimationDamageAudioProcessor() : base()
        {
            Order = ProcessorOrder.AttackAudio;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var auidoComp = kvp.Value.Component1;
                var damageComp = kvp.Value.Component2;
                var agentComp = kvp.Value.Component3;

                var audioKey = damageComp.IsDamageSuccess ? auidoComp.SuccessAudioKey : auidoComp.FailAudioKey;
                agentComp.PlayOneShot3D(audioKey);
            }
        }
    }
}
