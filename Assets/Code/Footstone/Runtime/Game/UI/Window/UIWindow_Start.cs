using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class UIWindow_Start : UIWindow
    {
        private UIView_Start view;
        private BGMProcessor bgmProcessor;

        public override int Orderer => GameConstant.Start;

        public UIWindow_Start(UIView view) : base(view)
        {

        }

        protected override void InitializeProcessors()
        {
            base.InitializeProcessors();
            bgmProcessor = GetProcessor<BGMProcessor>();
        }

        public override void BindView()
        {
            view = OriginView as UIView_Start;
            view.Btn_Start.onClick.AddListener(OnClick_Start);
            view.Btn_Continue.onClick.AddListener(OnClick_Continue);
            view.Btn_Quit.onClick.AddListener(OnClick_Quit);
        }

        public override void UnbindView()
        {
            view.Btn_Start.onClick.RemoveAllListeners();
            view.Btn_Continue.onClick.RemoveAllListeners();
            view.Btn_Quit.onClick.RemoveAllListeners();
            view = null;
        }
        
        private void OnClick_Start()
        {
            gameSceneManager.CreateNewScene(view.Scene, true);
        }

        private void OnClick_Continue()
        {
            storageSystem.Load();
        }

        private void OnClick_Quit()
        {

        }
    }

}
