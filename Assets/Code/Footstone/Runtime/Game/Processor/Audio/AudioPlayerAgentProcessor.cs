using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class AudioPlayerAgentProcessor : SimpleGameEntityProcessor<AudioPlayerAgentComponent>
    {
        private HashSet<AudioPlayerAgentComponent> newAudioPlayerAgentComponents = new();
        public Dictionary<ulong, AudioPlayerAgentComponent> AgentCompDic = new();

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            if(newAudioPlayerAgentComponents.Count > 0)
            {
                foreach (var component in newAudioPlayerAgentComponents)
                {
                    AgentCompDic[component.Id] = component;
                }
                newAudioPlayerAgentComponents.Clear();
            }
            foreach (var kvp in ComponentDatas)
            {
                var agentComp = kvp.Value.Component1;
                if(agentComp.IsDirty)
                {
                    bool isVaildEntity = sceneSystem.SceneInstance.IsVaildEntityUID(agentComp.AudioSourceEntityUID);
                    if(!isVaildEntity)
                    {
                        var pos = agentComp.Entity.Transform.Position;
                        agentComp.AudioSourceEntityUID = uniqueIdManager.CreateUniqueId();
                        var reference = agentComp.AudioSourceEntityReference;
                        cmd.InstantiateEntity(reference.Key, reference.Flag, pos, agentComp.AudioSourceEntityUID, entity =>
                        {
                            var objAudioComp = entity.Get<SceneObjectAudioComponent>();
                            if(objAudioComp) objAudioComp.AgentComponentUID = agentComp.Id;
                        });
                    }
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }

        protected override void OnEntityComponentAdding(Entity entity, AudioPlayerAgentComponent component, GameData<AudioPlayerAgentComponent> data)
        {
            base.OnEntityComponentAdding(entity, component, data);
            //componentId在读档后会变化
            newAudioPlayerAgentComponents.Add(component);
        }

        protected override void OnEntityComponentRemoved(Entity entity, AudioPlayerAgentComponent component, GameData<AudioPlayerAgentComponent> data)
        {
            base.OnEntityComponentRemoved(entity, component, data);
            if(!AgentCompDic.Remove(component.Id))
            {
                newAudioPlayerAgentComponents.Remove(component);
            }
        }
    }
}
