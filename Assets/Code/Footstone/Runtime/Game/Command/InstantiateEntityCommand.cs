using System;
using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    public class InstantiateEntityCommand : ICommand
    {
        public string Key;
        public ResFlag ResFlag;
        public ResPoolManager ResPoolManager;
        public Vector3 Position;
        public SceneSystem SceneSystem;
        public UniqueIdManager UniqueIdManager;
        public ulong Id;
        public Action<Entity> Callback;

        public void Execute()
        {
            var entity = ResPoolManager.InstantiateEntity(Key, ResFlag);
            entity.Transform.Position = Position;
            if(UniqueIdManager.IsVaild(Id))
                SceneSystem.SceneInstance.ChangeEntityId(entity, Id);
            Callback?.Invoke(entity);
        }
    }

    
}
