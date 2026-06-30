using Project.Systems.Abilities.Data;
using UnityEngine;

public class AbilityTargetAdjuster
{
    public void Adjust(
           Transform caster,
           AbilityData ability,
           ref AbilityTargetingData targetingData)
    {

        switch (ability.targetingSettings.targetingType)
        {
            case TargetingType.Point:
                if (ability.deliverySettings.deliveryType == DeliveryType.Projectile)
                    return;
                ClampPointToRange(caster, ability, ref targetingData);
                break;

            case TargetingType.Target:
                //Future
                break;
            case TargetingType.Self:
            case TargetingType.None:
            default:
                break;
        }
       

        // Future additions:
        // SnapToGround(...)
        // SnapToNavMesh(...)
        // AimAssist(...)
    }

    private void ClampPointToRange(Transform caster, AbilityData ability, ref AbilityTargetingData targetingData)
    {
        Vector3 toPoint = targetingData.targetPoint - caster.position;

        if (toPoint.magnitude <= ability.targetingSettings.castRange)
        {
            return;
        }

        Vector3 direction = toPoint.normalized;

        targetingData.targetPoint = caster.position + direction * ability.targetingSettings.castRange;
        
    }
}
