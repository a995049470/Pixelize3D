using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    /// <summary>
    /// 可拾取物体
    /// </summary>
    [DisallowMultipleComponent]
    [DefaultEntityComponentProcessor(typeof(AutoRecyclePickableProcessor))]
    [DefaultEntityComponentProcessor(typeof(BePickedUpProcessor))]
    [DefaultEntityComponentProcessor(typeof(FinishBePickedUpProcessor))]
    public class PickLabelComponent : EntityComponent
    {
        //该物体是否还能生效
        [System.NonSerialized]
        public bool IsVaild = true;
        //为空代表还未生效
        [System.NonSerialized]
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
        
        public void Pick(Entity entity)
        {
            Target = entity;
            IsVaild = true;
            IsEffect = false;
        }

        protected override void OnEnableRuntime()
        {
            
        }
    }

}
