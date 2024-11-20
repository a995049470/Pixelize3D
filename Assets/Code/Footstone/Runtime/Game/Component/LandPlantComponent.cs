using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    /// <summary>
    /// 这是一个 种在土地上的植被 组件
    /// </summary>
    [DefaultEntityComponentProcessor(typeof(AutoRecyclePlantProcessor))]    
    [DefaultEntityComponentProcessor(typeof(RenderLandPlantProcessor))]
    public class LandPlantComponent : EntityComponent
    {
        [System.NonSerialized]
        public ulong PlantEntityId;
        [System.NonSerialized]
        public string PlantKey;
        //[System.NonSerialized]
        public string NextPlantKey;
        public PlantFlag CurrentPlantFlag = PlantFlag.Seed; 
        public int RemainGrowthPower;
        //用于下一次结果的时间减少
        [HideInInspector]
        public float AdultStateSpeedUpProgress;
        
        public void Refruit(float speedUpProgress)
        {
            CurrentPlantFlag = PlantFlag.Adult;
            AdultStateSpeedUpProgress = speedUpProgress;
        }


        // protected override void OnEnableRuntime()
        // {
        //     CurrentPlantFlag = PlantFlag.Seed;
        //     RemainGrowthPower = 0;
        // }



    }
}
