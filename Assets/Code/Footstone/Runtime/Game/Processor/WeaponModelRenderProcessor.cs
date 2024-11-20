using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class WeaponModelRenderProcessor : SimpleGameEntityProcessor<WeaponModelComponent, EquipPointComponent>
    {
        public WeaponModelRenderProcessor() : base()
        {
            Order = ProcessorOrder.Render;
        }

        public void RenderWeaponModel(CommandBuffer cmd, Entity entity, ChildComponentReference<Transform> pointReference, string targetKey, ref string currentKey, ref ulong uid)
        {
            var isVaildEntity = sceneSystem.SceneInstance.TryGetEntity(uid, out var oldEntity);
            var isSameKey = currentKey == targetKey;
            var isReceyleOldEntity = isVaildEntity && !isSameKey;
            var isCreateNewEntity = (!isVaildEntity || !isSameKey) && !string.IsNullOrEmpty(targetKey);

            if(isReceyleOldEntity)
            {
                cmd.RecycleEntity(currentKey, ResFlag.Entity_Perview_Weapon, oldEntity);
                uid = uniqueIdManager.InvalidId;
            }
            if(isCreateNewEntity)
            {
                uid = uniqueIdManager.CreateUniqueId();
                currentKey = targetKey;
                cmd.InstantiateEntity(currentKey, ResFlag.Entity_Perview_Weapon, Vector3.zero, uid, weaponEntity =>
                {
                    var parentComp = weaponEntity.GetOrCreate<ParentComponent>();
                    parentComp.TargetReference.Set(entity, pointReference);
                    // entity.Transform.Parent = point;
                    // entity.Transform.LocalPosition = Vector3.zero;
                    // entity.Transform.LocalScale = Vector3.one;
                    // entity.Transform.LocalRotation = Quaternion.identity;
                });
            }
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var modelComp = kvp.Value.Component1;
                var pointComp = kvp.Value.Component2;
                var entity = pointComp.Entity;
                RenderWeaponModel(cmd, entity, pointComp.LeftHandPointReference, modelComp.TargetLeftHandWeaponKey, ref modelComp.CurrentLeftHandWeaponKey, ref modelComp.LeftHandWeaponEntityUID);
                RenderWeaponModel(cmd, entity, pointComp.RightHandPointReference, modelComp.TargetRightHandWeaponKey, ref modelComp.CurrentRightHandWeaponKey, ref modelComp.RightHandWeaponEntityUID);
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }

        private void RecycleWeaponModel(string currentKey, ref ulong uid)
        {
            var isVaildEntity = sceneSystem.SceneInstance.TryGetEntity(uid, out var oldEntity);
            if(isVaildEntity)
            {
                resPoolManager.RecycleEntity(currentKey, ResFlag.Entity_Perview_Weapon, oldEntity);
                uid = 0;
            }
        }

        protected override void OnEntityComponentRemoved(Entity entity, WeaponModelComponent component, GameData<WeaponModelComponent, EquipPointComponent> data)
        {
            var modelComp = component;
            RecycleWeaponModel(modelComp.CurrentLeftHandWeaponKey, ref modelComp.LeftHandWeaponEntityUID);
            RecycleWeaponModel(modelComp.CurrentRightHandWeaponKey, ref modelComp.RightHandWeaponEntityUID);
        }
    }
}
