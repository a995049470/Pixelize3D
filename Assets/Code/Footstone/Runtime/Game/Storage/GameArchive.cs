using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    [System.Serializable]
    public class GameArchive
    {
        public uint Max;
        public uint Magic;
        public uint[] RemainNums = new uint[0];
        public GameSceneArchive[] SceneArchives = new GameSceneArchive[0];
        public string[] OpenUIKeys = new string[0];

        public static GameArchive Create(GameSceneManager gameSceneManager, UniqueIdManager uniqueIdManager, UIManager uiManager){
            var archive = new GameArchive();
            archive.Max = uniqueIdManager.Max;
            archive.Magic = uniqueIdManager.Magic;
            archive.RemainNums = uniqueIdManager.GetRemainNums();
            var sceneSliceList = gameSceneManager.GetAllSceneSlices();
            var sceneSliceCount = sceneSliceList.Count;
            archive.SceneArchives = new GameSceneArchive[sceneSliceCount];
            for (int i = 0; i < sceneSliceCount; i++)
            {
                archive.SceneArchives[i] = GameSceneArchive.Create(sceneSliceList[i]);
            }
            archive.OpenUIKeys = uiManager.GetOpenWindowKeys();
            return archive;
        }
        
        /// <summary>
        /// 清空场景 加载存档
        /// </summary>
        /// <param name="gameSceneManager"></param>
        /// <param name="uniqueIdManager"></param>
        /// <param name="uiManager"></param>
        /// <param name="sceneSystem"></param>
        public void LoadGame(GameSceneManager gameSceneManager, UniqueIdManager uniqueIdManager, UIManager uiManager, SceneSystem sceneSystem, ResPoolManager resPoolManager)
        {
            //清空场景
            //先清除ui 防止ui出现无效引用
            uiManager.CloseAllWindow();
            gameSceneManager.ClearAll();
            resPoolManager.ClearPool();

            //根据存档恢复场景
            uniqueIdManager.Set(Magic, Max, RemainNums);
            var sceneCount = SceneArchives.Length;
            var sceneSlices = new GameSceneSlice[sceneCount];
            for (int i = 0; i < sceneCount; i++)
            {
                sceneSlices[i] = SceneArchives[i].Load(resPoolManager, sceneSystem);
            }
            gameSceneManager.Set(sceneSlices);
            foreach (var key in OpenUIKeys)
            {
                uiManager.OpenWindow(key);
            }
        }
    }
}
