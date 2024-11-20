using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    //地形组件 自动匹配合适的地图
    [DefaultEntityComponentProcessor(typeof(TerrainTileProcessor))]
    public class TerrainComponent : EntityComponent
    {
        public int TerrainId = 0;
        [UnityEngine.HideInInspector]
        public bool IsDitryTerrainTex = true;


        public void SetTerrain(int id, bool isDitry = false)
        {
            TerrainId = id;
            IsDitryTerrainTex = isDitry;
        }
    }

}
