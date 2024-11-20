using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{


    public class AStarProcessor : SimpleGameEntityProcessor<AStarElementComponent>
    {
        private HashSet<AStarElementComponent> dirtyBarrierComponents = new();
        public HashSet<AStarElementComponent> DirtyBarrierComponents { get => dirtyBarrierComponents; }
        
        

        private const int barrierPoint = 1;

        public AStarProcessor() : base()
        {
            Order = ProcessorOrder.AStarUpdate;
        }

     

        
        public override void Update(GameTime time)
        {
            
        }
        

        protected override void OnEntityComponentAdding(Entity entity, AStarElementComponent component, GameData<AStarElementComponent> data)
        {
            dirtyBarrierComponents.Add(component);
            
        }

        protected override void OnEntityComponentRemoved(Entity entity, AStarElementComponent component, GameData<AStarElementComponent> data)
        {
            
            bool isWrited = true;
            if(dirtyBarrierComponents.Count > 0)
            {
                //UnityEngine.Debug.LogError($"AStarBarrierComponent 不要在同一帧内添加删除。。。");
                isWrited = !dirtyBarrierComponents.Remove(component);
            }
            if(isWrited) 
            {
                aStarSystem.ClearElement(entity.Transform.Position, (int)component.ElementFlag);
            }
            bool isClear = aStarSystem.IsClearMapGridsOnComponetRemoving();
            if (isClear)
            {
                foreach (var kvp in ComponentDatas)
                {
                    if (kvp.Key != component)
                    {
                        dirtyBarrierComponents.Add(kvp.Key);
                    }
                }
            }
        }
    }
}
