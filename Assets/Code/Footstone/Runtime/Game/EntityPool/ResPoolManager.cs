using System.Collections.Generic;
using LitJson;
using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    public class ResPoolManager
    {   
        private List<Dictionary<string, EntityPool>> entityPoolDicList;
        private int defaultCapacity = 16;
        private JsonDataManager jsonDataManager;
        private ContentManager contentManager;
        private IServiceRegistry service;
        //TODO:考虑使用数组替代字典
        private Dictionary<ResFlag, string> flagConfigPathDic;
        private Dictionary<ResFlag, JsonData> configDic;


        public ResPoolManager(IServiceRegistry _service)
        {
            service = _service;
            entityPoolDicList = new();
            jsonDataManager = _service.GetService<JsonDataManager>();
            contentManager = _service.GetService<ContentManager>();

            configDic = new(64);
            flagConfigPathDic = new(64);
            flagConfigPathDic[ResFlag.Entity_Barrier] = GameConfig.barrierJsonUrl;
            flagConfigPathDic[ResFlag.Entity_Env] = GameConfig.envJsonPath;
            flagConfigPathDic[ResFlag.Entity_Drop] = GameConfig.dropJsonPath;
            flagConfigPathDic[ResFlag.Entity_Interactive] = GameConfig.interactiveJsonUrl;
            flagConfigPathDic[ResFlag.Entity_Monster] = GameConfig.monsterJsonUrl;
            flagConfigPathDic[ResFlag.Entity_Particle] = GameConfig.particleJsonPath;
            flagConfigPathDic[ResFlag.Entity_Player] = GameConfig.playerJsonPath;
            flagConfigPathDic[ResFlag.Entity_Wall] = GameConfig.wallJsonPath;
            flagConfigPathDic[ResFlag.UIView] = GameConfig.uiJsonPath;
            flagConfigPathDic[ResFlag.Entity_Camera] = GameConfig.cameraJsonPath;
            flagConfigPathDic[ResFlag.Entity_Light] = GameConfig.lightJsonPath;
            flagConfigPathDic[ResFlag.Text_Level] = GameConfig.levelJsonPath;
            flagConfigPathDic[ResFlag.Material] = GameConfig.materialJsonPath;
            flagConfigPathDic[ResFlag.Entity_Perview_Plant] = GameConfig.plantStateJsonPath;
            flagConfigPathDic[ResFlag.Config_Item] = GameConfig.itemJsonPath;
            flagConfigPathDic[ResFlag.Entity_Root] = GameConfig.rootJsonPath;
            flagConfigPathDic[ResFlag.Sprite_Icon] = GameConfig.iconJsonPath;
            flagConfigPathDic[ResFlag.Sprite_UI] = GameConfig.uiSpriteJsonPath;
            flagConfigPathDic[ResFlag.Entity_Power] = GameConfig.powerJsonPath;
            flagConfigPathDic[ResFlag.Entity_Plant] = GameConfig.plantJsonPath;
            flagConfigPathDic[ResFlag.Texture_Tile] = GameConfig.tileTextureJsonPath;
            flagConfigPathDic[ResFlag.Text_Tile] = GameConfig.tileJsonPath; 
            flagConfigPathDic[ResFlag.Entity_System] = GameConfig.systemEntityJsonPath; 
            flagConfigPathDic[ResFlag.Entity_Perview_Weapon] = GameConfig.weaponModelJsonPath;
            flagConfigPathDic[ResFlag.Texture_Map] = GameConfig.mapTextureJsonPath;
            flagConfigPathDic[ResFlag.Config_Scene] = GameConfig.sceneConfigJsonPath;
            flagConfigPathDic[ResFlag.Entity_Point] = GameConfig.pointEntityJsonPath;
            flagConfigPathDic[ResFlag.Data_Attack] = GameConfig.attackDataJsonPath;
            flagConfigPathDic[ResFlag.Data_AttackEvent] = GameConfig.attackEventDataJsonPath;
            flagConfigPathDic[ResFlag.Data_Goal] = GameConfig.goalDataJsonPath;
            flagConfigPathDic[ResFlag.Data_Graph] = GameConfig.graphDataJsonPath;
            flagConfigPathDic[ResFlag.Data_Entities] = GameConfig.entitiesDataJsonPath;
            flagConfigPathDic[ResFlag.Data_AnimationClipController] = GameConfig.animationClipEventControllerDataJsonPath;
            flagConfigPathDic[ResFlag.Entity_AnimationClipEvent] = GameConfig.animationClipEventEntityJsonPath;
            flagConfigPathDic[ResFlag.Entity_Bullet] = GameConfig.bulletEntityJsonPath;
            flagConfigPathDic[ResFlag.Data_AttackTable] = GameConfig.attackTableDataJsonPath;
            flagConfigPathDic[ResFlag.Entity_Tip] = GameConfig.tipEntityJsonPath;
            flagConfigPathDic[ResFlag.Audio] = GameConfig.audioJsonPath;
            flagConfigPathDic[ResFlag.Entity_AudioSource] = GameConfig.audioSourceEntityJsonPath;
            flagConfigPathDic[ResFlag.Entity_Place] = GameConfig.placeEntityJsonPath;
            flagConfigPathDic[ResFlag.Entity_LevelConfig] = GameConfig.levelConfigEntityJsonPath;
            flagConfigPathDic[ResFlag.Text_InteractiveTip] = GameConfig.interactiveTipTextJsonPath;
            flagConfigPathDic[ResFlag.Text_FixedBehavior] = GameConfig.fixedBehaviorTextJsonPath;
        }

        public JsonData LoadResConfigData(ResFlag flag)
        {
            if (!configDic.TryGetValue(flag, out var data))
            {
                var jsonPath = flagConfigPathDic[flag];
                var json = contentManager.LoadRes<UnityEngine.TextAsset>(jsonPath).text;
                data = JsonMapper.ToObject(json);
                configDic[flag] = data;
            }
            return data;
        }    

        
        

        private Dictionary<string, EntityPool> GetEntityPoolDic(ResFlag flag)
        {
            var flagId = (int)flag;
            for (int i = entityPoolDicList.Count; i < flagId + 1; i++)
            {
                entityPoolDicList.Add(new());
            }
            var dic = entityPoolDicList[flagId];
            return dic;
        }

        private EntityPool GetEntityPool(string key, ResFlag flag)
        {
            var dic = GetEntityPoolDic(flag);
            if(!dic.TryGetValue(key, out var pool))
            {
                pool = new EntityPool(defaultCapacity);
                dic.Add(key, pool);
            }
            return pool;
        }

        private EntityPool GetUIViewEntityPool(string key, ResFlag flag)
        {
            var dic = GetEntityPoolDic(flag);
             if(!dic.TryGetValue(key, out var pool))
            {
                pool = new UIViewEntityPool(defaultCapacity);
                dic.Add(key, pool);
            }
            return pool;
        }


        /// <summary>
        /// 回收Entity (UI 需要调用 RecycleUIView)
        /// </summary>
        public void RecycleEntity(string key, ResFlag flag, Entity entity)
        {
            var pool = GetEntityPool(key, flag);
            pool.Recycle(entity);
        }

        /// <summary>
        /// 回收包含UIView组件的Entity
        /// </summary>
        public void RecycleUIView(string key, UIView view)
        {
            var pool = GetUIViewEntityPool(key, ResFlag.UIView);
            pool.Recycle(view.Entity);
        }

        public AudioClip LoadAudioClipWithKey(string key, out float volume)
        {
            var data = LoadResConfigData(ResFlag.Audio);
            var auidoData = data[key];
            var url = auidoData[JsonKeys.url].ToString();
            volume = auidoData.ContainsKey(JsonKeys.volume) ? (float)auidoData[JsonKeys.volume] : 1.0f;
            return contentManager.LoadRes<AudioClip>(url);
        }

        public T LoadResWithKey<T>(string key, ResFlag flag) where T : Object
        {
            var data = LoadResConfigData(flag);
            var url = data[key][JsonKeys.url].ToString();
            return contentManager.LoadRes<T>(url);
        }

        public T LoadResWithKeyNoCache<T>(string key, ResFlag flag) where T : Object
        {
            var data = LoadResConfigDataNoCahe(flag);
        #if UNITY_EDITOR
            if(!data.ContainsKey(key))
            {
                Debug.LogError($"key:{key}, flag:{flag}  invaild");
                return null;
            }
        #endif
            var url = data[key][JsonKeys.url].ToString();
            return contentManager.LoadRes<T>(url);
        }


        public JsonData LoadResConfigDataNoCahe(ResFlag flag)
        {
            var jsonPath = flagConfigPathDic[flag];
            var asset = contentManager.LoadRes<UnityEngine.TextAsset>(jsonPath);
            if(asset == null)
            {
                Debug.LogError($"jsonPath:{jsonPath}  flag:{flag}  invaild");
                return null;
            }
            var json = asset.text;
            var data = JsonMapper.ToObject(json);
            return data;
        }    

    #if UNITY_EDITOR
        public string GetResKey(Object obj, ResFlag flag)
        {
            var targetKey = "";
            var asset = obj;
            var resPath = UnityEditor.AssetDatabase.GetAssetPath(asset);
            if(string.IsNullOrEmpty(resPath))
            {
                Debug.LogError($"{obj.name} not asset!");
            }
            var data = LoadResConfigDataNoCahe(flag);
            foreach (var key in data.Keys)
            {
                var singleData = data[key];
                var path = ((string)singleData[JsonKeys.url]);
                if(resPath == path)
                {
                    targetKey = key;
                    break;
                }
            }
            if(string.IsNullOrEmpty(targetKey) && !string.IsNullOrEmpty(resPath))
            {
                var jsonPath = flagConfigPathDic[flag];
                Debug.LogError($"{resPath} not in config {jsonPath}!");
            }
            return targetKey;
        }
    #endif


        private Entity InstantiateEntityWithResult(string key, ResFlag flag, bool isRecycle, bool isCreateTag, out bool isNewObj)
        {
            bool isSuccess = GetEntityPool(key, flag).TryGet(out var entity);
            if(!isSuccess)
            {
                var data = LoadResConfigData(flag);
                var path = data[key][JsonKeys.url].ToString();
                entity = contentManager.LoadEntity(path).Instantiate();
                if(isCreateTag)
                {
                    var tagComp = entity.GetOrCreate<TagComponent>();
                    tagComp.Flag = flag;
                    tagComp.Key = key;
                }
                //if(isAutoRecycle)
                {
                    var recycleComp = entity.GetOrCreate<AutoRecycleComponent>();
                    recycleComp.Key = key;
                    recycleComp.Flag = flag;
                    recycleComp.IsRecycle = isRecycle;
                }
            }
            isNewObj = !isSuccess;
            return entity;
        }

        public Entity InstantiateEntity(string name, ResFlag flag)
        {
            bool isRecycle = true;
            bool isCreateTag = true;
            switch (flag)
            {
                //不需要回收的实体
                case ResFlag.Entity_Barrier : 
                case ResFlag.Entity_Monster :
                case ResFlag.Entity_Env : 
                case ResFlag.Entity_Player : 
                case ResFlag.Entity_Wall : 
                    isRecycle = false;
                    break;
                //既不需要保存又不需要预览的实体（比如UI）
                case ResFlag.UIView : 
                case ResFlag.Entity_Root :
                    isRecycle = false;
                break;
                //只提供预览作用的实体
                case ResFlag.Entity_Perview_Plant :
                break;
            }
            var entity = InstantiateEntityWithResult(name, flag, isRecycle, isCreateTag, out var isNewObj);
            
            
            return entity;
        }

        public UIView InstantiateUIView(string key)
        {
            var entity = InstantiateEntity(key, ResFlag.UIView);
            var view = entity.Get<UIView>();
            if(view == null) throw new System.Exception($"{key} Not Found UIView Component.");
            return view;
        } 
        
        
        public JsonData LoadJsonData(string key, ResFlag flag)
        {
            var json = LoadResWithKey<TextAsset>(key, flag).text;
            var data = JsonMapper.ToObject(json);
            return data;
        }    

        public JsonData LoadJsonDataNoCache(string key, ResFlag flag)
        {
            var json = LoadResWithKeyNoCache<TextAsset>(key, flag).text;
            var data = JsonMapper.ToObject(json);
            return data;
        }    

        public JsonData LoadItemData(string key)
        {
            return LoadResConfigData(ResFlag.Config_Item)[key];
        }

     

        /// <summary>
        /// 设置对象池的容量 一般是增加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="flag"></param>
        /// <param name="capacity"></param>
        public void SetPoolCapacity(string key, ResFlag flag, int capacity)
        {
            var pool = GetEntityPool(key, flag);
            pool.SetCapacity(capacity);
        }

        public void ClearPool()
        {
            foreach (var dic in entityPoolDicList)
            {
                foreach (var kvp in dic)
                {
                    kvp.Value.DestoryAll();
                }
            }
        }

        public void ClearConfigDataCache()
        {
            configDic.Clear();
        }

        
    }
}