using Project.Systems.Abilities.Data;
using Project.Systems.Abilities.Runtime;
using System.Collections;
using UnityEngine;

namespace Project.Systems.Abilities
{
    public class DelayedAbilityRunner : MonoBehaviour
    {
        private GameObject _debugSphere;
        private GameObject _telegraphInstace;

        public void Init(AbilityUser user,
            AbilityData ability,
            ExecutionContext context,
            Material debugMaterial)
        {

            StartCoroutine(Run(user, ability, context));
            //CreateDebugSphere(context.aimPoint, ability.radius, debugMaterial);

        }

        private void CreateDebugSphere(Vector3 position, float radius, Material material)
        {
            _debugSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            _debugSphere.transform.position = position;
            _debugSphere.transform.localScale = Vector3.one * radius * 2f;

            Renderer renderer = _debugSphere.GetComponent<Renderer>();
            renderer.material = material;

            Destroy(_debugSphere.GetComponent<Collider>());
        }

        private IEnumerator Run(AbilityUser user, AbilityData ability, ExecutionContext context)
        {
            _telegraphInstace = SpawnTelegraphVFX(ability, context);

            yield return new WaitForSeconds(ability.delay);

            //var impact = BuildImpactContext(ability, context);
            context.impactPoint = context.aimPoint;
            context.hasImpactPoint = true;
            user.ResolveAreaImpact(ability, context);

            SpawnImpactVFX(ability, context);
            Destroy(_debugSphere);
            Destroy(gameObject);
        }

        
        
        private GameObject SpawnTelegraphVFX(AbilityData ability, ExecutionContext context)
        {
            if (ability.deliveryType != DeliveryType.Delayed) return null;

            if (ability.telegraphVFX == null) return null;

            GameObject obj = Instantiate(ability.telegraphVFX, context.aimPoint,Quaternion.identity);

            var telegraphVFX = obj.GetComponent<DelayedTelegraphVFX>();

            if (telegraphVFX != null)
            {
                telegraphVFX.Init(ability.delay);
            }

            return obj;
        }

        private void SpawnImpactVFX(AbilityData ability, ExecutionContext context)
        {
            if (ability.impactVFX == null)
                return;

            Instantiate(
                ability.impactVFX,
                context.aimPoint,
                Quaternion.identity
            );
        }

    }
}