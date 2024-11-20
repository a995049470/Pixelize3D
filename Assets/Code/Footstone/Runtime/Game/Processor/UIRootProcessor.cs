using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class UIRootProcessor : SimpleGameEntityProcessor<UIRootComponent>
    {
        public Transform Root;

        protected override void OnEntityComponentAdding(Entity entity, UIRootComponent component, GameData<UIRootComponent> data)
        {
            if(Root == null) Root = entity.transform;
        }

        protected override void OnEntityComponentRemoved(Entity entity, UIRootComponent component, GameData<UIRootComponent> data)
        {
            if(Root == entity.transform)
            {
                foreach (var kvp in ComponentDatas)
                {
                    if(Root != kvp.Key.transform)
                    {
                        Root = kvp.Key.transform;
                        break;
                    }
                }
                if(Root == entity.transform) Root = null;
            }
        }
    }
}
