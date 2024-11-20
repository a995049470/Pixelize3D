using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    [DefaultEntityComponentProcessor(typeof(BuffProcessor))]
    [DefaultEntityComponentProcessor(typeof(FinishBuffProcessor))]
    [DefaultEntityComponentProcessor(typeof(AutoRecycleBufferProcessor))]
    public class BufferComponent : EntityComponent
    {
        public bool IsEffect = false;
        public IEnumerable<EntityComponent> CacheComponents { get; set; }
        protected override void OnEnableRuntime()
        {
            base.OnEnableRuntime();
            IsEffect = false;
        }
    }
    
}
