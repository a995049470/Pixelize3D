using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class PlaceObjectUpdateActionMaskProcessor :  SimpleGameEntityProcessor<ActionMaskComponent, PlaceObjectComponent>
    {
        public PlaceObjectUpdateActionMaskProcessor() : base()
        {
            Order = ProcessorOrder.UpadteActionMask;
        }

        //建造期间禁止攻击
        public override void Update(GameTime time)
        {
            base.Update(time);
            foreach (var kvp in ComponentDatas)
            {
                var actionMask = kvp.Value.Component1;
                actionMask.DisabledAction(ActionFlag.Attack);
            }
        }
    }
}