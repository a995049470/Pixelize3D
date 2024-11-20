using System.Diagnostics;
using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    
    public class PoseStateProcessor : SimpleGameEntityProcessor<PoseComponent, StateMachineComponent>
    {
        private AnimationSettingProcessor animationSettingProcessor;
        public PoseStateProcessor() : base()
        {
            Order = ProcessorOrder.PoseUpdate;
        }
        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            animationSettingProcessor = GetProcessor<AnimationSettingProcessor>();
        }
        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            base.Update(time);
            var animationSetting = animationSettingProcessor.SingleComponent;
            foreach (var kvp in ComponentDatas)
            {
                var stateMachine = kvp.Value.Component2;
                var poseComp = kvp.Value.Component1;
                stateMachine.UpdateCurrentState();
                var currentState = stateMachine.CurrentState;
                var speed = currentState.Speed;
                var stateName = currentState.StateName;
                var loop = currentState.Loop;
                currentState.Run(time.DeltaTime * speed);
                var nextNormalizedTime = poseComp.CalculateAnimationCurrentNormalizedTime(stateName, currentState.CurrentTime, loop, currentState.Duration, animationSetting.FPS, out var realDuration);
                var eventNextNormalizeTime = currentState.CurrentTime / realDuration;
                eventNextNormalizeTime = eventNextNormalizeTime - Mathf.FloorToInt(eventNextNormalizeTime);
                var entity = poseComp.Entity;
                currentState.AnimationClipEventController?.TriggerEvent(cmd, entity, ref currentState.CurrentEventNormalizedTime, eventNextNormalizeTime, ref currentState.EventPtr);
                
                poseComp.Play(currentState.NameHash, nextNormalizedTime, realDuration, animationSetting.TransitionDuration);
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
