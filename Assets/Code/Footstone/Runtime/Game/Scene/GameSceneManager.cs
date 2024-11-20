using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class GameSceneManager
    {
        private ResPoolManager resPoolManager;
        private SceneSystem sceneSystem;
        private LevelManager levelManager;
        private UIManager uiManager;
        
        private Dictionary<string, GameSceneSlice> sceneSliceDic = new();
        private string currentScene = "";
        public string CurrentScene { get => currentScene; }
        
        private TagProcessor tagProcessor;
        private MapOriginPointProcessor mapOriginPointProcessor;
        private BGMProcessor bgmProcessor;
        private MapProcessor mapProcessor;

        public GameSceneManager()
        {
            
        }

        public void Initialize(IServiceRegistry _service)
        {
            resPoolManager = _service.GetService<ResPoolManager>();
            sceneSystem = _service.GetService<SceneSystem>();
            levelManager = _service.GetService<LevelManager>();
            uiManager = _service.GetService<UIManager>();

            tagProcessor = sceneSystem.SceneInstance.ForceGetProcessor<TagProcessor>();
            mapOriginPointProcessor = sceneSystem.SceneInstance.ForceGetProcessor<MapOriginPointProcessor>();
            bgmProcessor = sceneSystem.SceneInstance.ForceGetProcessor<BGMProcessor>();
            mapProcessor = sceneSystem.SceneInstance.ForceGetProcessor<MapProcessor>();
        }
        
        public void CreateNewScene(string newScene, bool isDestoryCurrentScene)
        {
            if(!string.IsNullOrEmpty(newScene))
            {
                if(isDestoryCurrentScene && !string.IsNullOrEmpty(currentScene))
                {
                    mapProcessor.SingleComponent?.Data?.DeleteSceneViewBuffer(currentScene);
                }
                SwitchScene(currentScene, newScene, isDestoryCurrentScene);
                currentScene = newScene;
            }
        }
        

        private void CloseSceneSlice(GameSceneSlice sceneSlice, bool isDestory)
        {
            if(isDestory)
            {
                sceneSlice.Destory();
                if(!string.IsNullOrEmpty(sceneSlice.Name))
                    sceneSliceDic.Remove(sceneSlice.Name);
            }
            else
            {
                sceneSlice.SetActive(false);
            }
        }

        public void Set(IEnumerable<GameSceneSlice> sceneSlices)
        {
            foreach (var sceneSlice in sceneSlices)
            {
                if(sceneSlice.Active)
                    currentScene = sceneSlice.Name;
                else
                    sceneSliceDic[sceneSlice.Name] = sceneSlice;
            }
        }

        public void ClearAll()
        {
            var sceneSliceList = GetAllSceneSlices();
            foreach (var sceneSlice in sceneSliceList)
            {
                sceneSlice.Destory();
            }
            sceneSliceDic.Clear();
            currentScene = "";
        }

        public List<GameSceneSlice> GetAllSceneSlices()
        {
            var count = sceneSliceDic.Count + 1;
            var sliceList = new List<GameSceneSlice>(count);
            sliceList.AddRange(sceneSliceDic.Values);
            sliceList.Add(PackAllEntityToSceneSlice());
            return sliceList;
        }

        /// <summary>
        /// 将当前场景中的所有物体打包成场景切片（保存用）
        /// </summary>
        private GameSceneSlice PackAllEntityToSceneSlice()
        {
            GameSceneSlice slice = new(resPoolManager);
            slice.Name = currentScene;
            slice.SceneEntities = tagProcessor.GetAllEntities();
            return slice;
        }
        
        /// <summary>
        /// 重启实体
        /// </summary>
        public void RestartEntity(ref Entity entity)
        {  
            entity.OnSceneCloseWithComponents();
            entity.SetUnityGameObjectActive(false);
            entity.SetUnityGameObjectActive(true);
        }

        private void SwitchScene(string srcScene, string dstScene, bool isDestorySrcScene)
        {   
            GameSceneSlice srcSceneSlice;
            string currentBGM = "";
            if(!string.IsNullOrEmpty(srcScene))
            {
                if(!sceneSliceDic.TryGetValue(srcScene, out srcSceneSlice))
                {   
                    srcSceneSlice = new GameSceneSlice(resPoolManager);
                    tagProcessor.GetEntities(out srcSceneSlice.PlayerEntity, out srcSceneSlice.SystemEntities, out srcSceneSlice.SceneEntities);
                    srcSceneSlice.Name = srcScene;
                    if(srcSceneSlice.PlayerEntity != null)
                        srcSceneSlice.PlayerPosition = srcSceneSlice.PlayerEntity.Transform.Position;
                    sceneSliceDic[srcScene] = srcSceneSlice;
                }
                if(isDestorySrcScene) 
                {
                    currentBGM = bgmProcessor.SingleComponent.GetCurrentBGM();
                    bgmProcessor.SingleComponent.PopBGM();
                }
            }
            else
            {
                srcSceneSlice = new(resPoolManager);
            }
            GameSceneSlice dstSceneSlice = null;

            //TODO:考虑把player先开后关，让部分AutoSizeGrids能缩小Bounds
            if(sceneSliceDic.TryGetValue(dstScene, out dstSceneSlice))
            {
                srcSceneSlice.EntityShiftToNextScene(ref dstSceneSlice.MissingPlayerKey, dstSceneSlice.MissingSystemKeys, out dstSceneSlice.PlayerEntity);
                //生效新场景前必须先关闭旧场景，防止产生寻路组件出错
                CloseSceneSlice(srcSceneSlice, isDestorySrcScene);
                dstSceneSlice.SetActive(true);

                tagProcessor.DeletePerviewEntities();
                sceneSliceDic.Remove(dstScene);
            }
            else
            {
                var config = resPoolManager.LoadResConfigData(ResFlag.Config_Scene);
                var sceneData = config[dstScene];
                var cameraKey = (string)sceneData[JsonKeys.camera];
                var levelKey = (string)sceneData[JsonKeys.level];
                var lightKey = (string)sceneData[JsonKeys.light];
                var envKey = (string)sceneData[JsonKeys.env];
                var uikeys = LitJsonUtil.ToStringArray(sceneData[JsonKeys.ui]);
                var systemKeysData = sceneData[JsonKeys.system];
                var systemKeysCount = systemKeysData.Count;
                var playerKey = (string)sceneData[JsonKeys.player];
                var systemKeys = new HashSet<string>();
                var entitiesKey = (string)sceneData[JsonKeys.entities];
                var bgmAudioKey = sceneData.ContainsKey(JsonKeys.bgm) ? (string)sceneData[JsonKeys.bgm] : currentBGM;
                
                
                
                for (int i = 0; i < systemKeysCount; i++)
                {
                    systemKeys.Add((string)systemKeysData[i]);
                }
                 //生效新场景前必须先关闭旧场景，防止产生寻路组件出错
                srcSceneSlice.EntityShiftToNextScene(ref playerKey, systemKeys, out var playerEntity);
                CloseSceneSlice(srcSceneSlice, isDestorySrcScene);
                
                foreach (var key in systemKeys)
                {
                    resPoolManager.InstantiateEntity(key, ResFlag.Entity_System);
                }
                if(!string.IsNullOrEmpty(entitiesKey))
                {
                    var json = resPoolManager.LoadResWithKey<TextAsset>(entitiesKey, ResFlag.Data_Entities).text;
                    var entityDatas = LitJson.JsonMapper.ToObject<List<EntityData>>(json);
                    foreach (var entityData in entityDatas)
                    {
                        entityData.Load(resPoolManager);
                    }
                }
                if(playerEntity == null && !string.IsNullOrEmpty(playerKey))
                {
                    playerEntity = resPoolManager.InstantiateEntity(playerKey, ResFlag.Entity_Player);
                }
                //env light 默认坐标全是0 (左下角)
                if(!string.IsNullOrEmpty(cameraKey)) resPoolManager.InstantiateEntity(cameraKey, ResFlag.Entity_Camera);
                if(!string.IsNullOrEmpty(lightKey)) resPoolManager.InstantiateEntity(lightKey, ResFlag.Entity_Light);
                if(!string.IsNullOrEmpty(envKey)) resPoolManager.InstantiateEntity(envKey, ResFlag.Entity_Env);
                if(!string.IsNullOrEmpty(levelKey)) levelManager.LoadLevel(levelKey, Vector2Int.zero);
                
                if(playerEntity != null)
                {
                    var playerPosition = mapOriginPointProcessor.OriginPosition;
                    playerEntity.GetOrCreate<TransportComponent>().TargetPosition = playerPosition;
                }
                var currentUIKeys = new HashSet<string>(uiManager.GetOpenWindowKeys());
                //加载ui
                foreach (var key in uikeys)
                {
                    currentUIKeys.Remove(key);
                    uiManager.OpenWindow(key);
                }
                foreach (var key in currentUIKeys)
                {
                    uiManager.CloseWindow(key);
                }
                
                //可能需要排序？

                //播放bgm
                bgmProcessor.SingleComponent.PushBGM(bgmAudioKey);
            }
            

            
        }   
        
    }
}