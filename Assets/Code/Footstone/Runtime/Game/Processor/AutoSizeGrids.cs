using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    //不可序列化
    public class AutoSizeGrids<T> 
    {
        public T[] Grids;
        private T[] tempGrids;
        private int planeY = 0;
        public int Width;
        public int Height; 
        public int Count { get => Width * Height; }
        public Vector2Int MapBoundsMax = Vector2Int.one * int.MinValue;
        public Vector2Int MapBoundsMin = Vector2Int.one * int.MaxValue;
        private int ex = 4;

        public AutoSizeGrids()
        {
            Grids = new T[0];
            tempGrids = new T[0];
        }

        //当元素只剩1个（存在跨场景物体时）或0个*时可以调用clear将尺寸缩小
        public void Clear()
        {   
            MapBoundsMax = Vector2Int.one * int.MinValue;
            MapBoundsMin = Vector2Int.one * int.MaxValue;
            Width = 0;
            Height = 0;
            System.Array.Clear(Grids, 0, Grids.Length);
        }

        public Vector2Int ConvertPositionToV2Int(Vector3 position)
        {
            return new Vector2Int(
                Mathf.FloorToInt(position.x + 0.5f),
                Mathf.FloorToInt(position.z + 0.5f)
            );
        }

        public Vector2Int ConvertPositionToIndex2(Vector3 position)
        {
            return new Vector2Int(
                Mathf.FloorToInt(position.x + 0.5f) - MapBoundsMin.x,
                Mathf.FloorToInt(position.z + 0.5f) - MapBoundsMin.y
            );
        }

        public int ConvertPositionToIndex(Vector3 position)
        {
            var x = Mathf.FloorToInt(position.x + 0.5f) - MapBoundsMin.x;
            var y = Mathf.FloorToInt(position.z + 0.5f) - MapBoundsMin.y;
            return x + y * Width;
        }
        public Vector3 ConvertV2IntIndexToPosition(Vector2Int id)
        {
            return new Vector3(
                MapBoundsMin.x + id.x, 
                planeY, 
                MapBoundsMin.y + id.y
            );
        }

        public Vector3 ConvertIndexToPosition(int id)
        {
            return new Vector3(
                MapBoundsMin.x + id % Width, 
                planeY, 
                MapBoundsMin.y + id / Width
            );
        }

        /// <summary>
        /// 改变地图尺寸 保留地图数据
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public bool TryChangeMapBounds(Vector2Int min, Vector2Int max)
        {
            bool isBoundsChange = max.x > MapBoundsMax.x || max.y > MapBoundsMax.y || 
                min.x < MapBoundsMin.x || min.y < MapBoundsMin.y;
            if(isBoundsChange)
            {
                var originMin = MapBoundsMin;
                var originWidth = Width;
                var originCount = Width * Height;
                var capacity = Grids.Length;
                //防止频繁的扩大
                max.x += max.x > MapBoundsMax.x ? ex : 0;
                max.y += max.y > MapBoundsMax.y ? ex : 0;
                min.x -= min.x < MapBoundsMin.x ? ex : 0;
                min.y -= min.y < MapBoundsMin.y ? ex : 0;
                
                MapBoundsMax = max;
                MapBoundsMin = min;
                Width = MapBoundsMax.x - MapBoundsMin.x;
                Height = MapBoundsMax.y - MapBoundsMin.y;
                var currentCount = Width * Height;
                //if (currentCount > capacity)
                {
                    if(tempGrids.Length < originCount)
                    {
                        tempGrids = new T[originCount];
                    }
                    System.Array.Copy(Grids, tempGrids, originCount);
                    if(currentCount > capacity)
                    {
                        Grids = new T[currentCount];
                    }
                    else
                    {
                        System.Array.Clear(Grids, 0, originCount);
                    }
                    for (int i = 0; i < originCount; i++)
                    {
                        var v = tempGrids[i];
                        if(v != null)
                        {
                            var x = i % originWidth + originMin.x - MapBoundsMin.x;
                            var y = i / originWidth + originMin.y - MapBoundsMin.y;
                            var target = x + y * Width;
                            Grids[target] = v;
                        }
                    }
                }
                // else
                // {
                //     System.Array.Clear(Grids, defaultValue, currentCount);
                // }
                //OnBoundsChangeEvent?.Invoke(this);
            }
            return isBoundsChange;
        }
        public void Set(Vector3 position, T value)
        {
            var id = ConvertPositionToIndex(position);
            Grids[id] = value;
        }

        /// <summary>
        /// 没有任何检查！需要自己判断是否过界
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns> <summary>
        public T Get(Vector3 position)
        {
            var id = ConvertPositionToIndex(position);
            return Grids[id];
        }

        public bool IsOutSide(Vector3 position)
        {
            var x = Mathf.FloorToInt(position.x + 0.5f) - MapBoundsMin.x;
            var y = Mathf.FloorToInt(position.z + 0.5f) - MapBoundsMin.y;
            return x < 0 || x >= Width || y < 0 || y >= Height;
        }

        public bool TryGet(Vector3 position, out T value)
        {
            bool isGet = !IsOutSide(position);
            if(isGet) value = Get(position);
            else value = default;
            return isGet;
        }

        public void CalculateMapBounds(out Vector2Int min, out Vector2Int max,  params Vector3[] positions)
        {
            min = MapBoundsMin;
            max = MapBoundsMax;
            foreach (var position in positions)
            {
                var pos = ConvertPositionToV2Int(position);
                max = Vector2Int.Max(max, pos + Vector2Int.one);
                min = Vector2Int.Min(min, pos);
            }
        }

        public void CalculateBounds(ref Vector2Int min, ref Vector2Int max,  Vector3 position)
        {
            var pos = ConvertPositionToV2Int(position);
            max = Vector2Int.Max(max, pos + Vector2Int.one);
            min = Vector2Int.Min(min, pos);
        }
    }
}
