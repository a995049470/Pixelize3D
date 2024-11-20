using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    [DefaultEntityComponentProcessor(typeof(DigProcessor))]
    [DefaultEntityComponentProcessor(typeof(PlayerDigControllerProcessor))]
    [DefaultEntityComponentProcessor(typeof(DigUpdateActionMaskProcessor))]
    [DefaultEntityComponentProcessor(typeof(DigStateProcessor))]
    [DefaultEntityComponentProcessor(typeof(DigModelRenderProcesssor))]
    public class DigComponent : EntityComponent, IComponentSaveOrLoadCallback
    {
        //正在挖
        [HideInInspector]
        public bool IsDiging;
        public string ModelKey = "hoe_left";
        public float Duration = 0.5f;
        [Range(0, 1)]
        public float DigNormalizedTime = 0.50f;
        [Range(0, 1)]
        public float EffectNormalizedTime = 0.45f;
        public string ParticleName;
        [HideInInspector]
        public float Timer = 0;
        [HideInInspector]
        public Vector3 DigDir;      
        public PowerReceicver Receicver = new();
        public bool IsShowModel()
        {
            return IsDiging && Timer < 0.9f * Duration && Timer > 0.1f * Duration;
        }

        public void StartDig(Vector3 dir)
        {
            IsDiging = true;
            Timer = 0;
            DigDir = dir;
        }

        public void DigUpdate(float deltaTime, out bool isDig, out string effectKey)
        {
            isDig = false;
            effectKey = null;
            float per = Timer / Duration;
            Timer += deltaTime;
            float cur = Timer / Duration;
            if(per <= DigNormalizedTime && cur > DigNormalizedTime)
                isDig = true;
            if(per <= EffectNormalizedTime && cur > EffectNormalizedTime)
                effectKey = ParticleName;
            IsDiging = cur < 1.0f;
        }

        public void ForceCompleteDig()
        {
            IsDiging = false;
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



