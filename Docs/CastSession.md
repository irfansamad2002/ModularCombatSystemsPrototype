# CastSession

## Purpose
Represents an ability that the player has started casting, but has not finished using yet
Stores temporary information during the casting process (like selected targets, aiming position, or charging state)
Keeps temporary casting data seperate from permanent gameplay systems like AbilityUser

---

## Why It Exists
Prevents gameplay systems from becoming messy with short-lived casting data
Seperates the "preparing the ability" phase from the "actually executing the ability" phase
Makes it easier to support abilities where the players must confirm a target before the skill activates (like gorund targeting, skillshots, or area placement abilities)

---

## Responsibilities
- track active cast state
- store pending ability reference
- manage targeting confirmation flow
- maintain temporary targeting data
- handle cast lifecycle states

---

## Does NOT Handle
- cooldown logic
- damage execution
- area queries
- projectile behavior
- effect application

---

## Architectural Benefits
- isolates transient cast state
- prevents AbilityUser bloat
- simplifies interruption handling
- reusable for future cast mechanics
- supports scalable targeting workflows

---

## Future Extensions
Potential future additions:
- cast interruption
- cast timers
- charge mechanics
- cast bars
- queued abilities
- prediction systems
- AI cast preparation
