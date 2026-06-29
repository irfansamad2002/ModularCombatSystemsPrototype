## Current Ability Pipeline

Player input
→ AbilityTargetResolver
→ AbilityTargetingCalculator
→ AbilityTargetingAdjuster
→ AbilityTargetingData

Instant Cast
→ AbilityValidator
→ AbilityUser

Confirm Cast
→ AbilityCast
    ↳ updates targeting every frame
→ AbilityValidator
→ AbilityUser

AbilityUser
→ Delivery
→ AbilityImpactExecutor
→ Effects