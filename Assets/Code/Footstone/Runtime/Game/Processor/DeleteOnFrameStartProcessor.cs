using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public abstract class DeleteOnFrameStartProcessor<T> : SimpleGameEntityProcessor<T> where T : EntityComponent
    {
        public DeleteOnFrameStartProcessor() : base()
        {
            Order = ProcessorOrder.FrameStart;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                cmd.RemoveEntityComponent(kvp.Value.Component1);
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }   
        
    }
}
