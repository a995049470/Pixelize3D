using System;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class GetEntityCommand : ICommand
    {
        public ulong UID;
        public Action<Entity> CallBack;
        public SceneSystem SceneSystem; 
        public void Execute()
        {
            if(SceneSystem.SceneInstance.TryGetEntity(UID, out var entity))
            {
                CallBack?.Invoke(entity);
            }
        }
    }
}
