using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    [DefaultEntityComponentProcessor(typeof(JumpProcessor))]
    [DefaultEntityComponentProcessor(typeof(JumpStateProcessor))]
    public class JumperComponent : EntityComponent
    {
        public AnimationCurve JumpCurve = new();
    }

}
