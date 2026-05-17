# Targeting System

## Purpose
Converts player input and aiming information into usable gameplay targeting data.

Acts as the targeting stage of the combat pipeline.

---

## Why It Exists
Separates targeting logic from ability execution logic.

Allows multiple abilities to share reusable targeting behavior.

---

## Current Targeting Types
- None
- Self
- Target
- Point

---

## Responsibilities
- perform targeting raycasts
- resolve target objects
- resolve target positions
- calculate cast direction
- provide targeting data for AbilityContext

---

## Does NOT Handle
- cooldowns
- damage
- delivery behavior
- area queries
- status effects

---

## Architectural Benefits
- reusable targeting logic
- reduced per-ability code
- supports future AI targeting
- simplifies confirm-cast workflows
- enables scalable ability combinations

---

## Future Extensions
Potential future additions:
- lock-on targeting
- ground validation
- line-of-sight checks
- AI target selection
- target filtering
- smart targeting assistance