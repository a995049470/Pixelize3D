using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    [System.Serializable]
    public class AnimationClipEventData 
    {
        //初始和开头部分可能因为Loop的关系被错误调用
        [Range(0.00f, 1.0f)]
        public float NormalizedTime;
        public AnimationClipEventEntityAssetReference EventEntityReference;
    }
}