using Lost.Runtime.Footstone.Core;
using System;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    [DisallowMultipleComponent]
    public class OwnerComponent : EntityComponent
    {
        [SerializeField][HideInInspector]
        private ulong uid;
        public Entity Target
        {
            set
            {
                if(value) uid = value.Id;
                else uid = uniqueIdManager.InvalidId;
            }
            get
            {
                sceneSystem.SceneInstance.TryGetEntity(uid, out var entity);
                return entity;
            }
        }
        
    }

}
