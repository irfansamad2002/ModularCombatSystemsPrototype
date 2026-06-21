using Project.Core.Health;
using Project.Systems.Effects;
using System.Collections;
using UnityEngine;
//TODO: Refactor using Status effect System & Buff/Debuff handlers
[CreateAssetMenu(menuName = "Effects/DamageOverTime")]
public class DamageOverTimeEffect : EffectData
{
    public float tickRate;
    public int damagePerTick;
    public float duration;

    public override void Apply(GameObject target, AbilityTargetingData context, float multiplier = 1f)
    {
        var health = target.GetComponent<Health>();
        if (health != null)
        {
            target.GetComponent<MonoBehaviour>()
                .StartCoroutine(ApplyDamageOverTime(health));
            Debug.Log($"Applied damage over time effect to {target.name} for {duration} seconds, dealing {damagePerTick} damage every {tickRate} seconds.");
        }
    }

    private IEnumerator ApplyDamageOverTime(Health health)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            health.TakeDamage(damagePerTick);
            yield return new WaitForSeconds(tickRate);
            elapsed += tickRate;
        }
    }
}