using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    [DefaultEntityComponentProcessor(typeof(PlantFruitStateProcessor))]
    [DefaultEntityComponentProcessor(typeof(PlantFruitStateComponentCopyProcessor))]
    [DefaultEntityComponentProcessor(typeof(InteractionPickFruitProcessor))]
    public class PlantFruitStateComponent : EntityComponent
    {
        public string Key;
        //达成会切换到下个阶段
        public int TargetPower = 5;
        public int CurrentPower;
        public string Fruit;
        public int Count;
        //成熟次数
        public int TargetRefruitCount = 1;
        //当前成熟次数
        [HideInInspector][SerializeField]
        private int currentRefruitCount = 0;

        protected override void OnEnableRuntime()
        {
            base.OnEnableRuntime();
            currentRefruitCount = 0;
        }

        public bool IsDieAfterPick()
        {
            bool isDie = false;
            currentRefruitCount += 1;
            if(currentRefruitCount >= TargetRefruitCount)
            {
                isDie = true;
                currentRefruitCount = 0;
            }
            return isDie;
        }
    }
}
