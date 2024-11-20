using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    //子弹飞
    public class BulletFlyProcessor : SimpleGameEntityProcessor<BulletComponent>
    {
        public BulletFlyProcessor() : base()
        {
            Order = ProcessorOrder.Move;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var bulletComp = kvp.Value.Component1;
                var speed = bulletComp.Speed;
                var dir = bulletComp.FlyDir;
                var position = bulletComp.Entity.Transform.Position;
                //飞到最远处 准备被回收
                if(Vector3.Distance(position, bulletComp.StartPosition) >= bulletComp.FlyDistance)
                {
                   bulletComp.IsDead = true; 
                }
                else
                {
                    var targetPosition = position + dir * speed * time.DeltaTime;
                    bulletComp.Entity.Transform.Position = targetPosition;
                }
            }
        }
    }
}
