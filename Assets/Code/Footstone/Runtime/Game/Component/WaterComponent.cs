using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    

    [DefaultEntityComponentProcessor(typeof(PlayerWaterControllerProcessor))]
    [DefaultEntityComponentProcessor(typeof(WaterProcessor))]
    [DefaultEntityComponentProcessor(typeof(WaterUpdateActionMaskProcessor))]
    [DefaultEntityComponentProcessor(typeof(WaterStateProcessor))]
    [DefaultEntityComponentProcessor(typeof(WaterModelRenderProcesssor))]
    public class WaterComponent : EntityComponent, IComponentSaveOrLoadCallback
    {
        //正在浇水
        public bool IsWatering;
        public string ModelKey = "watering_can_right";
        public float Duration = 0.75f;
        [Range(0, 1)]
        public float DigNormalizedTime = 0.60f;
        [Range(0, 1)]
        public float EffectNormalizedTime = 0.75f;
        public string ParticleName;
        [HideInInspector]
        public float Timer = 0;
        [HideInInspector]
        public Vector3 WaterDir;      
        public PowerReceicver Receicver = new();

        public bool IsShowModel()
        {
            return IsWatering && Timer < 0.95f * Duration && Timer > 0.1f * Duration;
        }

        public void StartWater(Vector3 dir)
        {
            IsWatering = true;
            Timer = 0;
            WaterDir = dir;
        }

        public void WaterUpdate(float deltaTime, out bool isWater, out string effectKey)
        {
            isWater = false;
            effectKey = null;
            float per = Timer / Duration;
            Timer += deltaTime;
            float cur = Timer / Duration;
            if(per <= DigNormalizedTime && cur > DigNormalizedTime)
                isWater = true;
            if(per <= EffectNormalizedTime && cur > EffectNormalizedTime)
                effectKey = ParticleName;
            IsWatering = cur < 1.0f;
        }

        public void ForceCompleteWater()
        {
            IsWatering = false;
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



