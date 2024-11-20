namespace Lost.Runtime.Footstone.Game
{
    public static class GameConstant
    {

        //-----------------------AI的固定行为Key---------------------------
        public const string FixedBehavior_Move = "move"; //移动到目标地点
        public const string FixedBehavior_Face = "face"; //朝向固定方向

        //------------------------UI窗口优先级-----------------------------
        public const int Start = 0;
        public const int LevelLoadTest = 0;
        public const int Main = 1;
        public const int Radar = 5;
        public const int Craft = 10;
        public const int Bag = 10;
        public const int Map = 10;
        public const int Sacrifice = 10;
        public const int InteractiveTip = 20;

        //-----------------------碰撞体层级------------------------
        public const int Layer0 = 1 << 0;
        public const int PlayerLayer = 1 << 6;      //玩家
        public const int EnemyLayer = 1 << 7;       //敌人
        public const int BarrierLayer = 1 << 8;     //障碍物
        public const int PickableLayer = 1 << 9;    //可拾取物体
        public const int CropLayer = 1 << 10;       //农作物
        public const int GameEventLayer = 1 << 11;       //被动触发游戏事件
        public const int InteractionLayer = 1 << 12;       //主动交互环境物体

        //----------------------------地图状态----------------------------
        public const int Wall = 0;
        public const int Way = 1;
        public const int Unknow = 3;

        //-----------------------------地图单位-------------------------
        public const int EmptyUnit = 0;
        public const int BarrierUnit = 1;
        public const int MonsterUnit = 2;
        public const int InteractiveUnit = 3;
        public const int FixedMonsterUnit = 4;
        public const int FixedInteractiveUnit = 5;
        public const int FixedPointUnit = 6;
        //一定是空的点 不能放任何单位
        public const int FixedEmptyUnit = 7;



        //-------------------动画状态的优先级----------------
        public const int IdelStateLevel = 0;
        public const int WalkStateLevel = 1;
        public const int AttackStateLevel = 2;
        public const int DigStateLevel = 3;
        public const int WaterStateLevel = 4;
        public const int SowStateLevel = 5;
        public const int EatStateLevel = 6;
        public const int DeadStateLevel = 8;
        public const int PickaxeStateLevel = 9;
        public const int MaxStateLevel = 9999;

        //----------------字符串颜色------------------------
        public const string GreenText = "<color=#00FF00>{0}</color>";
        public const string RedText = "<color=#FF0000>{0}</color>";
        //---------------后缀-------------------------------
        public const string Suffix_Open = "open";

        //---------------特殊IconKey---------------------
        public const string IconKey_Ban = "ban";
        public const string IconKye_Unknown = "unknown";

        //---------------特殊String---------------------
        public const string Name_Unknown = "? ? ?";
        public const string Num_Unknown = "??/??";
        
        //---------------特殊EntityKey---------------------
        public const string EntityKey_Dropped = "CustomDroppedItem";


        /// <summary>
        /// 用int储存float时的缩放倍数
        /// </summary>
        public const float FloatScale = 100;
        public const float ZERO = 0.0001f;

        //----------------编辑下保存数据用的key--------------------
        public const string DefalutSceneKey = "DefalutScene";

        //----------------运行时保存数据用的key--------------------
    }
}