using System.Collections.Generic;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public delegate bool IsVisiable(Vector3 pos);
    public interface IElementProcessor
    {
        public void FillMapElementDataDic(MapData mapData, IsVisiable func);
    }
}
