using UnityEngine;

namespace Project.Systems.Effects
{
    public abstract class EffectData : ScriptableObject
    {
        /// <summary>
        /// Inside your EffectHandler / EffectInstance logic
        /// if (existingEffect.effectid == newEffect.effectId)
        ///     existingEffect.onReapply();
        /// </summary>
        public string effectId; // So that dont apply effects, u apply them repeatdly over time

        public virtual void Apply(
         GameObject target,
         AbilityTargetingData context = default,
         float multiplier = 1f)
        {
            Apply(target, multiplier);
        }

        public virtual void Apply(
            GameObject target,
            float multiplier = 1f)
        {
            // fallback implementation
        }
    }
}