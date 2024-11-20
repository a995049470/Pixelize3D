namespace Lost.Runtime.Footstone.Game
{
    public enum GOAPStatusFlag
    {
        //有伤的
        Injured = 0,
        FollowPlayer = 1,
        //距离最近一次受伤的时间
        LastHurtTime = 2,
        //发现玩家
        DiscoverPlayer = 3,
        //被攻击惊醒
        WakeUpAfterAttack = 4,
        //攻击玩家
        AttackPlayer = 5,
        //接近玩家
        CloseToPlayer = 6,
        //静止
        Idle = 7,
        //当前能量
        Energy = 8,
        //攻击者的距离
        AttackTargetDistance = 9,
        //脚下的地形 0:水 1:普通地块
        StandTerrain = 10,
        TipOptionIndex = 11,
        //面向固定方向
        FaceFixedDirection = 12,
        //等待交互
        WaitInteractive = 13,
        //阶段Id
        StageIndex = 14
    }

}



