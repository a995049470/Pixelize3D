using System.Collections.Generic;
using Unity.VisualScripting;

namespace Lost.Runtime.Footstone.Game
{

    public class MapProcessor : SingleComponentProcessor<MapComponent>
    {
        private List<IElementProcessor> elementProcessors = new();
        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
        }

        public void UpdateMapData()
        {
            if(SingleComponent)
            {
                var mapData = SingleComponent.Data;
                mapData.UpdateAllMapElementData(gameSceneManager.CurrentScene);
            }
        }
    }
}
