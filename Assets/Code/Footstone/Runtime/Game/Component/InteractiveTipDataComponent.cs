using System.Diagnostics;
using Lost.Runtime.Footstone.Collection;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    //交互提示的数据
    [DefaultEntityComponentProcessor(typeof(InteractiveTipDataProcessor))]
    public class InteractiveTipDataComponent : EntityComponent
    {
        [UnityEngine.HideInInspector]
        public string Key_Tip;
        [UnityEngine.HideInInspector]
        public int OptionIndex = -1;
        public bool HasTipShowing { get => !string.IsNullOrEmpty(Key_Tip); }
        private UIManager uiManager;

        protected override void Initialize(IServiceRegistry registry)
        {
            base.Initialize(registry);
            uiManager = registry.GetService<UIManager>();
        }

        public bool TryShowTip(ulong uid, string key)
        {
            bool isSuccess = !HasTipShowing;
            if(isSuccess)
            {
                Key_Tip = key;
                uiManager.OpenWindow("Window_InteractiveTip");
            }
            else
            {
                UnityEngine.Debug.LogError($"无法处理同时处理多个交互提示!!! oldTip:{Key_Tip} newTip:{key}");
            }
            return isSuccess;
        } 

        public bool TryGetOptionIndex(ulong uid, out int index)
        {
            index = OptionIndex;
            bool isSuccess = index >= 0;
            if(isSuccess)
            {
                OptionIndex = -1;
                Key_Tip = "";
            }
            return isSuccess;
        }

    }

}