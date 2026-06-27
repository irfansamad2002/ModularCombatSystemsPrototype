using Project.Systems.Abilities.Data;
using Project.Systems.Abilities.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Entities.Player
{
    public class PlayerAbilityController : MonoBehaviour
    {
        [SerializeField] private InputActionAsset inputActions;
        [SerializeField] private AbilityUser abilityUser;
        [SerializeField] private LayerMask worldLayer;
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private Camera cam;
        private InputAction[] _abilityActions;

        private InputAction _confirmCast;
        private InputAction _cancelCast;

        private AbilityCast _activeCast;

        private AbilityTargetResolver _targetResolver;

        private AbilityTargetingCalculator _targetingCalculator;
    
        private void Awake()
        {
            var map = inputActions.FindActionMap("Player");
            _abilityActions = new InputAction[]
            {
                map.FindAction("First Ability"),
                map.FindAction("Second Ability"),
                map.FindAction("Third Ability"),
                map.FindAction("Fourth Ability")
            };

            _confirmCast = map.FindAction("Confirm Cast");
            _cancelCast = map.FindAction("Cancel Cast");

            _targetResolver = new AbilityTargetResolver(cam, worldLayer, targetLayer);
            _targetingCalculator = new AbilityTargetingCalculator(_targetResolver, abilityUser.transform);
        }

        private void OnEnable()
        {
            foreach (InputAction action in _abilityActions)
            {
                action.Enable();
            }
            _cancelCast.Enable();
            _confirmCast.Enable();
        }

        private void OnDisable()
        {
            foreach (InputAction action in _abilityActions)
            {
                action.Disable();
            }
            _cancelCast.Disable();
            _confirmCast.Disable();
        }

        private void Update()
        {
            ReadAbilityInput();

            ReadCastInput();

            _activeCast?.Update();

        }

        private void ReadCastInput()
        {
            if (_activeCast == null)
            {
                return;
            }

            if (_confirmCast.WasPressedThisFrame())
            {
                ConfirmCurrentCast();
            }

            if (_cancelCast.WasPressedThisFrame())
            {
                CancelCurrentCast();
            }
        }
        
        private void CancelCurrentCast()
        {
            if (_activeCast == null) return;

            _activeCast.CancelCast();
            ClearCurrentCast();
        }

        private void ReadAbilityInput()
        {
            for (int i = 0; i < _abilityActions.Length; i++)
            {
                if (_abilityActions[i].WasPressedThisFrame())
                {
                    StartAbilityCast(i);
                }
            }
        }

        private void StartAbilityCast(int index)
        {
            var ability = abilityUser.GetAbility(index);

            switch (ability.castMode)
            {
                case CastMode.Instant:
                    ExecuteInstantAbility(ability);
                    break;
                case CastMode.Confirm:
                    BeginCast(ability);
                    break;
            }
        }

        private void ExecuteInstantAbility(AbilityData ability)
        {
            AbilityTargetingData context = _targetingCalculator.CalculateTargeting(ability);

            abilityUser.TryUseAbility(ability, context);
        }

        private void BeginCast(AbilityData ability)
        {
            if (_activeCast != null && _activeCast.IsActive)
            {
                if (!abilityUser.CanStartCast(ability))
                    return;

                _activeCast.CancelCast();
            }
                
            if (!abilityUser.CanStartCast(ability))
            {
                return;
            }

            _activeCast = new AbilityCast(abilityUser, ability, _targetingCalculator);
        } 

        private void ConfirmCurrentCast()
        {
            if (_activeCast == null)
            {
                return;
            }

            if (!_activeCast.IsActive)
            {
                ClearCurrentCast();

                return;
            }

            _activeCast.ConfirmCast();

            ClearCurrentCast();
       }

        private void ClearCurrentCast()
        {
            _activeCast = null;
        }

        private void OnGUI()
        {
            if (_activeCast != null && _activeCast.IsActive)
            {
                _activeCast.DrawDebug();
            }
        }

    }
}