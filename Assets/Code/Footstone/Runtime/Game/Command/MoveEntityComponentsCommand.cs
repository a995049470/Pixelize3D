using System.Collections.Generic;
using Lost.Runtime.Footstone.Collection;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    
    /// <summary>
    /// 将Entity的Component赋给另一个Component
    /// </summary>
    public class MoveEntityComponentsCommand<T> : ICommand where T : class
    {
        public Entity Src;
        public Entity Dst;

        public void Execute()
        {
            var components = Src.RemoveComponents<T>();
            Dst.Add(components);
        }
    }

    public class MoveEntityComponentsCommand : ICommand 
    {
        public IEnumerable<EntityComponent> Components;
        public Entity Dst;

        public void Execute()
        {
            foreach (var component in Components)
            {
                if(component.Entity)
                    component.Entity.Remove(component, true);
            }
            Dst.Add(Components);
        }
    }
}
