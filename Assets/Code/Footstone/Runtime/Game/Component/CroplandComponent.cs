using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{


    [DefaultEntityComponentProcessor(typeof(WaterCroplandProcessor))]
    [DefaultEntityComponentProcessor(typeof(DigCroplandProcessor))]
    [DefaultEntityComponentProcessor(typeof(CroplandRenderProcessor))]
    public class CroplandComponent : EntityComponent
    {
        [SerializeField]
        private CroplandFlag croplandFlag;
        public CroplandFlag CurrnetCroplandFlag 
        {
            get => croplandFlag;
            set
            {
                if(value != croplandFlag)
                {
                    croplandFlag = value;
                    IsRenderDirty = true;
                }
            }
        }
        public int LandMaterialIndex = 0;
        //public bool IsPlanted { get; set; }
        
        /// <summary>
        /// 能倍播种？
        /// </summary>
        /// <returns></returns>
        //public bool IsCanBeSeeded { get => !IsPlanted && (croplandFlag & CroplandFlag.Dig) > 0; }
        public bool IsDig { get => (croplandFlag & CroplandFlag.Dig) > 0; }
        [System.NonSerialized]
        public bool IsRenderDirty = true;

        protected override void OnEnableRuntime()
        {
            IsRenderDirty = true;
        }

        private void OnValidate() {
            IsRenderDirty = true;
        }

        public int GetMaterialIndex()
        {
            return ((int)croplandFlag);
        }

    }

}
