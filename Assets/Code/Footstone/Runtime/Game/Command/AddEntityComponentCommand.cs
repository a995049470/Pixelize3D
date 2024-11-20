using System;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class AddEntityComponentCommand : ICommand 
    {

        public Entity Target;
        public EntityComponent Component;

        public void Execute()
        {
            Target.Add(Component);
        }
    }

    public class AddEntityComponentCommand<T> : ICommand where T : EntityComponent
    {
        public ulong UID;
        public Entity Target;
        public Action<T> CallBack;
        public SceneSystem SceneSystem;

        public void Execute()
        {
            if(Target == null)
                SceneSystem.SceneInstance.TryGetEntity(UID, out Target);
            var t = Target?.GetOrCreate<T>();
            CallBack?.Invoke(t);
        }
    }
}
