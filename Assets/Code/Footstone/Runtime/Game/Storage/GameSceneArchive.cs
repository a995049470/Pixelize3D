using UnityEngine;
using System;
using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    [System.Serializable]
    public class GameSceneArchive
    {
        public String Name;
        public bool Active;
        public Vector3 PlayerPosition;
        public EntityArchive PlayerEntityArchive = new();
        public EntityArchive[] SceneEntityArchives = new EntityArchive[0];
        public string MissingPlayerKey;
        public string[] MissingSystemKeys = new string[0];

        public static GameSceneArchive Create(GameSceneSlice sceneSlice)
        {
            GameSceneArchive archive = new();
            archive.Name = sceneSlice.Name;
            archive.Active = sceneSlice.Active;
            archive.PlayerPosition = sceneSlice.PlayerPosition;
            if(sceneSlice.PlayerEntity != null)
                archive.PlayerEntityArchive = EntityArchive.Create(sceneSlice.PlayerEntity);
            var sceneEntityCount = sceneSlice.SceneEntities.Count;
            archive.SceneEntityArchives = new EntityArchive[sceneEntityCount];
            for (int i = 0; i < sceneEntityCount; i++)
            {
                archive.SceneEntityArchives[i] = EntityArchive.Create(sceneSlice.SceneEntities[i]);
            }
            archive.MissingPlayerKey = sceneSlice.MissingPlayerKey;
            var systemKeyCount = sceneSlice.MissingSystemKeys.Count;
            archive.MissingSystemKeys = new string[systemKeyCount];
            sceneSlice.MissingSystemKeys.CopyTo(archive.MissingSystemKeys, 0, systemKeyCount);
            return archive;
        }

        

        public GameSceneSlice Load(ResPoolManager resPoolManager, SceneSystem sceneSystem)
        {
            var sceneSlice = new GameSceneSlice(resPoolManager);
            sceneSlice.Name = this.Name;
            sceneSlice.PlayerPosition = this.PlayerPosition;
            if(this.PlayerEntityArchive.IsVaild)
            {
                sceneSlice.PlayerEntity = this.PlayerEntityArchive.Load(resPoolManager, sceneSystem);
            }
            var sceneEntityCount = this.SceneEntityArchives.Length;
            sceneSlice.SceneEntities.Capacity = sceneEntityCount;
            for (int i = 0; i < sceneEntityCount; i++)
            {
                var entityArchive = this.SceneEntityArchives[i];
                if(entityArchive.IsLoadable())
                {
                    sceneSlice.SceneEntities.Add(entityArchive.Load(resPoolManager, sceneSystem));
                }
            }
            sceneSlice.MissingPlayerKey = this.MissingPlayerKey;
            sceneSlice.MissingSystemKeys = new HashSet<string>(this.MissingSystemKeys);
            sceneSlice.SetActive(this.Active);
            return sceneSlice;
        }
    }
}
