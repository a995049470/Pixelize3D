using System.Collections.Generic;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    [CreateAssetMenu(menuName = "Lost/Attack/AttackDataTable")]
    public class AttackDataTable : ScriptableObject
    {
        [SerializeField]
        private List<AttackData> datas = new();

        public AttackData GetAttackData(int index)
        {
            return datas[index];
        }
    }
}
