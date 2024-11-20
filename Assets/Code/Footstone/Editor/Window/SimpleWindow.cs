using System;
using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;
using Lost.Runtime.Footstone.Game;
using UnityEditor;

namespace Lost.Editor.Footstone
{
    public abstract class SimpleWindow : ScriptableWizard
    {
        private const string suffix = "_Lost.Editor.AssetBundle.SimpleWindow";
        private static Dictionary<Type, SimpleWindow> windowDic = new();
        
        private string GetKey()
        {
            return this.GetType().Name + suffix;
        }

        protected static SimpleWindow GetCurrentWindow<T>() where T : SimpleWindow
        {
            windowDic.TryGetValue(typeof(T), out var window);
            return window;
        }
        
        protected virtual void OnEnable()
        {
            windowDic[this.GetType()] = this;
            Load();
        }

        protected virtual void OnDisable()
        {
            windowDic.Remove(this.GetType());
            Save();
        }

        protected virtual void Save()
        {
            var key = GetKey();
            var json = EditorJsonUtility.ToJson(this);
            EditorPrefs.SetString(key, json);
        }

        private void Load()
        {
            var key = GetKey();
            var json = EditorPrefs.GetString(key, null);
            if(json != null)
            {
                EditorJsonUtility.FromJsonOverwrite(json, this);
            }
        }
        
        protected T GetServiceRuntime<T>() where T : class
        {
            return StoneHeart.Instance?.Services.GetService<T>();
        }

        protected T GetProcessorRuntime<T>() where T : EntityProcessor
        {
            return GetServiceRuntime<SceneSystem>().SceneInstance.ForceGetProcessor<T>();
        }
        

    }

}

