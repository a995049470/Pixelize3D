using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public abstract class DeleteOnFrameEndProcessor<T> : SimpleGameEntityProcessor<T> where T : EntityComponent
    {
        public DeleteOnFrameEndProcessor() : base()
        {
            Order = ProcessorOrder.FrameEnd;
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
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
