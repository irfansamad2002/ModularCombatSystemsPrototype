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

        //private InputAction _firstAbility;
        //private InputAction _secondAbility;
        //private InputAction _thirdAbility;
        //private InputAction _fourthAbility;

        private InputAction[] _abilityActions;

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
        }

        private void OnEnable()
        {
            foreach (InputAction action in _abilityActions)
            {
                action.Enable();
            }
        }

        private void OnDisable()
        {
            foreach (InputAction action in _abilityActions)
            {
                action.Disable();
            }
        }

        private void Update()
        {
            HandleAbilityInputs();

            _currentCast?.Update();
          
        }

        private void HandleAbilityInputs()
        {
            for (int i = 0; i < _abilityActions.Length; i++)
            {
                if (_abilityActions[i].WasPressedThisFrame())
                {
                    StartAbilityCast(i);
                }

                if (_abilityActions[i].WasReleasedThisFrame())
                {
                    ConfirmAbilityCast();
                }
            }
        }


        private void StartAbilityCast(int index)
        {
            if (_currentCast != null && _currentCast.IsActive)
            {
                _currentCast.Cancel();
            }
            var ability = abilityUser.GetAbility(index);

            _currentCast = new CastSession(abilityUser, ability, cam, worldLayer, targetLayer);
        }

       private void ConfirmAbilityCast()
        {
            if (_currentCast == null)
            {
                return;
            }

            if (!_currentCast.IsActive)
            {
                _currentCast = null;
                return;
            }

            _currentCast.Confirm();

            _currentCast = null;
        }

    }

}