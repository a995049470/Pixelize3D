using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    [System.Serializable]
    public class AttackEvent
    {
        //1.0f 时间点的事件不触发
        [Range(0, 0.99f)]
        public float NormalizedTime;
        public float Coefficient;
    }
}