using UnityEngine;

namespace Project.Systems.Effects
{
    public abstract class EffectData : ScriptableObject
    {
        public string effectId;

        public abstract void Apply(GameObject target, float multiplier = 1f);
    }
}