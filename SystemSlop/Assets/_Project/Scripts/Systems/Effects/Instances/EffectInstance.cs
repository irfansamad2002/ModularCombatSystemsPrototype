using UnityEngine;

namespace Project.Systems.Effects
{
    public abstract class EffectInstance
    {
        public string EffectId { get; private set; }

        protected GameObject _target;
        protected float _duration;
        protected float _timer;

        public void Init(GameObject target, float duration, string effectId)
        {
            _target = target;
            _duration = duration;
            EffectId = effectId;
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

        public virtual void OnReapply(EffectInstance newInstance) { }
        public virtual void OnEnd() { }

        protected abstract void OnStart();
        protected abstract void OnTick(float deltaTime);
    }
}