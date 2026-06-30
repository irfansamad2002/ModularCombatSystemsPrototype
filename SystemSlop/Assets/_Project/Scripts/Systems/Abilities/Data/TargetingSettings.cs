using UnityEngine;
[System.Serializable]
public class TargetingSettings
{
    public TargetingType targetingType;
    public CastMode castMode;
    public float castRange;
    public GameObject indicatorPrefab;//Optional : if null which mean no need indicators

}
