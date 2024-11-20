using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class RenderLandPlantProcessor : SimpleGameEntityProcessor<LandPlantComponent>
    {
        public RenderLandPlantProcessor() : base()
        {
            Order = ProcessorOrder.Render;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var plantComp = kvp.Value.Component1;
                //顺便清除一下剩下的生长能量
                plantComp.RemainGrowthPower = 0;
                //flag为none时清空Key
                if(plantComp.CurrentPlantFlag == PlantFlag.None || 
                plantComp.CurrentPlantFlag == PlantFlag.Death) plantComp.NextPlantKey = "";
                bool isVaildEntity = sceneSystem.SceneInstance.TryGetEntity(plantComp.PlantEntityId, out var oldEntity);
                bool isSameKey = plantComp.PlantKey == plantComp.NextPlantKey;
                bool isReceyleOldEntity = !isSameKey && isVaildEntity;
                bool isCreateNewEntity = (!isVaildEntity || !isSameKey) && (!string.IsNullOrEmpty(plantComp.NextPlantKey));
                if (isReceyleOldEntity)
                {
                    cmd.RecycleEntity(plantComp.PlantKey, ResFlag.Entity_Perview_Plant, oldEntity);
                    plantComp.PlantEntityId = uniqueIdManager.InvalidId;
                }
                if (isCreateNewEntity)
                {
                    var position = plantComp.Entity.Transform.Position;
                    var id = uniqueIdManager.CreateUniqueId();
                    plantComp.PlantKey = plantComp.NextPlantKey;
                    plantComp.PlantEntityId = id;
                    cmd.InstantiateEntity(plantComp.PlantKey, ResFlag.Entity_Perview_Plant, position, id);
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }


    }
}
