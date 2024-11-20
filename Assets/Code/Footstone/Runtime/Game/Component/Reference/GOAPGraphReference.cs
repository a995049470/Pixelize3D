namespace Lost.Runtime.Footstone.Game
{
    [System.Serializable]
    public class GOAPGraphReference : AssetReference<GOAPSerializableActionGraph>
    {
        public override ResFlag Flag => ResFlag.Data_Graph;
    }

}
