using Lost.Runtime.Footstone.Game;
using UnityEngine;
namespace Lost.Runtime.Footstone.Core
{
    public class PhysicsSystem
    {
        public bool Raycast(Vector3 from, Vector3 to, int layerMask = -1, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore)
        {
            var origin = from;
            var dir = to - from;
            var maxDis = dir.magnitude;
            dir = dir.normalized;
            bool isSuccess = Physics.Raycast(origin, dir, maxDis, layerMask, queryTriggerInteraction);
            return isSuccess;
        }

        public bool Raycast(Vector3 from, Vector3 to, out RaycastHit hit, int layerMask = -1, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore)
        {
            var origin = from;
            var dir = to - from;
            var maxDis = dir.magnitude;
            dir = dir.normalized;
            bool isSuccess = Physics.Raycast(origin, dir, out hit, maxDis, layerMask, queryTriggerInteraction);
            return isSuccess;
        }

        public int RaycastNonAlloc(Vector3 from, Vector3 to, RaycastHit[] raycastHits, int layerMask = -1, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore)
        {
            var origin = from;
            var dir = to - from;
            var maxDis = dir.magnitude;
            dir = dir.normalized;
            return Physics.RaycastNonAlloc(origin, dir, raycastHits, maxDis, layerMask, queryTriggerInteraction);
        }

        public int SphereCastNonAlloc(Vector3 position, float radius, Collider[] results, int layerMask = -1, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore)
        {
            return Physics.OverlapSphereNonAlloc(position, radius, results, layerMask, queryTriggerInteraction);
        }

    }
}



