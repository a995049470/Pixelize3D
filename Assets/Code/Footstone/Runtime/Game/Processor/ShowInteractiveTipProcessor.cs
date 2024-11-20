using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class ShowInteractiveTipProcessor : SimpleGameEntityProcessor<InteractiveComponent, ShowTipComponent, InteractiveLabelComponent>
    {
        private InteractiveTipDataProcessor tipDataProcessor;

        public ShowInteractiveTipProcessor() : base()
        {
            Order = ProcessorOrder.ShowInteractiveTip;
        }

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            tipDataProcessor = GetProcessor<InteractiveTipDataProcessor>();
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
            foreach (var kvp in ComponentDatas)
            {
                var interactiveComp = kvp.Value.Component1;
                var showTipComp = kvp.Value.Component2;
                if(interactiveComp.IsTriggerEffect)
                {
                    if(!showTipComp.IsSendRequest && !string.IsNullOrEmpty(showTipComp.TipKey))
                    {
                        showTipComp.IsSendRequest = tipDataProcessor.SingleComponent.TryShowTip(showTipComp.Id, showTipComp.TipKey);
                        if(showTipComp.IsWaitPlayerRespond)
                        {
                            interactiveComp.TriggerFlag = TriggerFlag.WaitPlayerRespond;
                            interactiveComp.NextTriggerFlag = TriggerFlag.WaitPlayerRespond;
                        }
                    }
                }
                else if(interactiveComp.IsWaitPlayerRespond && 
                showTipComp.IsSendRequest && 
                showTipComp.IsWaitPlayerRespond)
                { 
                    if(tipDataProcessor.SingleComponent.TryGetOptionIndex(showTipComp.Id, out showTipComp.OptionIndex))
                    {
                        showTipComp.IsSendRequest = false;
                        if(showTipComp.OptionIndex == 0)
                            interactiveComp.TriggerFlag = TriggerFlag.Triggering;
                        else
                            interactiveComp.NextTriggerFlag = TriggerFlag.WaitTrigger;
                    }
                    else
                    {
                        interactiveComp.TriggerFlag = TriggerFlag.WaitPlayerRespond;
                        interactiveComp.NextTriggerFlag = TriggerFlag.WaitPlayerRespond;
                    }
                }
            }
        }
    }

}
