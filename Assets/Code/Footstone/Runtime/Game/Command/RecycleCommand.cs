using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class RecycleCommand : ICommand
    {
        public Entity Target;
        public string Key;
        public ResFlag Flag;
        public ResPoolManager EntityPoolManager;
        public void Execute()
        {
            if(string.IsNullOrEmpty(Key))
                Target.DestoryUnityGameObject();
            else
                EntityPoolManager.RecycleEntity(Key, Flag, Target);
            Target = null;
        }
    }
}
