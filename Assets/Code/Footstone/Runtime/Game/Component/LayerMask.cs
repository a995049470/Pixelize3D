namespace Lost.Runtime.Footstone.Game
{
    [System.Flags]
    public enum LayerMask : int
    {
        Defalut = GameConstant.Layer0,
        Player = GameConstant.PlayerLayer,
        Enemy = GameConstant.EnemyLayer,
        Barrier = GameConstant.BarrierLayer,
        Pick = GameConstant.PickableLayer,
        Crop = GameConstant.CropLayer,
        GameEvent = GameConstant.GameEventLayer,
        All = -1,
    }
}
