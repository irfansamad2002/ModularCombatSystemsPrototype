using UnityEngine;

public struct AbilityTargetingData
{
    // ==== CAST / INTENT ====

    // Optional cast-time selected target
    public GameObject castTarget;

    // Raw aimed point from targeting
    public Vector3 aimPoint;

    // Shared directinal intent
    public Vector3 direction;

    // Whether aim point exists
    public bool hasAimPoint;

    // ==== IMPACT / RESOLUTION ====

    // Final validated point used at execution time
    public Vector3 impactPoint;

    // Whether impact point has been finalized
    public bool hasImpactPoint;
}