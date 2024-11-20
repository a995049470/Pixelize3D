using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class PlayerProcessor : SimpleGameEntityProcessor<PlayerComponent, TransformComponent>
    {
        public TransformComponent Target;
        public Entity PlayerEntity { get => Target.Entity; }

        protected override void OnEntityComponentAdding(Entity entity, PlayerComponent component, GameData<PlayerComponent, TransformComponent> data)
        {
            base.OnEntityComponentAdding(entity, component, data);
            if(Target == null) Target = entity.Transform;
        }

        protected override void OnEntityComponentRemoved(Entity entity, PlayerComponent component, GameData<PlayerComponent, TransformComponent> data)
        {
            base.OnEntityComponentRemoved(entity, component, data);
            if(Target == data.Component2)
            {
                foreach (var kvp in ComponentDatas)
                {
                    if(Target != kvp.Value.Component2)
                    {
                        Target = kvp.Value.Component2;
                        break;
                    }
                }
                if(Target == data.Component2) Target = null;
            }
        }
    }
}
