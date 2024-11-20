using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public abstract class InstantiateCallbackProcessor<T> : SimpleGameEntityProcessor<T> where T : EntityComponent, IEntityInstantiateCallback
    {
        private HashSet<IEntityInstantiateCallback> waitComponents;
        public InstantiateCallbackProcessor() : base()
        {
            Order = ProcessorOrder.FrameStart;
            waitComponents = new(32);
        }

        public override void Update(GameTime time)
        {
            if(waitComponents.Count > 0)
            {
                foreach (var component in waitComponents)
                {
                    component.LateEntityInstantiation();
                }
                waitComponents.Clear();
            }
        }

        protected override void OnEntityComponentAdding(Entity entity, T component, GameData<T> data)
        {
            base.OnEntityComponentAdding(entity, component, data);
            waitComponents.Add(component);
        }
        protected override void OnEntityComponentRemoved(Entity entity, T component, GameData<T> data)
        {
            base.OnEntityComponentRemoved(entity, component, data);
            waitComponents.Remove(component);
        }
    }
}
