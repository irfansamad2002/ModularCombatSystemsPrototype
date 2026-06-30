using UnityEngine;
namespace Project.Systems.Ability.Data
{
    [CreateAssetMenu(menuName = "Abilities/Projectile")]
    public class ProjectileData : ScriptableObject
    {
        public GameObject prefab;
        public float speed = 10f;
        public float lifetime = 5f;

        public float minDistanceThreshold = 0.1f;
        public float minFalloff = 0.2f;
    }
}