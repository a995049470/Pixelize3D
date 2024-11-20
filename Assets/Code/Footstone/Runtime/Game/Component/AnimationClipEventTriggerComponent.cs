using Lost.Runtime.Footstone.Core;
using System.Collections.Generic;

namespace Lost.Runtime.Footstone.Game
{

    //标记动画片段的事件逻辑组件
    [DefaultEntityComponentProcessor(typeof(AutoRecycleAnimationClipEventTriggerProcessor))]
    [DefaultEntityComponentProcessor(typeof(AnimationClipEventTriggerStartProcessor))]
    [DefaultEntityComponentProcessor(typeof(AnimationClipEventTriggerEndProcessor))]
    public class AnimationClipEventTriggerComponent : EntityComponent
    {
        [UnityEngine.HideInInspector]
        public bool IsTrigger = false;
        public IEnumerable<EntityComponent> CacheComponents { get; set; }

        protected override void OnEnableRuntime()
        {
            base.OnEnableRuntime();
            IsTrigger = false;
        }
    }
    
}
