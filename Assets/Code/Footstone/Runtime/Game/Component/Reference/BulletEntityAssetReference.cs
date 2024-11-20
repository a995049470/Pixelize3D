using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    [System.Serializable]
    public class BulletEntityAssetReference : AssetReference<GameObject>
    {
        public override ResFlag Flag => ResFlag.Entity_Bullet;
    }
    
}
