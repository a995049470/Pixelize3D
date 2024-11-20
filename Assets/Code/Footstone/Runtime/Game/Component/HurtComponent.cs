using Lost.Runtime.Footstone.Collection;
using Lost.Runtime.Footstone.Core;
using UnityEngine;


namespace Lost.Runtime.Footstone.Game
{


    /// <summary>
    /// 受伤组件（包括伤害和治疗组件）
    /// TODO:持续受伤和持续治疗组件
    /// </summary>
    [DisallowMultipleComponent]
    [DefaultEntityComponentProcessor(typeof(HurtProcessor))]
    [DefaultEntityComponentProcessor(typeof(HurtClearProcessor))]
    public class HurtComponent : EntityComponent
    {
        [HideInInspector]
        public DamageReceiver DamageReceiver = new();
        [HideInInspector]
        public float Heal;

        [System.NonSerialized]
        public bool IsReceiveHurt = true;
        [System.NonSerialized]
        public FastCollection<Vector3> HitDirs = new FastCollection<Vector3>(4);
        public bool IsReceiveDamage { get => DamageReceiver.Sum > 0; }
        public bool IsReceiveHeal { get => Heal > 0; }

        public void Clear()
        {
            HitDirs.Clear();
            DamageReceiver.Clear();
            Heal = 0;
        }
    }

}
