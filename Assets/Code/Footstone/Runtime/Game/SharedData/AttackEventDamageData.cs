using System.Collections.Generic;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    [CreateAssetMenu(menuName = "Lost/Attack/AttackEventDamageData")]
    public class AttackEventDamageData : ScriptableObject
    {
        //TODO:强制从大到小排列？
        [Header("需要从小到大排列")]
        [SerializeField]
        private AttackEvent[] attackEvents;

        public bool TryGetAttackEvent(int ptr, ref AttackEvent attackEvent)
        {
            bool isGet = false;
            if(ptr >= 0 && ptr < attackEvents.Length)
            {
                attackEvent = attackEvents[ptr];
                isGet = true;
            }
            return isGet;
        }
    }   
}