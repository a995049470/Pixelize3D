using Lost.Runtime.Footstone.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class PlaceProcessor : SimpleGameEntityProcessor<PlaceComponent>
    {
        private HashSet<PlaceComponent> newPlaceComponents = new();
        private AutoSizeMapGrids mapGrids = new();
        
        private const int placePoint = 1;
        private const int emptyPoint = 0;

        public PlaceProcessor() : base()
        {
            //mapGrids.OnBoundsChangeEvent += RecordAllPlacePosition;
        }
        
        public void RecordAllPlacePosition(AutoSizeMapGrids grids)
        {
            foreach (var kvp in ComponentDatas)
            {
                var pos = kvp.Key.Entity.Transform.Position;
                grids.Set(pos, placePoint);
            }   
        }

        public bool IsEmpty(Vector3 targetPos)
        {
            var min = mapGrids.MapBoundsMin;
            var max = mapGrids.MapBoundsMax;
            mapGrids.CalculateBounds(ref min, ref max, targetPos);
            if (newPlaceComponents.Count > 0)
            {
                foreach (var comp in newPlaceComponents)
                {
                    mapGrids.CalculateBounds(ref min, ref max, comp.Entity.Transform.Position);
                }
            }
            mapGrids.TryChangeMapBounds(min, max);
            if(newPlaceComponents.Count > 0)
            {
                foreach (var comp in newPlaceComponents)
                {
                    var pos = comp.Entity.Transform.Position;
                    mapGrids.Set(pos, placePoint);
                }
                newPlaceComponents.Clear();
            }
            return mapGrids.Get(targetPos) == emptyPoint;
        }

        protected override void OnEntityComponentAdding(Entity entity, PlaceComponent component, GameData<PlaceComponent> data)
        {
            newPlaceComponents.Add(component);
        }

        protected override void OnEntityComponentRemoved(Entity entity, PlaceComponent component, GameData<PlaceComponent> data)
        {
            bool isWrited = true;
            if(newPlaceComponents.Count > 0)
                isWrited = !newPlaceComponents.Remove(component);
            if(isWrited)
                mapGrids.Set(component.Entity.Transform.Position, emptyPoint);

            if(ComponentDatas.Count == 1)
            {
                mapGrids.Clear();
            }
        }
    }
}
