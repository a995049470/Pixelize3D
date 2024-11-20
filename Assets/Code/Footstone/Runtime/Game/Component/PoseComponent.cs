using Lost.Runtime.Footstone.Core;


using System;
using System.Collections.Generic;
using UnityEngine;


namespace Lost.Runtime.Footstone.Game
{
    [Serializable]
    public class ClipInfo
    {
        public string Name;
        public StateFlag Flag;
        public float Duration;
    }

    //TODO: 可能存在的 多段攻击动画 和 骨骼遮罩
    //[RequireComponent(typeof(Animator))]
    //[DefaultEntityComponentProcessor(typeof(PoseProcessor))]
    public class PoseComponent : UnityComponent<Animator>
    {
        [SerializeField]
        private List<ClipInfo> cacheClipInfos = new List<ClipInfo>();
        private Dictionary<string, float> clipDurationDic;
        //public float PlaySpeed { get; set; }
        private const int defalutLayer = 0;

        protected override void Initialize(IServiceRegistry registry)
        {
            base.Initialize(registry);
            clipDurationDic = new Dictionary<string, float>();
            foreach (var clipInfo in cacheClipInfos)
            {
                clipDurationDic[clipInfo.Name] = clipInfo.Duration;
            }
        }
    #if UNITY_EDITOR
        private void OnValidate() {
            if(clipDurationDic != null)
            {
                foreach (var clipInfo in cacheClipInfos)
                {
                    clipDurationDic[clipInfo.Name] = clipInfo.Duration;
                }
            }
        }
    #endif

        private float GetClipDuration(string stateName)
        {
            if (!clipDurationDic.TryGetValue(stateName, out var duration))
            {
                throw new Exception($"no animation:{stateName}");
            }
            return Mathf.Max(duration, 0.01f);
        }

        public float CalculateAnimationCurrentNormalizedTime(string name, float time, bool loop, float duration, float animationFPS, out float realDuration)
        {
            duration = duration > 0 ? duration : GetClipDuration(name);
            realDuration = duration;
            float currentFrame = Mathf.Round(time * animationFPS);
            float currentTime = 0;
            if(loop)
            {
                float frameCount = duration * animationFPS;
                currentTime = (currentFrame - (int)(currentFrame / frameCount) * frameCount) / animationFPS;
            }
            else
                currentTime = currentFrame / animationFPS;
            return currentTime / duration;
        }

        public void Play(int nameHash, float normalizedTime, float duration, float transitionDuration)
        {
            
            float normalizedTransitionTime = Mathf.Clamp01(duration * normalizedTime / transitionDuration);
            Component.CrossFade(nameHash, transitionDuration, defalutLayer, normalizedTime, normalizedTransitionTime);

            
        }
    }
}
