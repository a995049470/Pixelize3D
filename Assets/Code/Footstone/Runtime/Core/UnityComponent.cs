using UnityEngine;

namespace Lost.Runtime.Footstone.Core
{

    [ExecuteInEditMode]
    public abstract class UnityComponent<T> : EntityComponent where T : Component
    {
        
        /// <summary>
        /// 运行中添加组件时需要自己对Component进行赋值
        /// </summary>
        public T Component { get => reference.Component; set => reference.Component = value;}
        [SerializeField]
        private ChildComponentReference<T> reference = new();

        protected override void OnEnable()
        {
            base.OnEnable();
        #if UNITY_EDITOR
            if(!Application.isPlaying && Component == null) 
            {
                var target = this.GetComponent<T>();
                //编辑下的添加的UnityComponent必须对Component赋值
                //可以使用子物体的UnityComponent对Component
                if(target == null) Debug.LogError($" 注意对Component进行赋值  {this.GetType().Name}", this.gameObject);
                if(Component != target) Component = target;
            }
        #endif
        #if UNITY_EDITOR
            if(Application.isPlaying) 
        #endif
            {
                //运行时增加的UnityComponent组件需要自己新建Componet组件
                if(Component == null)
                {
                #if UNITY_EDITOR
                    if(Component == null) Debug.LogWarning("无组件! 注意添加对应组件");
                #endif
                }
            }
        }

        public override void UpdateReference()
        {
            base.UpdateReference();
            reference.Root = this.transform;
        }

        public void AddUnityComponent()
        {
            Component = this.gameObject.AddComponent<T>();
        }

        public void AddUnityComponent<U>() where U : T
        {
            Component = this.gameObject.AddComponent<U>();
        }

        public void SetUnityComponet(T component)
        {
            Component = component;
        }
        
    }
}



