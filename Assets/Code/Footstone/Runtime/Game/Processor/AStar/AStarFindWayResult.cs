using System.Collections.Generic;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class AStarFindWayResult
    {
        public bool IsSuccess;
        // private int mapWidth;
        // private int mapHeight;
        private Vector2Int mapBoundsMin;
        private Vector2Int mapBoundsMax;
        private Stack<Vector2Int> wayPoints;

        public AStarFindWayResult(Vector2Int max, Vector2Int min, Stack<Vector2Int> points)
        {
            mapBoundsMax = max;
            mapBoundsMin = min;
            wayPoints = points;
            IsSuccess = points.Count > 0;
        }

        public int GetRemainCount()
        {
            return wayPoints.Count;
        }

        private Vector3 V2IntToV3(Vector2Int id)
        {
            return new Vector3(
                mapBoundsMin.x + id.x, 
                0, 
                mapBoundsMin.y + id.y
            );
        }

        public bool TryGetNextPosition(out Vector3 position)
        {
            bool isSuccess = false;
            if(wayPoints.TryPop(out var point))
            {
                position = V2IntToV3(point);    
            }   
            else
                position = Vector3.zero;
            return isSuccess;
        }


    }
}
