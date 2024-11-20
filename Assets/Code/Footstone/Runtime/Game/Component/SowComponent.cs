using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    /// <summary>
    /// 播种组件（所有消耗和条件已经计算完毕，只需要执行播种逻辑即可）
    /// </summary>
    [DefaultEntityComponentProcessor(typeof(SowProcessor))]
    [DefaultEntityComponentProcessor(typeof(SowStateProcessor))]
    [DefaultEntityComponentProcessor(typeof(SowUpdateActionMaskProcessor))]
    [DefaultEntityComponentProcessor(typeof(PlayerSowControllerProcessor))]
    public class SowComponent : EntityComponent, IComponentSaveOrLoadCallback
    {
        public PowerReceicver Receicver = new();
        //种子名字
        public string Plant;
        //播种的数量
        public int Count;
        //目标地块的UID
        public ulong CroplandUID;

        public bool IsSowing;
        public float Duration = 0.5f;
        [Range(0, 1)]
        public float DigNormalizedTime = 0.50f;
        [Range(0, 1)]
        public float EffectNormalizedTime = 0.45f;
        public string ParticleName;
        [HideInInspector]
        public float Timer = 0;
        [HideInInspector]
        public Vector3 SowDir;  
        [HideInInspector]
        public int BagGridIndex;
        [HideInInspector]
        public ulong ItemUID;


        public void StartSow(string plant, ulong croplandUID, Vector3 dir, int gridIndex, ulong itemUID)
        {
            IsSowing = true;
            Plant = plant;
            Count = 1;
            CroplandUID = croplandUID;
            SowDir = dir;
            Timer = 0;
            BagGridIndex = gridIndex;
            ItemUID = itemUID;
        }

        public void SowUpdate(float deltaTime, out bool invokeSow, out string effectKey)
        {
            invokeSow = false;
            effectKey = null;
            float per = Timer / Duration;
            Timer += deltaTime;
            float cur = Timer / Duration;
            if(per <= DigNormalizedTime && cur > DigNormalizedTime)
                invokeSow = true;
            if(per <= EffectNormalizedTime && cur > EffectNormalizedTime)
                effectKey = ParticleName;
            IsSowing = cur < 1.0f;
        }

        public void ForceCompleteSow()
        {
            IsSowing = false;
            Count = 0;
            Timer = 0;
        }

        public void OnBeforeSave()
        {
            Receicver.OnBeforeSave();
        }

        public void OnAfterLoad()
        {
            Receicver.OnAfterLoad();
        }
    }

}



