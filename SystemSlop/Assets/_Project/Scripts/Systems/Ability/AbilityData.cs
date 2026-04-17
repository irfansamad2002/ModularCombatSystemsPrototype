using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Abilities/Ability")]
public class AbilityData : ScriptableObject
{
    public string abilityName;
    public float cooldown;

    public List<EffectData> effects;
}
