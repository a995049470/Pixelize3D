using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    
    
    public class WeaponPowerOnEquipProcessor : SimpleGameEntityProcessor<WeaponPowerComponent, AttackComponent, PowerReceiverComponent, WeaponModelComponent>
    {
        public WeaponPowerOnEquipProcessor() : base()
        {
            Order = ProcessorOrder.TakeEffectOnEquiping;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var powerComp = kvp.Value.Component1;
                var attackComp = kvp.Value.Component2;
                if(powerComp.IsEquipping)
                {
                    var weaponModelComp = kvp.Value.Component4;
                    var entity = kvp.Key.Entity;
                    weaponModelComp.TargetLeftHandWeaponKey = powerComp.LeafHandWeaponKey;
                    weaponModelComp.TargetRightHandWeaponKey = powerComp.RightHandWeaponKey;
                    attackComp.ForceCompleteAttack();
                    attackComp.AttackAttribute.ReceivePower(powerComp.PowerUID, powerComp.AttackValue);
                    attackComp.AttackDataReceicver.ReceivePower(powerComp.PowerUID, powerComp.WeaponAttackDataTable);
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }

        
        
    }

}
