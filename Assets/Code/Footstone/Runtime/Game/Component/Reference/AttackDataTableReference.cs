namespace Lost.Runtime.Footstone.Game
{
    [System.Serializable]
    public class AttackDataTableReference : AssetReference<AttackDataTable>
    {
        public override ResFlag Flag => ResFlag.Data_AttackTable;
    }
}
