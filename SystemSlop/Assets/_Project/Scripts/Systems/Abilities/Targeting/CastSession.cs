using Project.Systems.Abilities.Data;
using Project.Systems.VFX;
using UnityEngine;
namespace Project.Systems.Abilities.Runtime
{
    public class CastSession
    {
        // ==================== Core References ====================
        private readonly AbilityData _ability;
        private readonly AbilityUser _user;
        private readonly AbilityTargetResolver _targetResolver;

        private ExecutionContext _context;
        private TargetingPreview _indicator;

        // ==================== Lifecycle ====================
        private bool _isActive;
        public bool IsActive => _isActive;

        private CastState _state;
        public CastState State => _state;
        public bool IsInterruptible => _state == CastState.Casting;

        // ==================== Constructor ====================
        public CastSession(
            AbilityUser user,
            AbilityData ability,
            AbilityTargetResolver targetResolver)
        {
            _user = user;
            _ability = ability;
            _targetResolver = targetResolver;

            _context = new ExecutionContext();

            _isActive = true;
            _state = CastState.Casting;

            CreateIndicator();
        }

        // ========================================
        // 1. Targetting responsibility
        // ========================================
        public void Update()
        {
            if (!IsActive || _state != CastState.Casting) 
                return;

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

            Debug.DrawRay(_user.Firepoint.position, _context.direction * 5f, Color.blueViolet);
        }

        private void UpdateSelfTargeting() //target
        {
            _context.castTarget = _user.gameObject;
            _context.direction = _targetResolver.GetAimDirection();
        }

        private void UpdateTargetTargeting() //target
        {
            GameObject target = _targetResolver.RaycastEnemy();

            if (target == null|| !IsTargetInRange(target))
            {
                _context.castTarget = null;
                return;
            }

            _context.castTarget = target;
        }

        private void UpdatePointTargeting() //target
        {
            if (!_targetResolver.TryGetAimPoint(out var point))
            {
                _indicator?.SetValid(false);
                return;
            }

            Vector3 origin = _user.Firepoint.position;
            Vector3 toPoint = point - origin;

            _context.direction = toPoint.normalized;

            //Clamp to range
            if (toPoint.magnitude > _ability.castRange)
                point = origin + toPoint.normalized * _ability.castRange;
            

            // Store FINAL corrected point
            _context.aimPoint = point;
            _context.hasAimPoint = true;
           
            _indicator?.SetPosition(point);
            _indicator?.SetValid(true);
            
        }
        
        // ========================================
        // 2. Execution Trigger Responibility
        // ========================================
        public void Confirm() //Execution
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

        // ========================================
        // 3. Lifestyle Responibility
        // ========================================
        public void Cancel() // Session Control
        {
            CleanUp();
        }

        public void Interrupt() // Session Control
        {
            if (_state == CastState.Completed || _state == CastState.Interrupted) return;

            _state = CastState.Interrupted;
            CleanUp();
        }

        private void CleanUp()
        {
            if (_indicator != null)
                GameObject.Destroy(_indicator.gameObject);

            _isActive = false;
        }

        // ========================================
        // Indicator
        // ========================================
        private void CreateIndicator()
        {
            if (_ability.indicatorPrefab == null)
                return;

            var obj = Object.Instantiate(_ability.indicatorPrefab);
            _indicator = obj.GetComponent<TargetingPreview>();

            _indicator?.Init(_ability.radius);
        }
        // ========================================
        // Util
        // ========================================
        private bool IsTargetInRange(GameObject target) //targetValidation
        {
            return Vector3.Distance(_user.transform.position, target.transform.position) 
                <= _ability.castRange;
        }

        // ========================================
        // DEBUG
        // ========================================
        public void DrawDebug() //Debug
        {
            GUILayout.BeginVertical("box");

            // ================= BASIC INFO =================
            GUILayout.Label($"ABILITY: {_ability.abilityName}");
            GUILayout.Label($"ACTIVE: {_isActive}");
            GUILayout.Label($"STATE: {_state}");

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

        private string FormatTarget(GameObject target) //Debug UI
        {
            if (target == null)
                return "NULL";

            return target.name;
        }

        private bool IsTargetInRangeDebug() // Debug TargetValidation
        {
            if (_context.castTarget == null) return false;

            return Vector3.Distance(_user.transform.position, _context.castTarget.transform.position)
                   <= _ability.castRange;
        }

        private bool IsContextValidDebug() //Debug ContextValidation
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

        // ========================================
        // State
        // ========================================
        public enum CastState //State of Casting
        {
            Idle,
            Casting,
            Executing,
            Interrupted,
            Completed
        }

        


    }
}