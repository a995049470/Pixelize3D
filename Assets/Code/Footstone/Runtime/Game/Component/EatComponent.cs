using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    [DefaultEntityComponentProcessor(typeof(EatProcessor))]
    [DefaultEntityComponentProcessor(typeof(EatStateProcessor))]
    [DefaultEntityComponentProcessor(typeof(PlayerEatControllerProcessor))]
    public class EatComponent : EntityComponent, IComponentSaveOrLoadCallback
    {
        public float Duration = 2.0f;
        public float TotalRecover = 0.0f;
        [HideInInspector]
        public Vector3 EatDir;
        [HideInInspector]
        public float EatTimer = 0;
        [HideInInspector]
        public bool IsEatting = false;
        [HideInInspector]
        public PowerReceicver Receicver = new();
        public bool IsPlayEat { get => IsEatting && EatTimer > 0; }

        public void StartEatting(Vector3 dir)
        {
            IsEatting = true;
            EatTimer = 0;
            EatDir = dir;
        }

        public bool TryEat(float deltaTime)
        {
            if(IsEatting)
            {
                EatTimer += deltaTime;
                IsEatting = EatTimer < Duration;
            }
            return IsEatting;
        }

        public void ForceCompleteEat()
        {
            IsEatting = false;
        }

        public void OnAfterLoad()
        {
            Receicver.OnAfterLoad();
        }

        public void OnBeforeSave()
        {
            Receicver.OnBeforeSave();
        }
    }

}



