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
                    break;

                case TargetingType.Point:
                    if (TryGetAimPoint(out Vector3 point))
                    {
                        context.point = point;
                        context.hasPoint = true;
                    }
                        break;

                case TargetingType.Target:
                    context.target = RaycastEnemy();
                    break;
            }

            return context;
        }

        private bool TryGetAimPoint(out Vector3 point)
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(.5f, .5f, 0f));

            int mask = worldLayer | targetLayer;

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, mask))
            {
                point = hit.point;
                return true;
            }

            point = ray.origin + ray.direction * 100f;
            return true;
        }



        private GameObject RaycastEnemy()
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(.5f, .5f, 0f));

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, targetLayer))
            {
                return hit.collider.gameObject;
            }

            return null;
        }

        private void StartConfirmCast(AbilityData ability)
        {
            if (_currentCast != null && _currentCast.IsActive)
            {
                _currentCast.Cancel();
            }

            _currentCast = new CastSession(abilityUser, ability, cam, worldLayer, targetLayer);
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