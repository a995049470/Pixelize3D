using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class GOAPIdleProcessor : SimpleGameEntityProcessor<GOAPIdleComponent, GOAPAgentComponent, RotateComponent, ActionMaskComponent>
    {
        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var idleComp = kvp.Value.Component1;
                var rotateComp = kvp.Value.Component3;
                var actionMaskComp = kvp.Value.Component4;
                var worldStatus = kvp.Value.Component2.WorldStatus;
                var isFinsish = false;
                var rotateAngle = 0.0f;
                bool isRotate = actionMaskComp.IsActionEnable(ActionFlag.Rotate) && 
                      idleComp.TryRotate(random, time.DeltaTime, out rotateAngle);
                if(isRotate)
                {
                    rotateComp.Rotate(rotateAngle);
                }
                if(isFinsish)
                {
                    var task = kvp.Value.Component2.Task;
                    task.FinshCurrentAction(true);
                    //cmd.RemoveEntityComponent(idleComp);
                }

            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }

        protected override void OnEntityComponentRemoved(Entity entity, GOAPIdleComponent component, GameData<GOAPIdleComponent, GOAPAgentComponent, RotateComponent, ActionMaskComponent> data)
        {
            component.Reset();
        }
    }
}
