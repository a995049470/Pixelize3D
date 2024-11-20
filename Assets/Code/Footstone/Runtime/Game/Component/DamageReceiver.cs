using System.Collections.Generic;
using UnityEngine;


namespace Lost.Runtime.Footstone.Game
{
    [System.Serializable]
    public class DamageReceiver
    {
        [SerializeField]
        private List<float> damages = new List<float>();
        private bool isDirty = true;
        private float sum = 0;
        public float Sum 
        {
            get
            {
                if(isDirty)
                {
                    isDirty = false;
                    sum = 0;
                    for (int i = 0; i < damages.Count; i++)
                    {
                        sum += damages[i];
                    }
                }
                return sum;
            }
        }

        private void ResizeDamages(int index)
        {
            if (index >= damages.Count)
            {
                damages.AddRange(new float[index - damages.Count + 1]);
            }
        }

        public void ReceiveDamage(DamageFlag flag, float value)
        {
            var index = ((int)flag);
            ResizeDamages(index);
            damages[index] += value;
            isDirty = true;
        }

        public float GetDamage(DamageFlag flag)
        {
            var index = ((int)flag);
            ResizeDamages(index);
            return damages[index]; 
        }

        public void SetDamage(DamageFlag flag, float val)
        {
            var index = ((int)flag);
            //不检查越界
            var originVal = damages[index];
            if(originVal != val)
            {
                isDirty = true;
                damages[index] = val;
            }
        }
        

        public void Clear()
        {
            if(Sum > 0)
            {
                sum = 0;
                isDirty = false;
                for (int i = 0; i < damages.Count; i++)
                {
                    damages[i] = 0;
                }   
            }
        }
    }

}
