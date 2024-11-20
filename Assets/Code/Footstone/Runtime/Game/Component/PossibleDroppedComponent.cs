using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public enum DroppedItemFlag
    {
        //进入背包
        EnterBag = 0,
        //立即生效
        Effective = 1,
    }

    [System.Serializable]
    public class DroppedItem
    {
        public string Name;
        public int Count;
        public DroppedItemFlag Flag;
    }

    [System.Serializable]
    public class PossibleDroppedItem
    {
        public DroppedItem Item;
        //0 - 100
        [Range(0, 100)]
        public int Probability;
    }

    [DisallowMultipleComponent]
    public class PossibleDroppedComponent : EntityComponent
    {
        //标记已经掉落过
        [HideInInspector]
        public bool IsDropped = false;
        public List<PossibleDroppedItem> PossibleDroppedItems = new();
        [HideInInspector]
        public List<DroppedItem> DroppedItems = new();
        
        public void CreateDroppedItemList(System.Random random)
        {
            DroppedItems.Clear();
            foreach (var possibleDroppedItem in PossibleDroppedItems)
            {
                var isPass = random.Next(0, 100) < possibleDroppedItem.Probability;
                if (isPass)
                {
                    DroppedItems.Add(possibleDroppedItem.Item);
                }
            }
        }

        protected override void OnEnableRuntime()
        {
            base.OnEnableRuntime();
            IsDropped = false;
        }

        public void ReceiveDroppedItems(CommandBuffer cmd, BagData bagData, Entity target, System.Random random)
        {
            this.CreateDroppedItemList(random);
            var droppedItems = this.DroppedItems;
            foreach (var droppedItem in droppedItems)
            {
                switch (droppedItem.Flag)
                {
                    case DroppedItemFlag.EnterBag:
                        bagData.ReceiveItem(droppedItem.Name, droppedItem.Count);
                        break;
                    case DroppedItemFlag.Effective:
                        cmd.InstantiateEntity(droppedItem.Name, ResFlag.Entity_Power, Vector3.zero, 0, entity =>
                        {
                            entity.GetOrCreate<BufferLabelComponent>().Target = target;
                        });
                        break;
                }
            }
        }


    }

}
