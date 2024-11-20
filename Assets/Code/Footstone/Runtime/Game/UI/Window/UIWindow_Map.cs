using System.Collections.Generic;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class UIWindow_Map : UIWindow
    {
        public override int Orderer => GameConstant.Map;
        private UIView_Map view;
        private MapProcessor mapProcessor;
        private MapData mapData { get => mapProcessor.SingleComponent.Data; }
        private List<UIModel_MapElement> mapElements = new();
        

        public UIWindow_Map(UIView view) : base(view)
        {
            
        }

        protected override void InitializeProcessors()
        {
            base.InitializeProcessors();
            mapProcessor = GetProcessor<MapProcessor>();
        }
        
        public override void BindView()
        {
            view = OriginView as UIView_Map;
            //扩大池子容量
            uiManager.SetUIModelPoolCapacity(view.MapElementKey, 2048);
            mapProcessor.UpdateMapData();
            var elementDataList = mapData.ElementDataList;
            var center = mapData.Center;
            var gridSize = view.GridSize;
            foreach (var elementData in elementDataList)
            { 
                var model = uiManager.CreateUIModel(view.MapElementKey) as UIModel_MapElement;
                var parent = view.LayerNodes[elementData.Layer]; 
                model.SetParent(parent);
                var x = (elementData.Position.x - center.x) * gridSize;
                var y = (elementData.Position.z - center.z) * gridSize;
                model.OriginView.RectTransform.anchoredPosition = new Vector2(x, y);
                model.UpdateView(elementData);
                mapElements.Add(model);
            }
            view.Btn_Quit.onClick.AddListener(OnClick_Quit);
            
        }

        public override void UnbindView()
        {
            
            foreach (var model in mapElements)
            {
                uiManager.ReleaseUIModel(model);
            }
            mapElements.Clear();
            view.Btn_Quit.onClick.RemoveAllListeners();
        }

        private void OnClick_Quit()
        {
            uiManager.CloseWindow(this);
        }
    }

}
