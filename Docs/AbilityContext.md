# AbilityContext

## Purpose

Stores important information about an ability while it is being used (like who casted it, what target was hit, where it landed, etc)
Works like a shared "data package" that different combat systems can read and update during the ability process

## Why It Exists

Helps different systems talk to each other wihtout being tightly connected
Prevents functions form bneeding a huge list of parameters just to pass data around
Makes the combat system cleaner, easier to expand, and easier to maintain because all systems use the same shared runtime data object

## Responsibilities

- Store resolved cast target
- Store target position
- Store cast direction
- Store runtime targeting data
- Provide a consistent execution payload

## Does NOT Handle

- cooldown logic
- targeting logic
- damage calculation
- validation
- execution flow orchestration

---

## Architectural Benefits

- reduces parameter explosion
- improves decoupling
- reusable for projectile and delayed execution
- reusable for AI/NPC casting
- simplifies future feature expansion

## Future Extensions

Potential future additions:

- source actor
- faction/team
- crit information
- damage modifiers
- multiplayer metadata
- prediction IDs
- hit surface data