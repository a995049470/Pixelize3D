using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    public class InstantiateParticleCommand : ICommand
    {
        public string EffectName;
        public ResPoolManager EntityPoolManager;
        public Vector3 Position;
        public void Execute()
        {
            var entity = EntityPoolManager.InstantiateEntity(EffectName, ResFlag.Entity_Particle);
            entity.Transform.Position = Position;
        }
    }
}
