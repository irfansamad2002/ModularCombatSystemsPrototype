using System.Collections.Generic;
using UnityEngine;

public class OnTriggerCausesDamage : MonoBehaviour
{
    [SerializeField] private List<EffectData> effects;
    [SerializeField] private AbilityUser abilityUser;

    private void OnTriggerEnter(Collider other)
    {
        if(effects == null || effects.Count == 0) return;

        var health = other.GetComponent<Health>();
        if (health == null) return;
        
        //foreach (var effect in effects)
        //{
        //    if (effect == null)
        //    {
        //        Debug.Log("effect at Element " + effects.IndexOf(effect) + " is null, skipping");
        //        continue;
        //    }
        //    effect.Apply(other.gameObject);
        //}

        if (abilityUser != null)
        {
            abilityUser.UseAbility(0, other.gameObject);
        }

    }
}
