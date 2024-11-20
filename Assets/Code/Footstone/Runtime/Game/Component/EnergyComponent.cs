using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{   
    [DefaultEntityComponentProcessor(typeof(PlayerEnergeProcessor))]
    [DefaultEntityComponentProcessor(typeof(EnergyProcessor))]
    public class EnergyComponent : EntityComponent
    {
        public float MaxEnergy = 100;
        public float CurrentEnergy = 100;
        public float NaturalRecovery = 40;

        public float EnergyPercent { get => Mathf.Clamp01(CurrentEnergy / MaxEnergy); }  

        private float naturalRecoveryFailTimer = 0;
        

        public void ReceiveEnerge(float value)
        {
            CurrentEnergy = Mathf.Min(CurrentEnergy + value, MaxEnergy);
        }   

        public void NaturalRecoveryEnergy(float deltaTime)
        {
            naturalRecoveryFailTimer -= deltaTime;
            if(naturalRecoveryFailTimer < 0)
            {
                ReceiveEnerge(NaturalRecovery * deltaTime);
            }
        }
        
        /// <summary>
        /// 尝试消耗能量
        /// </summary>
        /// <param name="value">消耗良</param>
        /// <param name="isForceClear">能量不足时清零</param>
        /// <returns></returns>
        public bool TryCostEnerge(float value, float fatigueTime, bool isForceClear = false)
        {
            bool isSuccess = false;
            if(CurrentEnergy >= value)
            {
                isSuccess = true;
                CurrentEnergy -= value;
            }
            else if(isForceClear)
            {
                CurrentEnergy = 0;
            }
            naturalRecoveryFailTimer = Mathf.Max(fatigueTime, naturalRecoveryFailTimer);

            return isSuccess;
        }

        public void ClearFatigueTime()
        {
            naturalRecoveryFailTimer = 0;
        }

        public bool HasEngoughEnery(float value)
        {
            return CurrentEnergy >= value;
        }


        

    }

}
