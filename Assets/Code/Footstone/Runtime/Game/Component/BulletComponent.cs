using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    [DefaultEntityComponentProcessor(typeof(BulletFlyProcessor))]
    [DefaultEntityComponentProcessor(typeof(BulletAttackProcessor))]
    [DefaultEntityComponentProcessor(typeof(AutoRecycleBulletComponent))]
    public class BulletComponent : EntityComponent
    {
        [HideInInspector]
        public Vector3 StartPosition;
        [HideInInspector]
        public float FlyDistance = 1;
        public float Speed = 1;
        public ParticleEntityAssetReference HitEffectReference = new();
        [HideInInspector]
        public Vector3 FlyDir = Vector3.right;
        //伤判点
        public ChildComponentReference<Transform> DamageCheckPoint;
        //伤判半径
        public float CheckRadius = 0.2f;
        public PowerEntityAssetReference BufferEntityReference = new();
        [HideInInspector]
        public float Damage = 0.0f;
        [HideInInspector]
        public DamageFlag DamageFlag = DamageFlag.Physical;
        [HideInInspector]
        public LayerMask DamageLayerMask = LayerMask.Barrier;
        //TODO:可穿透 可范围伤害的子弹
        [HideInInspector]
        public bool IsDead = false;

        protected override void OnEnableRuntime()
        {
            base.OnEnableRuntime();
            IsDead = false;
        }

        public override void UpdateReference()
        {
            base.UpdateReference();
            DamageCheckPoint.Root = this.transform;
        }
    }
    
}
