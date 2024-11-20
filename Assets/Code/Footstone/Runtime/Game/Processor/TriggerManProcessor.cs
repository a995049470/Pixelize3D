using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class TriggerManProcessor : SimpleGameEntityProcessor<TriggerManComponent, ActionMaskComponent>
    {
        private Dictionary<ulong, TriggerManComponent> triggerManDic = new();
        private HashSet<TriggerManComponent> newTriggerManComponents = new();
        public TriggerManProcessor() : base()
        {
            Order = ProcessorOrder.UpadteActionMask;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var triggerManComp = kvp.Value.Component1;
                var actionMaskComp = kvp.Value.Component2;
                actionMaskComp.DisabledAction(triggerManComp.InvaildActions);
                triggerManComp.InvaildActions = ActionFlag.None;
            }
        }

        public bool TryGetTriggerMan(ulong uid, out TriggerManComponent triggerMan)
        {
            if(newTriggerManComponents.Count > 0)
            {
                foreach (var component in newTriggerManComponents)
                {
                    triggerManDic[component.Id] = component;
                }
                newTriggerManComponents.Clear();
            }
            return triggerManDic.TryGetValue(uid, out triggerMan);
        }

        protected override void OnEntityComponentAdding(Entity entity, TriggerManComponent component, GameData<TriggerManComponent, ActionMaskComponent> data)
        {
            //componentId可能在加载后需要赋值，所以先将组件加到缓存中
            newTriggerManComponents.Add(component);
        }

        protected override void OnEntityComponentRemoved(Entity entity, TriggerManComponent component, GameData<TriggerManComponent, ActionMaskComponent> data)
        {
            if(newTriggerManComponents.Contains(component))
            {
                newTriggerManComponents.Remove(component);
            }
            else
            {
                triggerManDic.Remove(component.Id);
            }
        }
    }
}
