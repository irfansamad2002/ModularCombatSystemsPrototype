
using Project.Systems.Abilities.Data;
using System;
namespace Project.Core.Event
{
    public static class AbilityEvents
    {
        public static Action<AbilityData, ExecutionContext> OnAbilityExecuted;
    }   
}
