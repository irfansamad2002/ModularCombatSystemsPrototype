using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Projectile")]
public class ProjectileData : ScriptableObject
{
    public GameObject prefab;
    public float speed = 10f;
    public float lifetime = 5f;
    [Header("AOE")]
    public float explosionRadius = 0f;
    public float minDistanceThreshold = 0.1f;
    public float minFalloff = 0.2f;
    [SerializeField] private AnimationCurve falloffCurve; //TODO: use this curve for more complex falloff patterns
    public LayerMask targetLayers;
    [Header("VFX")]
    public GameObject impactVFX;
}
