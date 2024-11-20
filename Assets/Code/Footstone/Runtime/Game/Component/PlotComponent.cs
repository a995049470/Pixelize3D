using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    [System.Flags]
    public enum PlotFlg
    {
        None = 0,
        //可行走(暂时没用)
        Walkable = 1 << 0,
        //可放置
        Placeable = 1 << 1,
        //可播种(暂时没用)
        Sowable = 1 << 2,
    }

    [DefaultEntityComponentProcessor(typeof(PlotProcessor))]
    public class PlotComponent : EntityComponent
    {   
        public PlotFlg Flag = PlotFlg.Walkable;
    }

}
