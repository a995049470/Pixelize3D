using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    //纯数据？
    public class PlayerHitPointProcessor : SimpleGameEntityProcessor<HitPointComponent, PlayerComponent>
    {
        public HitPointComponent HitPointComp;

        protected override void OnEntityComponentAdding(Entity entity, HitPointComponent component, GameData<HitPointComponent, PlayerComponent> data)
        {
            if(HitPointComp == null)
                HitPointComp = component;
        }

        protected override void OnEntityComponentRemoved(Entity entity, HitPointComponent component, GameData<HitPointComponent, PlayerComponent> data)
        {
            if(HitPointComp == component)
            {
                if(ComponentDatas.Count > 1)
                {
                    foreach (var key in ComponentDatas.Keys)
                    {
                        if(HitPointComp != key)
                        {
                            HitPointComp = key;
                            break;
                        }
                    }
                }
                else
                    HitPointComp = null;

            }
        }
    }
}
