using Project.Systems.Abilities;
using Project.Systems.Combat;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

namespace Project.Systems.Ability
{
    public class AbilityUser : MonoBehaviour
    {
        [SerializeField] private List<AbilityData> abilities;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float offsetForFirePointZAxis = 1f;

        //private Dictionary<AbilityData, float> _cooldowns = new Dictionary<AbilityData, float>();
        private float[] _cooldowns;

        private void Awake()
        {
            _cooldowns = new float[abilities.Count];
        }

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

            if (IsOnCooldown(index))
            {
                Debug.Log($"Ability {ability.abilityName} is on cooldown");
                return;
            }

            ExecuteAbility(ability, target);
            StartCooldown(index,ability);


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

        private bool IsOnCooldown(int index)
        {
            return _cooldowns[index] > 0f;
        }

        private void StartCooldown(int index, AbilityData ability)
        {
            _cooldowns[index] = ability.cooldown;
        }


        private void SpawnProjectile(AbilityData ability)
        {
            var projectileGO = Instantiate(
                ability.projectile.prefab,
                firePoint.position + firePoint.forward * offsetForFirePointZAxis,
                firePoint.rotation
            );

            var projectile = projectileGO.GetComponent<Projectile>();
            projectile.Init(ability.effects, ability.projectile.speed, ability.projectile.explosionRadius, ability.projectile.targetLayers, ability.projectile.impactVFX, ability.projectile.minDistanceThreshold, ability.projectile.minFalloff);
            var playerCollider = GetComponent<Collider>();
            //projectile.SetOwner(playerCollider);
            Destroy(projectileGO, ability.projectile.lifetime);
        }

        private void Update()
        {
            for (int i = 0; i < _cooldowns.Length; i++)
            {
                if (_cooldowns[i] > 0f)
                {
                    _cooldowns[i] -= Time.deltaTime;

                    if (_cooldowns[i] < 0f)
                    {
                        _cooldowns[i] = 0f;
                    }
                }
            }
        }

        private void OnGUI()
        {
            GUILayout.Label("Cooldowns:");

            for( int i = 0; i < abilities.Count; i++)
            {
                GUILayout.Label($"{abilities[i].abilityName}: {_cooldowns[i]:F2}");
            }
        }




    }
}