using Project.Systems.Abilities;
using Project.Systems.Abilities.Data;
using Project.Systems.Abilities.Runtime;
using UnityEngine;

public class DelayedDelivery
{

    public void Execute(AbilityUser user, AbilityData ability, AbilityTargetingData targetingData, Material debugMaterial, InstantDelivery instantDelivery)
    {
        GameObject runnerGO = new GameObject("Delayed Ability");

        var runner = runnerGO.AddComponent<DelayedAbilityRunner>();

        runner.Init(user, ability, targetingData, debugMaterial, instantDelivery);
    }
}
