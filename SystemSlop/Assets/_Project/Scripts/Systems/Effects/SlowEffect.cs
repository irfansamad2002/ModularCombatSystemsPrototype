using UnityEngine;

namespace Project.Systems.Effects
{
    [CreateAssetMenu(menuName = "Effects/Slow")]
    public class SlowEffect : EffectData
    {
        public float slowMultiplier = .5f;
        public float duration = 3f;

        public override void Apply(GameObject target,AbilityContext context, float multiplier = 1)
        {
            var handler = target.GetComponent<EffectHandler>();
            if (handler == null) return;

            var instance = new SlowInstance(slowMultiplier, duration);
            instance.Init(target, duration);

            handler.AddEffect(instance);
        }

    }
}