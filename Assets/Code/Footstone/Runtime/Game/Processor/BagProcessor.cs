using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class BagProcessor : SimpleGameEntityProcessor<BagComponent>
    {
        public BagComponent BagComp;

        protected override void OnEntityComponentAdding(Entity entity, BagComponent component, GameData<BagComponent> data)
        {
            if(BagComp == null) BagComp = component;
        }

        protected override void OnEntityComponentRemoved(Entity entity, BagComponent component, GameData<BagComponent> data)
        {
            if(BagComp == component)
            {
                foreach (var key in ComponentDatas.Keys)
                {
                    if(key != BagComp)
                    {
                        BagComp = key;
                        break;
                    }
                }
                if(BagComp == component) BagComp = null;
            }
            
        }
    }
}
