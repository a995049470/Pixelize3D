using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class PickaxeUpdateActionMaskProcessor : SimpleGameEntityProcessor<PickaxeComponent, ActionMaskComponent>
    {
        public PickaxeUpdateActionMaskProcessor() : base()
        {
            Order = ProcessorOrder.UpadteActionMask;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var pickaxeComp = kvp.Value.Component1;
                var actionMaskComp = kvp.Value.Component2;
                if(pickaxeComp.IsPickaxing)
                {
                    actionMaskComp.DisabledAction(ActionFlag.Attack);
                }
            }
        }
    }
}
