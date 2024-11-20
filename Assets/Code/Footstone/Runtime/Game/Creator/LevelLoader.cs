
using LitJson;
using Lost.Render.Runtime;
using Lost.Runtime.Footstone.Core;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = System.Random;

namespace Lost.Runtime.Footstone.Game
{
    public class RandomUnit
    {
        public string Key;
        // public Entity Prefab;
        private int weight;
        public int Weight { get => weight; set => weight = Math.Max(0, value); }
        public int WeightCut;
    }



    public class LevelLoader
    {
        private IServiceRegistry service;
        private ContentManager content;
        private SceneSystem sceneSystem;
        private ResPoolManager resPoolManager;
        private InteractiveRecorderProcessor interactiveRecorderProcessor;
        private PlotConfigProcessor plotConfigProcessor;
        private string levelKey;
        private string tileJsonName;
        private Vector2Int origin = Vector2Int.zero;

        public LevelLoader(IServiceRegistry _service, Vector2Int _origin, string _level)
        {
            sceneSystem = _service.GetService<SceneSystem>();
            content = _service.GetService<ContentManager>();
            resPoolManager = _service.GetService<ResPoolManager>();
            interactiveRecorderProcessor = sceneSystem.SceneInstance.ForceGetProcessor<InteractiveRecorderProcessor>();
            plotConfigProcessor = sceneSystem.SceneInstance.ForceGetProcessor<PlotConfigProcessor>();
            origin = _origin;
            levelKey = _level;
        }


        public void LoadLevel()
        {
            var levelConfig = resPoolManager.LoadResConfigData(ResFlag.Text_Level)[levelKey];
            bool isWorld = levelConfig.ContainsKey(JsonKeys.world) && (bool)levelConfig[JsonKeys.world];
            if(isWorld)
                CreateWorld();
            else
                CreateLevel();
        }

        private Vector3 IndexToPos(int id, int width, int height, Vector2 offset)
        {
            float x = origin.x + id % width;
            float y = 0;
            float z = origin.x + id / width;
            Vector3 pos = new Vector3(x, y, z);
            pos.x += offset.x;
            pos.z += offset.y;
            return pos;
        }

        private Vector3 IndexToPos(int id, int width, int height)
        {
            return IndexToPos(id, width, height, Vector2.zero);
        }



        private RandomUnit[] GetRandomUnitPrefabs(JsonData mapData, string key)
        {
            var unitDatas = mapData[key];
            var count = unitDatas.Count;
            var res = new RandomUnit[count];
            for (int i = 0; i < count; i++)
            {
                var unitData = unitDatas[i];
                var weight = (int)unitData[JsonKeys.weight];
                var weightCut = (int)unitData[JsonKeys.weightCut];
                var unitKey = unitData[JsonKeys.name].ToString();
                //var url = configData[unitData[JsonKeys.name].ToString()][JsonKeys.url].ToString();
                //var prefab = content.LoadEntity(url);
                res[i] = new RandomUnit()
                {
                    Key = unitKey,
                    Weight = weight,
                    WeightCut = weightCut
                };
            }
            return res;
        }

        public Entity InstantiateFixedUnit(string key, ResFlag flag, Vector3 pos)
        {

            var entity = resPoolManager.InstantiateEntity(key, flag);
            entity.Transform.Position = pos;
            return entity;
        }
        /// <summary>
        /// 创建随机单位 可能返回空...
        /// </summary>
        private Entity InstantiateRandomUnit(RandomUnit[] units, Random random, Vector3 pos, ResFlag flag)
        {
            int total = 0;
            foreach (var unit in units)
            {
                //防止权重被降低成负数
                unit.Weight = Mathf.Max(unit.Weight, 0);
                total += unit.Weight;
            }
            var value = random.Next(0, total);
            Entity res = null;
            foreach (var unit in units)
            {
                value -= unit.Weight;
                if (value < 0)
                {
                    //降低自身权重
                    unit.Weight -= unit.WeightCut;
                    //static collider 不能在加入场景后在设置位置
                    res = resPoolManager.InstantiateEntity(unit.Key, flag);
                    res.Transform.Position = pos;
                    //res.InvokeEntityInstantiateCallback();
                    //sceneSystem.SceneInstance.RootScene.Entities.Add(res);
                    break;
                }
            }
#if UNITY_EDITOR
            if (res == null) throw new Exception("产生了空单位 检查配置");
#endif
            return res;
        }

        private int CalculateWeight(int x, int y, int centerX, int centerY)
        {
            var dis = Vector2.Distance(new Vector2(x, y), new Vector2(centerX, centerY));
            var weight = Mathf.CeilToInt(100f / Mathf.Sqrt(dis));
            return weight;
        }

        private void LoadConfigEntity(JsonData levelData)
        {
            string[] keys;
            if(levelData.ContainsKey(JsonKeys.entity_config))
            {
                keys = LitJsonUtil.ToStringArray(levelData[JsonKeys.entity_config]);
            }
            else
            {
                keys = new string[0]; 
            }

            foreach (var key in keys)
            {
                resPoolManager.InstantiateEntity(key, ResFlag.Entity_LevelConfig);
            }
        }

        //生成所有的关卡传送点
        private void CreateWorld()
        {
            var data = resPoolManager.LoadJsonData(levelKey, ResFlag.Text_Level);
            LoadConfigEntity(data);
            var levels = data[JsonKeys.levels];
            var minDis = (int)data[JsonKeys.minDistance];
            var maxDis = (int)data[JsonKeys.maxDistance];
            var width = (int)data[JsonKeys.width];
            var height = (int)data[JsonKeys.height];
            var center = new Vector2Int(
                (int)data[JsonKeys.center][JsonKeys.x],
                (int)data[JsonKeys.center][JsonKeys.y]
            );
            var seed = (int)data[JsonKeys.seed];
            var random = new Random(seed);
            var disCount = Mathf.Max(maxDis - minDis + 1, 0);
            var num = width * height;
            var disArray = new int[num];
            //var suitIdList = new List<int>();
            var suitPointDic = new Dictionary<int, int>();
            var totalWeight = 0;

            if(data.ContainsKey(JsonKeys.tileData))
            {
                var tileDataKey = data[JsonKeys.tileData].ToString();
                //配置一下关卡的plotConfig
                plotConfigProcessor.SingleComponent.SetTileConfig(levelKey, tileDataKey);
            }

            for (int i = 0; i < num; i++)
            {
                var dis = Mathf.Abs(center.x - i % width) + Mathf.Abs(center.y - i / width);
                if (dis >= minDis && dis <= maxDis)
                {
                    var weight = CalculateWeight(i % width, i / width, center.x, center.y);
                    suitPointDic[i] = weight;
                    totalWeight += weight;
                }
                disArray[i] = dis;
            }

            var levelNum = levels.Count;
            for (int i = 0; i < levelNum; i++)
            {
                var levelData = levels[i];
                var targetId = -1;
                var levelPointKey = "";
                if(levelData.IsObject)
                {
                    var x = (int)levelData[JsonKeys.pos][JsonKeys.x];
                    var y = (int)levelData[JsonKeys.pos][JsonKeys.y];
                    targetId = x + y * width;
                    levelPointKey = (string)levelData[JsonKeys.name];
                }
                else if(totalWeight > 0)
                {
                    levelPointKey = (string)levelData;
                    var randomValue = random.Next(totalWeight);
                    foreach (var kvp in suitPointDic)
                    {
                        randomValue -= kvp.Value;
                        if(randomValue < 0)
                        {
                            targetId = kvp.Key;
                            break;
                        }
                    }
                }
                if(targetId > 0)
                {
                    var targetX = targetId % width;
                    var targetY = targetId / width;
                    var startX = targetX - maxDis;
                    var endX = targetX + maxDis + 1;
                    var startY = targetY - maxDis;
                    var endY = targetY + maxDis + 1;
                    startX = Mathf.Max(startX, 0);
                    endX = Mathf.Min(endX, width);
                    startY = Mathf.Max(startY, 0);
                    endY = Mathf.Min(endY, height);
                    for (int x = startX; x < endX; x++)
                    {
                        for (int y = startY; y < endY; y++)
                        {
                            var id = x + y * width;
                            var dis = Mathf.Abs(x - targetX) + Mathf.Abs(y - targetY);
                            var originDis = disArray[id];
                            if(dis < originDis)
                            {
                                bool isInList = originDis >= minDis && originDis <= maxDis;
                                bool isAdd = dis >= minDis && dis <= maxDis;
                                disArray[id] = dis;
                                if(!isInList && isAdd)
                                {
                                    var weight = CalculateWeight(x, y, center.x, center.y);
                                    suitPointDic[id] = weight; 
                                    totalWeight += weight;
                                }
                                else if(isInList && !isAdd)
                                {
                                    totalWeight -= suitPointDic[id];
                                    suitPointDic.Remove(id);
                                }
                            }
                        }
                    }

                    var pos = new Vector3(
                        origin.x + targetId % width - center.x,
                        0,
                        origin.y + targetId / width - center.y
                    );
                    var entity = resPoolManager.InstantiateEntity(levelPointKey, ResFlag.Entity_Point);
                    entity.Transform.Position = pos;
                }
            }
        }

        private void CreateLevel()
        {
            var data = resPoolManager.LoadJsonData(levelKey, ResFlag.Text_Level);
 
            
            int width, height, start, end, mapSeed, minAreaGridNum, maxAreaGridNum;
            double monsterDensity, pickableDensity;
            int unitSeed, prefabSeed;
            Texture2D mapTex = null;
            try
            {
                // width = ((int)data[JsonKeys.width]);
                // height = ((int)data[JsonKeys.height]);
                // start = ((int)data[JsonKeys.start]);
                var mapTexKey = (string)data[JsonKeys.mapTex];
                mapTex = resPoolManager.LoadResWithKey<Texture2D>(mapTexKey, ResFlag.Texture_Map);
                mapSeed = (int)data[JsonKeys.mapSeed];
                prefabSeed = (int)data[JsonKeys.prefabSeed];
                unitSeed = (int)data[JsonKeys.unitSeed];
                minAreaGridNum = (int)data[JsonKeys.minAreaGridMaxNum];
                maxAreaGridNum = (int)data[JsonKeys.maxAreaGridMaxNum];
                monsterDensity = (double)data[JsonKeys.monsterDensity];
                pickableDensity = (double)data[JsonKeys.interactiveDensity];
            }
            catch (Exception)
            {
                throw new Exception("format error!");
            }
            //加载关卡配置实体
            LoadConfigEntity(data);
            //TODO:增加数组处理地形
            bool isReadSuccess = MapCreator.TryReadMapTexture(mapTex, out width, out height, out start, out var areaStartIdList, out end, out var grids);
            int num = width * height;
            var isSuccess = isReadSuccess && MapCreator.TryCreateSimpleMap(grids, start, areaStartIdList, width, height, mapSeed);

            if (isSuccess)
            {
                //生成预制体使用的随机器
                var prefabRandom = new Random(prefabSeed);
                {
                    var mapKey = data[JsonKeys.mapEnv].ToString();
                    var map = resPoolManager.InstantiateEntity(mapKey, ResFlag.Entity_Env);
                    var pos = IndexToPos(height / 2 * width + width / 2, width, height);
                    map.Transform.Position = pos;
                    //map.InvokeEntityInstantiateCallback();

                }

                // {
                //     var playerKey = data[JsonKeys.player].ToString();
                //     //创建玩家
                //     if (playerKey != null)
                //     {                                 
                //         var player = resPoolManager.InstantiateEntity(playerKey, ResFlag.Entity_Player);
                //         var pos = IndexToPos(start, width, height);
                //         player.Transform.Position = pos;
                //         //player.InvokeEntityInstantiateCallback();
                //     }

                // }
                {
                    var startPointKey = (string)data[JsonKeys.startPoint];
                    if (!string.IsNullOrEmpty(startPointKey))
                    {
                        var startPoint = resPoolManager.InstantiateEntity(startPointKey, ResFlag.Entity_Point);
                        startPoint.Transform.Position = IndexToPos(start, width, height);
                    }
                    var endPointKey = (string)data[JsonKeys.endPoint];
                    if (!string.IsNullOrEmpty(endPointKey))
                    {
                        var endPoint = resPoolManager.InstantiateEntity(endPointKey, ResFlag.Entity_Point);
                        endPoint.Transform.Position = IndexToPos(end, width, height);
                    }
                }

                //创建墙体
                // if(false)
                {
                    var wallPrefabs = GetRandomUnitPrefabs(data, JsonKeys.wall);
                    for (int i = 0; i < num; i++)
                    {
                        var grid = grids[i];

                        if (!grid.IsNone && grid.WalkableState == MapCreator.Wall)
                        {
                            var pos = IndexToPos(i, width, height);
                            var wall = InstantiateRandomUnit(wallPrefabs, prefabRandom, pos, ResFlag.Entity_Wall);
                            wall.Transform.Position = pos;
                        }
                    };
                    // //往起点放个空气墙
                    // {
                    //     var pos = IndexToPos(start, width, height);
                    //     var wall = wallPrefab.Instantiate();
                    //     wall.Transform.Position = pos;
                    // }
                }

                MapCreator.CreateMapUnits(grids, width, height, start, end, unitSeed, minAreaGridNum, maxAreaGridNum, (float)monsterDensity, (float)pickableDensity);   

                //创建固定的地图单位
                {
                    var fixedInteractiveKeys = LitJsonUtil.ToStringArray(data[JsonKeys.fixedInteractive]);
                    var fixedMonsterKeys = LitJsonUtil.ToStringArray(data[JsonKeys.fixedMonster]);
                    var interactiveCount = 0;
                    var monsterCount = 0;
                    for (int i = 0; i < num; i++)
                    {
                        var unit = grids[i].UnitId;
                        if (unit == GameConstant.FixedInteractiveUnit && fixedInteractiveKeys.Length > 0)
                        {
                            var unitkeyId = interactiveCount % fixedInteractiveKeys.Length;
                            var key = fixedInteractiveKeys[unitkeyId];
                            bool isUniqueTriggered = interactiveRecorderProcessor.SingleComponent.IsUniqueTriggered(key);
                            if (!isUniqueTriggered)
                            {
                                var pos = IndexToPos(i, width, height);
                                InstantiateFixedUnit(key, ResFlag.Entity_Interactive, pos);
                            }
                            interactiveCount++;
                        }
                        else if (unit == GameConstant.FixedMonsterUnit && fixedMonsterKeys.Length > 0)
                        {
                            var unitkeyId = monsterCount % fixedMonsterKeys.Length;
                            var key = fixedMonsterKeys[unitkeyId];
                            var pos = IndexToPos(i, width, height);
                            InstantiateFixedUnit(key, ResFlag.Entity_Monster, pos);
                            monsterCount++;
                        }
                    }
                }
                //创建随机的地图单位
                {
                    var barrierPrefabs = GetRandomUnitPrefabs(data, JsonKeys.barrier);
                    var monsterPrefabs = GetRandomUnitPrefabs(data, JsonKeys.monster);
                    var interactivePrefabs = GetRandomUnitPrefabs(data, JsonKeys.interactive);

                    var isCreateBarrier = barrierPrefabs.Length > 0;
                    var isCreateMonster = monsterPrefabs.Length > 0;
                    var isCreateInteractive = interactivePrefabs.Length > 0;

                    //将单位打乱，防止必然生成的物体生成在前面几个空位
                    var randomArray = MapCreator.CreateRandomArray(num, prefabRandom);

                    for (int i = 0; i < num; i++)
                    {
                        var gridId = randomArray[i];
                        var unit = grids[gridId].UnitId;
                        if (isCreateBarrier && (unit == GameConstant.BarrierUnit))
                        {
                            var pos = IndexToPos(gridId, width, height);
                            var barrier = InstantiateRandomUnit(barrierPrefabs, prefabRandom, pos, ResFlag.Entity_Barrier);

                        }
                        if (isCreateMonster && (unit == GameConstant.MonsterUnit))
                        {
                            var pos = IndexToPos(gridId, width, height);
                            var monster = InstantiateRandomUnit(monsterPrefabs, prefabRandom, pos, ResFlag.Entity_Monster);
                            //monster.Get<VelocityComponent>()?.UpdatePos(pos);

                        }
                        //道具的生成
                        if (isCreateInteractive && (unit == GameConstant.InteractiveUnit))
                        {
                            var pos = IndexToPos(gridId, width, height);
                            InstantiateRandomUnit(interactivePrefabs, prefabRandom, pos, ResFlag.Entity_Interactive);
                        }
                    }
                }
            }

            if (isSuccess)
            {
                //创建tilemap
                {
                    var tileDataKey = data[JsonKeys.tileData].ToString();
                    var tileData = resPoolManager.LoadJsonData(tileDataKey, ResFlag.Text_Tile);
                    var tileSeed = (int)data[JsonKeys.tileSeed];
                    var tileRandom = new Random(tileSeed);

                    //配置一下关卡的plotConfig
                    plotConfigProcessor.SingleComponent.SetTileConfig(levelKey, tileDataKey);

                    //test
                    for (int i = 0; i < grids.Length; i++)
                    {
                        var grid = grids[i];
                        int terrainId = 0;
                        if(!grid.IsNone) terrainId = grid.WalkableState;
                        grids[i].TerrainId = terrainId;
                    }

                    //TODO:地形？
                    var tilemapCreator = new TilemapCreator(grids, width, height, tileData, tileRandom);
                    var tileKey = data[JsonKeys.entity_tile].ToString();
                    bool isTilemapSuccess = tilemapCreator.TryCreateTiles(out var allTileModels, out var tilemapWidth, out var tilemapHeight, out var terrainHashs);
                    if(!isTilemapSuccess)
                    {
                        Debug.LogError($"{levelKey}.isTilemapSuccess:{isTilemapSuccess}");
                    }
                    //if (isTilemapSuccess)
                    {
                        Vector2 offset = new Vector2(-0.5f, -0.5f);
                        var tileCount = width * height;
                        var tileModels = new TileModel[4];
                        var hashs = new int[4];
                        for (int i = 0; i < tileCount; i++)
                        {
                            if (grids[i].IsNone) continue;
                            var t = i % width + i / width * tilemapWidth;
                            tileModels[0] = allTileModels[t + tilemapWidth];
                            tileModels[1] = allTileModels[t + tilemapWidth + 1];
                            tileModels[2] = allTileModels[t + 1];
                            tileModels[3] = allTileModels[t];
                            
                            
                            var pos = IndexToPos(i, width, height);
                            var entity = resPoolManager.InstantiateEntity(tileKey, ResFlag.Entity_Env);
                            entity.Get<TileComponent>().SetTargetTextureKeys(tileModels);
                            entity.Get<TerrainComponent>().SetTerrain(grids[i].TerrainId, false);
                            entity.Transform.Position = pos;
                        }
                    }
                }
            }
        }
    }
}
