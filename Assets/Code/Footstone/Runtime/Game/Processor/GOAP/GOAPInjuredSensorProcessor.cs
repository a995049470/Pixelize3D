using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class GOAPInjuredSensorProcessor : SimpleGameEntityProcessor<GOAPInjuredSensorComponent, GOAPAgentComponent, HurtComponent>
    {
        public GOAPInjuredSensorProcessor() : base()
        {
            Order = ProcessorOrder.InjuredSensor;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var worldStatuses = kvp.Value.Component2.WorldStatus;
                var hurtComp = kvp.Value.Component3;
                if(hurtComp.IsReceiveDamage)
                {
                    worldStatuses.Set(GOAPStatusFlag.Injured, true);
                    worldStatuses.Set(GOAPStatusFlag.LastHurtTime, 0.0f, GameConstant.FloatScale);
                }
                //增加距离上次受伤的时间
                else if(worldStatuses.TryGetValue(GOAPStatusFlag.LastHurtTime, out var status))
                {
                    var t = status.GetFloat(GameConstant.FloatScale) + time.DeltaTime;
                    status.Set(t, GameConstant.FloatScale);
                }

            }
        }

        protected override void OnEntityComponentAdding(Entity entity, GOAPInjuredSensorComponent component, GameData<GOAPInjuredSensorComponent, GOAPAgentComponent, HurtComponent> data)
        {
            {
                var isNew = data.Component2.WorldStatus.ForceGetStatus(GOAPStatusFlag.Injured, out var status);
                if(isNew) status.Set(false);
            }

            {
                var isNew = data.Component2.WorldStatus.ForceGetStatus(GOAPStatusFlag.LastHurtTime, out var status);
                if(isNew) status.Set(99.0f, GameConstant.FloatScale);
            }

        }
    }
}
