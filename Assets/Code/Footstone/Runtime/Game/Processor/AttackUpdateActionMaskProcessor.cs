using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class AttackUpdateActionMaskProcessor : SimpleGameEntityProcessor<ActionMaskComponent, AttackComponent>
    {
        public AttackUpdateActionMaskProcessor() : base()
        {
            Order = ProcessorOrder.UpadteActionMask;
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
            foreach (var kvp in ComponentDatas)
            {
                var actionMask = kvp.Value.Component1;
                var attackComp = kvp.Value.Component2;            
                if(attackComp.IsAttacking)
                {
                    actionMask.DisabledAction(ActionFlag.BuildPlot);
                    if(attackComp.IsBanActionOnAttaking)
                        actionMask.DisabledAction(ActionFlag.Move);
                
                }
            }
        }
    }
}
