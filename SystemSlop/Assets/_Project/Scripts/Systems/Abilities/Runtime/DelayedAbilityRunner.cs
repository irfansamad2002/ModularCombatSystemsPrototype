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

        private InstantDelivery _instantDelivery;

        public void Init(AbilityUser user,
            AbilityData ability,
            AbilityTargetingData targetingData,
            Material debugMaterial,
            InstantDelivery instantDelivery)
        {
            _instantDelivery = instantDelivery;

            StartCoroutine(Run(user, ability, targetingData));
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

        private IEnumerator Run(AbilityUser user, AbilityData ability, AbilityTargetingData targetingData)
        {
            _telegraphInstace = SpawnTelegraphVFX(ability, targetingData);

            yield return new WaitForSeconds(ability.deliverySettings.delay);

            targetingData.impactPoint = targetingData.targetPoint;
            targetingData.hasImpactPoint = true;

            _instantDelivery.Execute(user, ability, targetingData);

            SpawnImpactVFX(ability, targetingData);

            Destroy(_debugSphere);
            Destroy(_telegraphInstace);
            Destroy(gameObject);
        }



        private GameObject SpawnTelegraphVFX(AbilityData ability, AbilityTargetingData context)
        {
            if (ability.deliverySettings.deliveryType != DeliveryType.Delayed) return null;

            if (ability.deliverySettings.telegraphVFX == null) return null;

            GameObject obj = Instantiate(ability.deliverySettings.telegraphVFX, context.targetPoint,Quaternion.identity);

            var telegraphVFX = obj.GetComponent<DelayedTelegraphVFX>();

            if (telegraphVFX != null)
            {
                telegraphVFX.Init(ability.deliverySettings.delay);
            }

            return obj;
        }

        private void SpawnImpactVFX(AbilityData ability, AbilityTargetingData context)
        {
            if (ability.deliverySettings.impactVFX == null)
                return;

            Instantiate(
                ability.deliverySettings.impactVFX,
                context.targetPoint,
                Quaternion.identity
            );
        }

    }
}