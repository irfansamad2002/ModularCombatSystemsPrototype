using Project.Systems.Abilities.Data;
using Project.Systems.Abilities.Runtime;
using UnityEngine;

public class AbilityValidator
{
    public bool CanStartCast(AbilityUser user, AbilityData ability)
    {
        return ValidateCooldown(user, ability);
    }

    public bool CanConfirmCast(AbilityUser user, AbilityData ability, AbilityTargetingData targetingData)
    {
        return ValidateTargeting(user,ability, targetingData);
    }


    public bool CanUse(AbilityUser user, AbilityData ability, AbilityTargetingData targetingData)
    {
        return ValidateCooldown(user,ability) && ValidateTargeting(user, ability, targetingData);
    }

    private bool ValidateCooldown(AbilityUser user, AbilityData ability)
    {
        return !user.IsOnCooldown(ability);
    }

    private bool ValidateTargeting(AbilityUser user, AbilityData ability, AbilityTargetingData targetingData)
    {
        switch (ability.targetingType)
        {
            case TargetingType.Point:
                return targetingData.hasTargetPoint;

            case TargetingType.Target:
                if (targetingData.target == null)
                    return false;

                if(!IsTargetInRange(user.Firepoint, ability, targetingData))
                    return false;

                return true;

            case TargetingType.Self:
            case TargetingType.None:
                return true;

            default:
                return false;
        }
    }

    private bool IsTargetInRange(Transform caster, AbilityData ability, AbilityTargetingData targetingData)
    {
        return Vector3.Distance(caster.position, targetingData.target.transform.position) <= ability.castRange;

    }
}
