using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class AnimationDamageProcessor : SimpleGameEntityProcessor<AnimationDamageEventComponent, AttackComponent>
    {
        public AnimationDamageProcessor() : base()
        {
            Order = ProcessorOrder.Attack;
        }
        
        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var damageEventComp = kvp.Value.Component1;
                var attackComp = kvp.Value.Component2;
                var atkDir = attackComp.AttackDir;
                var atkRange = attackComp.Data.AttackRange;
                var atkLayer = (int)attackComp.Data.AttackTargetLayer;
                var dmg = attackComp.AttackAttribute.GetFinalValue(time.FrameCount);
                var position = attackComp.Entity.Transform.Position;
                var from = position;
                var to = position + atkDir * atkRange;
                var hitCount = physicsSystem.RaycastNonAlloc(from, to, raycastHits, atkLayer);
                var uidList = damageEventComp.TargetUIDList;
                uidList.Clear();
                for (int i = 0; i < hitCount; i++)
                {
                    var entity = raycastHits[i].transform.GetComponent<TargetEntity>()?.Target;
                    var hurt = entity?.Get<HurtComponent>();
                    if (hurt != null && hurt.IsReceiveHurt)
                    {
                        var dmgVal = dmg * damageEventComp.DamageCoefficient;
                        var dmgFlag = attackComp.Data.DamageFlag;
                        hurt.DamageReceiver.ReceiveDamage(dmgFlag, dmgVal);
                        hurt.HitDirs.Add(-atkDir);
                        uidList.Add(entity.Id);
                    }
                }
                damageEventComp.IsDamageSuccess = hitCount > 0;
            }
        }
    }
}
