using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    
    public class GOAPFaceFixedDirectionProcessor : SimpleGameEntityProcessor<GOAPFaceFixedDirectionComponent, GOAPAgentComponent, RotateComponent, ActionMaskComponent>
    {
        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var faceComp = kvp.Value.Component1;
                var rotateComp = kvp.Value.Component3;
                var actionMaskComp = kvp.Value.Component4;

                if(actionMaskComp.IsActionEnable(ActionFlag.Rotate))
                {
                    rotateComp.FaceDirection = DirectionUtil.CalculateDirection(faceComp.FixedForward);
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
