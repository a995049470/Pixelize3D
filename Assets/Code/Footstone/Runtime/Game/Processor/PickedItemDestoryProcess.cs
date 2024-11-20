// using System.Collections.Generic;
// using Lost.Runtime.Footstone.Core;

// namespace Lost.Runtime.Footstone.Game
// {
//     public class PickedItemDestoryData
//     {
//         // public OwnerComponent Owner;
//         // public DestoryComponent Destory;
//     }

//     public class PickedItemDestoryProcess : GameEntityProcessor<DestoryComponent, PickedItemDestoryData>
//     {
//         public PickedItemDestoryProcess() : base(typeof(OwnerComponent))
//         {
//             Order = ProcessorOrder.PickedItemDestory;
//         }

//         public override void Update(GameTime time)
//         {
//             base.Update(time);
//             if (ComponentDatas.Count > 0)
//             {
//                 var keys = new List<DestoryComponent>(ComponentDatas.Keys);
//                 keys.ForEach(componet => RemoveEntity(componet.Entity));
//             }
//         }

//         protected override PickedItemDestoryData GenerateComponentData(Entity entity, DestoryComponent component)
//         {
//             return new PickedItemDestoryData();
//         }

//         protected override bool IsAssociatedDataValid(Entity entity, DestoryComponent component, PickedItemDestoryData associatedData)
//         {
//             return true;
//         }
//     }
// }
