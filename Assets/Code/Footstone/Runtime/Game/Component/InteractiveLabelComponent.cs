using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class InteractiveLabelComponent : EntityComponent
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
