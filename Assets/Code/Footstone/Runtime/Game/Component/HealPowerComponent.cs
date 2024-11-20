using Lost.Runtime.Footstone.Core;



namespace Lost.Runtime.Footstone.Game
{
    /// <summary>
    /// 治疗组件，通常是物体或技能携带 允许重复存在
    /// 不能两个 允许重复存在的组件 存在在一个processor中
    /// </summary>
    [DefaultEntityComponentProcessor(typeof(HealProcessor))]
    [AllowMultipleComponents]
    public class HealPowerComponent : EntityComponent, ITakeEffectOnPickFrame
    {
        //必须大于0
        public int Value;
    }
}
