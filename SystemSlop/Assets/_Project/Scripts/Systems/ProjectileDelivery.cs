using Project.Systems.Abilities.Data;
using Project.Systems.Abilities.Runtime;
using Project.Systems.Combat;
using UnityEngine;

public class ProjectileDelivery
{
    private readonly AbilityImpactExecutor _executor;

    public ProjectileDelivery(AbilityImpactExecutor executor)
    {
        _executor = executor;
    }

    public void Execute(Transform firePoint, Vector3 destination, AbilityData ability)
    {

        Vector3 dir = (destination - firePoint.position).normalized;

        var projectileGO = Object.Instantiate(ability.projectile.prefab,
            firePoint.position,
            Quaternion.LookRotation(dir));

        var projectile = projectileGO.GetComponent<Projectile>();

        projectile.Init(ability.effects,
            ability.projectile.speed,
            ability.radius,
            ability.targetLayers,
            ability.impactVFX,
            ability.minDistanceThreshold,
            ability.minFalloff
            );

        Object.Destroy(projectileGO, ability.projectile.lifetime);
    }

    
}
