using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    [DefaultEntityComponentProcessor(typeof(InteractionCostProcessor))]
    public class CostComponent : EntityComponent
    {
        public List<ItemInfo> CostItems = new();
        [UnityEngine.HideInInspector]
        public bool IsSuccessCost = false;

        protected override void OnEnableRuntime()
        {
            IsSuccessCost = false;
        }

        public bool TryCost(BagData bagData)
        {
            if(!IsSuccessCost)
            {
                IsSuccessCost = bagData.IsItemNumEnough(CostItems);

                if(IsSuccessCost)
                {
                    foreach (var item in CostItems)
                    {
                        bagData.LoseItem(item.Name, item.Count);
                    }
                }
            }
            
            return IsSuccessCost;
        }
    }

}
