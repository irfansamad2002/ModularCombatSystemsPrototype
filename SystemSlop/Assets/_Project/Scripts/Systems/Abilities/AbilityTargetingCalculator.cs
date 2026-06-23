using Project.Systems.Abilities.Data;
using UnityEngine;

public class AbilityTargetingCalculator
{
    private readonly AbilityTargetResolver _resolver;
    private readonly Transform _caster;

    public AbilityTargetingCalculator(AbilityTargetResolver resolver, Transform caster)
    {
        _resolver = resolver;
        _caster = caster;
    }

    public AbilityTargetingData CalculateTargeting(AbilityData ability)
    {
        var context = new AbilityTargetingData();

        context.direction = _resolver.GetAimDirection();
                
        switch (ability.targetingType)
        {
            case TargetingType.Point:
                if (_resolver.TryGetAimPoint(out var point))
                {
                    context.aimPoint = point;
                    context.hasAimPoint = true;
                }
                break;
            case TargetingType.Target:
                var target = _resolver.RaycastEnemy();

                if (target != null &&
                    IsTargetInRange(target, ability))
                {

                    context.castTarget = _resolver.RaycastEnemy();
                }
                
                break;
            case TargetingType.Self:
                context.castTarget = null;
                break;
        }
        return context;
    }

    private bool IsTargetInRange(GameObject target, AbilityData ability)
    {
        return Vector3.Distance(_caster.position, target.transform.position) <= ability.castRange;
    }
}
