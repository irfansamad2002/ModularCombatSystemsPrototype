using UnityEngine;

public class AbilityTargetResolver
{
    private Camera _cam;
    private LayerMask _worldLayer;
    private LayerMask _targetLayer;

    private float _maxDistanceForRays = 100f;

    public AbilityTargetResolver(Camera cam,
        LayerMask worldLayer,
        LayerMask targetLayer)
    {
        _cam = cam;
        _worldLayer = worldLayer;
        _targetLayer = targetLayer;
        
    }

    public bool TryGetAimPoint(out Vector3 point)
    {
        Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        // Only world + enemies, NEVER indicator layer
        int mask = _worldLayer | _targetLayer;

        if (Physics.Raycast(ray, out RaycastHit hit, _maxDistanceForRays, mask))
        {
            point = hit.point;
            return true;
        }

        point = ray.origin + ray.direction * 100f;
        return true;
    }

    public GameObject RaycastEnemy()
    {
        Ray ray = _cam.ViewportPointToRay(new Vector3(.5f, .5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, _targetLayer))
        {
            return hit.collider.gameObject;
        }

        return null;
    }

    public Vector3 GetAimDirection()
    {
        Vector3 direction = _cam.transform.forward;

        direction.y = 0f;

        return direction.normalized;
    }

}
