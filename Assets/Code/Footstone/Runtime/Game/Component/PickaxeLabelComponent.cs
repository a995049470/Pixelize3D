using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class PickaxeLabelComponent : EntityComponent
    {
        [UnityEngine.HideInInspector]
        public bool IsPickaxed;
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
        public void Pickaxe(Entity entity)
        {
            Target = entity;
            IsPickaxed = true;
        }        
    }
}



