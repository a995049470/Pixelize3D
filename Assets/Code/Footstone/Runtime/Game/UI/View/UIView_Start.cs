using UnityEngine.UI;

namespace Lost.Runtime.Footstone.Game
{
    public class UIView_Start : UIView
    {
        public Button Btn_Start;
        public Button Btn_Continue;
        public Button Btn_Quit;
        public string Scene = "test";
        protected override UIModel CreateModel()
        {
            return new UIWindow_Start(this);
        }
    }

}
