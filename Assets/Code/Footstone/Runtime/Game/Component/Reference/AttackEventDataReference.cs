namespace Lost.Runtime.Footstone.Game
{
    [System.Serializable]
    public class AttackEventDataReference : AssetReference<AttackEventDamageData>
    {
        public override ResFlag Flag { get; } = ResFlag.Data_AttackEvent;
    }

}
