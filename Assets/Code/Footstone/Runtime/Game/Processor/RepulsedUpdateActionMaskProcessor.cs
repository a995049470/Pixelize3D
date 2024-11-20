using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class RepulsedUpdateActionMaskProcessor : SimpleGameEntityProcessor<ActionMaskComponent, RepulsedComponent>
    {
        public RepulsedUpdateActionMaskProcessor() : base()
        {
            Order = ProcessorOrder.UpadteActionMask;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var actionMaskComp = kvp.Value.Component1;
                //var repulsedComp = kvp.Value.Component2;
                actionMaskComp.DisabledAction(ActionFlag.Attack | ActionFlag.Dig | ActionFlag.Sow | ActionFlag.Water | ActionFlag.Interaction | ActionFlag.BuildPlot | ActionFlag.Eat | ActionFlag.Move);
            }
        }
    }
}
