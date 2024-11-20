using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class DeadUpdateActionMaskProcessor : SimpleGameEntityProcessor<ActionMaskComponent, DeadComponent>
    {
        public DeadUpdateActionMaskProcessor() : base()
        {
            Order = ProcessorOrder.UpadteActionMask;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var actionMask = kvp.Value.Component1;
                var deadComp = kvp.Value.Component2;            
                if(deadComp.IsDeading())
                {
                    actionMask.DisabledAction(ActionFlag.All);
                }
            }
        }
    }
}
