using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    [AllowMultipleComponents]
    [DefaultEntityComponentProcessor(typeof(PlaceObjectPowerOnEquipProcessor))]
    [DefaultEntityComponentProcessor(typeof(PlaceObjectPowerOnPullProcessor))]
    public class PlaceObjectPowerComponent : EntityComponent, ITakeEffectOnEquipOrPullFrame
    {
        public bool IsEquipping { get; private set; }
        [UnityEngine.HideInInspector]
        public ulong PowerUID;
        public void SetTimePoint(bool isEquipping, ulong uid)
        {
            IsEquipping = isEquipping;
            PowerUID = uid;
        }
    }

}



