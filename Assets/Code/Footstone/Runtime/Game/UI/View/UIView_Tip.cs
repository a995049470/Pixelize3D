using TMPro;
using UnityEngine.UI;
namespace Lost.Runtime.Footstone.Game
{
    public class UIView_Tip : UIView
    {
        public TextMeshProUGUI Text_Name;
        protected override UIModel CreateModel()
        {
            return new UIModel_Tip(this);
        }
    }

}
