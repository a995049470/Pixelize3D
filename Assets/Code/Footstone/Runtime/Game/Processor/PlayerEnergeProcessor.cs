using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    //纯数据？
    public class PlayerEnergeProcessor : SimpleGameEntityProcessor<EnergyComponent, PlayerComponent>
    {
        public EnergyComponent EnergyComp;

        protected override void OnEntityComponentAdding(Entity entity, EnergyComponent component, GameData<EnergyComponent, PlayerComponent> data)
        {
            if(EnergyComp == null)
                EnergyComp = component;
        }

        protected override void OnEntityComponentRemoved(Entity entity, EnergyComponent component, GameData<EnergyComponent, PlayerComponent> data)
        {
            if(EnergyComp == component)
            {
                if(ComponentDatas.Count > 2)
                {
                    foreach (var key in ComponentDatas.Keys)
                    {
                        if(EnergyComp != key)
                        {
                            EnergyComp = key;
                            break;
                        }
                    }
                }
                else
                    EnergyComp = null;
            }
        }

    }
}
