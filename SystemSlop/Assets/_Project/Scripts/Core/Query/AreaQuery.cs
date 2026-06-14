using System.Collections.Generic;
using UnityEngine;

namespace Project.Systems.Combat.Query
{
    public static class AreaQuery
    {
        public static List<GameObject> GetTargetsSphere(
            Vector3 center,
            float radius,
            LayerMask layerMask,
            Transform self)
        {
            Collider[] hits = Physics.OverlapSphere(center, radius, layerMask);

            List<GameObject> results = new();

            foreach (var hit in hits)
            {
                if (self != null && hit.transform.root == self.root)
                    continue;

                results.Add(hit.gameObject);
            }
            return results;
        }

        public static List<GameObject> GetTargetsSphere(
            Vector3 center,
            float radius,
            LayerMask layerMask)
        {
            Collider[] hits = Physics.OverlapSphere(center, radius, layerMask);

            List<GameObject> results = new();

            foreach (var hit in hits)
            {
                results.Add(hit.gameObject);
            }

            return results;
        }

        public static List<GameObject> FilterCone(
            List<GameObject> input,
            Vector3 origin,
            Vector3 forward,
            float coneAngle)
        {
            List<GameObject> results = new();

            float halfAngle = coneAngle * .5f;

            forward = Vector3.ProjectOnPlane(forward, Vector3.up).normalized;

            foreach (var obj in input)
            {
                Vector3 toTarget = Vector3.ProjectOnPlane(obj.transform.position - origin, Vector3.up).normalized;

                float angle = Vector3.Angle(forward, toTarget);

                if(angle <= halfAngle)
                    results.Add(obj);
            }

            return results; 
        }
    
    }
}