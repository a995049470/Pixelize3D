using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    /// <summary>
    /// 可挖掘标间组件
    /// </summary>
    [DefaultEntityComponentProcessor(typeof(DeleteDigLabelProcessor))]
    public class DigLabelComponent : EntityComponent
    {
        //[System.NonSerialized]
        //public Entity Owner;
        [UnityEngine.HideInInspector]
        public bool IsDigged;
        public void Dig(Entity entity)
        {
            //Owner = entity;
            IsDigged = true;
        }

        // protected override void OnEnableRuntime()
        // {
        //     base.OnEnableRuntime();
        //     IsDigged = false;
        // }
    }

}



