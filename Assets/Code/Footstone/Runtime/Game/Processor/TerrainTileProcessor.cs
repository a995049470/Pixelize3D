using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    using TerrainTileData = GameData<TerrainComponent, TileComponent>;
    using AutoSizeTerrains = AutoSizeGrids<GameData<TerrainComponent, TileComponent>>;
    public class TerrainTileProcessor : SimpleGameEntityProcessor<TerrainComponent, TileComponent>
    {
        private HashSet<TerrainTileData> newDatas = new ();
        private AutoSizeTerrains terrains = new();
        //private string[] cacheTexureKeys = new string[4];
        private int[] cacheInices = new int[4];
        private TerrainTileData[] cacheDatas = new TerrainTileData[9];
        private MapGrid[] cacheMapGrids = new MapGrid[9];
        

        private TilemapCreator tilemapCreator = new();
        private PlotConfigProcessor plotConfigProcessor;

        public TerrainTileProcessor() : base()
        {
            Order = ProcessorOrder.TerrainTile;
        }

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            plotConfigProcessor = GetProcessor<PlotConfigProcessor>();
        }

        private int GetDistance(int index, int width, int height)
        {
            return Mathf.Min(
                index % width,
                width - 1 - index % width,
                index / width,
                height - 1 - index / width
            );
        }

        private void UpdateTerrains(Vector3 mid)
        {
            var width = 3;
            var height = 3;
            int defaultTerrainId = 1;
            for (int i = 0; i < 9; i++)
            {
                int x = i % width - width / 2;
                int z = i / width - height / 2;
                var pos = mid + new Vector3(x, 0, z);
                var data = terrains.Get(pos);
                var terrainId = data?.Component1?.TerrainId ?? defaultTerrainId;
                cacheMapGrids[i].TerrainId = terrainId;
                cacheDatas[i] = data;
            }
            var tilesDic = plotConfigProcessor.SingleComponent.GetTilesDic();
            tilemapCreator.Initialize(cacheMapGrids, width, height, tilesDic, random);
            bool isSuccess = tilemapCreator.TryCreateTiles(out var allTileModels, out var tilemapWidth, out var tilemapHeight, out var terrainHashs);
            for (int i = 0; i < 9; i++)
            {
                var tileComp = cacheDatas[i]?.Component2;
                if(tileComp)
                {
                    var t = i % width + i / width * tilemapWidth;
                    cacheInices[0] = t + tilemapWidth;
                    cacheInices[1] = t + tilemapWidth + 1;
                    cacheInices[2] = t + 1;
                    cacheInices[3] = t;
                    for (int j = 0; j < cacheInices.Length; j++)
                    {
                        var modelId = cacheInices[j];
                        if(GetDistance(modelId, tilemapWidth, tilemapHeight) == 1)
                        {
                            tileComp.SetTargetTextureKey(j, allTileModels[modelId]);
                        }
                    }
                   
                    
                }
            }
            
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
            if(newDatas.Count > 0)
            {
                var max = terrains.MapBoundsMax;
                var min = terrains.MapBoundsMin;
                
                foreach (var data in newDatas)
                {
                    var terrainComp = data.Component1;
                    var pos = terrainComp.Entity.Transform.Position;
                    terrains.CalculateBounds(ref min, ref max, pos);
                }
                //稍微扩大包围盒
                max += new Vector2Int(2, 2);
                min -= new Vector2Int(2, 2);
                terrains.TryChangeMapBounds(min, max);
                foreach (var data in newDatas)
                {
                    var terrainComp = data.Component1;
                    var pos = terrainComp.Entity.Transform.Position;
                    terrains.Set(pos, data);
                }

                foreach (var data in newDatas)
                {
                    var terrainComp = data.Component1;
                    if(terrainComp.IsDitryTerrainTex)
                    {
                        terrainComp.IsDitryTerrainTex = false;
                        var pos = terrainComp.Entity.Transform.Position;
                        UpdateTerrains(pos);
                       
                    }
                }
                newDatas.Clear();
            }
        }

        public void OnBuildPlot(Vector3 pos)
        {
            if (terrains.TryGet(pos, out var data) && data != null)
            {
                data.Component1.IsDitryTerrainTex = true;
                newDatas.Add(data);
            }
        }

        public void OnPlaceObject(Vector3 pos)
        {
            if (terrains.TryGet(pos, out var data) && data != null)
            {
                data.Component1.SetTerrain(0, true);
                newDatas.Add(data);
            }
        }

        protected override void OnEntityComponentAdding(Entity entity, TerrainComponent component, TerrainTileData data)
        {
            base.OnEntityComponentAdding(entity, component, data);
            newDatas.Add(data);
        }

        protected override void OnEntityComponentRemoved(Entity entity, TerrainComponent component, TerrainTileData data)
        {
            base.OnEntityComponentRemoved(entity, component, data);
            if(newDatas.Contains(data))
            {
                newDatas.Remove(data);
            }
            else
            {
                var pos = entity.Transform.Position;
                terrains.Set(pos, null);
            }

            if(ComponentDatas.Count == 1)
            {
                terrains.Clear();
            }
        }
    }
}
