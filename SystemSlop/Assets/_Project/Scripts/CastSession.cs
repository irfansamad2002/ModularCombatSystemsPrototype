using Project.Systems.Abilities;
using Project.Systems.Ability;
using Unity.VectorGraphics;
using UnityEngine;

public class CastSession
{
    private AbilityUser _user;
    private AbilityData _ability;

    private AOEIndicator _indicator;

    private AbilityContext _context;

    private bool _isActive;

    public bool IsActive => _isActive;

    public CastSession(AbilityUser user, AbilityData ability)
    {
        _user = user;
        _ability = ability;
        _isActive = true;

        if (ability.indicatorPrefab != null)
        {
            var obj = GameObject.Instantiate(_ability.indicatorPrefab);
            _indicator = obj.GetComponent<AOEIndicator>();
            _indicator.Init(ability.projectile.explosionRadius);
        }
    }
    public void Update(Camera cam, LayerMask groundLayer)
    {
        if (!IsActive) return;

        if (_ability.targetingType == TargetingType.Point)
        {
            if (TryGetGroundPoint(cam, groundLayer, out var point))
            {
                _context.point = point;

                float range = _ability.castRange;
                Vector3 origin = _user.transform.position;

                Vector3 dir = point - origin;
                float dist = dir.magnitude;

                bool isValid = dist <= range;

                if (!isValid)
                {
                    dir = dir.normalized * range;
                    point = origin + dir;
                }

                _indicator.SetPosition(point);
                _indicator.SetValid(isValid);
            }
            else
            {
                _indicator.gameObject.SetActive(false);
            }
        }
    }

    public void Confirm()
    {
        if (!_isActive) return;

        if (_ability.targetingType == TargetingType.Point)
        {
            if (_indicator == null || !_indicator.IsValid())
            {
                Cancel();
                return;
            }
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

    private bool TryGetGroundPoint(Camera cam, LayerMask groundLayer, out Vector3 point)
    {
        if (UnityEngine.InputSystem.Mouse.current == null)
        {
            point = Vector3.zero;
            return false;
        }

        Vector2 mousePos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();

        Ray ray = cam.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            point = hit.point;
            return true;
        }

        point = Vector3.zero;
        return false;   
    }
    
}
