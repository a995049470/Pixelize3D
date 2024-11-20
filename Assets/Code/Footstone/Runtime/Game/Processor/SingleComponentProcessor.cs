using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public abstract class SingleComponentProcessor<T> : SimpleGameEntityProcessor<T> where T : EntityComponent
    {
        public T SingleComponent;

        protected override void OnEntityComponentAdding(Entity entity, T component, GameData<T> data)
        {
            if(SingleComponent == null) SingleComponent = component;
        }

        protected override void OnEntityComponentRemoved(Entity entity, T component, GameData<T> data)
        {
            if(SingleComponent == component)
            {
                foreach (var kvp in ComponentDatas)
                {
                    if(SingleComponent != kvp.Key)
                    {
                        SingleComponent = kvp.Key;
                        break;
                    }
                }
                if(SingleComponent == component) SingleComponent = null;
            }
        }
    }
}
