using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class InteractionOpenUIProcessor : SimpleGameEntityProcessor<InteractiveComponent, OpenUIComponent>
    {
        public InteractionOpenUIProcessor() : base()
        {
            Order = ProcessorOrder.InteractionOpenUI;
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
            foreach (var kvp in ComponentDatas)
            {
                var interactionComp = kvp.Value.Component1;
                var openUIComp = kvp.Value.Component2;
                if(interactionComp.IsTriggerEffect)
                {
                    uiManager.OpenWindow(openUIComp.Key);
                }
            }
        }
    }

}
