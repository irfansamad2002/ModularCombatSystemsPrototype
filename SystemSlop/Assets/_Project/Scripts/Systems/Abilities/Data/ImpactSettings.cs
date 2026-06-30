using UnityEngine;
[System.Serializable]
public class ImpactSettings
{
    public float radius;
    public AreaShape areaShape;
    [Range(0f, 360f)]
    public float coneAngle = 90f;
    public LayerMask targetLayers;



}
