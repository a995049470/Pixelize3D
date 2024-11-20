using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    [System.Serializable]
    public class AnimationClipEventEntityAssetReference : AssetReference<GameObject>
    {
        public override ResFlag Flag => ResFlag.Entity_AnimationClipEvent;
    }
    
}
