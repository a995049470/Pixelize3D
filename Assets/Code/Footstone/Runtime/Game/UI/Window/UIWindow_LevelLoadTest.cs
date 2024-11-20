using System.Collections;
using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;
using Lost.Runtime.Footstone.Game;
using UnityEngine;
namespace Lost.Runtime.Footstone.Game
{
    public class UIWindow_LevelLoadTest : UIWindow
    {
        protected UIView_LevelLoadTest view;
        public override int Orderer => GameConstant.LevelLoadTest;

        public UIWindow_LevelLoadTest(UIView view) : base(view)
        {
        }


        public override void BindView()
        {
            view = OriginView as UIView_LevelLoadTest;
            view.saveButton.gameObject.SetActive(false);
            view.testButton.onClick.AddListener(()=>
            {   
                uiManager.CloseWindow(this);
                var levelManager = StoneHeart.Instance.Services.GetService<LevelManager>();   
                levelManager.LoadLevel(view.mapUrl, Vector2Int.zero);
                view.testButton.gameObject.SetActive(false);
                view.loadButton.gameObject.SetActive(false);
                view.saveButton.gameObject.SetActive(true);
                Debug.Log("onclick");
                uiManager.OpenWindow("UIWindow_Main");
            });

            // view.loadButton.onClick.AddListener(()=>
            // {
            //     var storageSystem = StoneHeart.Instance.Services.GetService<StorageSystem>();   
            //     storageSystem.ReadLoad();
            //     view.testButton.gameObject.SetActive(false);
            //     view.loadButton.gameObject.SetActive(false);
            //     view.saveButton.gameObject.SetActive(true);
            // });

            // view.saveButton.onClick.AddListener(()=>
            // {
            //     var storageSystem = StoneHeart.Instance.Services.GetService<StorageSystem>();   
            //     storageSystem.SaveWrite();
            // });
        }

        public override void UnbindView()
        {
            
        }
    }

}
