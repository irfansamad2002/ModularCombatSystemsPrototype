# Area Execution System

## Purpose
Handles spatial target collection for area-based abilities.

Determines which targets are affected based on AreaShape rules.

---

## Why It Exists
Separates target collection logic from delivery and effect execution.

Allows reusable area query behavior across multiple abilities.

---

## Current Area Shapes
- None
- Sphere
- Cone

---

## Responsibilities
- collect valid targets within area
- perform spatial overlap queries
- apply directional filtering
- provide affected targets for execution

---

## Does NOT Handle
- damage calculation
- cooldown logic
- projectile movement
- targeting confirmation
- status effect processing

---

## Architectural Benefits
- reusable area query behavior
- scalable shape expansion
- cleaner execution pipeline
- reduced hardcoded AoE logic
- supports future combat features

---

## Future Extensions
Potential future additions:
- box queries
- capsule queries
- chain targeting
- line attacks
- terrain-aware queries
- faction filtering
- visibility filtering