using Project.Systems.Ability;
using Project.Systems.Effects;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Systems.Abilities
{
    [CreateAssetMenu(menuName = "Abilities/Ability")]
    public class AbilityData : ScriptableObject
    {
        public string abilityName;
        public float cooldown;

        public List<EffectData> effects;
        public ProjectileData projectile;
    }
}