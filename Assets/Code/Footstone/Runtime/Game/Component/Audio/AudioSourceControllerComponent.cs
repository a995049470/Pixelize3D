using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class AudioSourceControllerComponent : EntityComponent
    {
        [HideInInspector][System.NonSerialized]
        public string CurrentLoopAudioKey = "";
        [HideInInspector]
        public float OneShotAudioRemainTime = 0;
       
        public ChildComponentReference<AudioSource> AudioSourceReference = new();

        public override void UpdateReference()
        {
            base.UpdateReference();
            AudioSourceReference.Root = this.transform;
        }

    #if UNITY_EDITOR
        private void OnValidate() {
            UpdateReference();
        }
    #endif
    }


}