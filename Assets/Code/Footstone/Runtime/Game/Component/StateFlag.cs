namespace Lost.Runtime.Footstone.Game
{
    /// <summary>
    /// 动画状态的枚举(标签不可修改)
    /// </summary>
    public enum StateFlag : int
    {
        None = 0,
        Idle = 100,
        Walk = 200,
        Attack = 300,
        Run = 400,
        Death = 500,
        Jump = 600,
        Hoeing = 700,
        Watering = 800,
        Sowing = 900,
        Eat = 1000,
        Open = 1100,
        Pickaxing = 1200
        
    }

}
