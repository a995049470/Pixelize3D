using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class WaterUpdateActionMaskProcessor : SimpleGameEntityProcessor<WaterComponent, ActionMaskComponent>
    {
        public WaterUpdateActionMaskProcessor() : base()
        {
            Order = ProcessorOrder.UpadteActionMask;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var waterComp = kvp.Value.Component1;
                var actionMaskComp = kvp.Value.Component2;
                if(waterComp.IsWatering)
                {
                    actionMaskComp.DisabledAction(ActionFlag.Move | ActionFlag.Attack | ActionFlag.Dig);
                }
            }
        }
    }
}
