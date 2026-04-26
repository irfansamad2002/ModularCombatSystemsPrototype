using Project.Systems.Ability;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Entities.Player
{
    public class PlayerAbilityInput : MonoBehaviour
    {
        [SerializeField] private InputActionAsset inputActions;
        [SerializeField] private AbilityUser abilityUser;

        private InputAction _ability1;
        private InputAction _ability2;
        private InputAction _ability3;
        private InputAction _ability4;

        [SerializeField] private GameObject aoeIndicatorPrefab;

        private AOEIndicator _currentIndicator;
        private int _currentAbilityIndex = -1;

        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Camera cam;

        private void Awake()
        {
            var map = inputActions.FindActionMap("Player");

            _ability1 = map.FindAction("First Ability");
            _ability2 = map.FindAction("Second Ability");
            _ability3 = map.FindAction("Third Ability");
            _ability4 = map.FindAction("Fourth Ability");
        }

        private void OnEnable()
        {
            _ability1.Enable();
            _ability2.Enable();
            _ability3.Enable();
            _ability4.Enable();
        }

        private void OnDisable()
        {
            _ability1.Disable();
            _ability2.Disable();
            _ability3.Disable();
            _ability4.Disable();
        }

        private void Update()
        {
            if (_ability1.IsPressed())
            {
                if (_currentIndicator == null)
                    StartIndicator(0);
            }

            if (_ability1.WasReleasedThisFrame())
            {
                ConfirmCast();
            }


            //if (_ability1.WasPressedThisFrame())
            //{
            //    abilityUser.UseAbility(0, GetTarget());
            //}
            if (_ability2.WasPressedThisFrame())
            {
                abilityUser.UseAbility(1, GetTarget());
            }
            if (_ability3.WasPressedThisFrame())
            {
                abilityUser.UseAbility(2, GetTarget());
            }
            if (_ability4.WasPressedThisFrame())
            {
                abilityUser.UseAbility(3, GetTarget());
            }

            if (_currentIndicator != null)
            {
                if (TryGetGroundPoint(out Vector3 point))
                {
                    _currentIndicator.SetPosition(point);
                }
            }
        }

        private GameObject GetTarget()
        {
            return GameObject.FindWithTag("Enemy"); // Temp
        }

        private void StartIndicator(int index)
        {
            var ability = abilityUser.GetAbility(index);

            if (ability.projectile == null) return;

            _currentAbilityIndex = index;

            var obj = Instantiate(aoeIndicatorPrefab);
            _currentIndicator = obj.GetComponent<AOEIndicator>();

            _currentIndicator.Init(ability.projectile.explosionRadius);
        }
        
        private void ConfirmCast()
        {
            if (_currentIndicator == null) return;

            Vector3 targetPoint = _currentIndicator.GetCurrentPosition();

            abilityUser.UseAbilityAtPoint(_currentAbilityIndex, targetPoint);

            Destroy(_currentIndicator.gameObject);
            _currentIndicator = null;
            _currentAbilityIndex = -1;
        }

        private bool TryGetGroundPoint(out Vector3 point)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
            {
                point = hit.point;
                return true;
            }

            point = Vector3.zero;
            return false;

        }

        
    }
}