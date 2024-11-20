using UnityEngine;

namespace Lost.Runtime.Footstone.Core
{
    
    //TODO:可能不需要依赖Entity
    [ExecuteInEditMode]
    [RequireComponent(typeof(Entity))]
    public abstract class EntityComponent : MonoBehaviour
    {
        //编辑下缓存entity
        [SerializeField]
        private ChildComponentReference<Entity> childEntityReference = new();
        protected Entity cacheEntity { get => childEntityReference.Component; set => childEntityReference.Component = value; }
        public Entity CacheEntity { get => cacheEntity; }
        private Entity entity;
        //在EntityComponentCollection中对其进行赋值 
        //标记为不可序列化 使其instantiate时不被赋值
        public Entity Entity
        {
            get => entity;
            set
            {
                entity = value;
                if(value != null && value != cacheEntity)
                    cacheEntity = value;
            }
        }

        //TODO：需要这个字段？
        [SerializeField][HideInInspector]
        public bool isLive;
        
        protected IServiceRegistry service;
        protected ContentManager content;
        protected SceneSystem sceneSystem;
        protected UniqueIdManager uniqueIdManager;
        
        
        [UnityEngine.HideInInspector]
        public ulong Id;

        private bool notRecycleUniqueIdNextTime = false;
        private bool notInvokeEnableRuntimeNextTime = false;
        private bool notInvokeDisableRuntimeNextTime = false;

        
        protected virtual void Awake()
        {
        #if UNITY_EDITOR
            if(Application.isPlaying)
        #endif
            {
                Initialize(StoneHeart.Instance.Services);
            }
        }

        protected virtual void OnEnable() {
            UpdateReference();
        #region 编辑器下执行
        #if UNITY_EDITOR
            if(!Application.isPlaying)
            {
                var target = this.GetComponent<Entity>();
                if(cacheEntity != target) 
                {
                    cacheEntity = target;
                    UnityEditor.EditorUtility.SetDirty(this);
                }
            }
        #endif
        #endregion

        #region 运行时执行
        //TODO:考虑增加开关控制运行时的OnEnbale的运行
        #if UNITY_EDITOR
            if(Application.isPlaying)
        #endif
            {
                //cache entity不为空, 说明该组件是在编辑器下添加的
                if(cacheEntity != null) 
                {
                    AddToEntity(cacheEntity);
                }
                if (!isLive)
                {
                    if(notInvokeEnableRuntimeNextTime)
                    {
                        notInvokeEnableRuntimeNextTime = false;
                    }
                    else
                    {
                        OnEnableRuntime();
                    }
                    isLive = true;
                }
            }
        #endregion
            
        }

        private void OnDisable() {
        #if UNITY_EDITOR
            if(Application.isPlaying)
        #endif
            {
                //通过代码添加的EntityComponent组件cacheEntity为空
                //OnDisable时需要对cacheEntity进行赋值
                //if(cacheEntity == null) cacheEntity = Entity;
                RemoveFromEntity();
                if(isLive)
                {
                    if(notInvokeDisableRuntimeNextTime)
                    {
                        notInvokeDisableRuntimeNextTime = false;
                    }
                    else
                    {
                        OnDisableRuntime();
                    }
                    isLive = false;
                }
            }
        }

        private void OnDestroy() {
            //销毁时可能存在uid没回收的情况，所以要进行强制回收
            if (Application.isPlaying)
            {
                ForceRecycleUniqueId();
            }
        }

        public virtual void UpdateReference()
        {
            childEntityReference.Root = this.transform;
        }

        /// <summary>
        /// 取消下次回收UID 让uid继续使用
        /// </summary>
        public void OnSceneClose()
        {
            notRecycleUniqueIdNextTime = true;
            notInvokeEnableRuntimeNextTime = true;
            notInvokeDisableRuntimeNextTime = true;
        }   
        
        public void CreateUniqueId()
        {
            if(!uniqueIdManager.IsVaild(Id))
            {
                Id = uniqueIdManager.CreateUniqueId();
            }
        }

        public void RecycleUniqueId()
        {
            if(notRecycleUniqueIdNextTime)
            {
                notRecycleUniqueIdNextTime = false;
            }
            else
            {
                uniqueIdManager.RecycleUniqueId(ref Id);
            }
        }

        private void ForceRecycleUniqueId()
        {
            uniqueIdManager.RecycleUniqueId(ref Id);
        }

        public void ResetUniqueId(ulong id)
        {
            if(uniqueIdManager.IsVaild(id))
            {
                uniqueIdManager.RecycleUniqueId(ref Id);
                Id = id;
            }
        }

        
        protected virtual void Initialize(IServiceRegistry registry)
        {
            service = registry;
            content = registry.GetService<ContentManager>();
            sceneSystem  = registry.GetService<SceneSystem>();
            uniqueIdManager = registry.GetService<UniqueIdManager>();
        }

        /// <summary>
        /// 运行模式OnEnable会调用该方法
        /// </summary>
        protected virtual void OnEnableRuntime()
        {
            
        }
        
        /// <summary>
        /// 运行模式OnDisable会调用该方法
        /// </summary>
        protected virtual void OnDisableRuntime()
        {

        }
        /// <summary>
        /// 将组件移除但是不清除自身的CacheEntity
        /// </summary>
        public void RemoveFromEntity()
        {
            if(Entity != null) 
            {
                Entity.Remove(this);
            }
        }

        /// <summary>
        /// 清理cacheEntity 防止激活时自动添加
        /// </summary>
        public void ClearCacheEntity()
        {
            cacheEntity = null;
        }

        /// <summary>
        /// 将组件添加,不会对物体的enable进行设置
        /// </summary>
        public void AddToEntity(Entity targetEntity)
        {
            if(Entity == null && targetEntity != Entity)
            {
                targetEntity.Add(this);
            }
        }

        /// <summary>
        /// 对Component设置active或销毁
        /// </summary>
        /// <param name="active"></param>
        /// <param name="isDestory"></param>
        public void SetUnityComponentActive(bool isActive, bool isDestory = false)
        {
            if(isDestory) Destroy(this);
            else this.enabled = isActive;
        }
    }
}



