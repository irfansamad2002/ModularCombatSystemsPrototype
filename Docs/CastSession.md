# CastSession

## Purpose

Represents an active cast process that has started but has not yet been confirmed or executed.

Stores temporary runtime casting state separately from permanent gameplay systems.

## Why It Exists

Prevents temporary cast data from polluting AbilityUser and other gameplay systems.

Separates cast preparation from final ability execution.

Supports confirm-style targeting workflows.

## Responsibilities

- track active cast state
- store pending ability reference
- manage targeting confirmation flow
- maintain temporary targeting data
- handle cast lifecycle states

## Does NOT Handle

- cooldown logic
- damage execution
- area queries
- projectile behavior
- effect application

## Architectural Benefits

- isolates transient cast state
- prevents AbilityUser bloat
- simplifies interruption handling
- reusable for future cast mechanics
- supports scalable targeting workflows

## Future Extensions

Potential future additions:

- cast interruption
- cast timers
- charge mechanics
- cast bars
- queued abilities
- prediction systems
- AI cast preparation
