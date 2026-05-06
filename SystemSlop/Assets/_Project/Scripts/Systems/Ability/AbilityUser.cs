using Project.Systems.Abilities;
using Project.Systems.Combat;
using System;
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
        //[SerializeField] private float offsetForFirePointZAxis = 1f;

        private float[] _cooldowns;

        private void Awake()
        {
            _cooldowns = new float[abilities.Count];
        }

        public void UseAbility(int index, AbilityContext context)
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

            ExecuteAbility(ability, context);
            StartCooldown(index, ability);
        }

       
        private void ExecuteAbility(AbilityData ability, AbilityContext context)
        {
            switch (ability.deliveryType)
            {
                case DeliveryType.Instant:
                    ExecuteInstant(ability, context);
                    break;
                case DeliveryType.Projectile:
                    ExecuteProjectile(ability, context);
                    break;
                default:
                    break;
            }

        }

        private void ExecuteInstant(AbilityData ability, AbilityContext context)
        {
            GameObject target = ResolveTarget(ability, context);

            if (target == null)
            {
                DebugHelper.WarnMissingComponent(target, nameof(GameObject));
                return;
            }

            foreach (var effect in ability.effects)
            {
                effect.Apply(target);
            }
        }

        private void ExecuteProjectile(AbilityData ability, AbilityContext context)
        {
            Vector3 point = ResolvePoint(ability, context);

            Vector3 dir = (point - firePoint.position).normalized;

            var projectileGO = Instantiate(ability.projectile.prefab,
                firePoint.position,
                Quaternion.LookRotation(dir));

            var projectile = projectileGO.GetComponent<Projectile>();

            projectile.Init(ability.effects,
                ability.projectile.speed,
                ability.projectile.explosionRadius,
                ability.projectile.targetLayers,
                ability.projectile.prefab,
                ability.projectile.minDistanceThreshold,
                ability.projectile.minFalloff
                );

            Destroy(projectileGO, ability.projectile.lifetime );
        }

        private GameObject ResolveTarget(AbilityData ability, AbilityContext context)
        {
            switch (ability.targetingType)
            {
                case TargetingType.None:
                    return null;
                case TargetingType.Point:
                    return null;
                case TargetingType.Target:
                    return context.target;
                case TargetingType.Self:
                    return gameObject;
                default:
                    return null;
            }
        }

        private Vector3 ResolvePoint(AbilityData ability, AbilityContext context)
        {
            switch (ability.targetingType)
            {
                case TargetingType.Point:
                    return context.point;   
                case TargetingType.Self:
                    return transform.position;
                case TargetingType.Target:
                case TargetingType.None:
                default:
                    return firePoint.position + firePoint.forward * 10f;
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

        public float GetCooldownRemaining(int index)
        {
            return _cooldowns[index];
        }

        public float GetCooldownMax(int index)
        {
            return abilities[index].cooldown;
        }

        public AbilityData GetAbility(int index)
        {
            return abilities[index];
        }

        private void SpawnProjectileAtPoint(AbilityData ability, Vector3 point)
        {
            Vector3 dir = (point - firePoint.position).normalized;

            var projectileGO = Instantiate(
                ability.projectile.prefab,
                firePoint.position,
                Quaternion.LookRotation(dir));

            var projectile =  projectileGO.GetComponent<Projectile>();
            projectile.Init(ability.effects, ability.projectile.speed, ability.projectile.explosionRadius, ability.projectile.targetLayers, ability.projectile.impactVFX, ability.projectile.minDistanceThreshold, ability.projectile.minFalloff);

            Destroy(projectileGO, ability.projectile.lifetime);
        }

        private void ApplyToTarget(AbilityData ability, GameObject targetGameObject)
        {
            foreach (var effect in ability.effects)
            {
                effect.Apply(targetGameObject);
            }
        }

    }

}
    
public struct AbilityContext
{
    public GameObject target;
    public Vector3 point;
}