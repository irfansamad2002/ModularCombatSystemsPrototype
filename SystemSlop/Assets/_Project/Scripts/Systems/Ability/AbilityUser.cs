using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class AbilityUser : MonoBehaviour
{
    [SerializeField] private List<AbilityData> abilities;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float offsetForFirePointZAxis = 1f;

    private Dictionary<AbilityData, float> _cooldowns = new Dictionary<AbilityData, float>();

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

        if (IsOnCooldown(ability))
        {
            Debug.Log($"Ability {ability.abilityName} is on cooldown");
            return;
        }

        ExecuteAbility(ability, target);
        StartCooldown(ability);

           
    }

    private void ExecuteAbility(AbilityData ability, GameObject target)
    {
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

    private bool IsOnCooldown(AbilityData ability)
    {
        return _cooldowns.ContainsKey(ability);
    }

    private void StartCooldown(AbilityData ability)
    {
        _cooldowns[ability] = ability.cooldown;
    }   


    private void SpawnProjectile(AbilityData ability)
    {
        var projectileGO = Instantiate(
            ability.projectile.prefab,
            firePoint.position + firePoint.forward * offsetForFirePointZAxis    ,
            firePoint.rotation
        );

        var projectile = projectileGO.GetComponent<Projectile>();
        projectile.Init(ability.effects, ability.projectile.speed, ability.projectile.explosionRadius, ability.projectile.targetLayers);
        var playerCollider = GetComponent<Collider>();
        projectile.SetOwner(playerCollider);
        Destroy(projectileGO, ability.projectile.lifetime);
    }

    private void Update()
    {
        var keys = new List<AbilityData>(_cooldowns.Keys);

        foreach (var ability in keys)
        {
            _cooldowns[ability] -= Time.deltaTime;

            if(_cooldowns[ability] <= 0)
            {
                _cooldowns.Remove(ability);
            }
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Cooldowns:");

        foreach (var kvp in _cooldowns)
        {
            GUILayout.Label($"{kvp.Key.name}: {kvp.Value:F2}");
        }
    }

    


}
