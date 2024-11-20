using System;
using System.Collections.Generic;
using Lost.Render.Runtime;
using UnityEngine;
using Random = System.Random;

namespace Lost.Runtime.Footstone.Game
{


    public class MapCreator
    {
        public const int Wall = GameConstant.Wall;
        public const int Way = GameConstant.Way;
        public const int Unknow = GameConstant.Unknow;

        private const int invalidDistance = -1;
        private const int waitCheck = -2;
        private const int hasChecked = -1;

        
        
        //地图标识颜色。。

        private static readonly Color color_way             = new(255 / 255.0f, 255 / 255.0f, 255 / 255.0f, 1.0f);
        private static readonly Color color_wall            = new(0 / 255.0f, 0 / 255.0f, 0 / 255.0f, 1.0f);
        private static readonly Color color_unknow          = new(127 / 255.0f, 127 / 255.0f, 127 / 255.0f, 1.0f);
        private static readonly Color color_startPoint      = new(255 / 255.0f, 127 / 255.0f, 0 / 255.0f, 1.0f);
        private static readonly Color color_endPoint        = new(255 / 255.0f, 255 / 255.0f, 0 / 255.0f, 1.0f);
        private static readonly Color color_interactive     = new(210 / 255.0f, 128 / 255.0f, 76 / 255.0f, 1.0f);
        private static readonly Color color_monster         = new(255 / 255.0f, 0 / 255.0f, 0 / 255.0f, 1.0f);
        private static readonly Color color_none            = new(0 / 255.0f, 0 / 255.0f, 0 / 255.0f, 0.0f);
        private static readonly Color color_hidden          = new(255 / 255.0f, 0 / 255.0f, 128 / 255.0f, 1.0f);
        //无道具的道路
        private static readonly Color color_way_open        = new(168 / 255.0f, 255 / 255.0f, 168 / 255.0f, 1.0f);
        private static readonly Color color_monster_water   = new(127 / 255.0f, 0 / 255.0f, 0 / 255.0f, 1.0f);
        //作为区域起点的墙体
        private static readonly Color color_wall_area_start = new(0 / 255.0f, 64 / 255.0f, 64 / 255.0f, 1.0f);
        private static readonly float colorError            = 0.05f;

        private static Dictionary<int, int> _blockWeightDic;
        private static Dictionary<int, int> blockWeightDic
        {
            get
            {
                if (_blockWeightDic == null)
                {
                    _blockWeightDic = GetBlockWeightDic(false);
                }
                return _blockWeightDic;
            }
        }

        private static Dictionary<int, int> _fixPointBlockWeightDic;
        private static Dictionary<int, int> fixPointBlockWeightDic
        {
            get
            {
                if (_fixPointBlockWeightDic == null)
                {
                    _fixPointBlockWeightDic = GetBlockWeightDic(true);
                }
                return _fixPointBlockWeightDic;
            }
        }

        


        private static List<int> cacheList = new List<int>();
        private static int[] GetNeighbors(int id, int width, int height)
        {
            var x = id % width;
            var y = id / width;
            cacheList.Clear();
            if (x > 0) cacheList.Add(id - 1);
            if (x < width - 1) cacheList.Add(id + 1);
            if (y > 0) cacheList.Add(id - width);
            if (y < height - 1) cacheList.Add(id + width);
            return cacheList.ToArray();
        }




        #region 创建地图

        /// <summary>
        /// 创建地图
        /// 0:墙 1:路 3:墙或路
        /// </summary>
        /// <param name="grids"></param>
        /// <param name="start"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static bool TryCreateSimpleMap(MapGrid[] grids, int start, List<int> areaStartIdList, int width, int height, int seed)
        {

            var random = new Random(seed);

            var isSuccsss = true;
            var temp = new MapGrid[width * height];
            for (int y = 0; y < height - 1; y++)
            {
                if (!isSuccsss) break;
                for (int x = 0; x < width - 1; x++)
                {
                    var id = y * width + x;
                    var neighbors = new int[4]
                    {
                        id, id + 1, id + width, id + width + 1
                    };
                    var block = new int[]
                    {
                        grids[neighbors[0]].WalkableState,
                        grids[neighbors[1]].WalkableState,
                        grids[neighbors[2]].WalkableState,
                        grids[neighbors[3]].WalkableState,
                    };
                    bool isFixedArea = IsAllPointFixed(neighbors, grids);
                    var blocks = new Vector2Int[0];
                    if(!isFixedArea)
                    {
                        bool isAnyPointFixed = IsAnyPointFixed(neighbors, grids);
                        isSuccsss = TryGetPossibleBlocks(block, out blocks);
                    }
                    if (!isSuccsss) break;
                    var blockList = new List<Vector2Int>(blocks);
                    while (blockList.Count > 0)
                    {
                        var sum = 0;
                        blockList.ForEach(b => sum += b.y);
                        if (sum == 0)
                        {
                            isSuccsss = false;
                            break;
                        }
                        var r = random.Next(0, sum);
                        int targetId = 0;
                        for (targetId = 0; targetId < blockList.Count; targetId++)
                        {
                            r -= blockList[targetId].y;
                            if (r < 0) break;
                        }
                        var targetBlock = blockList[targetId].x;


                        var isLegalBlock = true;
                        Array.Copy(grids, temp, width * height);
                        for (int i = 0; i < neighbors.Length; i++)
                        {
                            var neighborId = neighbors[i];
                            var value = (targetBlock & (1 << i)) >> i;
                            temp[neighborId].WalkableState = value;
                        }

                        var sx_test = System.Math.Max(0, x - 1);
                        var sy_test = System.Math.Max(0, y - 1);
                        var ex_test = System.Math.Min(width, x + 3);
                        var ey_test = System.Math.Min(height, y + 3);
                        isLegalBlock &= BlockTest(sx_test, sy_test, ex_test, ey_test, temp, grids, width);

                        if (isLegalBlock)
                            isLegalBlock &= PassTest(start, areaStartIdList, width, height, temp, grids);

                        if (isLegalBlock)
                        {
                            for (int i = 0; i < neighbors.Length; i++)
                            {
                                var neighborId = neighbors[i];
                                var value = (targetBlock & (1 << i)) >> i;
                                grids[neighborId].WalkableState = value;
                            }
                            blockList.Clear();
                        }
                        else
                        {
                            blockList.RemoveAt(targetId);
                            if (blockList.Count == 0) isSuccsss = false;
                        }
                    }
                    if (!isSuccsss) break;
                }
            }
            return isSuccsss;
        }

        static bool IsAllPointFixed(int[] neighbors, MapGrid[] grids)
        {
            bool isFixed = true;
            for (int i = 0; i < neighbors.Length; i++)
            {
                isFixed &= grids[neighbors[i]].WalkableFixed;
            }
            return isFixed;
        }

        static bool IsAnyPointFixed(int[] neighbors, MapGrid[] grids)
        {
            bool isFixed = false;
            for (int i = 0; i < neighbors.Length; i++)
            {
                isFixed |= grids[neighbors[i]].WalkableFixed;
            }
            return isFixed;
        }

        

        /// <summary>
        /// 检查一片区域的块是否合法
        /// </summary>
        static bool BlockTest(int sx, int sy, int ex, int ey, MapGrid[] temp, MapGrid[] grids, int width)
        {
            bool isSuccsss = true;
            int numX = ex - sx - 1;
            int numY = ey - sy - 1;
            int num = numX * numY;
            for (int i = 0; i < num; i++)
            {
                var id = sx + sy * width + i % numX + i / numX * width;
                var neighbors = new int[4]
                {
                    id, id + 1, id + width, id + width + 1
                };
                var block = new int[]
                {
                    temp[neighbors[0]].WalkableState,
                    temp[neighbors[1]].WalkableState,
                    temp[neighbors[2]].WalkableState,
                    temp[neighbors[3]].WalkableState,
                };
                bool isFixedArea = IsAllPointFixed(neighbors, grids);
                bool isAnyPointFiexd = IsAnyPointFixed(neighbors, grids);
                isSuccsss = isFixedArea || TryGetPossibleBlocks(block, out var res);
                if (!isSuccsss) break;
            }
            return isSuccsss;
        }
        static bool IsOutSide(int id, int total)
        {
            return id < 0 || id >= total;
        }

        //检测所有路是否联通
        static bool PassTest(int start, List<int> areaStartIdList, int width, int height, MapGrid[] temp, MapGrid[] grids)
        {
            var hasChecked = 8;
            var notCheck = 0;
            bool isPass = true;
            var stack = new Stack<int>();
            stack.Push(start);
            temp[start].PassTestChecked = true;
            foreach (var areaStartId in areaStartIdList)
            {
                stack.Push(areaStartId);
                temp[areaStartId].PassTestChecked = true;
            }
            while (stack.Count > 0)
            {
                var sid = stack.Pop();
                var sneighbors = GetNeighbors(sid, width, height);
                foreach (var nid in sneighbors)
                {
                    if (IsOutSide(nid, width * height)) continue;
                    var v = temp[nid];
                    if(v.IsNotPassTest) continue;
                    var isNotWall = v.WalkableState > 0;
                    if (isNotWall && !v.PassTestChecked)
                    {
                        temp[nid].PassTestChecked = true;
                        stack.Push(nid);
                    }
                }
            }


            for (int i = 0; i < temp.Length; i ++)
            {
                var tempGrid = temp[i];
                var isNotWall = tempGrid.WalkableState > 0;
                var isNotChecked = !tempGrid.PassTestChecked;
                var isNotFixed = !grids[i].WalkableFixed;
                var isPassTest = !grids[i].IsNotPassTest;
                if (isNotWall && isNotChecked && isNotFixed && isPassTest)
                {
                    isPass = false;
                    break;
                }
            }

            return isPass;
        }

        private static bool TryGetPossibleBlocks(int[] inputBlock, out Vector2Int[] possibleBlocks)
        {
            List<int> blocks = new List<int>();
            blocks.Add(0);
            for (int i = 0; i < inputBlock.Length; i++)
            {
                int grid = inputBlock[i];
                if (grid > 0)
                {
                    int gridNum = blocks.Count;
                    for (int j = 0; j < gridNum; j++)
                    {
                        if (grid == 1)
                        {
                            blocks[j] |= 1 << i;
                        }
                        else if (grid == 3)
                        {
                            blocks.Add(blocks[j] | (1 << i));
                        }
                    }
                }
            }
            int blockCount = blocks.Count;
            possibleBlocks = new Vector2Int[blockCount];
            int sumWeight = 0;
            var dic = blockWeightDic;
            for (int i = 0; i < blockCount; i++)
            {
                int block = blocks[i];
                if (!dic.TryGetValue(block, out var weight))
                {
                    weight = 0;
                }
                sumWeight += weight;
                possibleBlocks[i] = new Vector2Int(block, weight);
            }
            return sumWeight > 0;
        }

        //2x2 grid = block
        //2 3
        //0 1 
        private static Dictionary<int, int> GetBlockWeightDic(bool hasFixedPoint)
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();
            dic[Convert.ToInt32("1110", 2)] = 100;
            dic[Convert.ToInt32("1101", 2)] = 100;
            dic[Convert.ToInt32("1011", 2)] = 100;
            dic[Convert.ToInt32("0111", 2)] = 100;

            dic[Convert.ToInt32("1100", 2)] = 100;
            dic[Convert.ToInt32("1010", 2)] = 100;
            dic[Convert.ToInt32("1001", 2)] = 0;
            dic[Convert.ToInt32("0110", 2)] = 0;
            dic[Convert.ToInt32("0101", 2)] = 100;
            dic[Convert.ToInt32("0011", 2)] = 100;

            dic[Convert.ToInt32("1000", 2)] = 40;
            dic[Convert.ToInt32("0100", 2)] = 40;
            dic[Convert.ToInt32("0010", 2)] = 40;
            dic[Convert.ToInt32("0001", 2)] = 40;

            dic[Convert.ToInt32("1111", 2)] = 100;
            // if(hasFixedPoint)
            // {
            //     //dic[Convert.ToInt32("0000", 2)] = 30;
            //     dic[Convert.ToInt32("1111", 2)] = 30;
            // }
            //dic[Convert.ToInt32("1111", 2)] = 1;
            return dic;
        }
        
        private static float ColorDistance(Color c1, Color c2)
        {
            return Vector3.Distance(
                new Vector3(c1.r, c1.g, c1.b) * c1.a,
                new Vector3(c2.r, c2.g, c2.b) * c2.a
            ) + Mathf.Abs(c1.a - c2.a);
        }

        public static bool TryReadMapTexture(Texture2D tex, out int width, out int height, out int start, out List<int> areaStartIdList, out int end, out MapGrid[] grids)
        {
            width = tex.width;
            height = tex.height;
            var num = width * height;
            start = -1;
            end = -1;
            grids = new MapGrid[num];
            areaStartIdList = new();
            bool isSuccess = false;
            if(tex.isReadable)
            {
                isSuccess = true;
                var pixels = tex.GetPixels();
                for (int i = 0; i < pixels.Length; i++)
                {
                    var pixelId = i;
                    var pixel = pixels[pixelId];
                    int grid = -1;
                    bool isWalkableFixed = false;
                    int unitId = 0;
                    bool isNone = false;
                    bool isUnitFixed = false;
                    bool isNotCheckPass = false;
                    if(ColorDistance(pixel, color_wall) < colorError)
                    {
                        isWalkableFixed = true;
                        grid = Wall;   
                    }
                    else if(ColorDistance(pixel, color_way) < colorError)
                    {
                        isWalkableFixed = true;
                        grid = Way;   
                    }
                    else if(ColorDistance(pixel, color_unknow) < colorError)
                    {
                        isWalkableFixed = false;
                        grid = Unknow;
                    }
                    else if(ColorDistance(pixel, color_startPoint) < colorError)
                    {
                        isWalkableFixed = true;
                        unitId = GameConstant.FixedPointUnit;
                        grid = Way;
                        start = i;
                    }
                    else if(ColorDistance(pixel, color_endPoint) < colorError)
                    {
                        isWalkableFixed = true;
                        isNotCheckPass = true;
                        unitId = GameConstant.FixedPointUnit;
                        grid = Way;
                        end = i;
                    }
                    else if(ColorDistance(pixel, color_interactive) < colorError)
                    {
                        isWalkableFixed = true;
                        grid = Way;
                        unitId = GameConstant.FixedInteractiveUnit;
                    }
                    else if(ColorDistance(pixel, color_monster) < colorError)
                    {
                        isWalkableFixed = true;
                        grid = Way;
                        unitId = GameConstant.FixedMonsterUnit;
                    }
                    else if(ColorDistance(pixel, color_monster_water) < colorError)
                    {
                        isWalkableFixed = true;
                        grid = Way;
                        isNone = true;
                        unitId = GameConstant.FixedMonsterUnit;
                    }
                    else if(ColorDistance(pixel, color_none) < colorError)
                    {
                        isWalkableFixed = true;
                        grid = Wall;   
                        isNone = true;
                    }
                    else if(ColorDistance(pixel, color_hidden) < colorError)
                    {
                        isWalkableFixed = true;
                        grid = Way;   
                        isNone = true;
                        unitId = GameConstant.FixedPointUnit;
                    }
                    else if(ColorDistance(pixel, color_way_open) < colorError)
                    {
                        isWalkableFixed = true;
                        grid = Way;   
                        isNone = false;
                        unitId = GameConstant.FixedEmptyUnit;
                    }
                    else if(ColorDistance(pixel, color_wall_area_start) < colorError)
                    {
                        isWalkableFixed = true;
                        grid = Wall; 
                        areaStartIdList.Add(i); 
                    }
                    else
                    {
                        Debug.LogError($"无法识别颜色 ({i % width}, {i / width}) : {pixel}");
                        isSuccess = false;
                        break;
                    }
                    
                    grids[i].WalkableState = grid;
                    grids[i].WalkableFixed = isWalkableFixed;
                    grids[i].UnitId = unitId;
                    grids[i].IsNone = isNone;
                    grids[i].IsNotPassTest = isNotCheckPass;
                    //grids[i].UnitFixed = isUnitFixed;
                }
            }

            if(start < 0 || end < 0)
                isSuccess = false;
            return isSuccess;
        }

        public static int[] GetOriginGrids(int width, int height, int start)
        {
            int num = width * height;
            int[] grids = new int[num];
            for (int i = 0; i < num; i++)
            {
                bool isSide = i % width == 0 || i % width == width - 1 ||
                    i / width == 0 || i / width == height - 1;

                bool isStart = i == start;
                grids[i] = isStart ? Way : (isSide ? Wall : Unknow);
            }
            return grids;
        }

        #endregion

        static bool IsCanCreateBarrier(int i, MapGrid[] grids)
        {
            var grid = grids[i];
            return grid.WalkableState == GameConstant.Way && 
            grid.UnitId == GameConstant.EmptyUnit && 
            !grid.IsNone;
        }
        
        //被可摧毁物体占领的空点
        static bool IsDestoryableUnitPoint(int i, MapGrid[] grids)
        {
            var grid = grids[i];
            return grid.WalkableState == GameConstant.Way && 
            grid.UnitId != GameConstant.EmptyUnit && 
            grid.UnitId != GameConstant.BarrierUnit &&
            !grid.IsNone;
        }

        static bool IsNotBarrierPoint(int i, MapGrid[] grids)
        {
            var grid = grids[i];
            return grid.WalkableState == GameConstant.Way && 
            grid.UnitId != GameConstant.BarrierUnit && 
            !grid.IsNone;
        }

        static bool IsCanCreateUnit(int i, MapGrid[] grids)
        {
            var grid = grids[i];
            return grid.WalkableState == GameConstant.Way && 
            grid.UnitId == GameConstant.EmptyUnit && 
            !grid.IsNone;
        }

        #region 创建地图中的单位
        /// <summary>
        /// 创建地图中的单位
        /// </summary>
        /// <param name="grids">0:障碍 1:可行走</param>
        /// <param name="monsterDensity">每个区域的怪兽密度</param>
        public static void CreateMapUnits(MapGrid[] grids, int width, int height, int start, int end, int seed, int minAreaGridNum, int maxAreaGridNum, float monsterDensity, float pickableDensity)
        {
            var random = new Random(seed);
            //先计算所有位置到起点的位置
            var num = width * height;
            // int[] distances = new int[num];
            // int maxDis = 0;
            // {
            //     for (int i = 0; i < num; i++)
            //     {
            //         distances[i] = waitCheck;
            //     }
            //     var buffer0 = new List<int>();
            //     var buffer1 = new List<int>();
            //     buffer0.Add(start);
            //     int currentDis = 0;

            //     while (buffer0.Count > 0)
            //     {
            //         foreach (var id in buffer0)
            //         {
            //             distances[id] = currentDis;
            //         }
            //         currentDis += 1;
            //         foreach (var id in buffer0)
            //         {
            //             var neighbors = GetNeighbors(id, width, height);
            //             foreach (var neighborId in neighbors)
            //             {
            //                 if (grids[neighborId].WalkableState == Way && distances[neighborId] == waitCheck)
            //                 {
            //                     distances[neighborId] = hasChecked;
            //                     buffer1.Add(neighborId);
            //                 }
            //             }
            //         }

            //         //交换buffer
            //         buffer0.Clear();
            //         var temp = buffer0;
            //         buffer0 = buffer1;
            //         buffer1 = temp;
            //     }

            //     maxDis = Math.Max(currentDis - 1, 0);
            // }


            //开始根据距离划分区域
            int[] gridAreaIndices = new int[num];
            var areaGridNums = new List<int>();
            //初步分割
            int segmentCount = 1;       
            {
                // int t = maxDis / segmentCount;
                // if (t * segmentCount < maxDis) t += 1;
                
                for (int i = 0; i < num; i++)
                {
                    //重置区域Id
                    gridAreaIndices[i] = waitCheck;

                    // //设置障碍点
                    // var distance = distances[i];
                    // int areaId = distance / t;
                    // //设置障碍点
                    // if (areaId > 0 && areaId < segmentCount && areaId * t == distance)
                    // {
                    //     grids[i].UnitId = GameConstant.BarrierUnit;
                    // };
                }

                int currentAreaIndex = 0;
                var stack = new Stack<int>();
                for (int i = 0; i < num; i++)
                {
                    var grid = grids[i];
      
                    bool isCheck = IsNotBarrierPoint(i, grids) &&
                    gridAreaIndices[i] == waitCheck;
                    var gridNum = 0;
                    if(isCheck)
                    {
                        stack.Push(i);                       
                        while (stack.Count > 0)
                        {
                            var id = stack.Pop();
                            gridAreaIndices[id] = currentAreaIndex;
                            gridNum++;
                            var neighbors = GetNeighbors(id, width, height);
                            foreach (var neighbor in neighbors)
                            {
                                bool isCheckNeighbor = IsNotBarrierPoint(neighbor, grids) &&
                                gridAreaIndices[neighbor] == waitCheck;
                                if(isCheckNeighbor)
                                {
                                    gridAreaIndices[neighbor] = hasChecked;
                                    stack.Push(neighbor);
                                }
                            }
                        }
                        areaGridNums.Add(gridNum);
                        currentAreaIndex += 1;
                    }               
                }      
            }
         
            //第二次分割
            {
                var currentAreaMaxNum = 0;
                var gridIdCache = new List<int>();
                Dictionary<int, int> maxAreaGridNumDic = new Dictionary<int, int>();
                for (int i = 0; i < num; i++)
                {               
                    bool isEmptyGrid = IsNotBarrierPoint(i, grids);
                    if (!isEmptyGrid) continue;
                    var oldAreaId = gridAreaIndices[i];
                    if(!maxAreaGridNumDic.TryGetValue(oldAreaId, out currentAreaMaxNum))
                    {
                        currentAreaMaxNum = random.Next(minAreaGridNum, maxAreaGridNum);
                        maxAreaGridNumDic[oldAreaId] = currentAreaMaxNum;
                    }

                    bool isSuperArea = areaGridNums[oldAreaId] > currentAreaMaxNum;
                    if (!isSuperArea) continue;
                    //var emptyNeighborCount = 0;
                    //{
                    //    var neighbors = GetNeighbors(i, width, height);
                    //    foreach(var neighbor in neighbors)
                    //    {
                    //        var isEmptyNeighbor = grids[neighbor] == Way && units[neighbor] == GameConstant.EmptyUnit;
                    //        emptyNeighborCount += isEmptyNeighbor ? 1 : 0;                      
                    //    }
                    //}
                    var isStartPoint = true;
                    if (!isStartPoint) continue;
                    var areaGridNum = currentAreaMaxNum;
                    //开始分割区域
                    gridIdCache.Add(i);
                    var queue = new Queue<int>();
                    queue.Enqueue(i);
                    while (queue.Count > 0)
                    {
                        var id = queue.Dequeue();
                        var neighbors = GetNeighbors(id, width, height);
                        foreach (var neighbor in neighbors)
                        {
                            var isEmptyNeighbor = IsNotBarrierPoint(neighbor, grids);
                            var isNotContains = !gridIdCache.Contains(neighbor);
                            if(isEmptyNeighbor && isNotContains)
                            {
                                gridIdCache.Add(neighbor);
                                queue.Enqueue(neighbor);

                                if (gridIdCache.Count == areaGridNum) break;
                            }
                        }

                        if (gridIdCache.Count == areaGridNum)
                        {
                            break;
                        }
                    }
                    //分配新区域 生成新障碍
                    if(gridIdCache.Count == areaGridNum)
                    {
                        
                        //gridIdCache.ForEach(x => gridAreaIndices[x] = waitCheck);
                        //for (int j = gridIdCache.Count - 1; j >= 0; j--)
                        for (int j = 0; j < gridIdCache.Count; j++)
                        {
                            var gridId = gridIdCache[j];
                            var neighbors = GetNeighbors(gridId, width, height);
                            foreach (var neighbor in neighbors)
                            {
                                var isCanCreateBarrier = IsCanCreateBarrier(neighbor, grids);
                                var isOldAreaGrid = !gridIdCache.Contains(neighbor);
                                //设置为障碍
                                if (isCanCreateBarrier && isOldAreaGrid)
                                {
                                    grids[neighbor].UnitId = GameConstant.BarrierUnit;
                                }
                                else if(!isCanCreateBarrier && isOldAreaGrid)
                                {
                                    var isNotBarrierPoint = IsDestoryableUnitPoint(neighbor, grids);
                                    if(isNotBarrierPoint)
                                    {
                                        gridIdCache.Add(neighbor);
                                    }
                                }
                            }
                        }
                        gridIdCache.Clear();
                        areaGridNums[oldAreaId] = 0;
                        //设置新区域
                        for (int j = 0; j < num; j++)
                        {
                            var isNotBarrierUnit = IsNotBarrierPoint(j, grids);
                            var isOldArea = gridAreaIndices[j] == oldAreaId;
                            if (isNotBarrierUnit && isOldArea)
                            {
                                var newAreaId = areaGridNums.Count;
                                var newAreaGridNum = 0;
                                var stack = new Stack<int>();
                                stack.Push(j);
                                while (stack.Count > 0)
                                {
                                    var id = stack.Pop();
                                    gridAreaIndices[id] = newAreaId;
                                    newAreaGridNum++;
                                    var neighbors = GetNeighbors(id, width, height);
                                    foreach (var neighbor in neighbors)
                                    {
                                        var isNotBarrierNeighbor = IsNotBarrierPoint(neighbor, grids);
                                        var isNeighborOldArea = gridAreaIndices[neighbor] == oldAreaId;
                                        if (isNotBarrierNeighbor && isNeighborOldArea)
                                        {
                                            stack.Push(neighbor);
                                            gridAreaIndices[neighbor] = hasChecked;
                                        }
                                    }
                                }
                                areaGridNums.Add(newAreaGridNum);
                            }
                        }
                    }
                    
                }
            }
            var areaCount = areaGridNums.Count;
            var areaGridIdLists = new List<int>[areaCount];
            for (int i = 0; i < areaCount; i++)
            {
                areaGridIdLists[i] = new List<int>(areaGridNums[i]);
            }
            //TODO:放置宝物,放置怪物.....
            {
                var startPointAreaId = gridAreaIndices[start];
                for (int i = 0; i < num; i++)
                {
                    var isCanCreateUnit = IsCanCreateUnit(i, grids);
                    if (!isCanCreateUnit) continue;
                    var areaId = gridAreaIndices[i];
                    areaGridIdLists[areaId].Add(i);
                }
                //创建随机怪物
                CreateRadomUnit(startPointAreaId, areaGridIdLists, monsterDensity, random, grids, GameConstant.MonsterUnit);
                //创建随机道具
                CreateRadomUnit(startPointAreaId, areaGridIdLists, pickableDensity, random, grids, GameConstant.InteractiveUnit, 
                (gridId, grids) =>
                {
                    bool isPass = gridId != start && gridId != end;
                    if(isPass)
                    {
                        var neighbors = GetNeighbors(gridId, width, height);
                        var wallCount = 0;
                        for (int i = 0; i < neighbors.Length; i++)
                        {
                            var grid = grids[neighbors[i]];
                            var isWall = grid.WalkableState == GameConstant.Wall;
                            wallCount += isWall ? 1 : 0;
                        }
                        isPass = wallCount == 3;
                    }
                    return isPass;
                });
            }

        }

        /// <summary>
        /// 根据参数创建随机的单位
        /// </summary>
        private static void CreateRadomUnit(int startPointAreaId, List<int>[] areaGridIdLists, float density, System.Random random, MapGrid[] grids, int unitValue, Func<int, MapGrid[], bool> filterFunc = null)
        {
            var areaCount = areaGridIdLists.Length;
            
            for (int i = 0; i < areaCount; i++)
            {
                var areaId = i;
                //初始区域没怪
                if (areaId == startPointAreaId) continue;
                var gridIdList = areaGridIdLists[areaId];
                var areaGridNum = gridIdList.Count;
                if (areaGridNum == 0) continue;
                var p = (areaGridNum) * density;
                var f = p - (int)p;
                var targetUnitCount = (int)p + (f > random.NextDouble() ? 1 : 0);
                var currentUnitCount = 0;

                for (int j = 0; j < gridIdList.Count; j++)
                {
                    var gridId = gridIdList[j];
                    var grid = grids[gridId];
                    if(grid.UnitId == unitValue)
                    {
                        currentUnitCount ++;
                    }
                }
                
                for (int j = 0; j < areaGridNum; j++)
                {
                    if(currentUnitCount >= targetUnitCount)
                    {
                        break;
                    }
                    var r = random.Next(j, areaGridNum);
                    var gridId = gridIdList[r];
                    var grid = grids[gridId];
                    bool isPass = grid.UnitId == GameConstant.EmptyUnit && !grid.UnitFixed && (filterFunc?.Invoke(gridId, grids) ?? true);
                    if(isPass)
                    {
                        grids[gridId].UnitId = unitValue;
                        currentUnitCount ++;
                    }
                    //将抽取过的Id移到列表头部
                    gridIdList[r] = gridIdList[j];
                    gridIdList[j] = gridId;
                }
            }
        }

        /// <summary>
        /// 将0 ~ n 打乱（n = count - 1）
        /// </summary>
        /// <param name="count">数组长度</param>
        /// <param name="random"></param>
        /// <returns></returns>
        public static int[] CreateRandomArray(int count, Random random)
        {
            int[] nums = new int[count];
            for (int i = 0; i < count; i++)
            {
                nums[i] = i;
            }
            for (int i = 0; i < count; i++)
            {
                var r = random.Next(i, count);
                var temp = nums[r];
                nums[r] = nums[i];
                nums[i] = temp;
            }
            return nums;
        }

        #endregion

    }
}
