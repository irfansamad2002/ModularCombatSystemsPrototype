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
                +_currentCast.ability.name 
                + " CD:" 
                + _currentCast.ability.cooldown 
                + " castRange:" 
                + _currentCast.ability.castRange
                + " targetingType: " 
                + _currentCast.ability.targetingType
                + " deliveryType: " 
                + _currentCast.ability.deliveryType 
                + " Num of effects: " 
                + _currentCast.ability.effects.Count
                );
            
            
            
        }

        private void HandleAbilityReleased(int index)
        {
            
            if (_currentCast == null) return;

            if (_currentCast.index != index) return;

            if (_currentCast.indicator != null)
            {
                if (!TryGetGroundPoint(out _))
                {
                    CancelCast();
                    return;
                }
            }
            ConfirmCast();
        }

        private void HandleAbilityPressed(int index)
        {
            if (newAbilityCancelsOldOne)
                if (_currentCast.ability != null)//Cancel previous
                    CancelCast();
            else
                if (_currentCast.ability != null)//Block input
                    return;

            var ability = abilityUser.GetAbility(index);

            _currentCast = new CastSession()
            {
                ability = ability,
                index = index,
                context = new AbilityContext()
            };

            //DebugAbilitiesInfos(index);

            switch (ability.targetingType)
            {
                case TargetingType.None:
                    ConfirmCast();
                    break;
                case TargetingType.Point:
                    StartIndicator();
                    break;
                case TargetingType.Target:
                    _currentCast.context.target = GetTarget();
                    ConfirmCast();
                    break;
                case TargetingType.Self:
                    _currentCast.context.target = gameObject;
                    ConfirmCast();
                    break;
                default:
                    Debug.LogWarning("HandleAbilityPressed switch ability.targetingType went into default for some reason");
                    break;
            }
        }

        private void StartIndicator()
        {
            if (_currentCast.ability.indicatorPrefab == null) return;

            var obj = Instantiate(_currentCast.ability.indicatorPrefab);
            _currentCast.indicator= obj.GetComponent<AOEIndicator>();

            _currentCast.indicator.Init(_currentCast.ability.projectile.explosionRadius);
        }
        
        private void ConfirmCast()
        {
            if (_currentCast == null) return;

            abilityUser.UseAbility(_currentCast.index, _currentCast.context);

            if (_currentCast.indicator != null)
            {
                Destroy(_currentCast.indicator.gameObject);
            }

            _currentCast = null;
           
        }

        private bool TryGetGroundPoint(out Vector3 point)
        {
            //On Mouse Cursor
            //if (Mouse.current == null)
            //{
            //    point = Vector3.zero;
            //    return false;
            //}

            //Vector2 mousePos = Mouse.current.position.ReadValue();

            //Ray ray = cam.ScreenPointToRay(mousePos);

            //On Center Screen
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            if (Physics.Raycast(ray, out RaycastHit hit, _currentCast.ability.castRange, pointLayer))
            {
                point = hit.point;
                return true;
            }

            point = Vector3.zero;
            return false;

        }

        private void UpdateIndicator()
        {
            if (_currentCast == null) return;
            if (_currentCast.indicator == null) return;

            if (TryGetGroundPoint(out Vector3 point))
            {
                Vector3 origin = abilityUser.transform.position;
                float range = _currentCast.ability.castRange;


                Vector3 dir = point - origin;
                float dist = dir.magnitude;
                if (dist > range)
                {
                    dir = dir.normalized * range;
                    point = origin + dir;
                }
                bool isValid = dist <= range;

                _currentCast.indicator.SetValid(isValid);
                _currentCast.indicator.SetPosition(point);

                _currentCast.context.point = point;

            }
            else
            {
                _currentCast.indicator.gameObject.SetActive(false);
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
            if (_currentCast == null) return;

            if (_currentCast.indicator != null)
            {
                Destroy(_currentCast.indicator.gameObject);
            }

            _currentCast = null;
        }

        private GameObject GetTarget()
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            if (Physics.Raycast(ray, out RaycastHit hit, 100f))//TODO: Set a range for the raycast hit 
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    return hit.collider.gameObject;
                }
            }

            return null;                
        }

        private class CastSession
        {
            public AbilityData ability;
            public int index;
            public AbilityContext context;

            public AOEIndicator indicator;
        }
    }

}