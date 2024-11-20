using System;
using Lost.Runtime.Footstone.Collection;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public abstract class OneShotProcessor<T> : SimpleGameEntityProcessor<T> where T : EntityComponent
    {
        public OneShotProcessor() : base()
        {
            Order = ProcessorOrder.FrameEnd;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                cmd.RemoveEntityComponent(kvp.Key);
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
