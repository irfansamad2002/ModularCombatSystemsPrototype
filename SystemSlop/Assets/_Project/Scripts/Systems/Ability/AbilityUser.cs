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
        public Transform Firepoint => firePoint;
        private float[] _cooldowns;

        private void Awake()
        {
            _cooldowns = new float[abilities.Count];
        }

        public void UseAbility(AbilityData ability, AbilityContext context)
        {
            int index = abilities.IndexOf(ability);

            if (!CanUseAbility(ability, context))
            {
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
                //DebugHelper.WarnMissingComponent(target, nameof(GameObject));
                return;
            }

            foreach (var effect in ability.effects)
            {
                effect.Apply(target, context);
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

            Destroy(projectileGO, ability.projectile.lifetime);
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

        public bool CanUseAbility(AbilityData ability, AbilityContext context)
        {
            int index = abilities.IndexOf(ability);

            if (index < 0)
            {
                return false;
            }

            if (IsOnCooldown(index))
            {
                return false;
            }

            switch (ability.targetingType)
            {
                case TargetingType.Point:
                    return context.hasPoint;

                case TargetingType.Target:
                    return context.target != null;

                case TargetingType.Self:
                case TargetingType.None:
                    return true;
            }
            return false;
        }

    }
}
    
public struct AbilityContext
{
    public GameObject target;

    public Vector3 point;
    public bool hasPoint;

    public Vector3 direction;
}