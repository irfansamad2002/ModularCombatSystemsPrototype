using Project.Systems.Abilities.Data;
using Project.Systems.VFX;
using UnityEngine;
namespace Project.Systems.Abilities.Runtime
{
    public class AbilityCast
    {
        private readonly AbilityData _ability;
        private readonly AbilityUser _user;
        private readonly AbilityTargetingCalculator _targetingCalculator;

        private AbilityTargetingData _targetingData;
        private AbilityTargetingIndicator _indicator;
        private AbilityTargetAdjuster _targetAdjuster;

        private bool _isCasting;

        public bool IsActive => _isCasting;
        public AbilityData Ability => _ability;

        public AbilityCast(
            AbilityUser user,
            AbilityData ability,
            AbilityTargetingCalculator calculator)
        {
            _user = user;
            _ability = ability;
            _targetingCalculator = calculator;

            _targetingData = _targetingCalculator.CalculateTargeting(_ability);
            _targetAdjuster = new AbilityTargetAdjuster();

            CreateIndicator();

            _isCasting = true;
        }

        public void Update()
        {
            if (!IsActive) 
                return;

            _targetingData = _targetingCalculator.CalculateTargeting(_ability);

            _targetAdjuster.Adjust(_user.Firepoint, _ability, ref _targetingData);

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


        private void CleanUp()
        {
            _indicator?.DestroySelf();
            _isCasting = false;
        }

        private void UpdateIndicator()
        {
            if (_indicator == null)
                return;

            switch (_ability.targetingSettings.targetingType)
            {
                case TargetingType.Point:
                    _indicator.SetPosition(_targetingData.targetPoint);
                    _indicator.SetValid(_targetingData.hasTargetPoint);
                    break;

                case TargetingType.Target:
                    if (_targetingData.target != null)
                    {
                        _indicator.SetPosition(_targetingData.target.transform.position);
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
            if (_ability.targetingSettings.indicatorPrefab == null)
                return;

            var obj = Object.Instantiate(_ability.targetingSettings.indicatorPrefab);

            _indicator = obj.GetComponent<AbilityTargetingIndicator>();

            _indicator?.Initialize(_ability.impactSettings.radius);
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
            GUILayout.Label($"TARGETING TYPE: {_ability.targetingSettings.targetingType}");

            switch (_ability.targetingSettings.targetingType)
            {
                case TargetingType.Point:
                    GUILayout.Label($"Aim Point: {_targetingData.targetPoint}");
                    GUILayout.Label($"Has Aim Point: {_targetingData.hasTargetPoint}");
                    GUILayout.Label($"Direction: {_targetingData.direction}");
                    break;

                case TargetingType.Target:
                    GUILayout.Label($"Cast Target: {FormatTarget(_targetingData.target)}");
                    GUILayout.Label($"In Range: {IsTargetInRangeDebug()}");
                    break;

                case TargetingType.Self:
                    GUILayout.Label($"Self Target: {_targetingData.target}");
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
            if (_targetingData.target == null) return false;

            return Vector3.Distance(_user.transform.position, _targetingData.target.transform.position)
                   <= _ability.targetingSettings.castRange;
        }

        private bool IsContextValidDebug() //Debug ContextValidation
        {
            switch (_ability.targetingSettings.targetingType)
            {
                case TargetingType.Point:
                    return _targetingData.hasTargetPoint;

                case TargetingType.Target:
                    return _targetingData.target != null;

                case TargetingType.Self:
                    return true;

                default:
                    return false;
            }
        }



        


    }
}