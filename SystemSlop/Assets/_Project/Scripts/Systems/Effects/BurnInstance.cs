using Project.Core.Health;
using UnityEngine;

namespace Project.Systems.Effects
{
    public class BurnInstance : EffectInstance
    {
        private float _damage;
        private float _tickRate;
        private float _tickTimer;

        public BurnInstance(float damage, float tickRate, float tickTimer)
        {
            _damage = damage;
            _tickRate = tickRate;
        }

        protected override void OnStart()
        {
            _tickTimer = 0f;
        }

        protected override void OnTick(float deltaTime)
        {
            _tickTimer += deltaTime;

            if (_tickTimer >= _tickRate)
            {
                _tickTimer = 0f;

                var health = _target.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(_damage);
                }
            }
        }
    }
}