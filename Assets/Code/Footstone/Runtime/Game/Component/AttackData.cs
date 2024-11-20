using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    [CreateAssetMenu(menuName = "Lost/Attack/AttackData")]
    public class AttackData : ScriptableObject
    {   
        public float AttackDuration;
        public int AttackRange = 1;
        public AnimationClipEventControllerReference ClipEventControllerReference;
        public DamageFlag DamageFlag = DamageFlag.Physical;
        public LayerMask AttackTargetLayer;
        public float AttackCostEnergy = 20;
        //默认攻击的subIndex用0表示
        public int AttackSubIndex = 0;
        public int IdleSubIndex = 0;
        //攻击时是否禁止其他行为
        public bool IsBanAction = true;
    }
}
