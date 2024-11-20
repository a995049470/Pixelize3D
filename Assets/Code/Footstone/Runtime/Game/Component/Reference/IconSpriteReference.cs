using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    [System.Serializable]
    public class IconSpriteReference : AssetReference<Sprite>
    {
        public override ResFlag Flag => ResFlag.Sprite_Icon;
    }

}
