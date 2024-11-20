using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class DefenseClearProcessor : SimpleGameEntityProcessor<DefenseComponent>
    {
        public DefenseClearProcessor() : base()
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
