using LitJson;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class UIWindow_InteractiveTip : UIWindow
    {
        private UIView_InteractiveTip view;
        private bool isDitryView;
        private bool isDitrySelect;
        private int currentSelectIndex = 0;
        private int optionCount = 0;
        private InteractiveTipDataProcessor interactiveTipProcessor;
        private InputSettingProcessor inputSettingProcessor;
        private InputManager inputManager;

        public override int Orderer => GameConstant.InteractiveTip;
        

        public UIWindow_InteractiveTip(UIView view) : base(view)
        {   
            
        }

        protected override void InitializeService(IServiceRegistry service)
        {
            base.InitializeService(service);
            inputManager = service.GetService<InputManager>();
        }

        protected override void InitializeProcessors()
        {
            base.InitializeProcessors();
            interactiveTipProcessor = GetProcessor<InteractiveTipDataProcessor>();
            inputSettingProcessor = GetProcessor<InputSettingProcessor>();
        }

        public override void BindView()
        {
            view = OriginView as UIView_InteractiveTip;
            isDitryView = true;
            isDitrySelect = true;
            currentSelectIndex = 0;
            optionCount = 0;
        }

        public override void UnbindView()
        {
            view = null;
        }

        public override void UpdateView(GameTime time)
        {
            base.UpdateView(time);
            if(isDitryView)
            {
                isDitryView = false;
                isDitrySelect = true;
                var tipKey = interactiveTipProcessor.SingleComponent.Key_Tip;
                var tipData = resPoolManager.LoadResConfigData(ResFlag.Text_InteractiveTip)[tipKey];
                view.Txt_Tip.text = (string)tipData[JsonKeys.content];
                var options = tipData[JsonKeys.option];
                optionCount = options.Count;
                for (int i = 0; i < view.TxtArray_Option.Length; i++)
                {
                    bool isActive = i < optionCount;
                    var txt_option = view.TxtArray_Option[i];
                    txt_option.gameObject.SetActive(isActive);
                    if(isActive)
                        txt_option.text = (string)options[i][JsonKeys.desc];
                }
            }
            else if(isDitrySelect)
            {
                isDitrySelect = false;
                if(optionCount > 0)
                {
                    view.Rect_Select.gameObject.SetActive(true);
                    var rect_option = view.TxtArray_Option[currentSelectIndex].rectTransform;
                    var pos = view.Rect_Select.position;
                    pos.y = rect_option.position.y;
                    view.Rect_Select.position = pos;
                }
                else
                {
                    view.Rect_Select.gameObject.SetActive(false);
                }
            }
            else
            {
                var key_up = inputSettingProcessor.SingleComponent.UpRun;
                var key_down = inputSettingProcessor.SingleComponent.DownRun;
                var key_action = inputSettingProcessor.SingleComponent.Interaction;
                if(inputManager.IsKeyDown(key_action))
                {
                    interactiveTipProcessor.SingleComponent.OptionIndex = currentSelectIndex;
                    uiManager.DelayCloseWindow(this);
                }
                else if(optionCount > 0)
                {
                    if(inputManager.IsKeyDown(key_up))
                    {
                        currentSelectIndex = (currentSelectIndex + 1) % optionCount;
                        isDitrySelect = true;
                    }
                    else if(inputManager.IsKeyDown(key_down))
                    {
                        currentSelectIndex = (currentSelectIndex + optionCount - 1) % optionCount;
                        isDitrySelect = true;
                    }
                }
            }
            
            
        }


        
    }

}
