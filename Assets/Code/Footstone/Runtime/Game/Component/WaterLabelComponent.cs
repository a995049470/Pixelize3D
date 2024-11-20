using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    //浇水标签
    [DefaultEntityComponentProcessor(typeof(DeleteWaterLabelProcessor))]
    public class WaterLabelComponent : EntityComponent
    {
        // [System.NonSerialized]
        // public Entity Owner;
        [UnityEngine.HideInInspector]
        public bool IsWatered;
        public void Water(Entity entity)
        {
            //Owner = entity;
            IsWatered = true;
        }

        protected override void OnEnableRuntime()
        {
            base.OnEnableRuntime();
            IsWatered = false;
        }

    }
}



