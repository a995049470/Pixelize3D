using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    /// <summary>
    /// 会赋予目标挖掘能力 
    /// 
    /// </summary>
    [AllowMultipleComponents]
    [DefaultEntityComponentProcessor(typeof(DigPowerOnPullProcessor))]
    [DefaultEntityComponentProcessor(typeof(DigPowerOnEquipProcessor))]
    public class DigPowerComponent : EntityComponent, ITakeEffectOnEquipOrPullFrame
    {
        public bool IsEquipping { get; private set; }
        public ulong PowerUID;
        public void SetTimePoint(bool isEquipping, ulong uid)
        {
            IsEquipping = isEquipping;
            PowerUID = uid;
        }
    }
}



