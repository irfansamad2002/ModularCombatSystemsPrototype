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
        var targetingData = new AbilityTargetingData();

        targetingData.direction = _resolver.GetAimDirection();
                
        switch (ability.targetingType)
        {
            case TargetingType.Point:
                if (_resolver.TryGetAimPoint(out var point))
                {
                    targetingData.targetPoint = point;
                    targetingData.hasTargetPoint = true;
                }
                break;

            case TargetingType.Target:
                targetingData.target = _resolver.RaycastEnemy();
                break;

            case TargetingType.Self:
                targetingData.target = null;
                break;

        }
        return targetingData;
    }
}
