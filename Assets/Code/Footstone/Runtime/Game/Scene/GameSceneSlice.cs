using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class GameSceneSlice
    {
        public string Name;
        public Vector3 PlayerPosition;
        public List<Entity> SceneEntities = new();
        //不是所有场景都需要PlayerEntity
        public Entity PlayerEntity;
        public List<Entity> SystemEntities = new();
        public string MissingPlayerKey = "";
        public HashSet<string> MissingSystemKeys = new();
        public bool Active { get; private set; } = true;
        private ResPoolManager resPoolManager;

        public GameSceneSlice(ResPoolManager _resPoolManager)
        {
            resPoolManager = _resPoolManager;
        }
        
        public void Destory()
        {
            foreach (var entity in SceneEntities)
            {
                entity?.DestoryUnityGameObject();
            }
            foreach (var entity in SystemEntities)
            {
                entity?.DestoryUnityGameObject();
            }
            PlayerEntity?.DestoryUnityGameObject();
        }

        public void ForceSetActive(bool active)
        {
            if (!active)
            {
                foreach (var entity in SceneEntities)
                {
                    entity.OnSceneCloseWithComponents();
                }
                foreach (var entity in SystemEntities)
                {
                    entity.OnSceneCloseWithComponents();
                }
                PlayerEntity?.OnSceneCloseWithComponents();
            }
            foreach (var entity in SceneEntities)
            {
                entity.SetUnityGameObjectActive(active);
            }
            foreach (var entity in SystemEntities)
            {
                entity.SetUnityGameObjectActive(active);
            }
            PlayerEntity?.SetUnityGameObjectActive(active);
            if (active)
            {
                if (!string.IsNullOrEmpty(MissingPlayerKey))
                {
                    PlayerEntity = resPoolManager.InstantiateEntity(MissingPlayerKey, ResFlag.Entity_Player);
                    MissingPlayerKey = "";
                }
                foreach (var key in MissingSystemKeys)
                {
                    resPoolManager.InstantiateEntity(key, ResFlag.Entity_System);
                }
                MissingSystemKeys.Clear();
                if(PlayerEntity != null)
                {
                    PlayerEntity.GetOrCreate<TransportComponent>().TargetPosition = PlayerPosition;
                }
            }
            Active = active;
        }

        /// <summary>
        /// 设置场景切片的激活状态
        /// </summary>
        /// <param name="active">设置为true会自动补全缺少的实体</param>
        public void SetActive(bool active)
        {
            if (Active != active) 
            {
                ForceSetActive(active);
            }
        }

        /// <summary>
        /// 将部分实体转移到下一个实体
        /// </summary>
        public void EntityShiftToNextScene(ref string playerKey, HashSet<string> systemKeys, out Entity player)
        {
            //转移PlayerEntity
            player = null;
            if(PlayerEntity != null && PlayerEntity.Get<TagComponent>().Key == playerKey)
            {
                player = PlayerEntity;
                PlayerEntity = null;
                MissingPlayerKey = playerKey;
                playerKey = "";
            }
            //转移Entity
            var systemEntityCount = SystemEntities.Count;
            for (int i = systemEntityCount - 1; i >= 0 ; i--)
            {
                var entity = SystemEntities[i];
                var key = entity.Get<TagComponent>().Key;
                if(systemKeys.Contains(key))
                {
                    MissingSystemKeys.Add(key);
                    SystemEntities.RemoveAt(i);
                    systemKeys.Remove(key);
                }
            }
        }

    }
}