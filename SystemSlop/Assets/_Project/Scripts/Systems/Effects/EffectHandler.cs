using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Systems.Effects
{
    public class EffectHandler : MonoBehaviour
    {
        private List<EffectInstance> _activeEffects = new();

        private void Update()
        {
            for (int i = _activeEffects.Count - 1; i >= 0; i--)
            {
                var effects = _activeEffects[i];

                effects.Tick(Time.deltaTime);

                if(effects.IsFinished())
                {
                    _activeEffects.RemoveAt(i);
                }
            }
        }

        public void AddEffect(EffectInstance newEffect)
        {
            for (int i = 0; i < _activeEffects.Count; i++)
            {
                if (_activeEffects[i].EffectId == newEffect.EffectId)
                {
                    _activeEffects[i].OnReapply(newEffect);
                    return;
                }
            }
            _activeEffects.Add(newEffect);
        }
    }
}