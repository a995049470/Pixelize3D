using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    //施法状态
    public enum CastSpellState 
    {
        None = 0, 
        Casting = 1,
        Success = 2,
        Fail = 3
    }

    //撞击组件
    [DefaultEntityComponentProcessor(typeof(AnimationImpactStartProcessor))] [DefaultEntityComponentProcessor(typeof(AnimationImpactEndProcessor))]
    public class AnimationImpactEventComponent : EntityComponent, IAnimationClipEvent
    {
        public int SuccessAttackIndex = 1;
        public int FailAttackIndex = 2;
        public int RepelDis = 1;
        public int RepleSpeed = 10;
        [UnityEngine.HideInInspector]
        public CastSpellState ImpactState = CastSpellState.None;
        
    }
}
