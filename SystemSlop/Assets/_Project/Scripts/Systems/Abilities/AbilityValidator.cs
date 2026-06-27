using Project.Systems.Abilities.Data;
using Project.Systems.Abilities.Runtime;
using UnityEngine;

public class AbilityValidator
{
    public bool CanStartCast(AbilityUser user, AbilityData ability)
    {
        return ValidateCooldown(user, ability);
    }

    public bool CanConfirmCast(AbilityData ability, AbilityTargetingData targetingData)
    {
        return ValidateTargeting(ability, targetingData);
    }


    public bool CanUse(AbilityUser user, AbilityData ability, AbilityTargetingData targetingData)
    {
        return ValidateCooldown(user,ability) && ValidateTargeting(ability, targetingData);
    }

    private bool ValidateCooldown(AbilityUser user, AbilityData ability)
    {
        return !user.IsOnCooldown(ability);
    }

    private bool ValidateTargeting(AbilityData ability, AbilityTargetingData targetingData)
    {
        switch (ability.targetingType)
        {
            case TargetingType.Point:
                return targetingData.hasTargetPoint;

            case TargetingType.Target:
                return targetingData.target != null;

            case TargetingType.Self:
            case TargetingType.None:
                return true;

            default:
                return false;
        }
    }
}
