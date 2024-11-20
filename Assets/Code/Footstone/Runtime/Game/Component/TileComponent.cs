using Lost.Runtime.Footstone.Core;
using UnityEngine;
namespace Lost.Runtime.Footstone.Game
{
    [DefaultEntityComponentProcessor(typeof(TileRenderProcessor))]
    public class TileComponent : EntityComponent
    {
        [HideInInspector]
        public TileModel[] TargetTileModels = new TileModel[4];
        [HideInInspector][System.NonSerialized]
        public bool IsDitryTexture = true;

        public void SetTargetTextureKeys(TileModel[] tileModels)
        {
            for (int i = 0; i < 4; i++)
            {
                TargetTileModels[i] = tileModels[i].Clone();
            }
            IsDitryTexture = true;
        }

        public void SetTargetTextureKey(int i, TileModel tileModel)
        {
            TargetTileModels[i] = tileModel.Clone();
            IsDitryTexture = true;
        }
    }

}
