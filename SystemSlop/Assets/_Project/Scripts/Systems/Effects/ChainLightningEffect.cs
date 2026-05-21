using Project.Core.Health;
using Project.Systems.Effects;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Chain Lightning")]
public class ChainLightningEffect : EffectData
{
    public int maxChains = 5;
    public float chainRadius = 5f;
    public float damageFalloff = .8f;
    public LayerMask targetLayer;
    public int damage = 10;


    public override void Apply(GameObject target, ExecutionContext context, float multiplier = 1f)
    {
        HashSet<GameObject> hitTargets = new();

        Chain(target, multiplier, hitTargets, 0);
    }

    private void Chain(
        GameObject currentTarget,
        float multiplier,
        HashSet<GameObject> hitTargets,
        int depth)
    {
        if(currentTarget == null) return;

        if (depth >= maxChains) return;

        hitTargets.Add(currentTarget);

        //Apply damage
        var health = currentTarget.GetComponent<Health>();

        if (health != null)
        {
            health.TakeDamage(damage * multiplier);
        }

        //Find nearby candidates
        var nearby = AreaQuery.GetTargetsSphere(
            currentTarget.transform.position,
            chainRadius,
            targetLayer,
            null);

        //Debug.Log($"Nearby Count: {nearby.Count}");

        GameObject nextTarget = null;
        float closestDistance = float.MaxValue;

        foreach (var candidate in nearby)
        {
            if (hitTargets.Contains(candidate)) continue;

            float dist = Vector3.Distance(
                currentTarget.transform.position, candidate.transform.position );

            if (dist < closestDistance)
            {
                closestDistance = dist;
                nextTarget = candidate;
            }

        }

        if(nextTarget == null) return;

        Chain(nextTarget, multiplier * damageFalloff, hitTargets, depth + 1);
    }
}
