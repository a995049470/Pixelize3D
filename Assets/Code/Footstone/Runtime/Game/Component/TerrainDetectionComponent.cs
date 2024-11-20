using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    //检测地形组件
    [DefaultEntityComponentProcessor(typeof(TerrainUpdateIdleSubIndexProcessor))]
    public class TerrainDetectionComponent : EntityComponent
    {
        public int IdleSubIndex_Water = 0;
        public int IdleSubIndex_Plot = 1;
    }

}
