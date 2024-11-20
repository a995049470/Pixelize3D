using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    

    [DefaultEntityComponentProcessor(typeof(StartTakeEffectOnUseProcessor))]
    [DefaultEntityComponentProcessor(typeof(FinishTakeEffectOnUseProcessor))]
    [DefaultEntityComponentProcessor(typeof(AutoRecycleUseableProcessor))]
    public class UseLabelComponent : EntityComponent
    {
        [System.NonSerialized]
        public bool IsVaild = true;
        //为空代表还未生效
        public IEnumerable<EntityComponent> CacheComponents;
        //生效成功
        [HideInInspector]
        public bool IsEffect = false;
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

        public void Use(Entity entity)
        {
            IsVaild = true;
            IsEffect = false;
            Target = entity;
        }

        
    }

}
