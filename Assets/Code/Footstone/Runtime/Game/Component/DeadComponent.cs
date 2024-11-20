using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{


    [DisallowMultipleComponent]
    [DefaultEntityComponentProcessor(typeof(DeadRecycleProcessor))]
    [DefaultEntityComponentProcessor(typeof(DeadDropProcessor))]
    public class DeadComponent : EntityComponent
    {
        public float DestoryDelay = 0;
        //DeadTime只在DeadProcessor里产生计数
        [HideInInspector]
        public float DeadTime = 0;
        public string ParticleName;
        //正在死亡
        public bool IsDeading()
        {
            return DeadTime > 0;
        }
        
        public bool WillDesotry(float deltaTime)
        {
            return DeadTime <= DestoryDelay && DeadTime + deltaTime > DestoryDelay;
        }


        protected override void OnEnableRuntime()
        {
            DeadTime = 0;
        }

    }

}
