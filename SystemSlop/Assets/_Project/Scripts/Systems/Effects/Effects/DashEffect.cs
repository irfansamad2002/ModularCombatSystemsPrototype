using Project.Systems.Ability;
using UnityEngine;

namespace Project.Systems.Effects
{
    [CreateAssetMenu(menuName = "Effects/Dash")]
    public class DashEffect : EffectData
    {
        public float dashDistance = 5f;

        public override void Apply(GameObject target , AbilityTargetingData context, float multiplier = 1)
        {
            var controller = target.GetComponent<CharacterController>();

            if (controller == null)
            {
                DebugHelper.WarnMissingComponent(target, nameof(CharacterController));
                return;
            }
            //Debug.Log(context.direction);
            controller.Move(context.direction * dashDistance);


        }
    }
}

