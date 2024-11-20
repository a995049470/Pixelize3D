using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    /// <summary>
    /// TODO:随时同步或只在保存时同步
    /// </summary>
    [SerializeField]
    public class CharacterAttributesData 
    {
        public float Attack = 5;
        public float MaxHp = 100;
        //public float CurrentHp = 100;
        public float MaxEnergy = 100;
        //public float CurrentEnergy = 100;
        public float NaturalRecovery = 40;
    }
}
