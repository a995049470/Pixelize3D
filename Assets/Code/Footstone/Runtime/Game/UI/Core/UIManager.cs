using System;
using System.Collections.Generic;
using Lost.Runtime.Footstone.Collection;
using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    /// <summary>
    /// 依赖于ResPoolManager, sceneSystem
    /// </summary>
    //UIManager这里不需要有Entity
    public class UIManager : IUpdateable
    {
        
        protected FastCollection<UIWindow> uiWindows = new FastCollection<UIWindow>();
        

        public bool Enabled => true;
        public int UpdateOrder => 100;

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        private List<string> delayOpenWindowKeys = new();
        private List<string> delayCloseWindowKeys = new();

        private SceneSystem sceneSystem;
        private ResPoolManager resPoolManager;
        //window不支持重复
        private Dictionary<string, UIWindow> openWindowDic;
        private UIRootProcessor uiRootProcessor;
        private UIWindow topWindow;
        private Transform root
        {
            get
            {
                return uiRootProcessor.Root;
            }
        }
        

        public UIManager()
        {
            
        }

        public void Initialize(IServiceRegistry service)
        {
            sceneSystem = service.GetService<SceneSystem>();
            resPoolManager = service.GetService<ResPoolManager>();
            uiRootProcessor = sceneSystem.SceneInstance.ForceGetProcessor<UIRootProcessor>();
            openWindowDic = new();
        }

        public void Update(GameTime gameTime)
        {
            if(delayOpenWindowKeys.Count > 0)
            {
                foreach (var key in delayOpenWindowKeys)
                {
                    OpenWindow(key);
                }
                delayOpenWindowKeys.Clear();
            }
            foreach (var kvp in openWindowDic)
            {
                kvp.Value.UpdateView(gameTime);
            }
            if(delayCloseWindowKeys.Count > 0)
            {
                foreach (var key in delayCloseWindowKeys)
                {
                    CloseWindow(key);
                }
                delayCloseWindowKeys.Clear();
            }
        }

        public void DelayOpenWindow(string key)
        {
            delayOpenWindowKeys.Add(key);
        }

        public void DelayCloseWindow(string key)
        {
            delayCloseWindowKeys.Add(key);
        }

        public void DelayCloseWindow(UIWindow window)
        {
            delayCloseWindowKeys.Add(window.ResKey);
        }

        

        //TODO:返回（int, UIModel）如果UIModel的id不同代表失效？？是否有必要？
        public UIWindow OpenWindow(string key)
        {
            if(!openWindowDic.TryGetValue(key, out var window))
            {
                var originWindow = GetTopWindow();
                var view = resPoolManager.InstantiateUIView(key);
                window = view.GetOrCreateModel() as UIWindow;
                window.ResKey = key;
                var windowIndex = 0;
                foreach (var kvp in openWindowDic)
                {
                    if(window.Orderer < kvp.Value.Orderer)
                    {
                        break;
                    }
                    windowIndex ++;
                }
                openWindowDic[key] = window;
                window.BindView();
                window.SetParent(root, windowIndex);
                window.AfterBindView();
                view.transform.position = root.position;
                if(view.transform is RectTransform rectTrans)
                {
                    rectTrans.sizeDelta = Vector2.zero;
                }
                var currentWindow = GetTopWindow();
                if(currentWindow == window) 
                {
                    currentWindow.OnTop();
                    originWindow?.OnCovered();
                }
                 
            }
            
            return window;
        }

        /// <summary>
        /// 清理无效的window
        /// 编辑器下删除UI会出现无效Window的情况
        /// </summary>
        public void ClearInvaildWindow()
        {
            var keys = new List<string>(openWindowDic.Keys);
            foreach (var key in keys)
            {
                var window = openWindowDic[key];
                if(window == null || window.OriginView == null)
                {
                    openWindowDic.Remove(key);
                }
            }
        }
        
        /// <summary>
        /// 获取启用的窗口
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public UIWindow GetWindow(string key)
        {
            openWindowDic.TryGetValue(key, out var window);
            return window;
        }

        public string[] GetOpenWindowKeys()
        {
            var count = openWindowDic.Count;
            var keys = new string[count];
            openWindowDic.Keys.CopyTo(keys, 0);
            return keys;
        }

        //TODO:可能存在窗口关闭后仍然存在引用的情况
        //TODO:增加使用UniqueId是否被关闭过？
        public bool CloseWindow(string key)
        {
            bool isSuccess = false;
            if(openWindowDic.TryGetValue(key, out var window))
            {
                var originTopWindow = GetTopWindow();
                isSuccess = true;
                window.UnbindView();
                window.SetParent(null);
                resPoolManager.RecycleUIView(key, window.OriginView);
                openWindowDic.Remove(key);
                var currentTopWindow = GetTopWindow();
                if(currentTopWindow != originTopWindow)
                {
                    currentTopWindow?.OnTop();
                }
            }
            return isSuccess;
        }
        
        private UIWindow GetTopWindow()
        {
            var max = int.MinValue;
            topWindow = null;
            foreach (var kvp in openWindowDic)
            {
                if(kvp.Value.Orderer > max)
                {
                    max = kvp.Value.Orderer;
                    topWindow = kvp.Value;
                }
            }
            return topWindow;
        }

        public bool IsTopWindowAllowGameInput()
        {
            bool isAllow = topWindow?.AllowGameInput ?? true;
            return isAllow;
        }

        public bool CloseWindow(UIWindow window)
        {
            return CloseWindow(window.ResKey);
        }

        public void CloseAllWindow()
        {
            
            foreach (var kvp in openWindowDic)
            {
                kvp.Value.Destroy();
            }
            openWindowDic.Clear();
        }
      
        
        public UIModel CreateUIModel(string key)
        {
            var view = resPoolManager.InstantiateUIView(key);
            var model = view.GetOrCreateModel();
            model.ResKey = key;
            model.BindView();
            model.AfterBindView();
            return model;
        }

        public void SetUIModelPoolCapacity(string key, int capacity)
        {
            resPoolManager.SetPoolCapacity(key, ResFlag.UIView, capacity);
        }



        public void ReleaseUIModel(UIModel model)
        {
            model.UnbindView();
            model.SetParent(null);
            //TODO:可能存在重复回收的情况
            resPoolManager.RecycleUIView(model.ResKey, model.OriginView);
        }
    }
}
