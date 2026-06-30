using Project.Systems.Abilities.Data;
using Project.Systems.Abilities.Runtime;
using Project.Systems.Combat.Query;
using UnityEngine;

public class InstantDelivery
{
    private readonly AbilityImpactExecutor _impactExecutor;

    public InstantDelivery(AbilityImpactExecutor impactExecutor)
    {
        _impactExecutor = impactExecutor;
    }

    public void Execute(AbilityUser user, AbilityData ability, AbilityTargetingData targetingData)
    {
        switch (ability.impactSettings.areaShape)
        {
            case AreaShape.None:
                ExecuteSingleTargetInstant(user, ability, targetingData);
                break;
            case AreaShape.Sphere:
                ExecuteSphereArea(user, ability, targetingData);
                break;
            case AreaShape.Cone:
                ExecuteConeArea(user, ability, targetingData);
                break;
            default:
                break;
        }
    }

    private void ExecuteSingleTargetInstant(AbilityUser user,
    AbilityData ability,
    AbilityTargetingData targetingData)
    {
        GameObject target = ResolveTarget(user, ability, targetingData);

        if (target == null)
        {
            return;
        }

        _impactExecutor.ExecuteTarget(target,ability,targetingData);
    }
    private GameObject ResolveTarget(AbilityUser user, AbilityData ability, AbilityTargetingData targetingData)
    {
        switch (ability.targetingSettings.targetingType)
        {
            case TargetingType.Target:
                return targetingData.target;

            case TargetingType.Self:
                return user.gameObject;

            case TargetingType.None:
            case TargetingType.Point:
            default:
                return null;
        }
    }


    private void ExecuteSphereArea(AbilityUser user, AbilityData ability, AbilityTargetingData targetingData)
    {
        Vector3 center = user.GetTargetPosition(targetingData);

        var targets = AreaQuery.GetTargetsSphere(center, ability.impactSettings.radius, user.TargetLayer, user.transform);

        _impactExecutor.ExecuteTargets(targets,ability,targetingData);
    }

    private void ExecuteConeArea(AbilityUser user, AbilityData ability, AbilityTargetingData targetingData)
    {
        Vector3 origin = user.transform.position;

        var sphereTargets = AreaQuery.GetTargetsSphere(
            origin,
            ability.impactSettings.radius,
            user.TargetLayer,
            user.transform);

        var coneTargets = AreaQuery.FilterCone(
            sphereTargets,
            origin,
            targetingData.direction,
            ability.impactSettings.coneAngle);

        _impactExecutor.ExecuteTargets(coneTargets, ability, targetingData);
    }

}
