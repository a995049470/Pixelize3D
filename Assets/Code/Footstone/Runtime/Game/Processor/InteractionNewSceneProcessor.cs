using Lost.Runtime.Footstone.Core;
using UnityEngine.UIElements;

namespace Lost.Runtime.Footstone.Game
{

    public class InteractionNewSceneProcessor : SimpleGameEntityProcessor<InteractiveComponent, NewSceneComponent, InteractiveLabelComponent>
    {
        public InteractionNewSceneProcessor() : base()
        {
            Order = ProcessorOrder.InteractionNewScene;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var interactionComp = kvp.Value.Component1;
                var newSceneComp = kvp.Value.Component2;
                if(interactionComp.IsTriggerEffect)
                {
                    commandBufferManager.FrameEndBuffer.CreateGameScene(
                        newSceneComp.Scene, newSceneComp.IsDestoryCurrentScene
                    );
                }
            }
        }
    }

}
