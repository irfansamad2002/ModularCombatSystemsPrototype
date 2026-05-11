using Project.Systems.Abilities;
using Project.Systems.Ability;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Entities.Player
{
    public class PlayerAbilityInput : MonoBehaviour
    {
        [SerializeField] private InputActionAsset inputActions;
        [SerializeField] private AbilityUser abilityUser;
        [SerializeField] private LayerMask worldLayer;
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private Camera cam;
        private InputAction[] _abilityActions;

        private InputAction _confirmCast;
        private InputAction _cancelCast;

        private CastSession _currentCast;

        private AbilityTargetResolver _targetResolver;

        public bool newAbilityCancelsOldOne;
    


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
            HandleAbilityInputs();

            HandleCastControls();

            _currentCast?.Update();


          
        }

        private void HandleCastControls()
        {
            if (_currentCast == null)
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
            if (_currentCast == null) return;

            _currentCast.Cancel();
            ClearCurrentCast();
        }



        private void HandleAbilityInputs()
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
                    CastInstantAbility(ability);
                    break;
                case CastMode.Confirm:
                    StartConfirmCast(ability);
                    break;
            }

           
        }

        private void CastInstantAbility(AbilityData ability)
        {
            AbilityContext context = BuildInstantContext(ability);

            abilityUser.UseAbility(ability, context);
        }

        private AbilityContext BuildInstantContext(AbilityData ability)
        {
            AbilityContext context = new AbilityContext();

            switch (ability.targetingType)
            {
                case TargetingType.None:
                case TargetingType.Self:
                    context.target = abilityUser.gameObject;
                    context.direction = _targetResolver.GetAimDirection();
                    break;

                case TargetingType.Point:
                    if (_targetResolver.TryGetAimPoint(out Vector3 point))
                    {
                        context.point = point;
                        context.hasPoint = true;
                        Vector3 origin = abilityUser.Firepoint.position;

                        context.direction = (point - origin).normalized;
                    }
                        break;

                case TargetingType.Target:
                    context.target = _targetResolver.RaycastEnemy();
                    if (context.target != null)
                    {
                        Vector3 dir = context.target.transform.position - abilityUser.transform.position;

                        context.direction = dir.normalized;
                    }
                    break;
            }

            return context;
        }

       

        private void StartConfirmCast(AbilityData ability)
        {
            if (_currentCast != null && _currentCast.IsActive)
            {
                _currentCast.Cancel();
            }

            _currentCast = new CastSession(abilityUser, ability, _targetResolver);
        } 

       private void ConfirmCurrentCast()
        {
            if (_currentCast == null)
            {
                return;
            }

            if (!_currentCast.IsActive)
            {
                ClearCurrentCast();

                return;
            }

            _currentCast.Confirm();

            ClearCurrentCast();
       }

        private void ClearCurrentCast()
        {
            _currentCast = null;
        }

        private void OnGUI()
        {
            if (_currentCast != null && _currentCast.IsActive)
            {
                _currentCast.DrawDebug();
            }
        }

    }
}