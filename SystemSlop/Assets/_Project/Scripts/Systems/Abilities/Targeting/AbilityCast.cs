using Project.Systems.Abilities.Data;
using Project.Systems.VFX;
using UnityEngine;
namespace Project.Systems.Abilities.Runtime
{
    public class AbilityCast
    {
        // ==================== Core References ====================
        private readonly AbilityData _ability;
        private readonly AbilityUser _user;
        private readonly AbilityTargetingCalculator _targetingCalculator;

        private AbilityTargetingData _targetingData;
        private AbilityTargetingIndicator _indicator;

        // ==================== Lifecycle ====================  
        private bool _isCasting;
        public bool IsActive => _isCasting;

        // ==================== Constructor ====================
        public AbilityCast(
            AbilityUser user,
            AbilityData ability,
            AbilityTargetingCalculator calculator)
        {
            _user = user;
            _ability = ability;
            _targetingCalculator = calculator;

            _targetingData = _targetingCalculator.CalculateTargeting(_ability);


            CreateIndicator();

            _isCasting = true;
        }

        public void Update()
        {
            if (!IsActive) 
                return;

            _targetingData = _targetingCalculator.CalculateTargeting(_ability);

            UpdateIndicator();
        
        }

        public void ConfirmCast()
        {
            if (!_isCasting) return;
            
           if (!_user.CanConfirmCast(_ability, _targetingData))
           {
                CancelCast();
                return;
           }

            _user.TryUseAbility(_ability, _targetingData);
            CleanUp();
        }

        public void CancelCast()
        {
            CleanUp();
        }

        public void InterruptCast()
        {
            if (!_isCasting) return;

            CleanUp();
        }

        private void CleanUp()
        {
            _indicator?.DestroySelf();
            _isCasting = false;
        }

        private void UpdateIndicator()
        {
            if (_indicator == null)
                return;

            switch (_ability.targetingType)
            {
                case TargetingType.Point:
                    _indicator.SetPosition(_targetingData.aimPoint);
                    _indicator.SetValid(_targetingData.hasAimPoint);
                    break;

                case TargetingType.Target:
                    if (_targetingData.castTarget != null)
                    {
                        _indicator.SetPosition(_targetingData.castTarget.transform.position);
                        _indicator.SetValid(true);
                    }
                    else
                    {
                        _indicator.SetValid(false);
                    }
                        break;

                case TargetingType.Self:
                    _indicator.SetPosition(_user.transform.position);
                    _indicator.SetValid(true);
                    break;
                default:
                    break;
            }
        }

        private void CreateIndicator()
        {
            if (_ability.indicatorPrefab == null)
                return;

            var obj = Object.Instantiate(_ability.indicatorPrefab);

            _indicator = obj.GetComponent<AbilityTargetingIndicator>();

            _indicator?.Initialize(_ability.radius);
        }

        // ========================================
        // DEBUG
        // ========================================
        public void DrawDebug() //Debug
        {
            GUILayout.BeginVertical("box");

            // ================= BASIC INFO =================
            GUILayout.Label($"ABILITY: {_ability.abilityName}");
            GUILayout.Label($"ACTIVE: {_isCasting}");

            GUILayout.Space(5);

            // ================= TARGETING =================
            GUILayout.Label($"TARGETING TYPE: {_ability.targetingType}");

            switch (_ability.targetingType)
            {
                case TargetingType.Point:
                    GUILayout.Label($"Aim Point: {_targetingData.aimPoint}");
                    GUILayout.Label($"Has Aim Point: {_targetingData.hasAimPoint}");
                    GUILayout.Label($"Direction: {_targetingData.direction}");
                    break;

                case TargetingType.Target:
                    GUILayout.Label($"Cast Target: {FormatTarget(_targetingData.castTarget)}");
                    GUILayout.Label($"In Range: {IsTargetInRangeDebug()}");
                    break;

                case TargetingType.Self:
                    GUILayout.Label($"Self Target: {_targetingData.castTarget}");
                    break;
            }

            GUILayout.Space(5);

            // ================= DIAGNOSTICS =================
            GUILayout.Label("DIAGNOSTICS");

            GUILayout.Label($"Context Valid: {IsContextValidDebug()}");
            GUILayout.Label($"Confirm Ready: {_user.CanConfirmCast(_ability, _targetingData)}");

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
            if (_targetingData.castTarget == null) return false;

            return Vector3.Distance(_user.transform.position, _targetingData.castTarget.transform.position)
                   <= _ability.castRange;
        }

        private bool IsContextValidDebug() //Debug ContextValidation
        {
            switch (_ability.targetingType)
            {
                case TargetingType.Point:
                    return _targetingData.hasAimPoint;

                case TargetingType.Target:
                    return _targetingData.castTarget != null;

                case TargetingType.Self:
                    return true;

                default:
                    return false;
            }
        }



        


    }
}