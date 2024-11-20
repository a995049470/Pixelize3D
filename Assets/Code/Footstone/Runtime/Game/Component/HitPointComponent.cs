using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    /// <summary>
    /// 血量组件
    /// </summary>
    [DisallowMultipleComponent]
    [DefaultEntityComponentProcessor(typeof(DeadStateProcessor))]
    [DefaultEntityComponentProcessor(typeof(PlayerHitPointProcessor))]
    public class HitPointComponent : EntityComponent
    {
        public float MaxHp = 1;

        [HideInInspector]
        public float currentHp;
        public float CurrentHp { get => currentHp; private set => currentHp = System.Math.Clamp(value, 0, MaxHp); }
        public float HPPercent { get => Mathf.Clamp01(currentHp / MaxHp); }
        
        public bool IsDead { get => currentHp <= 0; }
        
        //正在死亡 会持续一帧 
        [HideInInspector]
        public bool IsDeadNow;

        protected override void OnEnableRuntime()
        {
            CurrentHp = MaxHp;
        }
        
        public void ReceiveDamage(float dmg)
        {
            if(!IsDead)
            {
                CurrentHp -= dmg;
                IsDeadNow = IsDead;
            }
        }

        public void ReceiveHeal(float heal)
        {
            if(!IsDead)
            {
                CurrentHp += heal;
            }
        }

        public float GetCurrentHitPointPercent()
        {
            return System.Math.Clamp((float)CurrentHp / MaxHp, 0, 1);
        }

    }

}
