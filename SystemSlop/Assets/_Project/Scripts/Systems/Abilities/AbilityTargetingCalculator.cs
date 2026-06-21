using Project.Systems.Abilities.Data;
using UnityEngine;

public class AbilityTargetingCalculator
{
    private readonly AbilityTargetResolver _resolver;

    public AbilityTargetingCalculator(AbilityTargetResolver resolver)
    {
        _resolver = resolver;
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
                context.castTarget = _resolver.RaycastEnemy();
                break;
            case TargetingType.Self:
                context.castTarget = null;
                break;
        }
        return context;
    }
}
