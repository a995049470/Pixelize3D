using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    //相当于UI组件，依赖于UIWindow存在
    public abstract class UIModel
    {
        protected UIManager uiManager;
        protected Entity root;
        public Entity Root { get => root; }
        private UIView originView;
        public UIView OriginView { get => originView; }
        public string ResKey;

        private BagProcessor bagProcessor;
        protected BagData bagData{ get => bagProcessor.BagComp.Data; }
        protected StorageSystem storageSystem;
        protected ResPoolManager resPoolManager;
        protected SceneSystem sceneSystem;
        protected GameSceneManager gameSceneManager;
        private bool _isFirstBind = true;
        protected bool isFirstBind { get => _isFirstBind; }
        
        public UIModel(UIView view)
        {
            originView = view;
        }

        public void Initialize(IServiceRegistry service)
        {
            storageSystem = service.GetService<StorageSystem>();
            resPoolManager = service.GetService<ResPoolManager>();
            uiManager = service.GetService<UIManager>();
            sceneSystem = service.GetService<SceneSystem>();
            gameSceneManager = service.GetService<GameSceneManager>();
            bagProcessor = GetProcessor<BagProcessor>();
            InitializeService(service);
            InitializeProcessors();
        }

        public void AfterBindView()
        {
            _isFirstBind = false;
        }

        protected virtual void InitializeProcessors()
        {

        }

        

        protected virtual void InitializeService(IServiceRegistry service)
        {

        }

        public T GetProcessor<T>() where T : EntityProcessor
        {
            return sceneSystem.SceneInstance.ForceGetProcessor<T>();
        }

        /// <summary>
        /// UI组件启用的时候会用调用BindView
        /// </summary>
        public abstract void BindView();
        
        
        /// <summary>
        /// UI组件关闭的时候会用调用UnbindView
        /// </summary>
        public abstract void UnbindView();
        /// <summary>
        /// 更新视图
        /// </summary>
        public virtual void UpdateView(GameTime time)
        {

        }

       
        public void SetParent(Transform parent)
        {
            originView.transform.SetParent(parent);
        }

        public void SetParent(Transform parent, int index)
        {
            originView.transform.SetParent(parent);
            originView.transform.SetSiblingIndex(index);
        }

        public void SetActive(bool active)
        {
            originView.gameObject.SetActive(active);
        }
        
        public void Destroy() {
            SetActive(false);
            GameObject.DestroyImmediate(originView.gameObject);
        }

    }
}
