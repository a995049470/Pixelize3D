using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    [DefaultEntityComponentProcessor(typeof(GOAPStrollProcessor))]
    public class GOAPStrollComponent : EntityComponent
    {
        public Vector3 CurrentDir;
    }

}
