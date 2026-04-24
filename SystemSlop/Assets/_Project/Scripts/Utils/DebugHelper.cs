using UnityEngine;

public class DebugHelper
{
   public static void WarnMissing(GameObject target, string componentName)
    {
        Debug.LogWarning($"[{componentName}] Missing on {target.name}", target);
    }
}
