using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    //物体的消逝
    public class DisappearComponent : EntityComponent
    {
        public float DisappearDuration = 1.0f;
        [UnityEngine.HideInInspector]
        public float DisappearTime = 0;

        protected override void OnEnableRuntime()
        {
            base.OnEnableRuntime();
            DisappearTime = 0;
        }

        public bool TryDisappear(float deltaTime)
        {
            DisappearTime += deltaTime;
            bool isDisappear = DisappearTime >= DisappearDuration && DisappearDuration >= 0;
            return isDisappear;
        }
    }

}
