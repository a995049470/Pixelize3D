using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class WaterModelRenderProcesssor : SimpleGameEntityProcessor<WaterComponent, WeaponModelComponent>
    {
        public WaterModelRenderProcesssor() : base()
        {
            //选择一个容易被覆盖的位置
            Order = ProcessorOrder.FrameStart;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var waterComp = kvp.Value.Component1;
                var weaponModelComp = kvp.Value.Component2;
                var isShowModel = waterComp.IsShowModel();
                weaponModelComp.TargetRightHandWeaponKey = isShowModel ? waterComp.ModelKey : "";
            }
        }

        protected override void OnEntityComponentRemoved(Entity entity, WaterComponent component, GameData<WaterComponent, WeaponModelComponent> data)
        {
            data.Component2.TargetRightHandWeaponKey = "";
        }
    }
}
