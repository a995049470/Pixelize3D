using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    //即将死亡 (例如：凋零的树)
    [DefaultEntityComponentProcessor(typeof(BeDyingDropProcessor))]
    [DefaultEntityComponentProcessor(typeof(BeDyingRecycleProcessor))]
    [DefaultEntityComponentProcessor(typeof(BeWaterOnBeDyingProcessor))]
    public class BeDyingComponent : EntityComponent
    {
        [UnityEngine.HideInInspector]
        public bool IsWillDie = false;

        protected override void OnEnableRuntime()
        {
            base.OnEnableRuntime();
            IsWillDie = false;
        }
    }

}
