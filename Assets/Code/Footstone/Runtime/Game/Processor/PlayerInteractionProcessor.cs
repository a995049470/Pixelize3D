using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class PlayerInteractionControllerProcessor : SimpleGameEntityProcessor<PlayerControllerComponent, InteractorComponent, ActionMaskComponent>
    {

        public PlayerInteractionControllerProcessor() : base()
        {
            Order = ProcessorOrder.InteractionCommandInput;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var controllerComp = kvp.Value.Component1;
                var interactionComp = kvp.Value.Component2;
                var actionMaskComp = kvp.Value.Component3;
                if(!interactionComp.IsInteracting && actionMaskComp.IsActionEnable(ActionFlag.Interaction))
                {
                    var cmd_ctrl = controllerComp.CurrentFrameInputCommand;
                    if((cmd_ctrl & InputCommand.Interaction) > 0)
                    {
                        interactionComp.IsInteracting = true;
                        controllerComp.InteractionSuccess();
                    }
                }
            }
        }
    }
}
