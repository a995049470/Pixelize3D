using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    [DefaultEntityComponentProcessor(typeof(PlayerPickaxeControllerProcessor))]
    [DefaultEntityComponentProcessor(typeof(PickaxeProcessor))]
    [DefaultEntityComponentProcessor(typeof(PickaxeUpdateActionMaskProcessor))]
    [DefaultEntityComponentProcessor(typeof(PickaxeStateProcessor))]
    [DefaultEntityComponentProcessor(typeof(PickaxeModelRenderProcesssor))]
    public class PickaxeComponent : EntityComponent, IComponentSaveOrLoadCallback
    {
        //正在挖
        [HideInInspector]
        public bool IsPickaxing;
        public string ModelKey = "pickaxe_left";
        public float Duration = 0.5f;
        [Range(0, 1)]
        public float DigNormalizedTime = 0.50f;
        [Range(0, 1)]
        public float EffectNormalizedTime = 0.45f;
        public string ParticleName;
        [HideInInspector]
        public float Timer = 0;
        [HideInInspector]
        public Vector3 PickaxeDir;      
        public PowerReceicver Receicver = new();
        public bool IsShowModel()
        {
            return IsPickaxing && Timer < 0.9f * Duration && Timer > 0.1f * Duration;
        }

        public void StartPickaxe(Vector3 dir)
        {
            IsPickaxing = true;
            Timer = 0;
            PickaxeDir = dir;
        }

        public void PickaxeUpdate(float deltaTime, out bool isDig, out string effectKey)
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
            IsPickaxing = cur < 1.0f;
        }

        public void ForceCompletePickaxe()
        {
            IsPickaxing = false;
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



