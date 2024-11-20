using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    public class MapOriginPointProcessor : SimpleGameEntityProcessor<MapOriginPointComponent>
    {
        private MapOriginPointComponent originPoint;
        public Vector3 OriginPosition { get => originPoint.Entity.Transform.Position; }

        protected override void OnEntityComponentAdding(Entity entity, MapOriginPointComponent component, GameData<MapOriginPointComponent> data)
        {
            //新的永远覆盖旧的
            if(originPoint == null) originPoint = component;
        }

        protected override void OnEntityComponentRemoved(Entity entity, MapOriginPointComponent component, GameData<MapOriginPointComponent> data)
        {
            if(originPoint == component)
            {
                foreach (var kvp in ComponentDatas)
                {
                    if(originPoint != kvp.Key)
                    {
                        originPoint = kvp.Key;
                        break;
                    }
                }
                if(originPoint == component) originPoint = null;
            }
        }
    }
}
