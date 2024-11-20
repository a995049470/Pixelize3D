using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class InstantiateFallObjectCommand : ICommand
    {
        public string FallName;
        public ResPoolManager EntityPoolManager;
        public Vector3 Position;
        public void Execute()
        {
            var entity = EntityPoolManager.InstantiateEntity(FallName, ResFlag.Entity_Drop);
            entity.Transform.Position = Position;
        }
    }

    
}
