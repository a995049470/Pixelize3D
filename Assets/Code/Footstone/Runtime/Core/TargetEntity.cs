using UnityEngine;

namespace Lost.Runtime.Footstone.Core
{
    public class TargetEntity : MonoBehaviour, ITargetEnityReference
    {
        [SerializeField]
        private Entity target;
        public Entity Target { get => target; set => target = value; }
    }
}



