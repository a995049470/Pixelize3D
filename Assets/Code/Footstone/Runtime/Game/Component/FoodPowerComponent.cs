using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    [AllowMultipleComponents]
    [DefaultEntityComponentProcessor(typeof(FoodPowerOnEquipProcessor))]
    [DefaultEntityComponentProcessor(typeof(FoodPowerOnPullProcessor))]
    public class FoodPowerComponent : EntityComponent, ITakeEffectOnEquipOrPullFrame
    {
        public float EatDuration = 1.0f;
        public float TotalRecover = 20;

        public bool IsEquipping { get; private set; }
        [HideInInspector]
        public ulong PowerUID;
        public void SetTimePoint(bool isEquipping, ulong uid)
        {
            IsEquipping = isEquipping;
            PowerUID = uid;
        }
    }
}



