using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    [DisallowMultipleComponent]   
    [DefaultEntityComponentProcessor(typeof(ParticleSystemRecycleProcessor))]
    public class AutoRecycleComponent : EntityComponent
    {
        public string Key;
        public ResFlag Flag;
        public bool IsRecycle = true;

        public void RecycleEntity(CommandBuffer cmd, Entity entity)
        {
            if(IsRecycle)
                cmd.RecycleEntity(Key, Flag, entity);
            else
                cmd.DestoryEntity(entity);
        }
        
        public void Recycle(CommandBuffer cmd)
        {
            if(IsRecycle)
                cmd.RecycleEntity(Key, Flag, this.Entity);
            else
                cmd.DestoryEntity(this.Entity);
        }
    }

}
