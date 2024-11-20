using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    //当前场景的地块配置
    [DefaultEntityComponentProcessor(typeof(PlotConfigProcessor))]
    public class PlotConfigComponent : EntityComponent
    {
        public string EntityKey_Plot = "";
        [UnityEngine.HideInInspector]
        public string Key_Tiles = "";
        [UnityEngine.HideInInspector]
        public string Key_Level = "";
        private bool isDitryTileData = true;
        private Dictionary<int, List<TileModel>> tilesDic;
        private ResPoolManager resPoolManager;

        protected override void Initialize(IServiceRegistry registry)
        {
            base.Initialize(registry);
            resPoolManager = registry.GetService<ResPoolManager>();
        }

        protected override void OnEnableRuntime()
        {
            base.OnEnableRuntime();
            isDitryTileData = true;
        }

        public void SetTileConfig(string level, string tiles)
        {
            if(level != Key_Level || tiles != Key_Tiles)
            {
                Key_Level = level;
                Key_Tiles = tiles;
                isDitryTileData = true;
            }
        }

        public Dictionary<int, List<TileModel>> GetTilesDic()
        {
            if(isDitryTileData)
            {
                isDitryTileData = false;
                var levelData = resPoolManager.LoadJsonData(Key_Level, ResFlag.Text_Level);
                var tileDataKey = levelData[JsonKeys.tileData].ToString();
                var tileData = resPoolManager.LoadJsonData(tileDataKey, ResFlag.Text_Tile);
                tilesDic = TilemapCreator.CreateTilesDic(tileData);
            }

            return tilesDic;
        }
        
    }

}
