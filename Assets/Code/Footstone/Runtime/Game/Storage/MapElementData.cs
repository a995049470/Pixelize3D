using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    //地图元素数据(可能是复合元素)
    public class MapElementData
    {
        public Vector3 Position;
        public IconSpriteReference SpriteReference = new();
        public int Layer = 0;
        public float Scale = 1;
    }
}
