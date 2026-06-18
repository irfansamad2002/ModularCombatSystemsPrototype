using Project.Core.Event;
using Project.Systems.Abilities.Data;
using UnityEngine;

namespace Project.Systems.VFX
{
    public class AbilityVFXManager : MonoBehaviour
    {
        [SerializeField] private GameObject fireballVFX;
        [SerializeField] private GameObject meteorVFX;

        private void OnEnable()
        {
            AbilityEvents.OnAbilityExecuted += HandleAbility;
        }

        private void OnDisable()
        {
            AbilityEvents.OnAbilityExecuted -= HandleAbility;
        }

        private void HandleAbility(AbilityData ability, ExecutionContext context)
        {

        }
    }
}
