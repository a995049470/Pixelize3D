using LitJson;
using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    /// <summary>
    /// 每个行为尽可能小 只能完成一个目标
    /// </summary>
    
    public abstract class GOAPActionData : ScriptableObject
    {
        public int Cost = 0;
        /// <summary>
        /// 增加权重来使行为具有一点随机性？
        /// </summary>
        // [HideInInspector]
        // public int Weight = 0;
        [SerializeField]
        private GOAPSerializableStatusDictionary serializedPreconditions = new();
        //失败条件 满足任意一条就失败
        [SerializeField]
        private GOAPSerializableStatusDictionary serializedFailPreconditions = new();
        [SerializeField]
        private GOAPStatus effect = new();
        private bool isDitryPreconditions = true;
        private bool isDitryEffects = true;

        public GOAPStatusFlag EffectFlag { get => effect.Flag; } 
        public GOAPStatus Effect { get => effect; } 
        public GOAPStatusDictionary Preconditions { get => serializedPreconditions.Dictionary; }
        public GOAPStatusDictionary FailPreconditions { get => serializedFailPreconditions.Dictionary; }


    
        private void OnValidate() {
            serializedPreconditions.SetDirty();
        }

        /// <summary>
        /// 将目标状态从缺失状态中，将前置条件加入到未完成状态中
        /// </summary>
        /// <returns></returns>
        //public abstract bool TrySatisfyPreconditions(GOAPStatusDictionary worldStatuses, GOAPStatusDictionary missingStatuses);
        public abstract void EnableGOAPActionComponent(CommandBuffer cmd, Entity entity);
        public abstract void DisableGOAPActionComponent(CommandBuffer cmd, Entity entity);
        /// <summary>
        /// 能完成某个前置条件的为依赖项
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool IsDependOn(GOAPActionData data)
        {
            bool isDepend = false;
            isDepend = Preconditions.TryGetValue(data.effect.Flag, out var status) && status.Pass(data.effect);
            return isDepend;
        }   

        public bool IsFail(GOAPStatusDictionary worldStatus)
        {
            bool isFail = false;
            if(FailPreconditions.Count > 0)
            {
                foreach (var kvp in FailPreconditions)
                {
                    isFail |= worldStatus.TryGetValue(kvp.Key, out var status) && status.Pass(kvp.Value);
                    if(isFail) break;
                }
            }
            return isFail;
        }



    }

}



