using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    /// <summary>
    /// 行为遮罩只会影响对角色的行为输入。 会中断角色行为
    /// </summary>
    [DisallowMultipleComponent]
    [DefaultEntityComponentProcessor(typeof(ActionMaskProcessor))]
    public class ActionMaskComponent : EntityComponent
    {

        [SerializeField]
        private ActionFlag originActionMask = ActionFlag.All;
        private int actionMask = -1;

        //禁用则表示改动作完全无法进行，进行中也会被打断
        public void DisabledAction(ActionFlag action)
        {
            actionMask &= ~((int)action);
        }

        public bool IsActionEnable(ActionFlag action) 
        {
            return (actionMask & ((int)action)) > 0;
        }

        public void ClearActionMask()
        {
            actionMask = ((int)originActionMask);
        }
    }



}
