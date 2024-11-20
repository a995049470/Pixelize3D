using Lost.Runtime.Footstone.Game;
using UnityEngine;
using UnityEngine.UI;
namespace Lost.Runtime.Footstone.Game
{
    public class UIView_LevelLoadTest : UIView
    {
        [SerializeField]
        public Button testButton;
        [SerializeField]
        public Button saveButton;
        [SerializeField]
        public Button loadButton;
        [SerializeField]
        public string mapUrl;

        protected override UIModel CreateModel()
        {
            return new UIWindow_LevelLoadTest(this);
        }
    }

}
