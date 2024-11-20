using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    //子弹伤害对象
    public class BulletAttackProcessor : SimpleGameEntityProcessor<BulletComponent>
    {
        public BulletAttackProcessor() : base()
        {
            Order = ProcessorOrder.Attack;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var bulletComp = kvp.Value.Component1;
                if(!bulletComp.IsDead)
                {
                    var checkPoision = bulletComp.DamageCheckPoint.Component.position;
                    var radius = bulletComp.CheckRadius;
                    var layerMask = (int)bulletComp.DamageLayerMask;
                    int castCount = physicsSystem.SphereCastNonAlloc(checkPoision, radius, castColliders, layerMask);
                    if(castCount > 0)
                    {
                        //造成伤害
                        bulletComp.IsDead = true;
                        var target = castColliders[0].transform.GetComponent<TargetEntity>()?.Target;
                        var hurt = target?.Get<HurtComponent>();
                        var pos = bulletComp.Entity.Transform.Position;
                        if (hurt != null && hurt.IsReceiveHurt)
                        {
                            var dmgVal = bulletComp.Damage;
                            var dmgFlag = bulletComp.DamageFlag;
                            hurt.DamageReceiver.ReceiveDamage(dmgFlag, dmgVal);
                            var hurtDir = checkPoision - target.Transform.Position;
                            hurtDir.Normalize();
                            hurt.HitDirs.Add(hurtDir);       
                            var bufferReference = bulletComp.BufferEntityReference;
                            if(bufferReference.IsVaild())
                            {
                                cmd.InstantiateEntity(bufferReference.Key, bufferReference.Flag, Vector3.zero, 0, entity =>
                                {
                                    entity.GetOrCreate<BufferLabelComponent>().Target = target;
                                });
                            }     
                        }
                        var hitEffectReference = bulletComp.HitEffectReference;
                        var effectPos = pos + bulletComp.FlyDir * bulletComp.CheckRadius;
                        if (hitEffectReference.IsVaild())
                        {
                            cmd.InstantiateEntity(hitEffectReference.Key, hitEffectReference.Flag, effectPos, 0);
                        }

                    }
                }

            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
