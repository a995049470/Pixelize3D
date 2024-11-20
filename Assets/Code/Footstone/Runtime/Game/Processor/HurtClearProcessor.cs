using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class HurtClearProcessor : SimpleGameEntityProcessor<HurtComponent>
    {
        public HurtClearProcessor() : base()
        {
            Order = ProcessorOrder.FrameEnd;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                kvp.Value.Component1.Clear();
            }
        }
    }
}
