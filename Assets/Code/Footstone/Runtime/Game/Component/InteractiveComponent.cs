using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    /// <summary>
    /// 可交互物体
    /// </summary>
    [DefaultEntityComponentProcessor(typeof(InteractionDropProcessor))]
    [DefaultEntityComponentProcessor(typeof(InteractiveTriggerStartProcessor))]
    [DefaultEntityComponentProcessor(typeof(InteractiveTriggerEndProcessor))]
    [DefaultEntityComponentProcessor(typeof(AutoRecycleInteractiveProcessor))]
    [DefaultEntityComponentProcessor(typeof(InteractiveComponentCopyProcessor))]
    public class InteractiveComponent : EntityComponent
    {
        public bool IsWaitTrigger { get => TriggerFlag == TriggerFlag.WaitTrigger; }
        public bool IsTriggerEffect { get => TriggerFlag == TriggerFlag.Triggering || TriggerFlag == TriggerFlag.TriggeringNotRecyle; }
        public bool IsTriggering { get => TriggerFlag == TriggerFlag.Triggering; }
        public bool IsTriggered { get => TriggerFlag == TriggerFlag.Triggered; }
        public bool IsWaitRecycle { get => TriggerFlag == TriggerFlag.WaitRecycle; }
        public bool IsWaitPlayerRespond { get => TriggerFlag == TriggerFlag.WaitPlayerRespond; }
        public TriggerFlag OriginTriggerFlag = TriggerFlag.WaitTrigger;
        //[UnityEngine.HideInInspector]
        public TriggerFlag TriggerFlag = TriggerFlag.WaitTrigger;
        [UnityEngine.HideInInspector]
        public TriggerFlag NextTriggerFlag = TriggerFlag.Invalid;
        public bool IsRepeatInteractive = false;

        protected override void OnEnableRuntime()
        {
            base.OnEnableRuntime();
            TriggerFlag = OriginTriggerFlag;
        }

    }

}
