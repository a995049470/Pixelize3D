using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    //固定掉落物的组件，一般用于怪物死亡掉落的可拾取物
    [DefaultEntityComponentProcessor(typeof(DroppedItemEffectProcessor))]
    public class DroppedItemCopmponent : EntityComponent
    {
        //[HideInInspector]
        public List<DroppedItem> DroppedItems = new();


        public void AddDroppedItem(IEnumerable<DroppedItem> droppedItems)
        {
            DroppedItems.Clear();
            DroppedItems.AddRange(droppedItems);
        }
    }

}
