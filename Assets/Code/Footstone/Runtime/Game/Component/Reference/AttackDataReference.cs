namespace Lost.Runtime.Footstone.Game
{

    [System.Serializable]
    public class AttackDataReference : AssetReference<AttackData>
    {
        public override ResFlag Flag => ResFlag.Data_Attack;
    }

}
