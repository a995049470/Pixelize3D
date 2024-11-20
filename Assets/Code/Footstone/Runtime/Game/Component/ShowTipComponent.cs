using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    //单纯的提示或者堵塞交互过
    [DefaultEntityComponentProcessor(typeof(ShowInteractiveTipProcessor))]
    public class ShowTipComponent : EntityComponent
    {
        public string TipKey = "";
        [UnityEngine.HideInInspector]
        public bool IsSendRequest = false;
        public bool IsWaitPlayerRespond = false;
        [UnityEngine.HideInInspector]
        public int OptionIndex = -1;

        protected override void OnEnableRuntime()
        {
            base.OnEnableRuntime();
            IsSendRequest = false;
            OptionIndex = -1;
        }

        public bool IsVaildOption()
        {
            return !string.IsNullOrEmpty(TipKey) && OptionIndex >= 0;
        }
    }

}
