namespace Lost.Runtime.Footstone.Game
{
    /// <summary>
    /// 在穿装备那帧获得赋予能力或脱装备那帧剥夺能力，只会存在一帧，必须靠影响其他组件来实现效果
    /// </summary>
    public interface ITakeEffectOnEquipOrPullFrame
    {
        //设置生效的时点
        void SetTimePoint(bool isEquipping, ulong uid);
    }

    
    
}



