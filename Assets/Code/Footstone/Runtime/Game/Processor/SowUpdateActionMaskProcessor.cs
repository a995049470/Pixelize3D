using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class SowUpdateActionMaskProcessor : SimpleGameEntityProcessor<SowComponent, ActionMaskComponent>
    {
        public SowUpdateActionMaskProcessor() : base()
        {
            Order = ProcessorOrder.UpadteActionMask;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var sowComp = kvp.Value.Component1;
                var actionMaskComp = kvp.Value.Component2;
                if(sowComp.IsSowing)
                {
                    actionMaskComp.DisabledAction(ActionFlag.Move | ActionFlag.Attack);
                }
            }
        }
    }
}
