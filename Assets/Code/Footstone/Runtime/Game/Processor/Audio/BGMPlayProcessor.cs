using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
   

    public class BGMPlayProcessor : SimpleGameEntityProcessor<BGMComponent, AudioSourceControllerComponent>
    {
        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var bgmComp = kvp.Value.Component1;
                var controlComp = kvp.Value.Component2;
                var currentBGMAudioKey = bgmComp.GetCurrentBGM();
                if (controlComp.CurrentLoopAudioKey != currentBGMAudioKey)
                {
                    var audioSource = controlComp.AudioSourceReference.Component;
                    audioSource.Stop();
                    controlComp.CurrentLoopAudioKey = currentBGMAudioKey;
                    if (!string.IsNullOrEmpty(currentBGMAudioKey))
                    {
                        var clip = resPoolManager.LoadResWithKey<AudioClip>(currentBGMAudioKey, ResFlag.Audio);
                        audioSource.clip = clip;
                        audioSource.Play();
                    }
                }
            }
        }
    }
}
