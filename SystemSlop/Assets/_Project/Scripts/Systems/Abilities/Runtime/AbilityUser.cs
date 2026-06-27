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
        public LayerMask TargetLayer => _targetLayer;
        private float[] _cooldowns;

        private ProjectileDelivery _projectileDelivery;
        private DelayedDelivery _delayedDelivery;
        private InstantDelivery _instantDelivery;

        private void Awake()
        {
            _cooldowns = new float[abilities.Count];
            _instantDelivery = new InstantDelivery();
            _projectileDelivery = new ProjectileDelivery();
            _delayedDelivery = new DelayedDelivery();
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
                    return context.hasTargetPoint;

                case TargetingType.Target:
                    return context.target != null;

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
                    return context.hasTargetPoint;

                case TargetingType.Target:
                    return context.target != null;

                case TargetingType.Self:
                case TargetingType.None:
                    return true;
            }
            return false;
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

    }
}
