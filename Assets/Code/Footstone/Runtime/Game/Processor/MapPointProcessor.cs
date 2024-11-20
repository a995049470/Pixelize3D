using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class MapPointProcessor : SimpleGameEntityProcessor<MapPointComponent, TagComponent>
    {
        private Dictionary<string, TransformComponent> pointDic = new();

        public bool TryGetTargetPoint(string key, out Vector3 pos)
        {
            pos = Vector3.zero;
            bool isGet = pointDic.TryGetValue(key, out var transComp); 
            if(isGet)
            {
                pos = transComp.Position;
            }
            return isGet;
        }


        protected override void OnEntityComponentAdding(Entity entity, MapPointComponent component, GameData<MapPointComponent, TagComponent> data)
        {
            base.OnEntityComponentAdding(entity, component, data);
            pointDic[data.Component2.Key] = entity.Transform;
        }

        protected override void OnEntityComponentRemoved(Entity entity, MapPointComponent component, GameData<MapPointComponent, TagComponent> data)
        {
            base.OnEntityComponentRemoved(entity, component, data);
            pointDic.Remove(data.Component2.Key);
        }
    }
}
