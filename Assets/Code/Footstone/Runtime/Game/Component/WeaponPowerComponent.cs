using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    //该组件允许复数存在，该组件在processor需要当Key
    [AllowMultipleComponents]
    [DefaultEntityComponentProcessor(typeof(WeaponPowerOnEquipProcessor))]
    [DefaultEntityComponentProcessor(typeof(WeaponPowerOnPullProcessor))]
    public class WeaponPowerComponent : EntityComponent, ITakeEffectOnEquipOrPullFrame
    {
        /// <summary>
        /// 武器攻击力数据
        /// </summary>
        public float AttackValue;
        /// <summary>
        /// 攻击过程相关数据
        /// </summary>
        public AttackDataTableReference WeaponAttackDataTable;
        public string LeafHandWeaponKey;
        public string RightHandWeaponKey;

        public bool IsEquipping { get; private set; }
        public ulong PowerUID;
        public void SetTimePoint(bool isEquipping, ulong uid)
        {
            IsEquipping = isEquipping;
            PowerUID = uid;
        }
    }
}



