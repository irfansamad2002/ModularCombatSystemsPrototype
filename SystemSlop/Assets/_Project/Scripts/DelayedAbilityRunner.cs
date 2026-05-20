using Project.Systems.Abilities;
using Project.Systems.Ability;
using System.Collections;
using UnityEngine;
/// <summary>
/// Executes delayed ability impact after a fixed delay.
/// Acts as a temporary runtime runner that waits, then triggers
/// the AbilityUser delayed impact pipeline using stored AbilityContext.
/// Handles lifetime of delayed VFX/debug visuals and self-destruction.
/// </summary>
public class DelayedAbilityRunner : MonoBehaviour
{
    private Material _debugMaterial;

    private GameObject _debugSphere;
    public void Init(AbilityUser user,
        AbilityData ability,
        ExecutionContext context,
        Material debugMaterial)
    {
        StartCoroutine(Run(user, ability, context));
        CreateDebugSphere(context.aimPoint, ability.radius, debugMaterial);
        _debugMaterial = debugMaterial;
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
        yield return new WaitForSeconds(ability.delay);

        //var impact = BuildImpactContext(ability, context);
        context.impactPoint = context.aimPoint;
        context.hasImpactPoint = true;
        user.ResolveAreaImpact(ability, context);

        Destroy(_debugSphere);
        Destroy(gameObject);
    }

   
}
