using System;
using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class CommandBuffer 
    {
        private Queue<ICommand> cmdQueue; 
        private JsonDataManager jsonDataManager;
        private ContentManager contentManager;
        private ResPoolManager resPoolManager;
        private UniqueIdManager uniqueIdManager;
        private SceneSystem sceneSystem;
        private GameSceneManager gameSceneManager;


        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        public CommandBuffer(IServiceRegistry service)
        {
            cmdQueue = new Queue<ICommand>(16);
            jsonDataManager = service.GetService<JsonDataManager>();
            contentManager = service.GetService<ContentManager>();
            resPoolManager = service.GetService<ResPoolManager>();
            uniqueIdManager = service.GetService<UniqueIdManager>();
            sceneSystem = service.GetService<SceneSystem>();
            gameSceneManager = service.GetService<GameSceneManager>();
        }

        public void Add(ICommand command)
        {
            cmdQueue.Enqueue(command);
        }

        public void CreateGameScene(string newScene, bool isDestoryCurrentScene)
        {
            cmdQueue.Enqueue(new CreateGameSceneCommand
            {
                NewScene = newScene,
                IsDestoryCurrentScene = isDestoryCurrentScene,
                GameSceneManager = gameSceneManager
            });
        }

        public void RemoveEntityComponent(EntityComponent component)
        {
            cmdQueue.Enqueue(new RemoveEntityComponentCommand()
            {
                Target = component.Entity,
                Component = component
            });
        }

        public void RemoveEntityComponent<T>(Entity entity) where T : EntityComponent
        {
            cmdQueue.Enqueue(new RemoveEntityComponentCommand<T>()
            {
                Target = entity
            });
        }

        public void AddEntityComponent<T>(Entity entity, System.Action<T> callback = null) where T : EntityComponent
        {
            cmdQueue.Enqueue(new AddEntityComponentCommand<T>()
            {
                Target = entity,
                CallBack = callback
            });
        }


        public void AddEntityComponent<T>(ulong uid, System.Action<T> callback = null) where T : EntityComponent
        {
            cmdQueue.Enqueue(new AddEntityComponentCommand<T>()
            {
                UID = uid,
                CallBack = callback
            });
        }

        public void GetEntity(ulong uid, System.Action<Entity> callback)
        {
            cmdQueue.Enqueue(new GetEntityCommand()
            {
                UID = uid,
                SceneSystem = sceneSystem,
                CallBack = callback
            });
        }

        public void AddEntityComponent(Entity entity, EntityComponent component) 
        {
            cmdQueue.Enqueue(new AddEntityComponentCommand()
            {
                Target = entity,
                Component = component
            });
        }


        public void MoveEntityComponents(IEnumerable<EntityComponent> entityComponents, Entity dst) 
        {
            cmdQueue.Enqueue(new MoveEntityComponentsCommand(){
                Components = entityComponents,
                Dst = dst
            });
        }

        // public void MoveEntityComponents<T>(Entity src, Entity dst) where T : class
        // {
        //     cmdQueue.Enqueue(new MoveEntityComponentsCommand<T>(){
        //         Src = src,
        //         Dst = dst
        //     });
        // }
        
        public void DestoryEntity(Entity entity)
        {
            cmdQueue.Enqueue(new DestoryCommand()
            {
                Target = entity
            });
        }

        public void InstantiateEntity(string key, ResFlag resFlag, Vector3 position, ulong id, System.Action<Entity> callback = null)
        {
            cmdQueue.Enqueue(new InstantiateEntityCommand()
            {
                Key = key,
                ResFlag = resFlag,
                Position = position,
                Id = id,
                Callback = callback,
                SceneSystem = sceneSystem,
                ResPoolManager = resPoolManager,
                UniqueIdManager = uniqueIdManager
            });
        }

        public void InstantiateParticle(string effectName, Vector3 position)
        {
            cmdQueue.Enqueue(new InstantiateParticleCommand()
            {
                EffectName = effectName,
                EntityPoolManager = resPoolManager,
                Position = position
            });
        }

        public void InstantiateFallObject(string fallName, Vector3 position)
        {
            cmdQueue.Enqueue(new InstantiateFallObjectCommand()
            {
                FallName = fallName,
                EntityPoolManager = resPoolManager,
                Position = position
            });
        }

        public void RecycleEntity(string effectName, ResFlag flag, Entity entity)
        {
            cmdQueue.Enqueue(new RecycleCommand()
            {
                Key = effectName,
                Flag = flag,
                Target = entity,
                EntityPoolManager = resPoolManager
            });
        }

        public void Execute()
        {
            while (cmdQueue.Count > 0)
            {
                cmdQueue.Dequeue().Execute();
            }
        }

        public void AllExecute()
        {
            while (cmdQueue.Count > 0)
            {
                cmdQueue.Dequeue().Execute();
            }
        }

        public void Clear()
        {
            cmdQueue.Clear();
        }

        
    }
}
