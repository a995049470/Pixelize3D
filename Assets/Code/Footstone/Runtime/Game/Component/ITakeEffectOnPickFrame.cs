using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    /// <summary>
    /// 被拾取物在被拾取那一帧生效的组件 只有一帧的生命,必须靠影响其他组件实现效果
    /// </summary>
    public interface ITakeEffectOnPickFrame
    {
        // void SetOriginEntity(Entity entity);
        // Entity GetOriginEntity();
    }

}
