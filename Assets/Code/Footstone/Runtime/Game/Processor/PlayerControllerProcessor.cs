using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class PlayerControllerProcessor : SimpleGameEntityProcessor<PlayerControllerComponent>
    {
        private InputSettingProcessor inputSettingProcessor;
        public PlayerControllerProcessor() : base()
        {
            Order = ProcessorOrder.CalculateInputCommand;
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
                bool isAllowGameInput = uiManager.IsTopWindowAllowGameInput();
                if(isAllowGameInput)
                {
                    playerControllerComp.CalculateCurrentFrameInputCommand(input, inputSettingProcessor.SingleComponent, time.TotalTime);
                }
                else
                {
                    playerControllerComp.ClearCurrentFrameInputCommand();
                }
                //快速装备
                if((playerControllerComp.CurrentFrameInputCommand & InputCommand.FastEquip) > 0)
                {
                    bagData.SetFastSelectIndex(playerControllerComp.FastEquipIndex);
                }
            }
        }
    }
}
