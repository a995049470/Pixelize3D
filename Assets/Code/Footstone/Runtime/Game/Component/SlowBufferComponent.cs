using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    /// <summary>
    /// 添加减速(攻速 移速 转身速度)buffer
    /// </summary>
    [DefaultEntityComponentProcessor(typeof(AddSlowBuffProcessor))]
    public class SlowBufferComponent : EntityComponent, IBufferComponent
    {
        [Range(0, 1)]
        public float SlowPercentage = 0.3f;
    
        public float SlowDuration = 0.0f;
    }
    
}
