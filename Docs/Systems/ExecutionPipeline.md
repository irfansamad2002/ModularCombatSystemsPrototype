# Execution Pipeline

## Purpose
Defines the overall combat execution flow from ability activation to final effect application.

Separates combat logic into reusable execution stages.

---

## High-Level Flow

Input
→ AbilityUser
→ CastSession
→ AbilityTargetResolver
→ AbilityContext Creation
→ Delivery Execution
→ Area Query
→ Effect Application

---

## Core Pipeline Stages

### 1. Ability Activation
AbilityUser initiates cast flow and validates execution conditions.

### 2. Target Resolution
AbilityTargetResolver converts player input into usable targeting data.

### 3. Context Creation
Resolved targeting data is packaged into AbilityContext.

### 4. Delivery Execution
Execution path selected based on DeliveryType:
- Instant
- Projectile
- Delayed

### 5. Area Resolution
AreaShape systems determine affected targets.

### 6. Effect Application
Effects are applied to valid targets.

---

## Architectural Goals
- modular execution flow
- reusable combat stages
- data-driven ability behavior
- reduced hardcoded ability logic
- scalable future expansion

---

## Architectural Benefits
- abilities become composable
- systems remain decoupled
- execution paths stay reusable
- new abilities require less custom logic
- supports future NPC reuse

## Execution Semantics (Important Rules)

### Snapshot Rule (Delayed Abilities)
All delayed abilities operate on a snapshot of cast-time targeting data.

This includes:
- target
- point
- direction

These values are NOT recomputed after delay.

Delayed area abilities resolve targets at impact time
using the current world state.

This means:
- targets can dodge out
- new targets can enter
- impact resolution is spatially dynamic

AbilityContext stores cast intent and impact location,
not guaranteed future victims.

---

### Live Resolution Rule (Area Queries)
Area-based effects (Sphere, Cone, etc.) are evaluated at impact time using the current world state.

This means:
- which objects are inside the area is determined at impact time
- physics queries (OverlapSphere, etc.) are always live

---

### Why This Rule Exists
This prevents desynchronization between:
- player intent at cast time
- world state at impact time

It ensures delayed abilities behave deterministically and consistently.

---

### System Implication
Any system that modifies AbilityContext after cast initiation will NOT affect delayed execution outcomes.

Delayed systems must treat AbilityContext as immutable after scheduling.