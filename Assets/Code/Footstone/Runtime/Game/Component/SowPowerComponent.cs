using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    [AllowMultipleComponents]
    [DefaultEntityComponentProcessor(typeof(SowPowerOnEquipProcessor))]
    [DefaultEntityComponentProcessor(typeof(SowPowerOnPullProcessor))]
    public class SowPowerComponent : EntityComponent, ITakeEffectOnEquipOrPullFrame
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



