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
            if (_ability1.WasPressedThisFrame())
            {
                StartIndicator(0);

            }

            if (_ability1.IsPressed())
            {
                UpdateIndicator();
            }

            if (_ability1.WasReleasedThisFrame())
            {
                if (TryGetGroundPoint(out var point))
                {
                    ConfirmCast();
                }
                else
                {
                    CancelCast();
                }
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

            if (!TryGetGroundPoint(out Vector3 point))
            {
                CancelCast();
                return;
            }

            abilityUser.UseAbilityAtPoint(_currentAbilityIndex, point);

            Destroy(_currentIndicator.gameObject);
            _currentIndicator = null;
            _currentAbilityIndex = -1;
        }

        private bool TryGetGroundPoint(out Vector3 point)
        {
            //Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            //if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
            //{
            //    point = hit.point;
            //    return true;
            //}

            //point = Vector3.zero;
            //return false;

            if (Mouse.current == null)
            {
                point = Vector3.zero;
                return false;
            }

            Vector2 mousePos = Mouse.current.position.ReadValue();

            Ray ray = cam.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
            {
                point = hit.point;
                return true;
            }

            point = Vector3.zero;
            return false;

        }

        private void UpdateIndicator()
        {
            if (_currentIndicator == null) return;

            if (TryGetGroundPoint(out Vector3 point))
            {
                Vector3 origin = abilityUser.transform.position;

                float range = abilityUser.GetAbility(_currentAbilityIndex).castRange;

                Vector3 dir = point - origin;
                float dist = dir.magnitude;
                if (dist > range) {
                    dir = dir.normalized * range;
                    point = origin + dir;

                    
                }
                bool isValid = dist <= range;
                _currentIndicator.SetValid(isValid);
                _currentIndicator.SetPosition(point);

                if (!_currentIndicator.gameObject.activeSelf)
                    _currentIndicator.gameObject.SetActive(true);

            }
            else
            {
                if (_currentIndicator.gameObject.activeSelf)
                    _currentIndicator.gameObject.SetActive(false);
            }

        }

        private void OnDrawGizmos()
        {
            if(cam == null || Mouse.current == null) return;


            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray ray = cam.ScreenPointToRay(mousePos);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(ray.origin, ray.direction * 50f);
        }

        private void CancelCast()
        {
            if (_currentIndicator != null)
            {
                Destroy(_currentIndicator.gameObject);
                _currentIndicator = null;
                _currentAbilityIndex = -1;
            }
        }
    }
}