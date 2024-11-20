using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    

    //交互者组件
    [DefaultEntityComponentProcessor(typeof(InteractionProcessor))]
    [DefaultEntityComponentProcessor(typeof(PlayerInteractionControllerProcessor))]
    public class InteractorComponent : EntityComponent
    {
        //获取到交互指令
        [UnityEngine.HideInInspector]
        public bool IsInteracting = false;

        [System.NonSerialized]
        public Vector2Int LastAutoCheckPosition = Vector2Int.one * int.MaxValue;

        public ChildComponentReference<Transform> TipPointReference = new();
        
        public override void UpdateReference()
        {
            base.UpdateReference();
            TipPointReference.Root = this.transform;
        }
    }
}
