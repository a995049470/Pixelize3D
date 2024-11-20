using Lost.Runtime.Footstone.Core;
using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    public abstract class GameEntityProcessor<TComponent, TData> : EntityProcessor<TComponent, TData> where TData : class where TComponent : EntityComponent
    {
        protected PhysicsSystem physicsSystem;
        protected InputManager input;
        protected SceneSystem sceneSystem;
        protected ContentManager content;
        protected JsonDataManager jsonDataSystem;
        protected CommandBufferManager commandBufferManager;
        protected ResPoolManager resPoolManager;
        protected UniqueIdManager uniqueIdManager;
        protected StorageSystem storageSystem;
        protected GameSceneManager gameSceneManager;
        protected AStarSystem aStarSystem;
        protected BagProcessor bagProcessor;
        protected UIManager uiManager;
        protected BagData bagData{ get => bagProcessor.BagComp.Data; }
        protected Logger log;
        protected RaycastHit[] raycastHits = new RaycastHit[16];
        protected Collider[] castColliders = new Collider[16];
        protected static Vector3 defaultScale = Vector3.one;
        protected static Quaternion defaultRotation = Quaternion.identity;
        protected const float tiny = 0.01f;
        //TODO:把随机结果一开始算好还是当场随机？
        private static System.Random _random;
        protected static System.Random random
        {
            get
            {
                if(_random == null)
                {
                    _random = new System.Random(System.DateTime.Now.Millisecond);
                }
                return _random;
            }
        }

        protected GameEntityProcessor( params Type[] requiredAdditionalTypes)
            : base(requiredAdditionalTypes)
        {

        }

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            bagProcessor = GetProcessor<BagProcessor>();
        }

        protected internal override void OnSystemAdd()
        {
            physicsSystem = physicsSystem ?? Services.GetService<PhysicsSystem>();
            input = input ?? Services.GetService<InputManager>();       
            sceneSystem = sceneSystem ?? Services.GetService<SceneSystem>();
            content = content ?? Services.GetService<ContentManager>();
            jsonDataSystem = jsonDataSystem ?? Services.GetService<JsonDataManager>();
            commandBufferManager = commandBufferManager ?? Services.GetService<CommandBufferManager>();
            resPoolManager = resPoolManager ?? Services.GetService<ResPoolManager>();
            uniqueIdManager = uniqueIdManager ?? Services.GetService<UniqueIdManager>();
            storageSystem = storageSystem ?? Services.GetService<StorageSystem>();
            gameSceneManager = gameSceneManager ?? Services.GetService<GameSceneManager>();
            aStarSystem = aStarSystem ?? Services.GetService<AStarSystem>();
            uiManager = uiManager ?? Services.GetService<UIManager>();
            
        }

        

        protected T GetProcessor<T>(bool isFroce = true) where T : EntityProcessor
        {
            T processor;
            if(isFroce) processor = sceneSystem.SceneInstance.ForceGetProcessor<T>();
            else processor = sceneSystem.SceneInstance.GetProcessor<T>();
            return processor;
        }

        
      
    }
}
