AbilityTargetResolver
- Performs camera and physics queries.

AbilityTargetingCalculator
- Converts raw query results into AbilityTargetingData.

AbilityTargetingAdjuster
- Adjusts targeting data before validation.
- Current rule:
    - Clamp point targeting to cast range.

AbilityValidator
- Determines whether an ability may execute.
- Current checks:
    - Cooldown
    - Point exists
    - Target exists
    - Target within range

AbilityUser
- Starts ability execution.
- Starts cooldowns.

ProjectileDelivery
- Spawns projectiles.

InstantDelivery
- Finds impacted targets.

DelayedDelivery
- Waits before using InstantDelivery.

AbilityImpactExecutor
- Applies effects to one or more targets.