using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class AStarSystem
    {
        private AutoSizeMapGrids mapGrids = new();
        private AStarProcessor aStarProcessor ;
        private const int emptyPoint = 0;
        private const int barrierPoint = 1;
        public AStarSystem(IServiceRegistry _service)
        {
            //mapGrids.OnBoundsChangeEvent += OnBoundsChangeEvent;
        }

        public void Initialize(IServiceRegistry _service)
        {
            var sceneInstance = _service.GetService<SceneSystem>().SceneInstance;
            aStarProcessor = sceneInstance.ForceGetProcessor<AStarProcessor>();
            
        }

        
        private void ResetMapGridBounds(params Vector3[] positions)
        {
            {
                var dirtyBarrierComponents = aStarProcessor.DirtyBarrierComponents;
                mapGrids.CalculateMapBounds(out var min, out var max, positions);
                if(dirtyBarrierComponents.Count > 0)
                {
                    foreach (var barrier in dirtyBarrierComponents)
                    {
                        var pos = mapGrids.ConvertPositionToV2Int(barrier.Entity.Transform.Position);
                        max = Vector2Int.Max(max, pos + Vector2Int.one);
                        min = Vector2Int.Min(min, pos);
                    }
                }
                mapGrids.TryChangeMapBounds(min, max);
                if(dirtyBarrierComponents.Count > 0)
                {
                    foreach (var barrier in dirtyBarrierComponents)
                    {
                        var pos = barrier.Entity.Transform.Position;
                        var grid = mapGrids.Get(pos);
                        mapGrids.Set(pos, grid | ((int)barrier.ElementFlag));
                    }
                    dirtyBarrierComponents.Clear();
                }
            }
        }

        public bool TryFindNextPoistion(Vector3 start, Vector3 end, int endPointRadius, out Vector3 nextPosition)
        {
            ResetMapGridBounds(start, end);
            var startId = mapGrids.ConvertPositionToIndex2(start);
            var endId = mapGrids.ConvertPositionToIndex2(end);
            bool isSuccessFindWay = AStar.TryFindWayToEnd(startId, endId, mapGrids.Grids, mapGrids.Width, mapGrids.Height, (int)AStarElementFlag.Way, out var pointList);
            var isSuccessToFindNextPoint = false;
            nextPosition = start;
            if(isSuccessFindWay && pointList.Count > endPointRadius + 1)
            {
                isSuccessToFindNextPoint = true;
                nextPosition = mapGrids.ConvertV2IntIndexToPosition(pointList[pointList.Count - 2]);
            }
            return isSuccessToFindNextPoint;
        }

        public bool IsWayPoint(Vector3 targetPos)
        {
            ResetMapGridBounds(targetPos);
            return mapGrids.Get(targetPos) == (int)AStarElementFlag.Way;
        }

        public void ClearElement(Vector3 pos, int value)
        {
            var grid = mapGrids.Get(pos);
            mapGrids.Set(pos, grid & (~value));
        }

        public bool IsClearMapGridsOnComponetRemoving()
        {
            int totalCount = aStarProcessor.ComponentDatas.Count;
            bool isClear = totalCount <= 2;
            if(isClear)
            {
                mapGrids.Clear();
            }
            return isClear;
        }
    }
}
