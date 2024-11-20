namespace Lost.Runtime.Footstone.Game
{
    [System.Serializable]
    public class GOAPGoalReference : AssetReference<GOAPGoalData>
    {
        public override ResFlag Flag => ResFlag.Data_Goal;
    }

}
