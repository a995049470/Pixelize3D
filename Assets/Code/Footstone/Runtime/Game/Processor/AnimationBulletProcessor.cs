using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    public class AnimationBulletProcessor : SimpleGameEntityProcessor<AnimationBulletComponent, AttackComponent>
    {
        public AnimationBulletProcessor() : base()
        {
            Order = ProcessorOrder.LaunchBullet;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var aniBulletComp = kvp.Value.Component1;
                var attackComp = kvp.Value.Component2;
                var key = aniBulletComp.BulletRefence.Key;
                var flag = aniBulletComp.BulletRefence.Flag;
                var launchPosition = aniBulletComp.Entity.Transform.Position;
                if(!string.IsNullOrEmpty(key))
                {
                    cmd.InstantiateEntity(key, flag, launchPosition, 0, entity =>
                    {
                        entity.Transform.Rotation = Quaternion.LookRotation(attackComp.AttackDir, Vector3.up);
                        var bulletComp = entity.Get<BulletComponent>();
                        if(bulletComp)
                        {
                            bulletComp.FlyDistance = attackComp.Data.AttackRange;
                            bulletComp.FlyDir = attackComp.AttackDir;
                            bulletComp.DamageLayerMask = attackComp.Data.AttackTargetLayer;
                            bulletComp.Damage = attackComp.AttackAttribute.GetFinalValue(time.FrameCount);
                            bulletComp.DamageFlag = attackComp.Data.DamageFlag;
                            bulletComp.StartPosition = launchPosition;

                        }
                    });
                }

            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
