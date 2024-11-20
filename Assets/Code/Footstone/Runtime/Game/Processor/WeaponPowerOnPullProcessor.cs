using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class WeaponPowerOnPullProcessor : SimpleGameEntityProcessor<WeaponPowerComponent, PowerReceiverComponent, AttackComponent, WeaponModelComponent>
    {
        public WeaponPowerOnPullProcessor() : base()
        {
            Order = ProcessorOrder.TakeEffectOnPulling;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var powerComp = kvp.Value.Component1;
                if(!powerComp.IsEquipping)
                {
                    var entity = kvp.Key.Entity;
                    var attackComp = kvp.Value.Component3;
                    var modelComp = kvp.Value.Component4;
                    modelComp.TargetLeftHandWeaponKey = "";
                    modelComp.TargetRightHandWeaponKey = "";
                    attackComp.ForceCompleteAttack();
                    attackComp.AttackAttribute.LostPower(powerComp.PowerUID);
                    attackComp.AttackDataReceicver.LostPower(powerComp.PowerUID);
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }

}
