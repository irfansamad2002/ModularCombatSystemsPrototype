using Project.Systems.Abilities;
using Project.Systems.Ability;
using System.Collections;
using UnityEngine;

public class DelayedAbilityRunner : MonoBehaviour
{
    private GameObject _debugSphere;
    public void Init(AbilityUser user,
        AbilityData ability,
        AbilityContext context)
    {
        StartCoroutine(Run(user, ability, context));
        CreateDebugSphere(context.point, ability.radius);
    }

    private void CreateDebugSphere(Vector3 position, float radius)
    {
        _debugSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        _debugSphere.transform.position = position;

        _debugSphere.transform.localScale = Vector3.one * radius * 2f;

        Destroy(_debugSphere.GetComponent<Collider>());
    }

    private IEnumerator Run(AbilityUser user, AbilityData ability, AbilityContext context)
    {
        yield return new WaitForSeconds(ability.delay);

        user.ExecuteDelayedImpact(ability, context);

        Destroy(_debugSphere);
        Destroy(gameObject);
    }
}
