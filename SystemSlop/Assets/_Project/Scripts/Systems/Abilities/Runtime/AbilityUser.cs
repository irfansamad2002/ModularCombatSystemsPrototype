using Project.Core.Event;
using Project.Systems.Abilities.Data;
using Project.Systems.Abilities.Runtime;
using Project.Systems.Combat;
using Project.Systems.Combat.Query;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Systems.Abilities.Runtime
{
    public class AbilityUser : MonoBehaviour
    {
        [SerializeField] private List<AbilityData> abilities;
        [SerializeField] private Transform firePoint;
        [SerializeField] private Material tempDebugMaterial;
        [SerializeField] private LayerMask _targetLayer;

        public Transform Firepoint => firePoint;
        private float[] _cooldowns;

        private void Awake()
        {
            _cooldowns = new float[abilities.Count];
        }

        public void TryUseAbility(AbilityData ability, AbilityTargetingData context)
        {
            int index = abilities.IndexOf(ability);

            if (!CanUseAbility(ability, context))
            {
                return;
            }

            ExecuteAbility(ability, context);
            StartCooldown(index, ability);
        }

        private void ExecuteAbility(AbilityData ability, AbilityTargetingData context)
        {
            switch (ability.deliveryType)
            {
                case DeliveryType.Instant:
                    ResolveAreaImpact(ability, context);
                    break;
                case DeliveryType.Projectile:
                    ExecuteProjectile(ability, context);
                    break;
                case DeliveryType.Delayed:
                    ExecuteDelayed(ability, context);
                    break;
                default:
                    break;
            }

            AbilityEvents.OnAbilityExecuted?.Invoke(ability, context);

        }

        public void ResolveAreaImpact(AbilityData ability, AbilityTargetingData context)
        {
            
            switch (ability.areaShape)  
            {
                case AreaShape.None:
                    ExecuteSingleTargetInstant(ability, context);
                    break;
                case AreaShape.Sphere:
                    ExecuteSphereArea(ability, context);
                    break;
                case AreaShape.Cone:
                    ExecuteConeArea(ability, context);
                    break;
                default:
                    break;
            }
        }

        private void ExecuteDelayed(AbilityData ability, AbilityTargetingData context)
        {
            GameObject runnerGO = new GameObject("Delayed Ability");

            var runner = runnerGO.AddComponent<DelayedAbilityRunner>(); 

            runner.Init(this, ability, context, tempDebugMaterial);
        }

        private void ExecuteSingleTargetInstant(AbilityData ability, AbilityTargetingData context)
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

        private void ExecuteSphereArea(AbilityData ability, AbilityTargetingData context)
        {
            Vector3 center = GetTargetPosition(ability, context);

            var targets = AreaQuery.GetTargetsSphere(center, ability.radius, _targetLayer, transform);

            foreach (var target in targets)
            {
                foreach (var effect in ability.effects)
                {
                    effect.Apply(target, context);
                }
            }
        }

        private void ExecuteConeArea(AbilityData ability, AbilityTargetingData context)
        {
            Vector3 origin = transform.position;

            var sphereTargets = AreaQuery.GetTargetsSphere(
                origin,
                ability.radius,
                _targetLayer,
                transform);

            var coneTargets = AreaQuery.FilterCone(
                sphereTargets,
                origin,
                context.direction,
                ability.coneAngle);


            foreach (var target in coneTargets)
            {
                foreach (var effect in ability.effects)
                {
                    effect.Apply(target, context);
                }
            }
        }

        private void ExecuteProjectile(AbilityData ability, AbilityTargetingData context)
        {
            Vector3 point = GetTargetPosition(ability, context);

            Vector3 dir = (point - firePoint.position).normalized;

            var projectileGO = Instantiate(ability.projectile.prefab,
                firePoint.position,
                Quaternion.LookRotation(dir));

            var projectile = projectileGO.GetComponent<Projectile>();

            projectile.Init(ability.effects,
                ability.projectile.speed,
                ability.radius,
                ability.targetLayers,
                ability.impactVFX,
                ability.minDistanceThreshold,
                ability.minFalloff
                );

            Destroy(projectileGO, ability.projectile.lifetime);
        }

        private GameObject ResolveTarget(AbilityData ability, AbilityTargetingData context)
        {
            switch (ability.targetingType)
            {
                case TargetingType.None:
                    return null;
                case TargetingType.Point:
                    return null;
                case TargetingType.Target:
                    return context.castTarget;
                case TargetingType.Self:
                    return gameObject;
                default:
                    return null;
            }
        }

        private Vector3 GetTargetPosition(AbilityData ability, AbilityTargetingData context)
        {
            switch (ability.targetingType)
            {
                case TargetingType.Point:
                    return context.aimPoint;

                case TargetingType.Self:
                    return transform.position;

                case TargetingType.Target:
                    if (context.castTarget != null)
                    {
                        return context.castTarget.transform.position;
                    }

                    break;
                }
            return firePoint.position + firePoint.forward * 2f;
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

        public bool CanUseAbility(AbilityData ability, AbilityTargetingData context)
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
                    return context.hasAimPoint;

                case TargetingType.Target:
                    return context.castTarget != null;

                case TargetingType.Self:
                case TargetingType.None:
                    return true;
            }
            return false;
        }

        public bool CanStartCast(AbilityData ability)
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
            return true;
        }

        public bool CanConfirmCast(AbilityData ability, AbilityTargetingData context)
        {
            switch (ability.targetingType)
            {
                case TargetingType.Point:
                    return context.hasAimPoint;

                case TargetingType.Target:
                    return context.castTarget != null;

                case TargetingType.Self:
                case TargetingType.None:
                    return true;
            }
            return false;
        }

        
    }
}
