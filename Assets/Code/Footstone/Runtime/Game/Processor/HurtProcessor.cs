using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class HurtProcessor : SimpleGameEntityProcessor<HurtComponent, HitPointComponent>
    {
        public HurtProcessor() : base()
        {
            Order = ProcessorOrder.Hurt;
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
            foreach(var kvp in ComponentDatas)
            {
                var hurtComp = kvp.Value.Component1;
                var hitpointComp = kvp.Value.Component2;
                hitpointComp.IsDeadNow = false;
                if (hurtComp.DamageReceiver.Sum > hurtComp.Heal)
                {
                   hitpointComp.ReceiveDamage(hurtComp.DamageReceiver.Sum - hurtComp.Heal);
                }
                else if (hurtComp.Heal > hurtComp.DamageReceiver.Sum)
                {
                    hitpointComp.ReceiveHeal(hurtComp.Heal - hurtComp.DamageReceiver.Sum);
                }
                hurtComp.IsReceiveHurt = !hitpointComp.IsDead;
                hurtComp.DamageReceiver.Clear();
                hurtComp.Heal = 0;
            };
            
        }

       
    }
}
