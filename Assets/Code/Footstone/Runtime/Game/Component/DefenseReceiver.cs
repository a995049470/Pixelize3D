using System;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    //单项防御值的接受器
    [System.Serializable]
    public class DefenseReceiver
    {
        //基础减伤率
        [SerializeField][Range(0, 1)]
        private float baseDamageReductionRate = 0;
        //基础固定减伤值
        [SerializeField]
        private float baseFixedDamageReduction = 0;
        
        private float extraDamageReductionRate = 0;
        private float extraFixedDamageReduction = 0;

        public void ReceiveDamageReductionRate(float val)
        {
            extraDamageReductionRate = 1 - (1 - extraDamageReductionRate) * (1 - val);
        }
        
        public void ReceiveFixedDamageReduction(float val)
        {
            extraFixedDamageReduction += val;
        }

        public float CalculateDamage(float dmg)
        {
            var result = dmg * (1 - baseDamageReductionRate) * (1 - extraDamageReductionRate) - (baseFixedDamageReduction + extraFixedDamageReduction);
            result = Mathf.Max(result, 0);
            return result;
        }

        public void Claer()
        {
            extraDamageReductionRate = 0;
            extraFixedDamageReduction = 0;
        }
        
    }
}
