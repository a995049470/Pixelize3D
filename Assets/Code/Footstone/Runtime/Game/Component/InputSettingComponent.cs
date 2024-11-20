using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    [DefaultEntityComponentProcessor(typeof(InputSettingProcessor))]
    public class InputSettingComponent : EntityComponent
    {
        [HideInInspector]
        public KeyCode UpRun = KeyCode.W;
        [HideInInspector]
        public KeyCode DownRun = KeyCode.S;
        [HideInInspector]
        public KeyCode LeftRun = KeyCode.A;
        [HideInInspector]
        public KeyCode RightRun = KeyCode.D;
        [HideInInspector]
        public KeyCode UpAction = KeyCode.I;
        [HideInInspector]
        public KeyCode DownAction = KeyCode.K;
        [HideInInspector]
        public KeyCode LeftAction = KeyCode.J;
        [HideInInspector]
        public KeyCode RightAction = KeyCode.L;
        [HideInInspector]
        public KeyCode Interaction = KeyCode.Space;
        [HideInInspector]
        public KeyCode Pick = KeyCode.Space;
        [HideInInspector]
        public KeyCode[] FastEquipKeys = new KeyCode[10]
        {
            KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5,
            KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0
        };
    }

}