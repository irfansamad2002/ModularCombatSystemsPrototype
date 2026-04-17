using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Damage")]
public class DamageEffect : EffectData
{
    public int damage;

    public override void Apply(GameObject target)
    {
        var health = target.GetComponent<Health>();

        if ((health != null))
        {
            Debug.Log("DamageEffect applied to " + target.name + " for " + damage + " damage.");
            health.TakeDamage(damage);
        }
    }
}
