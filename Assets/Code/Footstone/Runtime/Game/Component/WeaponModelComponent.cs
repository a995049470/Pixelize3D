using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    [DefaultEntityComponentProcessor(typeof(WeaponModelRenderProcessor))]
    public class WeaponModelComponent : EntityComponent
    {
        [HideInInspector]
        public string CurrentLeftHandWeaponKey;
        [HideInInspector]
        public string CurrentRightHandWeaponKey;
        [HideInInspector]
        public ulong LeftHandWeaponEntityUID;
        [HideInInspector]
        public ulong RightHandWeaponEntityUID;
        
        public string TargetLeftHandWeaponKey;
        public string TargetRightHandWeaponKey;
    }

}



