using Lost.Runtime.Footstone.Core;
using Lost.Runtime.Footstone.Game;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    
    public abstract class AssetReference<T> : UnityObjectReference where T : Object
    {
        private static ResPoolManager _resPoolManager;
        protected static ResPoolManager resPoolManager
        {
            get 
            {
                if(_resPoolManager == null)
                {
                    IServiceRegistry service = null;
                #if UNITY_EDITOR
                    if(!Application.isPlaying)
                    {
                        service = new ServiceRegistry();
                        service.AddService(new ContentManager());
                        service.AddService(new ResPoolManager(service));
                    }
                    else
                #endif
                    {
                        service = StoneHeart.Instance.Services;
                    }
                    _resPoolManager = service.GetService<ResPoolManager>();
                }
                return _resPoolManager;
            }
        }

        [SerializeField][HideInInspector]
        private string key;
        public string Key { get => key; }
        public abstract ResFlag Flag { get;}
        private T asset;
        public T Asset
        {
            get{
                if(!asset && !string.IsNullOrEmpty(key))
                {
                    asset = LoadAsset();
                }
                return asset;
            }
        }

        public Object ReferenceOwner { get; set; }

        public void SetKey(string _key)
        {
            if(key != _key)
            {
                key = _key;
                asset = null;
            }
        }
        
        public bool IsVaild()
        {
            return !string.IsNullOrEmpty(key);
        }

        private T LoadAsset()
        {
            T v = null;
            if(!string.IsNullOrEmpty(key))
            {
                v = resPoolManager.LoadResWithKey<T>(key, Flag);
            }
            return v;
        }

        private T LoadAssetNoCache()
        {
            T v = null;
            if(!string.IsNullOrEmpty(key))
            {
                v = resPoolManager.LoadResWithKeyNoCache<T>(key, Flag);
            }
            return v;
        }

        public override Object GetDisplayObject()
        {
            asset = LoadAssetNoCache();
            return asset;
        }

        protected override void UpdateReference()
        {
        #if UNITY_EDITOR
            if(asset)
            {
                key = resPoolManager.GetResKey(asset, Flag);
                if(string.IsNullOrEmpty(key))
                    asset = null;
            }
            else
            {
                key = null;
            }
        #endif
        }

        public override bool UpdateUnityObject(Object o)
        {
            bool isUpdate = false;
            if (o != asset)
            {
                isUpdate = true;
                asset = o as T;
                UpdateReference();
            }
            return isUpdate;
        }

        public override System.Type GetObjectType()
        {
            return typeof(T);
        }

        public override Object GetOwner()
        {
            return ReferenceOwner;
        }
    }

}
