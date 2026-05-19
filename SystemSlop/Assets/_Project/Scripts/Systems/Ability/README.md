# Ability System

## Purpose
Handles gameplay ability execution, delivery routing,
targeting coordination, and effect application.

---

## Core Systems

### AbilityUser
Coordinates abilitiy execution flow and cooldowns.

### AbilityTargetResolver
Coverts player intent into runtime targeting data.

### AbilityContext
Carries runtime cast data between systems.

### AreaQuery
Handles reusable combat spatial queries.

---

## Execution Flow

Player Input
→ Cast Session
→ AbilityTargetResolver
→ AbilityContext
→ AbilityUser
→ Delivery Execution
→ AreaQuery
→ Effects

---

## Supported Features

 - Instant abilities
 - Projectile abilities
 - Delayed abilities
 - Cone AoE
 - Sphere AoE
 - Confirm targeting
 - Self targeting

---

## Current Architecture Goals
 - modular execution
 - reusable combat system
 - scalable area queries
 - seperation of concerns
 - NPC-compatible combat flow