using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class InteractivePickChestProcessor : SimpleGameEntityProcessor<InteractiveComponent, ChestComponent, InteractiveLabelComponent>
    {
        private InteractiveTipDataProcessor interactiveTipProcessor;

        public InteractivePickChestProcessor() : base()
        {
            Order = ProcessorOrder.PickChest;
        }

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            interactiveTipProcessor = GetProcessor<InteractiveTipDataProcessor>();
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
            foreach (var kvp in ComponentDatas)
            {
                var interactiveComp = kvp.Value.Component1;
                var chestComp = kvp.Value.Component2;
                if(interactiveComp.IsTriggerEffect)
                {
                    if(chestComp.IsOpen)
                    {
                        if(!chestComp.IsSendInteractiveRequest && !string.IsNullOrEmpty(chestComp.TipKey))
                        {
                            chestComp.IsSendInteractiveRequest = interactiveTipProcessor.SingleComponent.TryShowTip(chestComp.Id, chestComp.TipKey); 
                            interactiveComp.TriggerFlag = TriggerFlag.WaitPlayerRespond;
                            interactiveComp.NextTriggerFlag = TriggerFlag.WaitPlayerRespond;
                        }   
                        else
                        {
                            bagData.ReceiveItem(chestComp.ChestKey, 1);   
                            interactiveComp.NextTriggerFlag = TriggerFlag.WaitRecycle;
                        }
                    }
                    else
                    {
                        chestComp.IsOpen = true;
                        interactiveComp.NextTriggerFlag = TriggerFlag.WaitTrigger;
                    }
                }
                else if(chestComp.IsSendInteractiveRequest && interactiveComp.IsWaitPlayerRespond)
                {
                    if(interactiveTipProcessor.SingleComponent.TryGetOptionIndex(chestComp.Id, out var index))
                    {
                        chestComp.IsSendInteractiveRequest = false;
                        //成功交互
                        if(index == 0)
                        {
                            bagData.ReceiveItem(chestComp.ChestKey, 1);
                            interactiveComp.NextTriggerFlag = TriggerFlag.WaitRecycle;
                        }
                        //失败交互
                        else 
                        {
                            interactiveComp.NextTriggerFlag = TriggerFlag.WaitTrigger;
                        }
                    }
                    else
                    {
                        interactiveComp.NextTriggerFlag = TriggerFlag.WaitPlayerRespond;
                    }
                }
            }
        }

    }

}
