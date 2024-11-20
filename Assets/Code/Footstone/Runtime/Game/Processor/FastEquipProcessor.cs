using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class FastEquipProcessor : SimpleGameEntityProcessor<FastEquipComponent>
    {
        public FastEquipProcessor() : base()
        {
            Order = ProcessorOrder.CreatePowerEntity;
        }

        
        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var fastEquipComp = kvp.Value.Component1;
                var fastSelectIndex = bagData.FastSelectIndex; 
                var itemData = bagData.GetBagGridItemData(fastSelectIndex);
                var itemUid = !itemData.IsVaild()? uniqueIdManager.InvalidId : itemData.UID;
                bool isLostPower = itemUid != fastEquipComp.ItemUid && uniqueIdManager.IsVaild(fastEquipComp.ItemUid);
                bool isReceivePower = uniqueIdManager.IsVaild(itemUid) && itemUid != fastEquipComp.ItemUid;
                if(isLostPower)
                {
                    sceneSystem.SceneInstance.TryGetEntity(fastEquipComp.PowerUID, out var powerEntity);
                    //var equipableComp = powerEntity.GetOrCreate<EquipLabelComponent>();
                    cmd.AddEntityComponent<EquipLabelComponent>(powerEntity, comp =>
                    {
                        comp.SetTimePoint(false, fastEquipComp.Entity);
                    });
                    
                    fastEquipComp.ItemUid = uniqueIdManager.InvalidId;
                    fastEquipComp.PowerUID = uniqueIdManager.InvalidId;
                }
                if(isReceivePower)
                {
                    var powerKey = itemData.GetPowerEntityKey();
                    if(!string.IsNullOrEmpty(powerKey))
                    {
                        var uid = uniqueIdManager.CreateUniqueId();
                        fastEquipComp.PowerUID = uid;
                        fastEquipComp.ItemUid = itemUid;
                        cmd.InstantiateEntity(powerKey, ResFlag.Entity_Power, UnityEngine.Vector3.zero, uid);
                        cmd.GetEntity(uid, entity => 
                        {
                            var equipLabelComp = entity.GetOrCreate<EquipLabelComponent>();
                            equipLabelComp.SetTimePoint(true, fastEquipComp.Entity);
                        });
                    }
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }

    }
}
