using UnityEngine;

public abstract class EffectData : ScriptableObject
{
    public abstract void Apply(GameObject target, float multiplier = 1f);
}
