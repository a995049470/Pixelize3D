using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class PlayerControllerCacheProcessor : SimpleGameEntityProcessor<PlayerControllerComponent>
    {
        private InputSettingProcessor inputSettingProcessor;
        public PlayerControllerCacheProcessor() : base()
        {
            Order = ProcessorOrder.FrameEnd;
        }

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            inputSettingProcessor = GetProcessor<InputSettingProcessor>();
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var playerControllerComp = kvp.Value.Component1;
                playerControllerComp.CacheButton(time.TotalTime, inputSettingProcessor.SingleComponent);
            }
        }
    }
}
