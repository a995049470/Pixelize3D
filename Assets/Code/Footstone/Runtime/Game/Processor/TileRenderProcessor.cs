using System.Collections.Generic;
using Lost.Render.Runtime;
using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
 
    public class TileRenderProcessor : SimpleGameEntityProcessor<TileComponent, RendererComponent>
    {
        private MaterialPropertyBlock materialPropertyBlock;
        private Dictionary<string, int> texIdDic = new();
        public TileRenderProcessor() : base()
        {
            materialPropertyBlock = new MaterialPropertyBlock();
            Order = ProcessorOrder.Render;
        }

        public int GetTextureId(string key)
        {
            if(!texIdDic.TryGetValue(key, out var id))
            {
                id = texIdDic.Count + 1;
                texIdDic[key] = id;
            }
            return id;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var tileComp = kvp.Value.Component1;
                if(tileComp.IsDitryTexture)
                {
                    tileComp.IsDitryTexture = false;
                    var rendererComp = kvp.Value.Component2;
                    for (int i = 0; i < 4; i++)
                    {
                        var tileModel = tileComp.TargetTileModels[i];
                        if (tileModel != null)
                        {
                            var texture = resPoolManager.LoadResWithKey<Texture2D>(tileModel.Key, ResFlag.Texture_Tile);
                            if (texture != null)
                            {
                                var param = tileModel.GetRotateFlipParameter();
                                rendererComp.GetPropertyBlock(i, materialPropertyBlock);
                                materialPropertyBlock.SetTexture(ShaderConstant._MainTex, texture);
                                materialPropertyBlock.SetVector(ShaderConstant._MainTexRotate, param);
                                rendererComp.SetPropertyBlock(i, materialPropertyBlock);
                                var sortingOrder = (i * 4096) + GetTextureId(tileModel.Key);
                                rendererComp.SetSortingOrder(i, sortingOrder);
                            }
                        }
                    }

                }
            }
        }
    }
}
