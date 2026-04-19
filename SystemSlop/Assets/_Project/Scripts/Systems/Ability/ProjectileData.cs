using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Projectile")]
public class ProjectileData : ScriptableObject
{
    public GameObject prefab;
    public float speed = 10f;
    public float lifetime = 5f;
    [Header("AOE")]
    public float explosionRadius = 0f;
    public LayerMask targetLayers;
    [Header("VFX")]
    public GameObject impactVFX;
}
