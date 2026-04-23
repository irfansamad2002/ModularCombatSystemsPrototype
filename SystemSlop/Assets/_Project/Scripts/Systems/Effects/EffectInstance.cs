using UnityEngine;

namespace Project.Systems.Effects
{
    public abstract class EffectInstance
    {
        protected GameObject _target;
        protected float _duration;
        protected float _timer;

        public void Init(GameObject target, float duration)
        {
            _target = target;
            _duration = duration;
            OnStart();
        }

        public void Tick(float deltaTime)
        {
            _timer += deltaTime;
            OnTick(deltaTime);
        }

        public bool IsFinished()
        {
            return _timer >= _duration;
        }

        protected abstract void OnStart();
        protected abstract void OnTick(float deltaTime);
    }
}