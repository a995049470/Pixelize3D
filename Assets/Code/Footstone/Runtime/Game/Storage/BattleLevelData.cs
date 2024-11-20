using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    
    
    //战斗关卡数据
    [System.Serializable]
    public class BattleLevelData
    {
        public List<StaticEntityData> EntityDatas;
        private ResPoolManager resPoolManager;

        public BattleLevelData(IServiceRegistry _service)
        {
            resPoolManager = _service.GetService<ResPoolManager>();
            EntityDatas = new();
        }

        public void Save(IEnumerable<TagComponent> tagComponents)
        {
            EntityDatas.Clear();
            if(tagComponents != null)
            {
                foreach (var tag in tagComponents)
                {
                    EntityDatas.Add(new StaticEntityData(tag));
                }
            }
        }

        public void OnLoad(IServiceRegistry service)
        {
            resPoolManager = service.GetService<ResPoolManager>();
            foreach (var entityData in EntityDatas)
            {
                entityData.OnLoad(resPoolManager);
            }
        }
    }
}
