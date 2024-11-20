using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    /// <summary>
    /// 受伤表现组件
    /// </summary>
    [DefaultEntityComponentProcessor(typeof(HurtEffectProcessor))]
    [DefaultEntityComponentProcessor(typeof(HurtFlashProcessor))]
    public class HurtEffectComponent : EntityComponent
    {
        [SerializeField]
        private float flashDuration;
    
        public string ParticletName;        

        public Color FlashColor;

        [HideInInspector]
        public Vector2 FlashTimer;

        public bool IsFlash { get => FlashTimer.y > FlashTimer.x; }
        [HideInInspector]
        public bool IsFlashStateChange = false;


        public void OnReceiveDamage()
        {
            FlashTimer.x = 0;
            FlashTimer.y = flashDuration;
        }


    }

}
