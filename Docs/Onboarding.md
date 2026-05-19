# Adding New Abilities

New abilities are created using AbilityData assets.

Execution behavior is determined through:
- DeliveryType
- TargetingType
- AreaShape
- Effect configuration

AbilityUser routes abilities into the correct execution pipeline automatically.

# Targeting Ownership

AbilityTargetResolver owns targeting intent resolution.

Responsibilities include:
- direction calculation
- raycast targeting
- point selection
- target acquisition

AreaQuery handles spatial candidate gathering separately.

# Execution Flow

Input
→ CastSession
→ AbilityTargetResolver
→ AbilityContext
→ AbilityUser
→ Delivery Execution
→ AreaQuery
→ Effects

# Shared Runtime Data

AbilityContext is the shared runtime payload used during execution.

Current fields:
- target
- point
- direction
- hasPoint

# Reusable Systems

The following systems are designed for reuse:

- AbilityTargetResolver
- AreaQuery
- AbilityContext
- Effect system
- Delivery execution pipeline

These systems are intended to support future:
- NPC combat
- AI casting
- new area shapes
- additional delivery types