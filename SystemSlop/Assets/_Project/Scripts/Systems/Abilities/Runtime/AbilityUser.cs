using Project.Core.Event;
using Project.Systems.Abilities.Data;
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
        public LayerMask TargetLayer => _targetLayer;
        private float[] _cooldowns;

        private ProjectileDelivery _projectileDelivery;
        private DelayedDelivery _delayedDelivery;
        private InstantDelivery _instantDelivery;
        private AbilityValidator _validator;
        private AbilityImpactExecutor _impactExecutor;
        private AbilityTargetAdjuster _targetAdjuster;

        private void Awake()
        {
            _cooldowns = new float[abilities.Count];

            _targetAdjuster = new AbilityTargetAdjuster();
            _validator = new AbilityValidator(); 

            _impactExecutor = new AbilityImpactExecutor();

            _instantDelivery = new InstantDelivery(_impactExecutor);
            _projectileDelivery = new ProjectileDelivery(_impactExecutor);
            _delayedDelivery = new DelayedDelivery();

        }

        public void TryUseAbility(AbilityData ability, AbilityTargetingData targetingData)
        {
            _targetAdjuster.Adjust(firePoint, ability, ref targetingData);

            if (!_validator.CanUse(this, ability, targetingData))
                return;

            ExecuteAbility(ability, targetingData);
            StartCooldown(ability);
        }

        private void ExecuteAbility(AbilityData ability, AbilityTargetingData targetingData)
        {
            switch (ability.deliveryType)
            {
                case DeliveryType.Instant:
                    _instantDelivery.Execute(this, ability, targetingData);
                    break;
                case DeliveryType.Projectile:
                    _projectileDelivery.Execute(firePoint, GetTargetPosition(ability, targetingData), ability);
                    break;
                case DeliveryType.Delayed:
                    _delayedDelivery.Execute(this, ability, targetingData, tempDebugMaterial, _instantDelivery);
                    break;
                default:
                    break;
            }

            AbilityEvents.OnAbilityExecuted?.Invoke(ability, targetingData);

        }

        public bool IsOnCooldown(AbilityData ability)
        {
            int index = GetAbilityIndex(ability);

            if (index < 0)
                return false;

            return _cooldowns[index] > 0f;  
        }

        private void StartCooldown(AbilityData ability)
        {
            int index = GetAbilityIndex(ability);

            if (index < 0) return;

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

        private int GetAbilityIndex(AbilityData ability)
        {
            return abilities.IndexOf(ability);
        }

        private Vector3 GetTargetPosition(AbilityData ability, AbilityTargetingData targetingData)
        {
            switch (ability.targetingType)
            {
                case TargetingType.Point:
                    return targetingData.targetPoint;

                case TargetingType.Self:
                    return transform.position;

                case TargetingType.Target:
                    if (targetingData.target != null)
                    {
                        return targetingData.target.transform.position;
                    }

                    break;
            }
            return firePoint.position + firePoint.forward * 2f;
        }

        public Vector3 GetTargetPosition(AbilityTargetingData targetingData)
        {
            if (targetingData.target != null)
                return targetingData.target.transform.position;

            if (targetingData.hasTargetPoint)
                return targetingData.targetPoint;

            return transform.position;
        }

        public bool CanConfirmCast(AbilityData ability, AbilityTargetingData targetingData)
        {
            return _validator.CanConfirmCast(this, ability, targetingData);
        }

        public bool CanStartCast(AbilityData ability)
        {
            return _validator.CanStartCast(this,ability);
        }

    }

}
