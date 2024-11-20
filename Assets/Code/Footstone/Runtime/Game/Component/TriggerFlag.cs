namespace Lost.Runtime.Footstone.Game
{
    public enum TriggerFlag
    {
        //等待条件判断后再决定是否再次触发，防止物体反复触发
        WaitCheck,
        //等待触发
        WaitTrigger,
        //正在触发 结束后被回收
        Triggering,
        //正在触发, 结束后不回收
        TriggeringNotRecyle,
        //触发完成
        Triggered,
        //等待回收
        WaitRecycle,
        //等待玩家选择后结束触发
        WaitPlayerRespond,
        
        //无效
        Invalid = -1,
        
    }

}
