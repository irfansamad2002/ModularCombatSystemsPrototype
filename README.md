# Modular Ability Combat Framework (Unity)
This project is a Unity-based combat system focused on **clean architecture and reusable gameplay systems**, rather than building a full content-heavy game.(not yet at least)

The goal is to demonstrate how a combat system can be structured so that abilities can be added, modified, and combined without changing core logic.

---

## 🧠 Core Idea
The system is split into 3 parts

- **Abilities = what you use**
- **Execution = how it happens**
- **Effects = what it does**

This separation allows new abilities to be created through configuration instead of new gameplay code.

---

## ⚙️ System Overview

### 1. Ability (Setup Layer)
Abilities are defined as data objects (ScriptableObjects). They contain configuration only:

- Targeting type (Point / Target / Self)
- Delivery type (Instant / Projectile / Delayed / Chain)
- Area shape (None / Sphere / Cone)
- Effect list (Damage, Heal, Slow, etc.)

Abilities do not contain gameplay logic.

---

### 2. Execution (Runtime Layer)
Execution defines how the ability is processed in-game:

- **Instant Execution**
  - Resolves immediately within an area

- **Projectile Execution**
  - Spawns a projectile that travels and resolves on impact

- **Delayed Execution**
  - Waits for a duration before resolving

- **Chain Execution**
  - Transfers effects across multiple targets based on rules

---

### 3. Targeting System
The targeting system handles how valid targets are selected:

- Aim direction and point calculation
- Target detection (raycast / overlap checks)
- Range and angle validation

This ensures consistent targeting behavior across all abilities.

---

### 4. Effect System
Effects define what happens to targets after execution:

- Damage
- Heal
- Slow / movement modifiers

Important rule
- Abilities never modify health or stats directly
- All changes are applied through effects

This keeps gameplay logic modular and consistent.

---

## 🔥 Example Abilities
These abilities demonstrate different execution patterns:

---

### 1. Fireball (Projectile AoE System)
A projectile that explodes on impact, dealing area damage.

Demonstrates:
- Projectile lifecycle (spawn → travel → impact)
- Collision-based resolution
- Area-of-effect damage handling

---

### 2. Chain Lightning (Multi-Target Chain)
An ability that jumps between multiple enemies.

Demonstrates:
- Multi-target selection logic
- Distance-based chaining rules
- Prevention of duplicate hits

---

### 3. Cleave (Directional Melee)
A fast melee attack that hits enemies in front of the player.

Demonstrates:
- Cone-based hit detection
- Instant execution flow
- Close-range combat logic

---

### 4. Meteor (Delayed Impact)
A targeted location is marked, then a meteor falls after a delay.

Demonstrates:
- Delayed execution system
- Separation of cast time and impact time
- Position-based validation at impact

---

## 🧩 Key Technical Highlights

- Fully data-driven ability system using ScriptableObjects
- Clear separation between ability, execution, and effects
- Reusable effect system across all abilities
- Consistent targeting system across melee, ranged, and AoE
- Modular execution types (instant, projectile, delayed, chain)

---

## 🧪 Design Intent

This project is not a game content demo.

It is a **combat system framework**, designed to demonstrate:

- scalable gameplay architecture
- reusable system design
- clean separation of responsibilities
- predictable execution flow

---

## 🚧 Future Improvements

- Interruptible abilities and reaction system
- Buff / debuff (status effect) system
- Advanced crowd control system
- Multiplayer-safe deterministic execution
- Animation-event driven execution hooks

---

## 📌 Summary

Abilities are defined as configuration.
Execution controls how they run.
Effects handle all gameplay changes.

This results in a combat system that is modular, extensible, and easy to expand without modifying core logic.