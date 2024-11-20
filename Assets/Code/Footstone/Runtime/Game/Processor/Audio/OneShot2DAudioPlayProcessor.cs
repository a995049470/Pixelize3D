using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    public class OneShot2DAudioPlayProcessor : SimpleGameEntityProcessor<OneShot2DAudioComponent, AudioSourceControllerComponent>
    {
        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var oneShotComp = kvp.Value.Component1;
                var controlComp = kvp.Value.Component2;

                var oneShotAudioKeys = oneShotComp.OneShotAudioKeys;
                if(oneShotAudioKeys.Count > 0)
                {
                    var audioSource = controlComp.AudioSourceReference.Component;
                    foreach (var key in oneShotAudioKeys)
                    {
                        var clip = resPoolManager.LoadAudioClipWithKey(key, out var volume);
                        audioSource.PlayOneShot(clip, volume);
                    }
                    oneShotAudioKeys.Clear();
                }
            }
        }
    }
}
