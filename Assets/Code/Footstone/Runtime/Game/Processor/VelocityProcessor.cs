using Lost.Runtime.Footstone.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    public class VelocityProcessor : SimpleGameEntityProcessor<VelocityComponent>
    {
        private AutoSizeMapGrids mapGrids = new();
        private HashSet<VelocityComponent> newVelocityComponents = new(16); 
        private const int placePoint = 1;
        private const int emptyPoint = 0;
        private PlotProcessor plotProcessor;
        private AStarProcessor aStarProcessor;
        
        public VelocityProcessor() : base()
        {
            //mapGrids.OnBoundsChangeEvent += RecordTargetPosition;
        }

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            plotProcessor = GetProcessor<PlotProcessor>();
            aStarProcessor = GetProcessor<AStarProcessor>();
        }

        public void RecordTargetPosition(AutoSizeMapGrids grids)
        {
            foreach (var kvp in ComponentDatas)
            {
                var pos = kvp.Key.TargetPos;
                grids.Set(pos, placePoint);
            }
            
        }

        public bool TryFindEntity(Vector3 pos, out Entity entity)
        {
            bool isSuccess = false;
            entity = null;

            foreach (var kvp in ComponentDatas)
            {
                if(kvp.Key.TargetPos == pos)
                {
                    isSuccess = true;
                    entity = kvp.Key.Entity;
                    break;
                }
            }

            return isSuccess;
        }

        public bool IsVaildTargetPos(Vector3 targetPos)
        {
            var min = mapGrids.MapBoundsMin;
            var max = mapGrids.MapBoundsMax;
            mapGrids.CalculateBounds(ref min, ref max, targetPos);
            if (newVelocityComponents.Count > 0)
            {
                foreach (var velocityComp in newVelocityComponents)
                {
                    mapGrids.CalculateBounds(ref min, ref max, velocityComp.TargetPos);
                }
            }
            mapGrids.TryChangeMapBounds(min, max);
            if (newVelocityComponents.Count > 0)
            {
                foreach (var velocityComp in newVelocityComponents)
                {
                    var pos = velocityComp.TargetPos;
                    mapGrids.Set(pos, placePoint);
                }
                newVelocityComponents.Clear();
            }
            
            return mapGrids.Get(targetPos) == emptyPoint;
        }

        public bool TryMove(Vector3 src, Vector3 dst)
        {
            //修改mapgrid的bounds
            bool isVaildDst = IsVaildTargetPos(dst);
            bool isPlotDst = plotProcessor.IsPlot(dst);
            bool isWay = aStarSystem.IsWayPoint(dst);
            bool isMove = isVaildDst && isPlotDst && isWay;
            if(isMove)
            {
                mapGrids.Set(src, emptyPoint);
                mapGrids.Set(dst, placePoint);
            }
            return isMove;
        }

        

        public bool TryTransport(VelocityComponent velocityComp, Vector3 dst)
        {
            bool isVaildDst = IsVaildTargetPos(dst);
            mapGrids.Set(velocityComp.TargetPos, emptyPoint);
            if(isVaildDst)
            {
                mapGrids.Set(dst, placePoint);
            }
            else
            {
                mapGrids.Set(velocityComp.TargetPos, placePoint);
            }
            return isVaildDst;
        }

        protected override void OnEntityComponentAdding(Entity entity, VelocityComponent component, GameData<VelocityComponent> data)
        {
            newVelocityComponents.Add(component);
        }

        protected override void OnEntityComponentRemoved(Entity entity, VelocityComponent component, GameData<VelocityComponent> data)
        {
            bool isWrited = true;
            if(newVelocityComponents.Count > 0)
                isWrited = !newVelocityComponents.Remove(component);
            if(isWrited)
                mapGrids.Set(component.TargetPos, emptyPoint);

            //切场景东西时候player不删导致Clear清理不完全 有点蠢。。
            if(ComponentDatas.Count <= 2)
            {
                mapGrids.Clear();
                foreach (var kvp in ComponentDatas)
                {
                    if(kvp.Key != component)
                    {
                        newVelocityComponents.Add(kvp.Key);
                    }
                }
            }
        }

    }
}
