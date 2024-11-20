using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class FollowParentProcessor : SimpleGameEntityProcessor<ParentComponent, TransformComponent>
    {
        public FollowParentProcessor() : base()
        {
            Order = ProcessorOrder.FrameEnd;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var parentComp = kvp.Value.Component1;
                var transComp = kvp.Value.Component2;
                bool isGet = parentComp.TargetReference.TryGetComponent(sceneSystem, out var target);
                if(isGet)
                {
                    transComp.Position = target.position;
                    transComp.Rotation = target.rotation;
                    transComp.LocalScale = target.lossyScale;
                }
            }
        }

        protected override void OnEntityComponentRemoved(Entity entity, ParentComponent component, GameData<ParentComponent, TransformComponent> data)
        {
            base.OnEntityComponentRemoved(entity, component, data);
            component.TargetReference.Clear();
        }
    }
}
