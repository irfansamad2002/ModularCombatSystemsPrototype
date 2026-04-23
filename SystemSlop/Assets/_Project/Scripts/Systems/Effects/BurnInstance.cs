using Project.Core.Health;
using UnityEngine;

namespace Project.Systems.Effects
{
    public class BurnInstance : EffectInstance
    {
        private float _damage;
        private float _tickRate;
        private float _tickTimer;

        private int _stacks = 1;
        private int _maxStacks = 5;

        public BurnInstance(float damage, float tickRate, float tickTimer)
        {
            EffectId = "burn";

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
                    float totalDamage = _damage * _stacks;
                    health.TakeDamage(totalDamage);
                }
            }
        }
    
        public override void OnReapply(EffectInstance newInstance)
        {
            //refresh duration
            _timer = 0;

            //Stack up
            if (_stacks < _maxStacks)
            {
                _stacks++;
            }
        }
    }
}