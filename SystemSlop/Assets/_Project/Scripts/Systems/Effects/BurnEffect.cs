using System.Collections;
using UnityEngine;
//TODO: Refactor using Status effect System & Buff/Debuff handlers
[CreateAssetMenu(menuName = "Effects/Burn")]
public class BurnEffect : EffectData
{
    public float tickRate;
    public int damagePerTick;
    public float duration;

    public override void Apply(GameObject target)
    {
        var health = target.GetComponent<Health>();
        if (health != null)
        {
            target.GetComponent<MonoBehaviour>()
                .StartCoroutine(ApplyBurn(health));
            Debug.Log($"Applied burn effect to {target.name} for {duration} seconds, dealing {damagePerTick} damage every {tickRate} seconds.");
        }
    }

    private IEnumerator ApplyBurn(Health health)
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