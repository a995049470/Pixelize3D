using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    /// <summary>
    /// 被播种
    /// </summary>
    public class BeSeededProcesor : SimpleGameEntityProcessor<BeSeededComponent, LandPlantComponent>
    {
        public BeSeededProcesor() : base()
        {
            Order = ProcessorOrder.BeSeeded;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var plant = kvp.Value.Component1.Plant;
                var landPlantComp = kvp.Value.Component2;
                if(landPlantComp.CurrentPlantFlag == PlantFlag.None)
                {
                    landPlantComp.CurrentPlantFlag = PlantFlag.Seed;
                    landPlantComp.AdultStateSpeedUpProgress = 0;
                    cmd.InstantiateEntity(plant, ResFlag.Entity_Plant, UnityEngine.Vector3.zero, uniqueIdManager.InvalidId, entity => 
                    {
                        var copyLabelComp = entity.GetOrCreate<CopyLabelComponent>();
                        copyLabelComp.Target = landPlantComp.Entity;  
                    });
                    var beSeededComp = kvp.Value.Component1;
                    cmd.RemoveEntityComponent(beSeededComp);
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);

        }
    }
}
