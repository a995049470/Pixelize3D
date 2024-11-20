using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    //面向固定方向
    [DefaultEntityComponentProcessor(typeof(GOAPFaceFixedDirectionProcessor))]
    public class GOAPFaceFixedDirectionComponent : EntityComponent
    {
        public Vector3 FixedForward = new Vector3(0, 0, 1);
    }

}



