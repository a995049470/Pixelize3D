

using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class ActionMaskProcessor : SimpleGameEntityProcessor<ActionMaskComponent>
    {
        public ActionMaskProcessor() : base()
        {
            Order = ProcessorOrder.FrameEnd;
        }
        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var data = kvp.Value;
                data.Component1.ClearActionMask();
            }
        }
    }
}
