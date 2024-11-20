using Lost.Runtime.Footstone.Core;
using UnityEngine;
using System.IO;
using Lost.Runtime.Footstone.Reflection;
using System.Diagnostics;

namespace Lost.Runtime.Footstone.Game
{

    /// <summary>
    /// 储存系统
    /// 直接用jsonData?
    /// </summary>
    public class StorageSystem 
    {
        private IServiceRegistry service;
        private SceneSystem sceneSystem;
        private ResPoolManager resPoolManager;
        private GameSceneManager gameSceneManager;
        private UIManager uiManager;
        private UniqueIdManager uniqueIdManager;
        private Stopwatch sw = new Stopwatch();

        private string testStoragePath => Application.streamingAssetsPath + "/Storage/test1.txt";
        

        public StorageSystem(IServiceRegistry _service)
        {
            service = _service;
            sceneSystem = _service.GetService<SceneSystem>();
            resPoolManager = _service.GetService<ResPoolManager>();
            gameSceneManager = _service.GetService<GameSceneManager>();
            uiManager = _service.GetService<UIManager>();
            uniqueIdManager = _service.GetService<UniqueIdManager>();
        }

        

         /// <summary>
        /// 保存存档并写入本地
        /// </summary>
        public void Save()
        {
            sw.Restart();
            var tagProcessor = sceneSystem.SceneInstance.GetProcessor<TagProcessor>();
            var tagComponents = tagProcessor?.ComponentDatas?.Keys;
            var gameArchive = GameArchive.Create(gameSceneManager, uniqueIdManager, uiManager);
            var json = LitJson.JsonMapper.ToJson(gameArchive);
            var floder = Path.GetDirectoryName(testStoragePath);
            if(!Directory.Exists(floder)) Directory.CreateDirectory(floder);
            File.WriteAllText(testStoragePath, json);
        #if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
        #endif

            sw.Stop();

            UnityEngine.Debug.Log($"存档消耗:{sw.ElapsedMilliseconds} ms");
        }

        /// <summary>
        /// 读取文件并加载存档
        /// </summary>
        public void Load()
        {
            if(File.Exists(testStoragePath))
            {
                sw.Restart();
                var json = File.ReadAllText(testStoragePath);
                var gameArchive = LitJson.JsonMapper.ToObject<GameArchive>(json);
                gameArchive.LoadGame(gameSceneManager, uniqueIdManager, uiManager, sceneSystem, resPoolManager);
                sw.Stop();
                UnityEngine.Debug.Log($"读档消耗:{sw.ElapsedMilliseconds} ms");
            }
        }
    
    }
}
