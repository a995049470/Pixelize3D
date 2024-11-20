namespace Lost.Runtime.Footstone.Core
{
    /// <summary>
    /// 在entity实例化并完成对组件赋值后的回调
    /// TODO:尝试在实例化后的第一帧对部分组件进行调用
    /// </summary>
    public interface IEntityInstantiateCallback
    {
        void LateEntityInstantiation();
    }
}



