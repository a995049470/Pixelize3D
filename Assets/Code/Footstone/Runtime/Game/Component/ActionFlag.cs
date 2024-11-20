namespace Lost.Runtime.Footstone.Game
{
    [System.Flags]
    public enum ActionFlag : int
    {
        None        = 0,
        Idle        = 1 << 0,//静
        Move        = 1 << 1,//动
        Attack      = 1 << 2,//攻
        Pick        = 1 << 3,//捡
        Dig         = 1 << 4,//挖
        Water       = 1 << 5,//浇
        Sow         = 1 << 6,//种
        Rotate      = 1 << 7,//转？
        Interaction = 1 << 8,//交互
        BuildPlot   = 1 << 9,//建造地块
        Eat         = 1 << 10,//吃
        FastUse     = 1 << 11,
        PlaceObject = 1 << 12,
        Pickaxe     = 1 << 13,//镐击
        All         = -1
    }



}
