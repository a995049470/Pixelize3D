using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    public class PlayerPickControllerProcessor : SimpleGameEntityProcessor<PlayerControllerComponent, PickComponent, ActionMaskComponent>
    {
        public PlayerPickControllerProcessor() : base()
        {
            Order = ProcessorOrder.PickCommandInput;
            
        }

        public override void Update(GameTime time)
        {

            foreach (var kvp in ComponentDatas)
            {
                var ctrlComp = kvp.Value.Component1;
                var inputCmd = ctrlComp.CurrentFrameInputCommand;
                var actionMaskComp = kvp.Value.Component3;
                var pickComp = kvp.Value.Component2;
                if(!pickComp.IsPicking && actionMaskComp.IsActionEnable(ActionFlag.Pick))
                {
                    if((inputCmd & InputCommand.Pick) > 0)
                    {
                        pickComp.IsPicking = true;
                    }
                }
            }

        }

    }
}
