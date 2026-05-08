using Project.Systems.Abilities;
using Project.Systems.Ability;
using Unity.VectorGraphics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CastSession
{
    private AbilityData _ability;
    private AbilityUser _user;
    private Camera _cam;

    private AOEIndicator _indicator;
    private AbilityContext _context;

    private bool _isActive;
    public bool IsActive => _isActive;

    private LayerMask _worldLayer;
    private LayerMask _targetLayer;

    private float _maxDistanceForRays = 100f;

    public CastSession(
        AbilityUser user,
        AbilityData ability,
        Camera cam,
        LayerMask worldLayer,
        LayerMask targetLayer)
    {
        _user = user;
        _ability = ability;
        _cam = cam;
        _worldLayer = worldLayer;
        _targetLayer = targetLayer;

        _isActive = true;

        _context = new AbilityContext();

        if (ability.indicatorPrefab != null) 
        {
            var obj = GameObject.Instantiate(_ability.indicatorPrefab);
            _indicator = obj.GetComponent<AOEIndicator>();

            if(_ability.projectile != null)
                _indicator.Init(ability.projectile.explosionRadius);
        }

    }
    public void Update()
    {
        if (!IsActive) return;

        switch (_ability.targetingType)
        {
            case TargetingType.None:
                break;
            case TargetingType.Point:
                UpdatePointTargeting();
                break;
            case TargetingType.Target:
                UpdateTargetTargeting();
                break;
            case TargetingType.Self:
                break;
            default:
                break;
        }
    }

    private void UpdateTargetTargeting()
    {
        Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f,0.5f,0f));

        if (Physics.Raycast(ray, out RaycastHit hit, _maxDistanceForRays, _targetLayer))
        {
            GameObject target = hit.collider.gameObject;

            if (IsTargetInRange(target))
            {
                _context.target = target;
                return;
            }
        }
        _context.target = null;
       
    }

    private void UpdatePointTargeting()
    {
        TryGetAimPoint(out var point);

        Vector3 origin = _user.Firepoint.position;

        Vector3 toPoint = point - origin;
        float dist = toPoint.magnitude;

        float range = _ability.castRange;

        //Clamp to range
        if (dist > range)
        {
            toPoint = toPoint.normalized * range;
            point = origin + toPoint;
        }

        // Store FINAL corrected point
        _context.point = point;
        _context.hasPoint = true;

        if (_indicator != null)
        {
            _indicator.gameObject.SetActive(true);
            _indicator.SetPosition(point);
            _indicator.SetValid(true);
        }
    }

    public void Confirm()
    {
        if (!_isActive) return;

        switch (_ability.targetingType)
        {
            case TargetingType.None:
                break;
            case TargetingType.Point:
                if (_indicator == null || !_indicator.IsValid())
                {
                    Cancel();
                    return;
                }
                break;
            case TargetingType.Target:
                if (_context.target == null)
                {
                    Cancel();
                    return;
                }
                break;
            case TargetingType.Self:
                break;
            default:
                break;
        }


        _user.UseAbility(_ability, _context);
        CleanUp();
    }

    public void Cancel()
    {
        CleanUp();
    }

    private void CleanUp()
    {
        if (_indicator != null)
        {
            GameObject.Destroy(_indicator.gameObject);
        }

        _isActive = false;
    }

    private bool TryGetAimPoint(out Vector3 point)
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


    private bool IsTargetInRange(GameObject target)
    {
        float distance = Vector3.Distance(_user.transform.position, target.transform.position);

        return distance <= _ability.castRange;
    }

    
}
