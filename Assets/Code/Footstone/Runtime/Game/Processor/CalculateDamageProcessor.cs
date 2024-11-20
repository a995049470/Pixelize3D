using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class CalculateDamageProcessor : SimpleGameEntityProcessor<HurtComponent, DefenseComponent>
    {
        public CalculateDamageProcessor() : base()
        {
            Order = ProcessorOrder.ComputeHurtValue;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var hurtComp = kvp.Value.Component1;
                var defenseComp = kvp.Value.Component2;
                var emptyHandDmg = hurtComp.DamageReceiver.GetDamage(DamageFlag.EmptyHand);
                if(emptyHandDmg > 0)
                {
                    emptyHandDmg = defenseComp.EmptyHandDamageDefense.CalculateDamage(emptyHandDmg);
                    hurtComp.DamageReceiver.SetDamage(DamageFlag.EmptyHand, emptyHandDmg);
                }
                var physicalDmg = hurtComp.DamageReceiver.GetDamage(DamageFlag.Physical);
                if(physicalDmg > 0)
                {
                    physicalDmg = defenseComp.PhysicalDamageDefense.CalculateDamage(physicalDmg);
                    hurtComp.DamageReceiver.SetDamage(DamageFlag.Physical, physicalDmg);
                }
            } 
        }
    }
}
