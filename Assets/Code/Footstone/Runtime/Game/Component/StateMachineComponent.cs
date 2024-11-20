using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    [DisallowMultipleComponent]
    [DefaultEntityComponentProcessor(typeof(PoseStateProcessor))]
    public class StateMachineComponent : EntityComponent
    {
        [HideInInspector]
        public UnitState CurrentState;
        private StateFlag nextState; 
        private int nextStateLevel = -1;
        private int nextSubIndex = 0;
        private float nextStatePlaySpeed = 1;
        private float nextProtectedDuration = 0;
        private bool nextLoop;
        private float nextStateDuration = -1;
        private string nextEventControllerKey = null;

        public void UpdateCurrentState()
        {
            //level >= 0才会判定有效
            if(nextStateLevel >= 0)
            {
                if(CurrentState.Switchable(nextState, nextSubIndex))
                {
                    CurrentState.Init(nextState, nextSubIndex, nextProtectedDuration, 0, nextStatePlaySpeed, nextLoop, nextStateDuration, nextEventControllerKey);
                }
                else
                {
                    CurrentState.TryChangeSpeed(nextState, nextSubIndex, nextStatePlaySpeed);
                }
            }
            nextStateLevel = -1;
        }

        protected override void OnEnableRuntime()
        {
            CurrentState.SetDirty();
        }

        //TODO:动作状态机设置
        public void TrySwitchState(StateFlag flag, int subIndex, int level, int protectDuration = 0, float speed = 1, bool _loop = true, float stateDuration = -1, string eventControllerKey = null)
        {
            if(level > nextStateLevel)
            {
                nextStateLevel = level;
                nextState = flag;
                nextProtectedDuration = protectDuration;
                nextLoop = _loop;
                nextStatePlaySpeed = speed;
                nextSubIndex = subIndex;
                nextStateDuration = stateDuration;
                nextEventControllerKey = eventControllerKey;
            }
        }
        
        // public void Run(float time)
        // {
        //     CurrentState.Run(time);
        // }
#if UNITY_EDITOR
        private void OnValidate() {
            if(Application.isPlaying)
                CurrentState.SetDirty();
                
        }
#endif
    }

}
