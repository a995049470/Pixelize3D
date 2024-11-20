

using Lost.Runtime.Footstone.Core;
using UnityEngine;


namespace Lost.Runtime.Footstone.Game
{
    public static class GameLauncher 
    {
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void GameStart()
        {
            //锁60
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 1;

            LitJsonUtil.LitJsonRegister();
            StoneHeart.OnGameStart();
            var service = StoneHeart.Instance.Services;
            service.AddService(new JsonDataManager(service));
            service.AddService(new ResPoolManager(service));
            service.AddService(new LevelManager(service));
            var uiManager = new UIManager();
            service.AddService(uiManager);
            var gameSceneManager = new GameSceneManager();
            service.AddService(gameSceneManager);
            service.AddService(new StorageSystem(service));
            service.AddService(new CommandBufferManager(service));
            var aStarSystem = new AStarSystem(service);
            service.AddService(aStarSystem);
            aStarSystem.Initialize(service);
            gameSceneManager.Initialize(service);
            uiManager.Initialize(service);

#if UNITY_EDITOR
            var defalutScene = UnityEditor.EditorPrefs.GetString(GameConstant.DefalutSceneKey);
            if (!string.IsNullOrEmpty(defalutScene))
            {
                var currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                if(currentSceneName != defalutScene)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(defalutScene, UnityEngine.SceneManagement.LoadSceneMode.Single);
                }
            }
#endif

            
            
        }

        
        


    }
}
