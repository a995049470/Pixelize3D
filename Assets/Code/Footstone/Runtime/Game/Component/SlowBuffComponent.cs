using System.Collections.Generic;
using Lost.Runtime.Footstone.Collection;
using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    [System.Serializable]
    public class SlowBuff
    {
        public float SlowPercentage = 0.3f;
        public float SlowTime = 0.0f;
        public float SlowDuration = 0.0f;
    }

    //减速buff
    [DefaultEntityComponentProcessor(typeof(SlowBuffEffectProcessor))]
    public class SlowBuffComponent : EntityComponent, IComponentSaveOrLoadCallback
    {
        //并存多种减速buff
        [SerializeField][HideInInspector]
        private SerializableDictionary<int, SlowBuff> slowBuffDic = new();
        private List<int> delList = new();
        private int lastUpdateFrame = -1;
        private float lastSlowPercentage = 0;

        public void AddSolwBuff(int id, float percentage, float duration)
        {
            slowBuffDic[id] = new SlowBuff()
            {
                SlowPercentage = percentage,
                SlowDuration = duration,
                SlowTime = 0
            };
        }

        public void OnAfterLoad()
        {
            slowBuffDic.OnAfterLoad();
        }

        public void OnBeforeSave()
        {
            slowBuffDic.OnBeforeSave();
        }

        public bool TryCompleteSlow(float deltaTime, int frame, out float slowPercentage)
        {
            if(frame == lastUpdateFrame)
            {
                slowPercentage = lastSlowPercentage;
            }
            else
            {
                slowPercentage = 0;
                if(slowBuffDic.Count > 0)
                {
                    foreach (var kvp in slowBuffDic)
                    {
                        var buff = kvp.Value;
                        buff.SlowTime += deltaTime;
                        if(buff.SlowTime < buff.SlowDuration)
                        {
                            slowPercentage += buff.SlowPercentage;
                        }
                        else
                        {
                            delList.Add(kvp.Key);
                        }
                    }
                    foreach (var id in delList)
                    {
                        slowBuffDic.Remove(id);
                    }
                    delList.Clear();
                }
                lastUpdateFrame = frame;
                lastSlowPercentage = slowPercentage;
            }
            bool isComplete = slowBuffDic.Count == 0;
            return isComplete;
        }
    }
    
}
