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
        public float castRange = 10f; //Optional
        public TargetingType targetingType;
        public DeliveryType deliveryType;

        public List<EffectData> effects;
        public ProjectileData projectile; //Optional
        public GameObject indicatorPrefab;//Optional : if null which mean no need indicators
    }
}

public enum TargetingType
{
    None, //    no targeting needed  ex: passive ,instant , auto-activate ability. NOT for player-aimed skills
    Point, //   picka position in world
    Target,//   pick an entity
    Self//      always yourself
}

public enum DeliveryType
{
    Instant,
    Projectile
}