using Project.Systems.Abilities.Data;
using Project.Systems.VFX;
using UnityEngine;
namespace Project.Systems.Abilities.Runtime
{
    public class CastSession
    {
        private AbilityData _ability;
        private AbilityUser _user;

        private AOEIndicator _indicator;
        private ExecutionContext _context;

        private bool _isActive;
        public bool IsActive => _isActive;

        private bool _isValidCast;
        public bool IsValidCast => _isValidCast;

        private AbilityTargetResolver _targetResolver;

        private CastState _state;
        public CastState State => _state;

        public bool IsInterruptible => _state == CastState.Casting;

        public CastSession(
            AbilityUser user,
            AbilityData ability,
            AbilityTargetResolver targetResolver)
        {
            _user = user;
            _ability = ability;
            _targetResolver = targetResolver;

            _isActive = true;

            _context = new ExecutionContext();
            _state = CastState.Casting;

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

            if (_state != CastState.Casting) return;

            switch (_ability.targetingType)
            {
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


            _isValidCast = _user.CanConfirmCast(_ability, _context);

            Debug.DrawRay(_user.Firepoint.position, _context.direction * 5f, Color.blueViolet);
        }

        private void UpdateSelfTargeting()
        {
            _context.castTarget = _user.gameObject;
            _context.direction = _targetResolver.GetAimDirection();
        }

        private void UpdateTargetTargeting()
        {
            GameObject target = _targetResolver.RaycastEnemy();

            if (target == null)
            {
                _context.castTarget = null;
                return;
            }

            if (!IsTargetInRange(target))
            {
                _context.castTarget = null;
                return;
            }

            _context.castTarget = target;

        }

        private void UpdatePointTargeting()
        {
            if (!_targetResolver.TryGetAimPoint(out var point))
            {
                if (_indicator != null)
                {
                    _indicator.gameObject.SetActive(false);
                    Debug.Log("AOEIndicator not set");
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
            _context.aimPoint = point;
            _context.hasAimPoint = true;

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

           if (!_user.CanConfirmCast(_ability, _context))
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
            _user.NotifyCastFinished(this);
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

    

        public void DrawDebug()
        {
            GUILayout.BeginVertical("box");

            // ================= BASIC INFO =================
            GUILayout.Label($"ABILITY: {_ability.abilityName}");
            GUILayout.Label($"ACTIVE: {_isActive}");
            GUILayout.Label($"STATE: {_state}");
            GUILayout.Label($"VALID CAST: {_isValidCast}");

            GUILayout.Space(5);

            // ================= TARGETING =================
            GUILayout.Label($"TARGETING TYPE: {_ability.targetingType}");

            switch (_ability.targetingType)
            {
                case TargetingType.Point:
                    GUILayout.Label($"Aim Point: {_context.aimPoint}");
                    GUILayout.Label($"Has Aim Point: {_context.hasAimPoint}");
                    GUILayout.Label($"Direction: {_context.direction}");
                    break;

                case TargetingType.Target:
                    GUILayout.Label($"Cast Target: {FormatTarget(_context.castTarget)}");
                    GUILayout.Label($"In Range: {IsTargetInRangeDebug()}");
                    break;

                case TargetingType.Self:
                    GUILayout.Label($"Self Target: {_context.castTarget}");
                    break;
            }

            GUILayout.Space(5);

            // ================= DIAGNOSTICS =================
            GUILayout.Label("DIAGNOSTICS");

            GUILayout.Label($"Context Valid: {IsContextValidDebug()}");
            GUILayout.Label($"Confirm Ready: {_user.CanConfirmCast(_ability, _context)}");

            GUILayout.EndVertical();
        }

        private string FormatTarget(GameObject target)
        {
            if (target == null)
                return "NULL";

            return target.name;
        }

        private bool IsTargetInRangeDebug()
        {
            if (_context.castTarget == null) return false;

            return Vector3.Distance(_user.transform.position, _context.castTarget.transform.position)
                   <= _ability.castRange;
        }

        private bool IsContextValidDebug()
        {
            switch (_ability.targetingType)
            {
                case TargetingType.Point:
                    return _context.hasAimPoint;

                case TargetingType.Target:
                    return _context.castTarget != null;

                case TargetingType.Self:
                    return true;

                default:
                    return false;
            }
        }

        public enum CastState
        {
            Idle,
            Casting,
            Executing,
            Interrupted,
            Completed
        }

        public void Interrupt()
        {
            if (_state == CastState.Completed || _state == CastState.Interrupted) return;

            _state = CastState.Interrupted;
            CleanUp();
        }


    }
}