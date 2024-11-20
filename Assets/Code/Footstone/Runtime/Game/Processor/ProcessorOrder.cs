namespace Lost.Runtime.Footstone.Game
{
    /// <summary>
    /// 不能存在重复的Order 考虑Processor之间的依赖性和顺序
    /// </summary>
    public static class ProcessorOrder
    {
        /// <summary>
        /// 最早执行, 用以回收一帧生命的组件
        /// </summary>
        public const int FrameStart = 0;
        public const int AnimationClipEventTriggerStart = 1;
        public const int Transport = 10;
        public const int CheckTriggerDevice = 20;
        public const int LaunchBullet = 21;
        public const int AnimationAuidoPlay = 22;
        //更新植物状态 100 ~ 150
        public const int UpdatePlantState = 100;
        
        public const int UpdateFinalPlantState = 150;
        
        public const int AStarUpdate = 151;
        /// <summary>
        /// 触发事件或者机关
        /// </summary>
        public const int Trigger = 152;
        
        /// <summary>
        /// 生成能力的逻辑实体,处理装备逻辑
        /// </summary>
        public const int CreatePowerEntity = 1000;
        /// <summary>
        /// 处理穿装备或脱装备的逻辑
        /// </summary>
        public const int EquipOrPull = 1001;
        /// <summary>
        /// 处理Buffer实体逻辑
        /// </summary>
        public const int Buff = 1002;
        /// <summary>
        /// 添加buff
        /// </summary>
        public const int AddBuff = 1003;
        
        /// <summary>
        /// 普通buff生效
        /// </summary>
        public const int NormalBuffEffect = 1004;
        public const int StartTakeEffectOnUse = 1005;
        /// <summary>
        /// 脱装备时生效
        /// </summary>
        public const int TakeEffectOnPulling = 1106;
        /// <summary>
        /// 穿装备时生效
        /// </summary>
        public const int TakeEffectOnEquiping = 1107;

        /// <summary>
        /// 更新行为遮罩
        /// </summary>
        public const int UpadteActionMask = 1210;
        
        /// <summary>
        /// 计算当前帧的命令输入
        /// </summary>
        public const int CalculateInputCommand = 1230;
        /// <summary>
        /// AI控制器思考策略
        /// </summary>
        public const int AIThink = 1240;
        /// <summary>
        /// AI控制输入
        /// </summary>
        public const int AICommand = 1500;

        public const int FastUseCommandInput = 2046;

        public const int EatCommandInput = 2047;
        /// <summary>
        /// 浇水输入
        /// </summary>
        public const int WaterCommandInput = 2048;
        /// <summary>
        /// 挖输入
        /// </summary>
        public const int DigCommandInput = 2049;
        /// <summary>
        /// 拾取输入
        /// </summary>
        public const int PickCommandInput = 2050;
        /// <summary>
        /// 播种输入
        /// </summary>
        public const int SowCommandInput = 2051;
        /// <summary>
        /// 构建地块输入
        /// </summary>
        public const int BuildPlotCommandInput = 2052;
        /// <summary>
        /// 放置物体输入
        /// </summary>
        public const int PlaceObjectCommandInput = 2053;
        /// <summary>
        /// 放置物体输入
        /// </summary>
        public const int PickaxeCommandInput = 2054;
        /// <summary>
        /// 攻击输入
        /// </summary>
        public const int AttackCommandInput = 2065;
        /// <summary>
        /// 交互命令输入
        /// </summary>
        public const int InteractionCommandInput = 2066;
        /// <summary>
        /// 移动输入
        /// </summary>
        public const int MoveCommandInput = 2070;
        /// <summary>
        /// 拾取物体
        /// </summary>
        public const int Pick = 2071;   
        /// <summary>
        /// 掉落物体生效（直接进背包或者生成相应的逻辑实体）
        /// </summary>
        public const int DroppedItemEffect = 2072;
        /// <summary>
        /// 交互
        /// </summary>
        public const int Interaction = 2073;
        /// <summary>
        /// 开始交互物触发逻辑
        /// </summary>
        public const int InteractiveTriggerStart = 2074;
        public const int InteractiveCooldown = 2075;
        /// <summary>
        /// 展示交互提示 可能需要等待玩家响应
        /// </summary>
        public const int ShowInteractiveTip = 2076;
        public const int TipSensor = 2078;
        /// <summary>
        /// 检查可交互物是否上锁
        /// </summary>
        public const int InteractiveLockedCheck = 2080;
        public const int InteractiveCostCheck = 2081;
        public const int InteractiveChest = 2082;
        /// <summary>
        /// 物体被拾取
        /// </summary>
        public const int BePickedUp = 2085;   
        public const int TriggerStart = 2086;  
        /// <summary>
        /// 被捡到的物体生效
        /// </summary>
        public const int PickedItemTakeEffect = 2090;
        public const int FastUse = 2092;
        public const int Discover = 2095;
        /// <summary>
        /// 计算角色能力数值
        /// </summary>
        public const int ComputePower = 2100;
        
        //吃东西恢复体力
        public const int Eat = 2110;
        /// <summary>
        /// 给予对手伤害数值(未计算防御值)(可能需要考虑伤害来源,伤害类型)
        /// 攻击状态更新，触发攻击的帧事件
        /// </summary>
        public const int Attack = 2200;
        /// <summary>
        /// 攻击时的音效
        /// </summary>
        public const int AttackAudio = 2201;
        /// <summary>
        /// 击退目标
        /// </summary>
        public const int Repel = 2203;
        public const int ImpactStart = 2204;
        /// <summary>
        /// 治疗
        /// </summary>
        public const int Heal = 2300;
        /// <summary>
        /// 计算防御能力
        /// </summary>
        public const int ComputeDef = 2800;
        /// <summary>
        /// 计算真正税后伤害
        /// </summary>
        public const int ComputeHurtValue = 2900;
        /// <summary>
        /// 更新HurtComponent组件(计数器相关)
        /// </summary>
        public const int UpdateHurtComponent = 2901;
        /// <summary>
        /// 执行受伤效果
        /// </summary>
        public const int ExecuteHurtEffect = 2902;
        /// <summary>
        /// 受伤探测器
        /// </summary>
        public const int InjuredSensor = 2910;
        /// <summary>
        /// 普通探测器
        /// </summary>
        public const int Sensor  = 2920;
        /// <summary>
        /// 角色真正收到伤害
        /// </summary>
        public const int Hurt = 3000;
        /// <summary>
        /// 摘果实
        /// </summary>
        public const int PickFruit = 3001;
        /// <summary>
        /// 所有形式的掉落物品
        /// </summary>
        public const int Drop = 3010;
        public const int UniqueChest = 3014;
        /// <summary>
        /// 拾取空箱子
        /// </summary>
        public const int PickChest = 3015;
        /// <summary>
        /// 解锁
        /// </summary>
        public const int Unlock = 3020;
        
        /// <summary>
        /// 交互生成新场景
        /// </summary>
        public const int InteractionNewScene = 3030;
        // /// <summary>
        // /// 捡到的物体进入销毁流程
        // /// </summary>
        // public const int PickedItemDestory = 3000;
        
        /// <summary>
        /// 跳跃者实时改变移动速度
        /// </summary>
        public const int JumperChangeSpeed = 3501;
        /// <summary>
        /// 处理移动
        /// 处理移动中的帧率事件
        /// </summary>
        public const int Move = 4000;
        /// <summary>
        /// 撞击结束
        /// </summary>
        public const int ImpactEnd = 4001;
        /// <summary>
        /// 处理旋转
        /// </summary>
        public const int Rotate = 4100;
        /// <summary>
        /// 跟随主角
        /// </summary>
        public const int FollowPlayer = 5000;
        /// <summary>
        /// 挖掘
        /// </summary>
        public const int Dig = 5100;
        /// <summary>
        /// 浇水
        /// </summary>
        public const int Water = 5101;
        /// <summary>
        /// 播种
        /// </summary>
        public const int Sow = 5102;
        /// <summary>
        /// 建造地块
        /// </summary>
        public const int BuildPlot = 5103;
        /// <summary>
        /// 放置物体
        /// </summary>
        public const int PlaceObject = 5104;
        /// <summary>
        /// 被浇水
        /// </summary>
        public const int BeWater = 5105;
        /// <summary>
        /// 凿
        /// </summary>
        public const int Pickaxe = 5106;
        /// <summary>
        /// 被挖
        /// </summary>
        public const int DiggedOut = 5200;
        /// <summary>
        /// 被浇水
        /// </summary>
        public const int BeWatered = 5201;
        
        /// <summary>
        /// 被播种
        /// </summary>
        public const int BeSeeded = 5205;
        
        /// <summary>
        /// 组件拷贝
        /// </summary>
        public const int CopyComponent = 5210;
        //动画阶段
        /// <summary>
        /// 更新Idle动画的子Id
        /// </summary>
        public const int UpdateIdleSubIndex = 5300;
        public const int TerrainUpdateIdleSubIndex = 5301;
        /// <summary>
        /// 尝试切换状态
        /// </summary>
        public const int StateSwitch = 6000;
        public const int FinishAttack = 6099;
        /// <summary>
        /// 更新状态，驱动动画
        /// </summary>
        public const int PoseUpdate = 6100;


        public const int TriggerDeviceActivated = 6101;
        /// <summary>
        /// 能量自动恢复
        /// </summary>
        public const int NaturalRecoveryEnergy = 6200;
        
        public const int PositionReset = 6202;
        public const int InteractionOpenUI = 6250;
        /// <summary>
        /// 独特的交互物被触发
        /// </summary>
        public const int UniqueInteractiveTrigger = 7019;
        /// <summary>
        /// 结束交互物触发逻辑
        /// </summary>
        public const int InteractiveTriggerEnd = 7020;
        public const int TriggerEnd = 7021;

        public const int AnimationClipEventTriggerEnd = 7025;
        public const int ItemEffectEnd = 7026;
        /// <summary>
        /// 结束道具或者装备的生效
        /// </summary>
        public const int FinshTakeEffect = 7030;
        /// <summary>
        /// 结束装备的穿或者脱
        /// </summary>
        public const int FinshEquipOrPull = 7031;
        /// <summary>
        /// 结束Buffer实体逻辑
        /// </summary>
        public const int FinishBuff = 7032;

        public const int TerrainTile = 7050;
        /// <summary>
        /// 更新渲染相关组件
        /// </summary>
        public const int Render = 7150;
        /// <summary>
        /// 回收物体
        /// </summary>
        public const int Recycle = 7180;

        /// <summary>
        /// 死亡 也许会销毁物体
        /// </summary>
        public const int Dead = 7200;

        public const int FastEquip = 7300;

        //TODO:思考清理 是否可能清理失误
        //最后阶段清空一些数据
        public const int FrameEnd = 10000;
    }
}
