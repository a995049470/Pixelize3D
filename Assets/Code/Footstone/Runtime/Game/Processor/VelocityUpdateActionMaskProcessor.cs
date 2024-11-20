using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class VelocityUpdateActionMaskProcessor : SimpleGameEntityProcessor<ActionMaskComponent, VelocityComponent>
    {
        public VelocityUpdateActionMaskProcessor() : base()
        {
            Order = ProcessorOrder.UpadteActionMask;
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
            foreach (var kvp in ComponentDatas)
            {
                var actionMask = kvp.Value.Component1;
                var velocity = kvp.Value.Component2;            
                if(velocity.IsMoving)
                {
                    actionMask.DisabledAction(ActionFlag.Attack | ActionFlag.Dig | ActionFlag.Sow | ActionFlag.Water | ActionFlag.Interaction | ActionFlag.BuildPlot | ActionFlag.Eat | ActionFlag.PlaceObject | ActionFlag.Pickaxe);
                }
            }
        }
    }
}
