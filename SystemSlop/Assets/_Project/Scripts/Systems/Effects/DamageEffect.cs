using Project.Core.Health;
using Project.Systems.Effects;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Damage")]
public class DamageEffect : EffectData
{
    public float damage;    

    public override void Apply(GameObject target,AbilityContext context, float multiplier = 1f)
    {
        var health = target.GetComponent<Health>();
        if (health == null)
        {
            DebugHelper.WarnMissingComponent(target, nameof(Health));
            return;
        }

        float finalDamage = damage * multiplier;
        health.TakeDamage(finalDamage);
        Debug.Log($"Applied {finalDamage} damage to {target.name}");

    }
}
