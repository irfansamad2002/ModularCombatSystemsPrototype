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