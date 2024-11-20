using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    public class FastUseProcessor : SimpleGameEntityProcessor<FastUseComponent, ActionMaskComponent, RotateComponent>
    {
        //10°的cos值
        public FastUseProcessor() : base()
        {
            Order = ProcessorOrder.FastUse;
        }

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
        }

        private bool IsCorrectFaceDir(Vector3 faceDir, Vector3 attackDir)
        {
            return DirectionUtil.IsCorrectFaceDir(faceDir, attackDir);
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var fastUseComp = kvp.Value.Component1;
                var actionMaskComp = kvp.Value.Component2;
                if(fastUseComp.IsFastUsing)
                {
                    var rotateComp = kvp.Value.Component3;
                    bool isSowUpdate = true;
                    bool isInvokeFastUse = false;
                    if(!actionMaskComp.IsActionEnable(ActionFlag.FastUse) || 
                       rotateComp.FaceDirection != fastUseComp.FastUseDir)
                    {
                        fastUseComp.ForceCompleteFastUse();
                        isSowUpdate = false;
                    }
                    
                    if(!IsCorrectFaceDir(fastUseComp.Entity.Transform.Forward, fastUseComp.FastUseDir))
                    {
                        isSowUpdate = false;
                    }
                    if(isSowUpdate)
                    {
                        isInvokeFastUse = true;
                        fastUseComp.ForceCompleteFastUse();
                    }
                    if(isInvokeFastUse)
                    {
                        var bagGridIndex = fastUseComp.BagGridIndex;
                        var itemData = bagData.GetBagGridItemData(bagGridIndex);
                        if(itemData.IsVaild())
                        {
                            //先拿数据 在消耗物品 防止拿不到数据的情况
                            var effectKey = itemData.GetEffectEntityKey();
                            if(bagData.TryLoseItem(bagGridIndex, fastUseComp.ItemUID, 1))
                            {
                                cmd.InstantiateEntity(effectKey, ResFlag.Entity_Power, UnityEngine.Vector3.zero, 0, entity =>
                                {
                                    entity.GetOrCreate<UseLabelComponent>().Use(fastUseComp.Entity);
                                });
                            }
                        }
                    }      
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
