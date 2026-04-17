using System.Collections.Generic;
using UnityEngine;

public class AbilityUser : MonoBehaviour
{
    [SerializeField] private List<AbilityData> abilities;
    [SerializeField] private Transform firePoint;

    public void UseAbility(int index, GameObject target)
    {
        if (index < 0)
        {
            return;
        }

        if (index >= abilities.Count)
        {
            Debug.Log("Dont have ability assigned to that slot");
            return;
        }


        var ability = abilities[index];

        if (ability.projectile != null)
        {
            //Debug.Log($"Using ability: {ability.abilityName} with projectile");
            SpawnProjectile(ability);
        }
        else
        {
            //Debug.Log($"Using ability: {ability.abilityName} instantly");
            //fallback: instant
            foreach (var effect in ability.effects)
            {
                effect.Apply(target);
            }
        }    
    }
    private void SpawnProjectile(AbilityData ability)
    {
        var projectileGO = Instantiate(
            ability.projectile.prefab,
            firePoint.position,
            firePoint.rotation
        );

        var projectile = projectileGO.GetComponent<Projectile>();
        projectile.Init(ability.effects, ability.projectile.speed);
        Destroy(projectileGO, ability.projectile.lifetime);
    }
}
