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
        public string description;
        public Sprite icon;

        public float cooldown;
        public float castRange = 10f; //Optional
        public TargetingType targetingType;
        public DeliveryType deliveryType;

        public List<EffectData> effects;
        public ProjectileData projectile; //Optional
        public GameObject indicatorPrefab;//Optional : if null which mean no need indicators

        public CastMode castMode;

        [Header("Area")]
        public AreaShape areaShape;
        public float radius = 3f;
        [Range(0f, 360f)]
        public float coneAngle = 90f;
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