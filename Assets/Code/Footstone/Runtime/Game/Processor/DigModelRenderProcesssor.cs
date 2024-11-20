using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class DigModelRenderProcesssor : SimpleGameEntityProcessor<DigComponent, WeaponModelComponent>
    {
        public DigModelRenderProcesssor() : base()
        {
            //选择一个容易被覆盖的位置
            Order = ProcessorOrder.FrameStart;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var digComp = kvp.Value.Component1;
                var weaponModelComp = kvp.Value.Component2;
                var isShowModel = digComp.IsShowModel();
                weaponModelComp.TargetLeftHandWeaponKey = isShowModel ? digComp.ModelKey : "";
            }
        }

        protected override void OnEntityComponentRemoved(Entity entity, DigComponent component, GameData<DigComponent, WeaponModelComponent> data)
        {
            data.Component2.TargetLeftHandWeaponKey = "";
        }
    }
}
