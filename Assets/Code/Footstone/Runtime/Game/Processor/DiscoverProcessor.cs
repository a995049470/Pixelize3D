using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class DiscoverProcessor : SimpleGameEntityProcessor<DiscoverComponent, PowerReceiverComponent>
    {
        private MapProcessor mapProcessor;
        public DiscoverProcessor() : base()
        {
            Order = ProcessorOrder.Discover;
        }

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            mapProcessor = GetProcessor<MapProcessor>();
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var discoverComp = kvp.Value.Component1;
                mapProcessor.SingleComponent.Data.AddKnownKey(discoverComp.Key);
            }
        }
    }

}
