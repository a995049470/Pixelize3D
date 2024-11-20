using Lost.Runtime.Footstone.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    /// <summary>
    /// 可以装备的
    /// </summary>
    [DefaultEntityComponentProcessor(typeof(EquipProcessor))]
    [DefaultEntityComponentProcessor(typeof(FinishEquipProcessor))]
    [DefaultEntityComponentProcessor(typeof(AutoRecycleEquipableProcessor))]
    public class EquipLabelComponent : EntityComponent
    {
        [HideInInspector]
        public bool IsEquipping;
        [HideInInspector]
        public bool IsEffect = false;
        public IEnumerable<EntityComponent> CacheComponents { get; set; }
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
        
        public void SetTimePoint(bool isEquipping, Entity entity)
        {
            IsEquipping = isEquipping;
            IsEffect = false;
            Target = entity;
        }
    }
    
}
