using Project.Systems.Abilities.Data;
using Project.Systems.Abilities.Runtime;
using Project.Systems.Combat.Query;
using UnityEngine;

public class InstantDelivery
{
    public void Execute(AbilityUser user, AbilityData ability, AbilityTargetingData targetingData)
    {
        switch (ability.areaShape)
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

        foreach (var effect in ability.effects)
        {
            effect.Apply(target, targetingData);
        }
    }

    private GameObject ResolveTarget(AbilityUser user, AbilityData ability, AbilityTargetingData targetingData)
    {
        switch (ability.targetingType)
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

        var targets = AreaQuery.GetTargetsSphere(center, ability.radius, user.TargetLayer, user.transform);

        foreach (var target in targets)
        {
            foreach (var effect in ability.effects)
            {
                effect.Apply(target, targetingData);
            }
        }
    }

    private void ExecuteConeArea(AbilityUser user, AbilityData ability, AbilityTargetingData context)
    {
        Vector3 origin = user.transform.position;

        var sphereTargets = AreaQuery.GetTargetsSphere(
            origin,
            ability.radius,
            user.TargetLayer,
            user.transform);

        var coneTargets = AreaQuery.FilterCone(
            sphereTargets,
            origin,
            context.direction,
            ability.coneAngle);


        foreach (var target in coneTargets)
        {
            foreach (var effect in ability.effects)
            {
                effect.Apply(target, context);
            }
        }
    }

}
