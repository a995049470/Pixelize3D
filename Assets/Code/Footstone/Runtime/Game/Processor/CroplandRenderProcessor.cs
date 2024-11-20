using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    
    public class CroplandRenderProcessor : SimpleGameEntityProcessor<CroplandComponent, RendererComponent>
    {
        private string[] materialKeys;

        public CroplandRenderProcessor() : base()
        {
            Order = ProcessorOrder.Render;
            materialKeys = new string[]
            {
                "cropland",
                "cropland_dig",
                "cropland_wet",
                "cropland_dig_wet",

                "cropland_rain",
                "cropland_dig_rain",
                "cropland_rain",
                "cropland_dig_rain",
            };
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var croplandComp = kvp.Value.Component1;
                var rendererComp = kvp.Value.Component2;
                if(croplandComp.IsRenderDirty)
                {
                    croplandComp.IsRenderDirty = false;
                    var materialIndex = croplandComp.GetMaterialIndex();
                    if(materialIndex < 0)
                        materialIndex = materialKeys.Length - 1;
                    var material = resPoolManager.LoadResWithKey<Material>(materialKeys[materialIndex], ResFlag.Material);
                    rendererComp.SetSharedMaterial(0, croplandComp.LandMaterialIndex, material);
                }
            }
        }
    }
}
 