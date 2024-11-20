using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    [System.Serializable]
    public class ParticleEntityAssetReference : AssetReference<GameObject>
    {
        public override ResFlag Flag => ResFlag.Entity_Particle;
    }
    
}
