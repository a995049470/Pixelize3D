using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    /// <summary>
    /// 快捷装备栏
    /// </summary>
    [DefaultEntityComponentProcessor(typeof(FastEquipProcessor))]
    public class FastEquipComponent : EntityComponent
    {
        public ulong ItemUid;
        public ulong PowerUID;
    }

}



