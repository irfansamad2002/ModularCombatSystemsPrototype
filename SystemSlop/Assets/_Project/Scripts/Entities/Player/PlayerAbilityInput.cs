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
        [SerializeField] private LayerMask pointLayer;
        [SerializeField] private Camera cam;

        private InputAction _firstAbility;
        private InputAction _secondAbility;
        private InputAction _thirdAbility;
        private InputAction _fourthAbility;

        //private AOEIndicator _currentIndicator;
        //private AbilityData _currentAbility;
        //private int _currentIndex;

        private CastSession _currentCast;

        public bool newAbilityCancelsOldOne;

        private void Awake()
        {
            var map = inputActions.FindActionMap("Player");

            _firstAbility = map.FindAction("First Ability");
            _secondAbility = map.FindAction("Second Ability");
            _thirdAbility = map.FindAction("Third Ability");
            _fourthAbility = map.FindAction("Fourth Ability");
        }

        private void OnEnable()
        {
            _firstAbility.Enable();
            _secondAbility.Enable();
            _thirdAbility.Enable();
            _fourthAbility.Enable();
        }

        private void OnDisable()
        {
            _firstAbility.Disable();
            _secondAbility.Disable();
            _thirdAbility.Disable();
            _fourthAbility.Disable();
        }

        private void Update()
        {
            if (_firstAbility.WasPressedThisFrame())
            {
                HandleAbilityPressed(0);
            }

            if (_firstAbility.IsPressed())
            {
                _currentCast?.Update(cam, pointLayer);
            }

            if (_firstAbility.WasReleasedThisFrame())
            {
                _currentCast?.Confirm();
            }

            if (_secondAbility.WasPressedThisFrame())
            {
                HandleAbilityPressed(1);
            }

            if (_secondAbility.IsPressed())
            {
                _currentCast?.Update(cam, pointLayer);
            }

            if (_secondAbility.WasReleasedThisFrame())
            {
                _currentCast?.Confirm();
            }

            if (_thirdAbility.WasPressedThisFrame())
            {
                HandleAbilityPressed(2);
            }

            if (_thirdAbility.IsPressed())
            {
                _currentCast?.Update(cam, pointLayer);
            }

            if (_thirdAbility.WasReleasedThisFrame())
            {
                _currentCast?.Confirm();
            }

          
        }



        private void HandleAbilityPressed(int index)
        {
            if (_currentCast != null && _currentCast.IsActive)
            {
                _currentCast.Cancel();
            }
            var ability = abilityUser.GetAbility(index);

            _currentCast = new CastSession(abilityUser, ability);
        }

        private void OnDrawGizmos()
        {
            if(cam == null || Mouse.current == null) return;


            //Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));


            Gizmos.color = Color.red;
            //if(_currentCast != null)
                Gizmos.DrawRay(ray.origin, ray.direction * 10f);
        }

    }

}