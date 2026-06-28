using Project.Systems.Abilities.Data;
using System.Collections.Generic;
using UnityEngine;

public class AbilityImpactExecutor
{
    public void ExecuteTarget(GameObject target, AbilityData ability, AbilityTargetingData targetingData)
    {
        if (target == null)
            return;

        foreach (var effect in ability.effects)
        {
            effect.Apply(target, targetingData);
        }
    }

    public void ExecuteTargets(IEnumerable<GameObject> targets, AbilityData ability, AbilityTargetingData targetingData)
    {
        foreach (var target in targets)
        {
            ExecuteTarget(target, ability, targetingData);
        }
    }
}
