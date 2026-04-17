using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Projectile")]
public class ProjectileData : ScriptableObject
{
    public GameObject prefab;
    public float speed = 10f;
    public float lifetime = 5f;
}
