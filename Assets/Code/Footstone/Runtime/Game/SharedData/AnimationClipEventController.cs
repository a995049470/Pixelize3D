using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    //动画片段事件控制器
    [CreateAssetMenu(menuName = "Lost/Attack/AnimationClipEventController")]
    public class AnimationClipEventController : ScriptableObject
    {
        [SerializeField]
        [Header("需要从小到大排列")]
        private AnimationClipEventData[] eventDatas;

        //触发时间点包括current 不包括next, 所有事件触发的事件点的归一化时间应小于1
        public void TriggerEvent(CommandBuffer cmd, Entity target, ref float currentNormalizedTime, float nextNormalizedTime, ref int ptr)
        {
            if(eventDatas.Length == 0) return;
            if(nextNormalizedTime < currentNormalizedTime)
            {
                nextNormalizedTime = 1.0001f;
            }
            ptr = Mathf.Clamp(ptr, 0, eventDatas.Length - 1);
            while (eventDatas[ptr].NormalizedTime > currentNormalizedTime)
            {
                if(ptr == 0) break;
                ptr --;
            }
            for (;ptr < eventDatas.Length; ptr++)
            {
                var eventData = eventDatas[ptr];
                var normalizedTime = eventData.NormalizedTime;
                if(normalizedTime < nextNormalizedTime)
                {
                    if(normalizedTime >= currentNormalizedTime)
                    {
                        //触发事件
                        cmd.InstantiateEntity(eventData.EventEntityReference.Key, eventData.EventEntityReference.Flag, Vector3.zero, 0, entity =>
                        {   
                            entity.GetOrCreate<AnimationClipEventTriggerLabelComponent>().Target = target;
                        });
                        
                    }
                }
                else
                {
                    ptr = Mathf.Max(0, ptr - 1);
                    break;
                }
            }
            ptr %= eventDatas.Length;
            currentNormalizedTime = nextNormalizedTime >= 1.0f ? 0.0f : nextNormalizedTime;
        }

    }
}