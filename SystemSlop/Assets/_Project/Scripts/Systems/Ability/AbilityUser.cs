using System.Collections.Generic;
using UnityEngine;

public class AbilityUser : MonoBehaviour
{
    [SerializeField] private List<AbilityData> abilities;

    public void UseAbility(int index, GameObject target)
    {
        if (index < 0 || index >= abilities.Count) return;

        var ability = abilities[index];

        foreach (var effect in ability.effects)
        {
            effect.Apply(target);
        }
    }
}
