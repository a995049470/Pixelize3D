using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    public enum InputCommand
    {
        None = 0,
        MoveUp = 1 << 0,
        MoveDown = 1 << 1,
        MoveLeft = 1 << 2,
        MoveRight = 1 << 3,
        Move  = (1 + 2 + 4 + 8),

        ActionUp = 1 << 4,
        ActionDown = 1 << 5,
        ActionLeft = 1 << 6,
        ActionRight = 1 << 7,
        Action = (16 + 32 + 64 + 128),
        Cache = 1 << 8,
        Interaction = 1 << 9,
        Pick = 1 << 10,
        FastEquip = 1 << 11
    }

    [DisallowMultipleComponent]
    [DefaultEntityComponentProcessor(typeof(PlayerMoveControllerProcessor))]
    [DefaultEntityComponentProcessor(typeof(PlayerAttackControllerProcessor))]
    [DefaultEntityComponentProcessor(typeof(PlayerControllerProcessor))]
    [DefaultEntityComponentProcessor(typeof(PlayerControllerCacheProcessor))]
    public class PlayerControllerComponent : EntityComponent
    {   
        private KeyCode lastInputKey;
        private bool isReicveActionKey = false;
        private float keepTime = 0;
        private float cacheInputTime = 0;
        public InputCommand CurrentFrameInputCommand;
        private PlayerInputCache inputCache = new(0.4f);
        [System.NonSerialized]
        public bool DisableMoveUntilNoMoveButtonDown = false;
        [HideInInspector]
        public int FastEquipIndex = 0;
        
        //物品UID 用于校验
        public ulong ItemUID;
        //背包位置
        public int GridIndex;

        public bool TryGetFastEquipCommand(InputManager inputManager, InputSettingComponent setting, out int fastGridIndex)
        {
            bool isGet = false;
            var fastKeys = setting.FastEquipKeys;
            fastGridIndex = 0;
            for (int i = 0; i < fastKeys.Length; i++)
            {
                if(inputManager.IsKeyDown(fastKeys[i]))
                {
                    fastGridIndex = i;
                    isGet = true;
                    break;
                }
            }
            return isGet;
        }
        
        //清空当前帧所有输入指令
        public void ClearCurrentFrameInputCommand()
        {
            CurrentFrameInputCommand = 0;
        }

        public void CalculateCurrentFrameInputCommand(InputManager inputManager, InputSettingComponent setting, float time)
        {
            var cmd = InputCommand.None;
        
            if(inputManager.IsKey(setting.UpRun))
            {
                cmd |= InputCommand.MoveUp;
            }
            else if(inputManager.IsKey(setting.DownRun))
            {
                cmd |= InputCommand.MoveDown;
            }
            else if(inputManager.IsKey(setting.LeftRun))
            {
                cmd |= InputCommand.MoveLeft;
            }
            else if(inputManager.IsKey(setting.RightRun))
            {
                cmd |= InputCommand.MoveRight;
            }
            else
            {
                DisableMoveUntilNoMoveButtonDown = false;
            }

            if(DisableMoveUntilNoMoveButtonDown)
            {
                cmd &= ~InputCommand.Move;
            }

            bool isPeekSuccess = inputCache.TryPeekInput(time, out var cacheCode);

            if(inputManager.IsKeyDown(setting.UpAction))
            {
                cmd |= InputCommand.ActionUp;
            }
            else if(inputManager.IsKeyDown(setting.DownAction))
            {
                cmd |= InputCommand.ActionDown;
            }
            else if(inputManager.IsKeyDown(setting.LeftAction))
            {
                cmd |= InputCommand.ActionLeft;
            }
            else if(inputManager.IsKeyDown(setting.RightAction))
            {
                cmd |= InputCommand.ActionRight;
            }
            else if(inputManager.IsKeyDown(setting.Interaction))
            {
                cmd |= InputCommand.Interaction;
            }
            else if(isPeekSuccess)
            {
                cmd |= InputCommand.Cache;
                if (cacheCode == setting.UpAction)
                {
                    cmd |= InputCommand.ActionUp;
                }
                else if (cacheCode == setting.DownAction)
                {
                    cmd |= InputCommand.ActionDown;
                }
                else if (cacheCode == setting.LeftAction)
                {
                    cmd |= InputCommand.ActionLeft;
                }
                else if (cacheCode == setting.RightAction)
                {
                    cmd |= InputCommand.ActionRight;
                }
                else if (cacheCode == setting.Interaction)
                {
                    cmd |= InputCommand.Interaction;
                }
            }

            //捡东西命令 不需要缓存
            if(inputManager.IsKeyDown(setting.Pick))
            {
                cmd |= InputCommand.Pick;
            }
            
            if(TryGetFastEquipCommand(inputManager, setting, out var fastGridIndex))
            {
                cmd |= InputCommand.FastEquip;
                FastEquipIndex = fastGridIndex;
            }
            

            CurrentFrameInputCommand = cmd;
        }

        /// <summary>
        /// 消耗行动命令
        /// </summary>
        private void CostActionCommand()
        {
            CurrentFrameInputCommand &= ~InputCommand.Action;
        }   

        /// <summary>
        /// 消耗交互命令
        /// </summary>
        private void CostInteractionCommand()
        {
            CurrentFrameInputCommand &= ~InputCommand.Interaction;
        }

        /// <summary>
        /// 成功消耗了缓存输入
        /// </summary>
        private void CostCacheInput()
        {
            if((CurrentFrameInputCommand & InputCommand.Cache) > 0)
            {
                CurrentFrameInputCommand &= ~InputCommand.Cache;
                inputCache.CostInput();
            }
        }

        /// <summary>
        /// 成功行动
        /// </summary>
        public void ActionSuccess()
        {
            CostActionCommand();
            CostCacheInput();
        }

        /// <summary>
        /// 成功交互
        /// </summary>
        public void InteractionSuccess()
        {
            CostInteractionCommand();
            CostCacheInput();
        }

        public void CacheButton(float time, InputSettingComponent setting)
        {
            if((CurrentFrameInputCommand & InputCommand.Cache) == 0)
            {
                if((CurrentFrameInputCommand & InputCommand.ActionUp) > 0)
                    inputCache.CacheInput(time, setting.UpAction);
                else if((CurrentFrameInputCommand & InputCommand.ActionDown) > 0)
                    inputCache.CacheInput(time, setting.DownAction);
                else if((CurrentFrameInputCommand & InputCommand.ActionLeft) > 0)
                    inputCache.CacheInput(time, setting.LeftAction);
                else if((CurrentFrameInputCommand & InputCommand.ActionRight) > 0)
                    inputCache.CacheInput(time, setting.RightAction);
                else if((CurrentFrameInputCommand & InputCommand.Interaction) > 0)
                    inputCache.CacheInput(time, setting.Interaction);
                CostActionCommand();
                CostInteractionCommand();
            }
        }

        
    }
}
