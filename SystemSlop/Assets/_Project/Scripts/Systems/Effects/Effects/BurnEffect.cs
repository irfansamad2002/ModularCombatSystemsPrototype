using UnityEngine;

namespace Project.Systems.Effects
{
    [CreateAssetMenu(menuName = "Effects/Burn")]
    public class BurnEffect : EffectData
    {
        public float damagePerTick = 1f;
        public float tickRate = 1f;
        public float duration = 5f;


        public override void Apply(GameObject target, AbilityTargetingData context, float multiplier = 1)
        {
            var handler = target.GetComponent<EffectHandler>();
            if (handler == null) return;

            var instance = new BurnInstance(
                damagePerTick * multiplier,
                tickRate);

            instance.Init(target, duration, effectId);

            handler.AddEffect(instance);
        }
    }
}