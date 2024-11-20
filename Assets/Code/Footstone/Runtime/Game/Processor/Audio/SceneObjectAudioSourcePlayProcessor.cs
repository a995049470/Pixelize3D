using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    public class SceneObjectAudioSourcePlayProcessor : SimpleGameEntityProcessor<SceneObjectAudioComponent, AudioSourceControllerComponent, AutoRecycleComponent>
    {
        private AudioPlayerAgentProcessor audioPlayerAgentProcessor;

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            audioPlayerAgentProcessor = GetProcessor<AudioPlayerAgentProcessor>();
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var objAudioComp = kvp.Value.Component1;
                var controlComp = kvp.Value.Component2;
                var recycleComp = kvp.Value.Component3;
                var agentId = objAudioComp.AgentComponentUID;
                bool isVaildAgent = audioPlayerAgentProcessor.AgentCompDic.TryGetValue(agentId, out var agentComp);
                bool isRecycle = true;
                if(isVaildAgent)
                {
                    var remainTime = controlComp.OneShotAudioRemainTime;
                    if(agentComp.IsDirty)
                    {
                        agentComp.IsDirty = false;
                        var audioSource = controlComp.AudioSourceReference.Component;
                        if(controlComp.CurrentLoopAudioKey != agentComp.LoopAudioKey)
                        {
                            audioSource.Stop();
                            controlComp.CurrentLoopAudioKey = agentComp.LoopAudioKey;
                            if(!string.IsNullOrEmpty(agentComp.LoopAudioKey))
                            {
                                var clip = resPoolManager.LoadResWithKey<AudioClip>(agentComp.LoopAudioKey, ResFlag.Audio);
                                audioSource.clip = clip;
                                audioSource.Play();
                            }
                        }
                        var audioKeys = agentComp.OneShot3DAudioKeys;
                        if(audioKeys.Count > 0)
                        {
                            foreach (var key in audioKeys)
                            {
                            
                                var clip = resPoolManager.LoadAudioClipWithKey(key, out var volume);
                                remainTime = Mathf.Max(remainTime, clip.length);
                                audioSource.PlayOneShot(clip, volume);
                            }
                            audioKeys.Clear();
                        }
                    }
                    isRecycle = remainTime <= 0 && string.IsNullOrEmpty(controlComp.CurrentLoopAudioKey);
                    remainTime -= time.DeltaTime;
                    controlComp.OneShotAudioRemainTime = remainTime;
                    var pos = agentComp.Entity.Transform.Position;
                    controlComp.Entity.Transform.Position = pos;
                }

                if(isRecycle)
                {
                    recycleComp.RecycleEntity(cmd, recycleComp.Entity);
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
