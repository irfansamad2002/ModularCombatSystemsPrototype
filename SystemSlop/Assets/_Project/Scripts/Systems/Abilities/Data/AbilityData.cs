using Project.Systems.Ability.Data;
using Project.Systems.Effects;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Systems.Abilities.Data
{
    [CreateAssetMenu(menuName = "Abilities/Ability")]
    public class AbilityData : ScriptableObject
    {
        public string abilityName;
        public string description;
        public Sprite icon;

        public float cooldown;
        public float castRange = 10f; //Optional
        public float radius;
        public TargetingType targetingType;
        public DeliveryType deliveryType;
        public CastMode castMode;

        public List<EffectData> effects;

        [Header("VFX")]
        public GameObject impactVFX;
        public GameObject telegraphVFX;
        //public GameObject castVFX; // example: glowly hand type shit

        [Header("Projectile")]
        public ProjectileData projectile; //Optional
        public LayerMask targetLayers;
        [Space(10)]
        public GameObject indicatorPrefab;//Optional : if null which mean no need indicators
        [Space(10)]
        public float minDistanceThreshold = 0.1f;
        public float minFalloff = 0.2f;


        [Header("Area")]
        public AreaShape areaShape;
        [Range(0f, 360f)]
        public float coneAngle = 90f;

        [Header("Delayed")]
        public float delay = 1f;
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
    Projectile,
    Delayed
}

public enum CastMode
{
    Instant,    //Cast immediately on press
    Confirm     //enter targeting mode first
}

public enum AreaShape
{
    None,
    Sphere,
    Cone
}