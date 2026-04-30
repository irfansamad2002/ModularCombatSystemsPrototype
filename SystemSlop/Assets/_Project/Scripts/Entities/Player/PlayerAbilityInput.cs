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

        private AOEIndicator _currentIndicator;
        private AbilityData _currentAbility;

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
                UpdateIndicator();
            }

            if (_firstAbility.WasReleasedThisFrame())
            {
                HandleAbilityReleased(0);
                //DebugAbilitiesInfos(0);


            }


            if (_secondAbility.WasPressedThisFrame())
            {
                //abilityUser.UseAbility(1, GetTarget());
            }
            if (_thirdAbility.WasPressedThisFrame())
            {
                //abilityUser.UseAbility(2, GetTarget());
            }
            if (_fourthAbility.WasPressedThisFrame())
            {
                //abilityUser.UseAbility(3, GetTarget());
            }

        }

        private void DebugAbilitiesInfos(int index)
        {


            //AbilityData ability = abilityUser.GetAbility(_currentAbility);
            Debug.Log("name: " 
                +_currentAbility.name 
                + " CD:" 
                + _currentAbility.cooldown 
                + " castRange:" 
                + _currentAbility.castRange
                + " targetingType: " 
                + _currentAbility.targetingType
                + " deliveryType: " 
                + _currentAbility.deliveryType 
                + " Num of effects: " 
                + _currentAbility.effects.Count
                );
            
            
            
        }

        private void HandleAbilityReleased(int index)
        {
            
            if (_currentIndicator == null) return;

            var ability = abilityUser.GetAbility(index);

            if (_currentAbility != ability) return;

            if (TryGetGroundPoint(out var point))
            {
                ConfirmCast(index);
            }
            else
            {
                CancelCast();
            }
        }

        private void HandleAbilityPressed(int index)
        {
            if (newAbilityCancelsOldOne)
                if (_currentAbility != null)//Cancel previous
                    CancelCast();
            else
                if (_currentAbility != null)//Block input
                    return;
            

            _currentAbility = abilityUser.GetAbility(index);
            DebugAbilitiesInfos(index);

            switch (_currentAbility.targetingType)
            {
                case TargetingType.None:
                    //abilityUser.UseAbility(index, null);
                    Debug.Log("No need any target for ability");
                    break;
                case TargetingType.Point:
                    StartIndicator(index);
                    Debug.Log("somewhere in the world will be the target for ability");
                    break;
                case TargetingType.Target:
                    //abilityUser.UseAbility(index, GetTarget());
                    Debug.Log("needa  target for ability");
                    break;
                case TargetingType.Self:
                    //abilityUser.UseAbility(index, gameObject);
                    Debug.Log("cast on self for ability");
                    break;
                default:
                    Debug.LogWarning("TargetingType prolly not set");
                    break;
            }

            //if (_currentAbility.requiresIndicator)
            //    {
            //        StartIndicator(index);
            //    }
            //else
            //{
            //    abilityUser.UseAbility(index, GetTarget());
            //    _currentAbility = null;
            //}


        }

        private GameObject GetTarget()
        {
            return GameObject.FindWithTag("Enemy"); // Temp
        }

        private void StartIndicator(int index)
        {
            //if (_currentAbility.projectile == null) return;

            
            var obj = Instantiate(_currentAbility.indicatorPrefab);
            _currentIndicator = obj.GetComponent<AOEIndicator>();

            _currentIndicator.Init(_currentAbility.projectile.explosionRadius);
        }
        
        private void ConfirmCast(int index)
        {
            var context = new AbilityContext();

            if (_currentIndicator == null) return;

            if (!TryGetGroundPoint(out Vector3 point))
            {
                CancelCast();
                return;
            }

            context.point = point;

            //abilityUser.useab
            abilityUser.UseAbility(index, context);

            Destroy(_currentIndicator.gameObject);
            _currentIndicator = null;
        }

        private bool TryGetGroundPoint(out Vector3 point)
        {
            //On Center Screen
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            if (Physics.Raycast(ray, out RaycastHit hit, _currentAbility.castRange, pointLayer))
            {
                point = hit.point;
                return true;
            }

            point = Vector3.zero;
            return false;


            //On Mouse Cursor
            //if (Mouse.current == null)
            //{
            //    point = Vector3.zero;
            //    return false;
            //}

            //Vector2 mousePos = Mouse.current.position.ReadValue();

            //Ray ray = cam.ScreenPointToRay(mousePos);

            //if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
            //{
            //    point = hit.point;
            //    return true;
            //}

            //point = Vector3.zero;
            //return false;

        }

        private void UpdateIndicator()
        {
            if (_currentIndicator == null) return;

            if (TryGetGroundPoint(out Vector3 point))
            {
                Vector3 origin = abilityUser.transform.position;

                float range = _currentAbility.castRange;

                Vector3 dir = point - origin;
                float dist = dir.magnitude;
                if (dist > range)
                {
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
            }
        }
    }
}