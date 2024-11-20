namespace Lost.Runtime.Footstone.Game
{
    //资产种类
    public enum ResFlag
    {
        //障碍（可破坏）
        Entity_Barrier = 0,
        //环境
        Entity_Env = 1,
        //掉落物
        Entity_Drop = 2,
        //交互物
        Entity_Interactive = 3,
        //怪物
        Entity_Monster = 4,
        //粒子
        Entity_Particle = 5,
        //玩家
        Entity_Player = 6,
        //墙体
        Entity_Wall = 7,
        //ui预制体(必须包含UIView组件)
        UIView = 8,
        //摄像机
        Entity_Camera = 9,
        //灯光
        Entity_Light = 10,
        //关卡
        Text_Level = 11,
        //材质球
        Material = 12,
        //植被各个阶段 预览用
        Entity_Perview_Plant = 13,
        //道具
        Config_Item = 14,
        //根物体
        Entity_Root = 15,
        //图标
        Sprite_Icon = 16,
        //UI上的精灵（不包括图标）
        Sprite_UI = 17,
        //物体赋予的功能(包括Buffer) 
        Entity_Power = 18,
        //包含一颗植物的所有状态
        Entity_Plant = 19,
        //tilemap的texture
        Texture_Tile = 20,
        Text_Tile = 21,
        //系统的数据实体，包括存档，画质调节等最外围数据。背包等存档数据
        Entity_System = 22,
        //只提供预览作用的武器模型
        Entity_Perview_Weapon = 23,
        //代表初始地图的Texture
        Texture_Map = 24,
        //游戏场景的配置
        Config_Scene = 25,
        //场景点位
        Entity_Point = 26,
        //攻击
        Data_Attack = 27,
        //攻击事件
        Data_AttackEvent = 28,
        //GOAP Graph
        Data_Graph = 29,
        //GOAP Goal
        Data_Goal = 30,
        //场景中所有Entity的序列化数据
        Data_Entities = 31,
        //动画事件控制器实体
        Data_AnimationClipController = 32,
        //动画片段事件实体
        Entity_AnimationClipEvent = 33,
        Entity_Bullet = 34,
        Data_AttackTable = 35,
        //提示
        Entity_Tip = 36,
        //音效片段
        Audio = 37,
        //音效播放器实体
        Entity_AudioSource = 38,
        //放置物体
        Entity_Place = 39,
        //关卡的独特配置
        Entity_LevelConfig = 40,
        //交互提示的文本配置
        Text_InteractiveTip = 41,
        //AI的固定行为
        Text_FixedBehavior = 42,
    }
}
