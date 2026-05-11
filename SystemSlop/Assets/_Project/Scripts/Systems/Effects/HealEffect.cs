using Project.Core.Health;
using Project.Systems.Effects;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Heal")]
public class HealEffect : EffectData
{
    public float healAmount;

    public override void Apply(GameObject target,AbilityContext context, float multiplier = 1)
    {
        var health = target.GetComponent<Health>();
        if (health == null)
        {
            DebugHelper.WarnMissingComponent(target, nameof(Health));
            return;
        }

        health.HealsUp(healAmount * multiplier);

    }
}
