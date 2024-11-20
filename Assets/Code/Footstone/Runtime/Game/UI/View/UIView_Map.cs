using UnityEngine;
using UnityEngine.UI;

namespace Lost.Runtime.Footstone.Game
{

    public class UIView_Map : UIView
    {
        public Button Btn_Quit;
        public string MapElementKey = "";
        public int GridSize = 40;
        public RectTransform ElementRootNode;
        public RectTransform[] LayerNodes;

        protected override UIModel CreateModel()
        {
            return new UIWindow_Map(this);
        }
    }

}
