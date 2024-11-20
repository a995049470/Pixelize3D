using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    [DefaultEntityComponentProcessor(typeof(GOAPAttackPlayerProcessor))]
    public class GOAPAttackPlayerComponent : EntityComponent
    {
        public int AttackIndex = 0;
        [HideInInspector]
        public bool IsWaitAttackFinsh = false;
        

        public void Reset()
        {
            IsWaitAttackFinsh = false;
        }
    }

}
