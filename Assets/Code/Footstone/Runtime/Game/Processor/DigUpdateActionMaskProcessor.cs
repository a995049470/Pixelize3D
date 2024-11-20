using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class DigUpdateActionMaskProcessor : SimpleGameEntityProcessor<DigComponent, ActionMaskComponent>
    {
        public DigUpdateActionMaskProcessor() : base()
        {
            Order = ProcessorOrder.UpadteActionMask;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var digComp = kvp.Value.Component1;
                var actionMaskComp = kvp.Value.Component2;       
                if(digComp.IsDiging)
                {
                    actionMaskComp.DisabledAction(ActionFlag.Move | ActionFlag.Attack | ActionFlag.Water);
                }
            }
        }
    }
}
