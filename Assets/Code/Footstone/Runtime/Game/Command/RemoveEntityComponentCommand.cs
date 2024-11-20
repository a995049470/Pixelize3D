using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class RemoveEntityComponentCommand : ICommand 
    {
        public Entity Target;
        public EntityComponent Component;

        public void Execute()
        {
            Target?.Remove(Component, true);
        }
    }

    public class RemoveEntityComponentCommand<T> : ICommand where T : EntityComponent
    {
        public Entity Target;

        public void Execute()
        {
            Target?.Remove<T>(true);
        }
    }
}
