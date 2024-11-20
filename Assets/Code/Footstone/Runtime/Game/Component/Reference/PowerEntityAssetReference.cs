using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    [System.Serializable]
    public class PowerEntityAssetReference : AssetReference<GameObject>
    {
        public override ResFlag Flag => ResFlag.Entity_Power;
    }
    
}
