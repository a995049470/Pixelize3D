using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    [AllowMultipleComponents]
    [DefaultEntityComponentProcessor(typeof(PickaxePowerOnEquipProcessor))]
    [DefaultEntityComponentProcessor(typeof(PickaxePowerOnPullProcessor))]
    public class PickaxePowerComponent : EntityComponent, ITakeEffectOnEquipOrPullFrame
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



