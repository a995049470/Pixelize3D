using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    [System.Serializable]
    public class AudioSourceEntityAssetReference : AssetReference<GameObject>
    {
        public override ResFlag Flag => ResFlag.Entity_AudioSource;
    }
    
}
