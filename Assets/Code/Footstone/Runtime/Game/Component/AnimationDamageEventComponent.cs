using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    [DefaultEntityComponentProcessor(typeof(AnimationDamageProcessor))]
    public class AnimationDamageEventComponent : EntityComponent, IAnimationClipEvent
    {
        //伤害系数
        public float DamageCoefficient = 1;
        [UnityEngine.HideInInspector]
        public bool IsDamageSuccess = false;
        [UnityEngine.HideInInspector]
        public List<ulong> TargetUIDList = new();
    }
    
}
