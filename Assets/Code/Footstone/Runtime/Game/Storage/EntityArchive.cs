using Lost.Runtime.Footstone.Core;
using UnityEngine;
using System.Collections.Generic;
using Lost.Runtime.Footstone.Reflection;

namespace Lost.Runtime.Footstone.Game
{

    [System.Serializable]
    public class EntityArchive
    {
        public string Key;
        public ResFlag Flag;
        public bool IsOverwrite;
        public ulong Id;
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 LocalScale;
        public EntityComponentArchive[] ComponentArchives = new EntityComponentArchive[0];
        public bool IsVaild { get => Id != 0; }

        public static EntityArchive Create(Entity entity)
        {
            var archive = new EntityArchive(); 
            var entityComponents = new List<EntityComponent>();

            if(entity.Active)
            {
                entityComponents.AddRange(entity.Components);
                var transformComp = entity.Transform;
                archive.Position = transformComp.Position;
                archive.Rotation = transformComp.Rotation;
                archive.LocalScale = transformComp.LocalScale;
                
                var tagComp = entity.Get<TagComponent>();
                archive.Key = tagComp.Key;
                archive.Flag = tagComp.Flag;
                archive.IsOverwrite = tagComp.IsOverwrite;
            }
            else
            {
                var components = entity.GetComponentsInChildren<EntityComponent>(true);
                foreach (var component in components)
                {
                    if(component.CacheEntity == entity)
                    {
                        if (component is TransformComponent transformComp)
                        {
                            archive.Position = transformComp.Position;
                            archive.Rotation = transformComp.Rotation;
                            archive.LocalScale = transformComp.LocalScale;
                        }
                        else if (component is TagComponent tagComp)
                        {
                            archive.Key = tagComp.Key;
                            archive.Flag = tagComp.Flag;
                            archive.IsOverwrite = tagComp.IsOverwrite;
                        }
                        entityComponents.Add(component);
                    }
                }
            }
            if(archive.IsOverwrite)
            {
                var componentCount = entityComponents.Count;
                archive.ComponentArchives = new EntityComponentArchive[componentCount];
                archive.Id = entity.Id;
                for (int i = 0; i < componentCount; i++)
                {
                    var component = entityComponents[i];
                    archive.ComponentArchives[i] = EntityComponentArchive.CreateArchive(component);
                }
            }
            return archive;
        }
        /// <summary>
        /// UI实体不需要被加载 只做预览用的实体不需要被加载
        /// </summary>
        /// <returns></returns>
        public bool IsLoadable()
        {
            return IsVaild && Flag != ResFlag.UIView && Flag != ResFlag.Entity_Perview_Weapon && Flag != ResFlag.Entity_Perview_Plant;
        }

        public Entity Load(ResPoolManager resPoolManager, SceneSystem sceneSystem)
        {
            var entity = resPoolManager.InstantiateEntity(Key, Flag);
            sceneSystem.SceneInstance.ChangeEntityId(entity, Id);
            entity.Transform.Position = Position;
            entity.Transform.Rotation = Rotation;
            entity.Transform.LocalScale = LocalScale;
            if(IsOverwrite)
            {
                var componentDic = new Dictionary<string, EntityComponent>(entity.Components.Count);
                foreach (var component in entity.Components)
                {
                    var typeName = component.GetType().FullName;
                    componentDic[typeName] = component;
                }
                for (int i = 0; i < ComponentArchives.Length; i++)
                {
                    var archive = ComponentArchives[i];
                    if(componentDic.TryGetValue(archive.Type, out var component))
                    {
                        archive.Load(component);
                        componentDic.Remove(archive.Type);
                    }
                    else
                    {
                        var type = AssemblyRegistry.FindTypeByName(archive.Type);
                        component = entity.AddNoCheck(type);
                        archive.Load(component);
                    }
                }

                foreach (var component in componentDic.Values)
                {
                    entity.Remove(component);
                }
            }
            return entity;
        }
    }
}
