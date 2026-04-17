using UnityEngine;

public abstract class EffectData : ScriptableObject
{
    public abstract void Apply(GameObject target);
}
