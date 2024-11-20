using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Lost.Runtime.Footstone.Collection;
using UnityEngine;

namespace Lost.Runtime.Footstone.Core
{
    
    [ExecuteInEditMode]
    public class Entity : MonoBehaviour
    {
        public static Entity CreateEmpty()
        {
            var go = new GameObject();
            var entity = go.AddComponent<Entity>();
            entity.GetOrCreate<TransformComponent>();
            return entity;
        }

        public string Name { get => gameObject.name; set => gameObject.name = value; }
        public TransformComponent Transform;
        public EntityManager EntityManager { get; set; }
        private EntityComponentCollection components;
        public EntityComponentCollection Components { get => components; }
        
        [UnityEngine.HideInInspector]
        public ulong Id;
        
        private bool notRecycleUniqueIdNextTime = false;

        public bool Active 
        { 
            get => this.gameObject.activeSelf; 
            set => this.gameObject.SetActive(value);
        }
        
        [NonSerialized]
        public Scene SceneValue;
        
        private SceneSystem sceneSystem;
        private UniqueIdManager uniqueIdManager;
        //最外部的entity
        private bool isRootEntity;
        //所有Enity都会有Scene
        public Scene Scene
        {
            get
            {
                return SceneValue;
                //return this.FindRoot().SceneValue;
            }
            set
            {
                var oldScene = SceneValue;
                if (oldScene == value)
                    return;

                oldScene?.Entities.Remove(this);
                value?.Entities.Add(this);
            }
        }

        private void Awake() {
#if UNITY_EDITOR
            if (Application.isPlaying)
#endif
            {
                Initialize(StoneHeart.Instance.Services);
            }
        }

        private void OnEnable()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (this.GetComponent<TransformComponent>() == null) Transform = this.gameObject.AddComponent<TransformComponent>();
            }
#endif

#if UNITY_EDITOR
            if (Application.isPlaying)
#endif
            {
                //TODO:根Enity才需要把自己加到Entities中?
                //TODO:Entity一定需要在根物体
                //isRootEntity = Transform.Parent == null;
                //if(isRootEntity) 
                    AddToScene(sceneSystem.SceneInstance.RootScene);
            }
        }

        private void OnDisable()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
#endif
            {
                    RemoveFromScene();
            }
        }

        private void OnDestroy() {
            //销毁时可能存在uid没回收的情况，所以要进行强制回收
            if (Application.isPlaying)
            {
                ForceRecycleUniqueId();
            }
        }

        protected virtual void Initialize(IServiceRegistry service)
        {
            //初始化
            components = new(this);
            sceneSystem = service.GetService<SceneSystem>();
            uniqueIdManager = service.GetService<UniqueIdManager>();
        }
        

        public T GetOrCreate<T>() where T : EntityComponent
        {
            var component = Get<T>();
            if (component == null)
            {
                component = this.gameObject.GetComponent<T>();
                
                if(component == null) component = this.gameObject.AddComponent<T>();
                
                components.Add(component);
                    
            }
            return component;
        }

        /// <summary>
        /// 将自身添加到Entity中
        /// </summary>
        /// <param name="component"></param>
        public void Add(EntityComponent component)
        {
            components.Add(component);
        }

        public void Add(IEnumerable<EntityComponent> _components)
        {
            components.AddRange(_components);
        }

        /// <summary>
        /// 添加组件,但是不检查
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T AddNoCheck<T>() where T : EntityComponent
        {
            var component = this.gameObject.AddComponent<T>();
            components.Add(component);
            return component;
        }

        public EntityComponent AddNoCheck(Type type)
        {
            var component = this.gameObject.AddComponent(type) as EntityComponent;
            components.Add(component);
            return component;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Get<T>() where T : EntityComponent
        {
            return components.Get<T>();
        }


        public FastCollection<EntityComponent> GetEntityComponents<T>() where T : class
        {
            var res = new FastCollection<EntityComponent>();
            var count = components.Count;
            for (int i = 0; i < count; i++)
            {
                var comp = components[i];
                if (comp is T t)
                {
                    res.Add(comp);
                }
            }
            return res;
        }

        public FastCollection<EntityComponent> GetEntityComponents(Func<EntityComponent, bool> func) 
        {
            var res = new FastCollection<EntityComponent>();
            var count = components.Count;
            for (int i = 0; i < count; i++)
            {
                var comp = components[i];
                if(func.Invoke(comp))
                {
                    res.Add(comp);
                }
            }
            return res;
        }

        public int GetEntityComponentsNonAlloc<T>(EntityComponent[] entityComponents) where T : class
        {
            var maxCount = entityComponents.Length;
            var currentCount = 0;
            var count = components.Count;
            for (int i = 0; i < count; i++)
            {
                if(maxCount == currentCount) break;
                var comp = components[i];
                 if (comp is T)
                {
                    entityComponents[currentCount] = comp;
                    currentCount ++;
                }
            }
            return currentCount;
        }

        public bool Has<T>() where T : EntityComponent
        {
            return components.Get<T>() != null;
        }

        public void Remove<T>(bool isClearCacheEntity = false) where T : EntityComponent
        {
            var component = Get<T>();
            if (component != null)
            {
                components.Remove(component);
                if(isClearCacheEntity) component.ClearCacheEntity();
            }
        }

        internal void Remove(EntityComponent component, bool isClearCacheEntity = false)
        {
            components.Remove(component);
            if(isClearCacheEntity) component.ClearCacheEntity();
        }

        public void Remove(IEnumerable<EntityComponent> entityComponents, bool isClearCacheEntity = false)
        {
            foreach (var entityComponent in entityComponents)
            {
                Remove(entityComponent, isClearCacheEntity);
            }
        }

        public FastCollection<EntityComponent> RemoveComponents<T>()
        {
            int count = components.Count;
            var res = new FastCollection<EntityComponent>();
            for (int i = count - 1; i >= 0; i--)
            {
                var comp = components[i];
                if(comp is T)
                {
                    components.RemoveAt(i);
                    res.Add(comp);
                }
            }
            return res;
        }
        
        /// <summary>
        /// 阻止 Entity和持有的EntityComponent 的下次uid回收
        /// </summary>
        public void OnSceneCloseWithComponents()
        {
            this.OnSceneClose();
            foreach (var component in components)
            {
                component.OnSceneClose();
            }
        }

        public void OnSceneClose()
        {
            notRecycleUniqueIdNextTime = true;
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

        public void OnComponentChanged(int index, EntityComponent oldComponent, EntityComponent newComponent)
        {
            EntityManager?.NotifyComponentChanged(this, index, oldComponent, newComponent);
        }

        internal void AddReferenceInternal()
        {

        }

        internal void ReleaseInternal()
        {

        }


        // public void RemoveRootEntity()
        // {
        //     if(TryFindRootEntity(out var root)) root.RemoveFromScene();
        // }

        // private bool TryFindRootEntity(out Entity root)
        // {
        //     root = this;
        //     while (root != null && !root.isRootEntity)
        //     {
        //         var parent = root.Transform.Parent;
        //         root = parent == null ? null : parent.Entity;
        //     }
        //     return root != null;
        // }

        /// <summary>
        /// 将Entity移除，不会对GameObject的Active进行设置
        /// </summary>
        public void RemoveFromScene()
        {
            if(Scene != null) 
                Scene.Entities.Remove(this);
            
        }   

        /// <summary>
        /// 将Entity添加，不会对GameObject的Active进行设置
        /// </summary>
        public void AddToScene(Scene _scene)
        {
            if(Scene == null) _scene.Entities.Add(this);
        }
        
        /// <summary>
        /// 对gameobject设置active
        /// </summary>
        /// <param name="active"></param>
        /// <param name="isDestory"></param>
        public void SetUnityGameObjectActive(bool active)
        {
            this.gameObject.SetActive(active);
        }

        /// <summary>
        /// 对gameobject进行销毁
        /// </summary>
        public void DestoryUnityGameObject()
        {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }

        /// <summary>
        /// 部分组件需要在Entity实例化过后进行某些操作
        /// </summary>
        public void InvokeEntityInstantiateCallback()
        {
            // foreach (var component in components)
            // {
            //     if(component is IEntityInstantiateCallback callback)
            //         callback.LateEntityInstantiation();
            // }
        }

        public Entity Instantiate()
        {
        #if UNITY_EDITOR
            //var go = UnityEditor.PrefabUtility.InstantiatePrefab(this.gameObject) as GameObject;
            var go = GameObject.Instantiate(this.gameObject);
        #else
            var go = GameObject.Instantiate(this.gameObject);
        #endif
            var result = go.GetComponent<Entity>();
            return result;
        }

    }
}



