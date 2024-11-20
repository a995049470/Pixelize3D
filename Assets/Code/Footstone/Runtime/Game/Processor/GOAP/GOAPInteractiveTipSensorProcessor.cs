using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class GOAPInteractiveTipSensorProcessor : SimpleGameEntityProcessor<GOAPInteractiveTipSensorComponent, GOAPAgentComponent, ShowTipComponent, InteractiveLabelComponent>
    {
        public GOAPInteractiveTipSensorProcessor() : base()
        {
            Order = ProcessorOrder.TipSensor;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var tipComp = kvp.Value.Component3;
                var sensorComp = kvp.Value.Component1;
                if(tipComp.IsVaildOption() && 
                (tipComp.OptionIndex != sensorComp.CacheOptionIndex || 
                tipComp.TipKey != sensorComp.CacheTipKey))
                {
                    var worldStatuses = kvp.Value.Component2.WorldStatus;
                    sensorComp.CacheOptionIndex = tipComp.OptionIndex;
                    sensorComp.CacheTipKey = tipComp.TipKey;
                    var tipData = resPoolManager.LoadResConfigData(ResFlag.Text_InteractiveTip)[tipComp.TipKey];
                    var option = tipData[JsonKeys.option][tipComp.OptionIndex];
                    if(option.ContainsKey(JsonKeys.id_stage))
                    {
                        var id = (int)option[JsonKeys.id_stage];
                        worldStatuses.Set(GOAPStatusFlag.StageIndex, id);
                    }

                    worldStatuses.Set(GOAPStatusFlag.TipOptionIndex, tipComp.OptionIndex);
                    
                }

                
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }

        protected override void OnEntityComponentAdding(Entity entity, GOAPInteractiveTipSensorComponent component, GameData<GOAPInteractiveTipSensorComponent, GOAPAgentComponent, ShowTipComponent, InteractiveLabelComponent> data)
        {
            base.OnEntityComponentAdding(entity, component, data);
            var worldStatus = data.Component2.WorldStatus;
            worldStatus.ForceSetNewStatus(GOAPStatusFlag.StageIndex, 0);
            worldStatus.ForceSetNewStatus(GOAPStatusFlag.TipOptionIndex, -1);
        }
    }
}
