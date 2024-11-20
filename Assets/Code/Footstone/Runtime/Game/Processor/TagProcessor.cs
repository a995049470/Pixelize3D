using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class TagProcessor : SimpleGameEntityProcessor<TagComponent>
    {
        public void GetEntities(out Entity playerEntity, out List<Entity> systemEntities, out List<Entity> sceneEntities)
        {
            var totalCount = ComponentDatas.Count;
            sceneEntities = new List<Entity>(totalCount);
            systemEntities = new List<Entity>(8);
            var playerEntities = new List<Entity>();

            foreach (var kvp in ComponentDatas)
            {
                if(kvp.Key.IsPlayerEntity())
                    playerEntities.Add(kvp.Key.Entity);
                else if(kvp.Key.IsSystemEntity())
                    systemEntities.Add(kvp.Key.Entity);
                else if(kvp.Key.IsSceneEntity())
                    sceneEntities.Add(kvp.Key.Entity);
            }
            if(playerEntities.Count > 0)
                playerEntity = playerEntities[0];
            else
                playerEntity = null;
        }

        //有些entity挂在节点下 加载场景时候可能出错，需要删除
        public void DeletePerviewEntities()
        {
            var delTagList = new List<TagComponent>();
            foreach (var kvp in ComponentDatas)
            {
                if(kvp.Key.IsChildPerviewEntity())
                    delTagList.Add(kvp.Key);
            }
            foreach (var tag in delTagList)
            {
                resPoolManager.RecycleEntity(tag.Key, tag.Flag, tag.Entity);
            }
        }


        public List<Entity> GetAllEntities()
        {
            var count = ComponentDatas.Count;
            var entities = new List<Entity>(count);
            foreach (var kvp in ComponentDatas)
            {
                entities.Add(kvp.Key.Entity);
            }
            return entities;
        }
    }
}
