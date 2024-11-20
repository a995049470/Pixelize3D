using Lost.Runtime.Footstone.Core;


namespace Lost.Runtime.Footstone.Game
{

    /// <summary>
    /// 标签组件，暂定用于保存功能
    /// </summary>
    [DefaultEntityComponentProcessor(typeof(TagProcessor))]
    public class TagComponent : EntityComponent
    {
        public ResFlag Flag;
        public string Key;
        public bool IsOverwrite = true;
        
        /// <summary>
        /// 玩家实体。可能会随着玩家销毁而销毁
        /// </summary>
        /// <returns></returns>
        public bool IsPlayerEntity()
        {
            return Flag == ResFlag.Entity_Player;
        }

        /// <summary>
        /// 系统实体。 可能会随着场景销毁而销毁
        /// </summary>
        /// <returns></returns>
        public bool IsSystemEntity()
        {
            return Flag == ResFlag.Entity_System;
        }

        public bool IsUIEntity()
        {
            return Flag == ResFlag.UIView;
        }

        /// <summary>
        /// 逻辑实体，暂定不删除。。
        /// </summary>
        /// <returns></returns>
        public bool IsPowerEntity()
        {
            return Flag == ResFlag.Entity_Power;
        }

        public bool IsChildPerviewEntity()
        {
            return Flag == ResFlag.Entity_Perview_Weapon;
        }
        /// <summary>
        /// 场景实体，一定会跟随场景销毁而销毁
        /// </summary>
        /// <returns></returns>
        public bool IsSceneEntity()
        {
            return !IsPlayerEntity() && !IsUIEntity() && !IsPowerEntity();
        }

        
        
    }

}
