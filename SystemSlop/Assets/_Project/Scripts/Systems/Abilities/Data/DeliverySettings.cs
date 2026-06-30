using Project.Systems.Ability.Data;
using UnityEngine;

[System.Serializable]
public class DeliverySettings
{
    public DeliveryType deliveryType;
    public ProjectileData projectile; //Optional
    public float delay = 1f;
    public GameObject telegraphVFX;
    public GameObject impactVFX;
}
