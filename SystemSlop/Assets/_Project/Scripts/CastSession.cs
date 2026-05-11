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

    private AOEIndicator _indicator;
    private AbilityContext _context;

    private bool _isActive;
    public bool IsActive => _isActive;

    private float _maxDistanceForRays = 100f;

    private bool _isValidCast;
    public bool IsValidCast => _isValidCast;

    private AbilityTargetResolver _targetResolver;

    public CastSession(
        AbilityUser user,
        AbilityData ability,
        AbilityTargetResolver targetResolver)
    {
        _user = user;
        _ability = ability;
        _targetResolver = targetResolver;

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

        ResetContext();

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
                UpdateSelfTargeting();
                break;
        }

        _isValidCast = CanConfirmCast();
    }

    private void UpdateSelfTargeting()
    {
        _context.target = _user.gameObject;
        _context.direction = _targetResolver.GetAimDirection();
    }

    private void UpdateTargetTargeting()
    {
        GameObject target = _targetResolver.RaycastEnemy();

        if (target == null)
        {
            _context.target = null;
            return;
        }

        if (!IsTargetInRange(target))
        {
            _context.target = null;
            return;
        }

        _context.target = target;

    }

    private void UpdatePointTargeting()
    {
        if (!_targetResolver.TryGetAimPoint(out var point))
        {
            if (_indicator != null)
            {
                _indicator.gameObject.SetActive(false);
            }

            return;
        }

        Vector3 origin = _user.Firepoint.position;

        Vector3 toPoint = point - origin;
        _context.direction = toPoint.normalized;
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

       if (!_user.CanUseAbility(_ability, _context))
       {
            Cancel();
            return;
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

    private bool IsTargetInRange(GameObject target)
    {
        float distance = Vector3.Distance(_user.transform.position, target.transform.position);

        return distance <= _ability.castRange;
    }

    private bool CanConfirmCast()
    {
        switch (_ability.targetingType)
        {
            case TargetingType.None:
                return true;
            case TargetingType.Point:
                return _context.hasPoint;
            case TargetingType.Target:
                return _context.target != null;
            case TargetingType.Self:
                return true;
        }

        return false;
    }

    private void ResetContext()
    {
        _context = new AbilityContext();
    }

    public void DrawDebug()
    {
        GUILayout.Label($"Ability: {_ability.abilityName}");
        GUILayout.Label($"Active: {_isActive}");
        GUILayout.Label($"Valid: {_isValidCast}");
        GUILayout.Label($"Targeting: {_ability.targetingType}");

        switch (_ability.targetingType)
        {
            case TargetingType.Point:
                GUILayout.Label($"Point: {_context.point}");
                GUILayout.Label($"HasPoint: {_context.hasPoint}");
                break;

            case TargetingType.Target:
                GUILayout.Label($"Target: {_context.target}");
                break;
        }
    }

    
}
