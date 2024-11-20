using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    

    public class NewSceneTriggerProcessor : SimpleGameEntityProcessor<NewSceneComponent, TriggerDeviceComponent, TriggerDeviceLabelComponent>
    {
        public NewSceneTriggerProcessor() : base()
        {
            Order = ProcessorOrder.TriggerDeviceActivated;
        }

        public override void Update(GameTime time)
        {
            
            foreach (var kvp in ComponentDatas)
            {
                var newSceneComp = kvp.Value.Component1;
                var triggerDeviceComp = kvp.Value.Component2;
                if(triggerDeviceComp.IsTriggering)
                {
                    if(!string.IsNullOrEmpty(newSceneComp.Scene))
                    {
                        commandBufferManager.FrameEndBuffer.CreateGameScene(newSceneComp.Scene, newSceneComp.IsDestoryCurrentScene);
                    }
                }
            }
        }
    }
}
