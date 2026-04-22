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
                abilityUser.UseAbility(0, GetTarget());
            }
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
    }
}