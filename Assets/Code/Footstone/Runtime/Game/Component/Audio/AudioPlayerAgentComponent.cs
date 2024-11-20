using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    [DefaultEntityComponentProcessor(typeof(AudioPlayerAgentProcessor))]
    public class AudioPlayerAgentComponent : EntityComponent
    {
        public List<string> OneShot3DAudioKeys = new();
        public string LoopAudioKey = "";
        public AudioSourceEntityAssetReference AudioSourceEntityReference = new();
        [UnityEngine.HideInInspector]
        public ulong AudioSourceEntityUID = 0;
        [UnityEngine.HideInInspector]
        public bool IsDirty = false;


        protected override void OnEnableRuntime()
        {
            base.OnEnableRuntime();
            IsDirty = !string.IsNullOrEmpty(LoopAudioKey) || OneShot3DAudioKeys.Count > 0;
        }

        public void PlayOneShot3D(string key)
        {
            if(!string.IsNullOrEmpty(key))
            {
                OneShot3DAudioKeys.Add(key);
                IsDirty = true;
            }
        }

        public void SwitchLoopAudio(string key)
        {
            if(LoopAudioKey != key)
            {
                LoopAudioKey = key;
                IsDirty = true;
            }
        }

    }
}