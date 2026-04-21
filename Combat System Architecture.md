# High Level Architecture
Player Input
    →
AbilityUser
    →
AbilityData (ScriptableObject)
    →
Projectile (spawned)
    →
OnTriggerEnter → Explode
    →
OverlapSphere (AoE detection)
    →
Falloff Calculation
    →
EffectData.Apply(target, multiplier)
    →
Health.TakeDamage()

# Input → Ability
## PlayerAbilityInput
Listens for key press →
calls AbilityUser.UseAbility(index)

## AbilityUser
UseAbility(index):
get AbilityData
if projectile:
SpawnProjectile()
else:
apply effects instantly

# Data Layer (ScriptableObjects)
## AbilityData
```csharp
public class AbilityData : ScriptableObject
{
    public ProjectileData projectile;
    public List<EffectData> effects;

    public float cooldown;

    // AoE tuning
    public float explosionRadius;
    public float minDistanceThreshold;
    public float minFalloff;
}
```
## ProjectileData
```csharp
public class ProjectileData : ScriptableObject
{
    public GameObject prefab;
    public float speed;
    public float lifetime;
}
```
## EffectData (Core Abstraction)
```c#
public abstract class EffectData : ScriptableObject
{
    public abstract void Apply(GameObject target, float multiplier = 1f);
}
```
### Example:
```c#
public class DamageEffect : EffectData
{
    public float damage;

    public override void Apply(GameObject target, float multiplier = 1f)
    {
        var health = target.GetComponent<Health>();
        if (health == null) return;

        float finalDamage = damage * multiplier;
        health.TakeDamage(finalDamage);
    }
}
```
# ProjectileSystem
## Projectile
Moves forward →
OnTriggerEnter →
Explode →
Apply AoE
## Core Flow
OnTriggerEnter:
    if already hit → ignore
    Explode()
    Destroy self
## Explode
OverlapSphere(center, radius, damageLayers)

foreach hit:
    ApplyAoE(hit)
## ApplyAOE
distance = Distance(center, target)

// 🔥 Direct hit stabilisation
if (distance <= minDistanceThreshold)
    distance = 0

normalized = clamp(distance / radius)

falloff = (1 - normalized)^2

// 🔥 Minimum damage floor
falloff = max(falloff, minFalloff)

foreach effect:
    effect.Apply(target, falloff)
# Collision Design
Projectile collider → ONLY triggers explosion (WHEN)

Explosion radius → decides damage (WHO + HOW MUCH)
## Layer Setup
damageLayers  = Enemy                      (damage)
# Health System
## Health
```c#
public float currentHealth;
public float maxHealth;

public void TakeDamage(float amount)
{
    currentHealth -= amount;
    currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

    if (currentHealth <= 0)
        Die();
}
```
Key decisions made
float health ✔
NOT int ✔
Because:
•	falloff 
•	DoT 
•	Scaling
# Damage Falloff System
falloff = Mathf.Pow(1f - normalized, 2f);
Meaning:
Center → 100%
Mid    → ~75%
Edge   → ~0% (but clamped by minFalloff)
# Stability Adjustments
## Direct hit consistency
if (distance <= minDistanceThreshold)
    distance = 0;
## Minimum damage floor
falloff = Mathf.Max(falloff, minFalloff);
# DO NOT DO!!
❌ damage in OnTriggerEnter
❌ mixing projectile size with AoE
❌ putting explosion logic in EffectData
❌ overusing virtual/overloads

