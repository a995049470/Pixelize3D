using System.Collections.Generic;
using Lost.Runtime.Footstone.Collection;
using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    //地图的数据
    [System.Serializable]
    public class MapData
    {
        public Vector3 Center;
        //通过道具发现的关卡
        [SerializeField][HideInInspector]
        private SerializableHashSet<string> knownKeys = new();
        public float MapScale = 1.0f;
        private List<MapElementData> elementDataList;
        public List<MapElementData> ElementDataList { get => elementDataList; }
  
        public SerializableDictionary<string, AutoSizeMapGrids> ViewBufferDic;

        private const int visiable = 1;
        private MapObjectElementProcessor mapObjectElementProcessor;
        private PlayerElementProcessor playerElementProcessor;
        private LevelDoorElementProcessor levelDoorElementProcessor;
        private PlotProcessor plotProcessor;

        private string knownLevelIconKey = "map_red";
        private string waterIconKey = "map_water";
        private int knownLevelIconScale = 8;
        private int arriveLevelIconScale = 1;

        public MapData()
        {
            ViewBufferDic = new();
            elementDataList = new();
        }

        public void Initialized(IServiceRegistry service)
        {
            var sceneInstance = service.GetService<SceneSystem>().SceneInstance;
            mapObjectElementProcessor = sceneInstance.ForceGetProcessor<MapObjectElementProcessor>();
            playerElementProcessor = sceneInstance.ForceGetProcessor<PlayerElementProcessor>();
            levelDoorElementProcessor = sceneInstance.ForceGetProcessor<LevelDoorElementProcessor>();
            plotProcessor = sceneInstance.ForceGetProcessor<PlotProcessor>();
        }

        private bool IsVisiable(AutoSizeMapGrids grids, Vector3 pos)
        {
            grids.CalculateMapBounds(out var min, out var max, pos);
            grids.TryChangeMapBounds(min, max);
            return  grids.Get(pos) == visiable;
        }

        private void FillWater(AutoSizeMapGrids grids)
        {
            for (int i = 0; i < grids.Count; i++)
            {
                var isVisiable = grids.Grids[i] == visiable;
                if(isVisiable)
                {
                    var pos = grids.ConvertIndexToPosition(i);
                    var isWater = !plotProcessor.IsPlot(pos);
                    if(isWater)
                    {
                        var data = new MapElementData();
                        data.Position = pos;
                        data.Scale = 1;
                        data.Layer = 0;
                        data.SpriteReference.SetKey(waterIconKey);
                        elementDataList.Add(data);
                    }
                }
            }
        }

        private void FillMapObject(AutoSizeMapGrids grids)
        {
            var componentDatas = mapObjectElementProcessor.ComponentDatas;
            foreach (var kvp in componentDatas)
            {
                var mapElementComp = kvp.Value.Component1;
                var uid = mapElementComp.Id;
                var position = mapElementComp.Entity.Transform.Position;
                if(IsVisiable(grids, position))
                {
                    var data = new MapElementData();
                    data.Position = position;
                    data.SpriteReference.SetKey(mapElementComp.IconKey);
                    data.Scale = 1;
                    data.Layer = mapElementComp.Layer;
                    elementDataList.Add(data);
                }
            }
        }

        private void FillPlayer()
        {
            var componentDatas = playerElementProcessor.ComponentDatas;
            foreach (var kvp in componentDatas)
            {
                var mapElementComp = kvp.Value.Component1;
                var uid = mapElementComp.Id;
                var data = new MapElementData();
                data.Position = mapElementComp.Entity.Transform.Position;
                data.SpriteReference.SetKey(mapElementComp.IconKey);
                data.Scale = 1;
                data.Layer = mapElementComp.Layer;
                elementDataList.Add(data);
                Center = data.Position;
            }
        }

        private void FillLevelDoor(AutoSizeMapGrids grids)
        {
            var componentDatas = levelDoorElementProcessor.ComponentDatas;
            foreach (var kvp in componentDatas)
            {
                var mapElementComp = kvp.Value.Component1;
                var position = mapElementComp.Entity.Transform.Position;
                //if(IsVisiable(grids, position))
                {
                    var tagComp = kvp.Value.Component3;
                    var isKonownLevel = IsKnownKey(tagComp.Key);
                    var isVisiable = IsVisiable(grids, position);
                    if(isKonownLevel || isVisiable)
                    {
                        var uid = mapElementComp.Id;
                        var data = new MapElementData();
                        data.Position = position;
                        var iconKey = isVisiable ? mapElementComp.IconKey : knownLevelIconKey;
                        var scale = isVisiable ? arriveLevelIconScale : knownLevelIconScale;
                        data.SpriteReference.SetKey(iconKey);
                        data.Scale = scale;
                        data.Layer = mapElementComp.Layer;
                        elementDataList.Add(data);
                    }
                }
            }
        }

         public void UpdateAllMapElementData(string scene)
        {
            elementDataList.Clear();
            if(!ViewBufferDic.TryGetValue(scene, out var grids))
            {
                grids = new AutoSizeMapGrids();
                ViewBufferDic[scene] = grids;
            }
            FillMapObject(grids);
            FillPlayer();
            FillLevelDoor(grids);
            FillWater(grids);
        }

        public void AddKnownKey(string key)
        {
            knownKeys.Add(key);
        }
        
        public bool IsKnownKey(string key)
        {
            return knownKeys.Contains(key);
        }   

        public void AddView(string scene, params Vector3[] positions)
        {
            if(!ViewBufferDic.TryGetValue(scene, out var grids))
            {
                grids = new AutoSizeMapGrids();
                ViewBufferDic[scene] = grids;
            }
            
            grids.CalculateMapBounds(out var min, out var max, positions);
            grids.TryChangeMapBounds(min, max);
            foreach (var pos in positions)
            {
                grids.Set(pos, visiable);
            }
        }

        public void DeleteSceneViewBuffer(string scene)
        {
            ViewBufferDic.Remove(scene);
        }

        public void OnBeforeSave()
        {
            knownKeys.OnBeforeSave();
            ViewBufferDic.OnBeforeSave();
        }

        public void OnAfterLoad()
        {
            knownKeys.OnAfterLoad();
            ViewBufferDic.OnAfterLoad();
        }
    }
}
