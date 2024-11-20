using Lost.Runtime.Footstone.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class PlotProcessor : SimpleGameEntityProcessor<PlotComponent>
    {
        private AutoSizeMapGrids mapGrids = new();
        private HashSet<PlotComponent> newPlotComponents = new(16); 
        private const int empty = 0;
        
        public PlotProcessor() : base()
        {

        }

        
        public int GetPlotFlag(Vector3 pos)
        {
            if(newPlotComponents.Count > 0)
            {
                var min = mapGrids.MapBoundsMin;
                var max = mapGrids.MapBoundsMax;
                foreach (var plotComponent in newPlotComponents)
                {
                    mapGrids.CalculateBounds(ref min, ref max, plotComponent.Entity.Transform.Position);
                }
                mapGrids.TryChangeMapBounds(min, max);
                if(newPlotComponents.Count > 0)
                {
                    foreach (var plotComponent in newPlotComponents)
                    {
                        mapGrids.Set(plotComponent.Entity.Transform.Position, (int)plotComponent.Flag);
                    }
                    newPlotComponents.Clear();
                }
            }

            var isOutOfBounds = mapGrids.IsOutSide(pos);
            var flag = isOutOfBounds ? 0 : mapGrids.Get(pos);
            return flag;
        }

        //判断该位置是否有地块
        public bool IsPlot(Vector3 pos)
        {
            return GetPlotFlag(pos) != empty;
        }

        public bool IsPlaceable(Vector3 pos)
        {
            return (GetPlotFlag(pos) & (int)PlotFlg.Placeable) > 0;
        }

        protected override void OnEntityComponentAdding(Entity entity, PlotComponent component, GameData<PlotComponent> data)
        {
            base.OnEntityComponentAdding(entity, component, data);
            newPlotComponents.Add(component);
        }

        protected override void OnEntityComponentRemoved(Entity entity, PlotComponent component, GameData<PlotComponent> data)
        {
            base.OnEntityComponentRemoved(entity, component, data);
            if(newPlotComponents.Contains(component))
            {
                newPlotComponents.Remove(component);
            }
            else
            {
                mapGrids.Set(entity.Transform.Position, empty);
            }
            
            if(ComponentDatas.Count == 1)
            {
                mapGrids.Clear();
            }
        }

    }
}
