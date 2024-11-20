using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    //触发装置
    [DefaultEntityComponentProcessor(typeof(DeviceTriggerStartProcessor))]
    [DefaultEntityComponentProcessor(typeof(DeviceTriggerEndProcessor))]
    [DefaultEntityComponentProcessor(typeof(TriggerDeviceProcessor))]
    public class TriggerDeviceComponent : EntityComponent
    {
        public bool IsRepeatTrigger = true;
        //component uid
        [HideInInspector]
        public ulong TirggerManUID = 0;
        public bool IsUnknow { get => TriggerFlag == TriggerFlag.WaitCheck; }
        public bool IsTriggered { get => TriggerFlag == TriggerFlag.Triggered; }
        public bool IsWaitRecycle { get => TriggerFlag == TriggerFlag.WaitRecycle; }
        public bool IsTriggering { get => TriggerFlag == TriggerFlag.Triggering; }
        public bool IsWaitTrigger { get => TriggerFlag == TriggerFlag.WaitTrigger; }
        public TriggerFlag TriggerFlag = TriggerFlag.WaitCheck;
    
    }

}
