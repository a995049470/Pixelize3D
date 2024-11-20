using Lost.Runtime.Footstone.Collection;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    // //献祭之后
    // public delegate void OnAfterSacrifice(string key);

    [DefaultEntityComponentProcessor(typeof(SacrificeProcessor))]
    public class SacrificeComponent : EntityComponent, IComponentSaveOrLoadCallback
    {
        [UnityEngine.HideInInspector]
        private SerializableDictionary<string, OblationData> oblationDataDic = new();
        
        [UnityEngine.HideInInspector]
        public int SelectIndex = 0;
        private ResPoolManager resPoolManager;
        private BagProcessor bagProcessor;

        protected override void Initialize(IServiceRegistry registry)
        {
            base.Initialize(registry);
            resPoolManager = registry.GetService<ResPoolManager>();
            bagProcessor = sceneSystem.SceneInstance.ForceGetProcessor<BagProcessor>();
        }

        public OblationData GetOblationData(string key)
        {
            if(!oblationDataDic.TryGetValue(key, out var data))
            {
                data = new OblationData(key, resPoolManager);
                oblationDataDic[key] = data;
            }
            return data;
        }

        //献祭
        public bool TrySacrifice(string key, int count, int exp)
        {
            var bagData = bagProcessor.BagComp.Data;
            bool isSuccess = bagData.TryLoseItem(key, count);
            if(isSuccess)
            {
                var rewards = GetOblationData(key).GetRewards(count, exp);
                foreach (var single in rewards)
                {
                    bagData.ReceiveItem(single.Item1, single.Item2);
                }
            }
            return isSuccess;
        }



        public void OnAfterLoad()
        {
            oblationDataDic.OnAfterLoad();
            foreach (var kvp in oblationDataDic)
            {
                kvp.Value.OnAfterLoad(resPoolManager);
            }
        }

        public void OnBeforeSave()
        {
            oblationDataDic.OnBeforeSave();
        }
    }

}



