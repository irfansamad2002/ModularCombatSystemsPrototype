using Project.Systems.Abilities.Data;
using UnityEditor;
[CustomEditor(typeof(AbilityData))]
public class AbilityDataEditor : Editor
{
    // General Settings
    private SerializedProperty abilityNameProp;
    private SerializedProperty descriptionProp;
    private SerializedProperty iconProp;
    private SerializedProperty cooldownProp;

    private SerializedProperty targetingSettingsProp;
    private SerializedProperty targetingTypeProp;
    private SerializedProperty castModeProp;
    private SerializedProperty castRangeProp;
    private SerializedProperty indicatorPrefabProp;

    private SerializedProperty deliverySettingsProp;
    private SerializedProperty deliveryTypeProp;
    private SerializedProperty projectileProp;
    private SerializedProperty delayProp;
    private SerializedProperty telegraphVFXProp;
    private SerializedProperty impactVFXProp;

    private SerializedProperty impactSettingsProp;
    private SerializedProperty radiusProp;
    private SerializedProperty areaShapeProp;
    private SerializedProperty coneAngleProp;
    private SerializedProperty targetLayersProp;


    private void OnEnable()
    {
        abilityNameProp = serializedObject.FindProperty("abilityName");
        descriptionProp = serializedObject.FindProperty("description");
        iconProp = serializedObject.FindProperty("icon");
        cooldownProp = serializedObject.FindProperty("cooldown");

        targetingSettingsProp = serializedObject.FindProperty("targetingSettings");
        targetingTypeProp = targetingSettingsProp.FindPropertyRelative("targetingType");
        castModeProp = targetingSettingsProp.FindPropertyRelative("castMode");
        castRangeProp = targetingSettingsProp.FindPropertyRelative("castRange");
        indicatorPrefabProp = targetingSettingsProp.FindPropertyRelative("indicatorPrefab");

        deliverySettingsProp = serializedObject.FindProperty("deliverySettings");
        deliveryTypeProp = deliverySettingsProp.FindPropertyRelative("deliveryType");
        projectileProp = deliverySettingsProp.FindPropertyRelative("projectile");
        delayProp = deliverySettingsProp.FindPropertyRelative("delay");
        telegraphVFXProp = deliverySettingsProp.FindPropertyRelative("telegraphVFX");
        impactVFXProp = deliverySettingsProp.FindPropertyRelative("impactVFX");

        impactSettingsProp = serializedObject.FindProperty("impactSettings");
        radiusProp = impactSettingsProp.FindPropertyRelative("radius");
        areaShapeProp = impactSettingsProp.FindPropertyRelative("areaShape");
        coneAngleProp = impactSettingsProp.FindPropertyRelative("coneAngle");
        targetLayersProp = impactSettingsProp.FindPropertyRelative("targetLayers");

    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawGeneral();
        DrawTargeting();
        DrawDelivery();
        DrawImpact();

        serializedObject.ApplyModifiedProperties();

    }

    private void DrawGeneral()
    {
        EditorGUILayout.LabelField("General", EditorStyles.boldLabel);
            
        EditorGUILayout.PropertyField(abilityNameProp);
        EditorGUILayout.PropertyField(descriptionProp);
        EditorGUILayout.PropertyField(iconProp);
        EditorGUILayout.PropertyField(cooldownProp);

        EditorGUILayout.Space();
    }

    private void DrawTargeting()
    {
        EditorGUILayout.LabelField("Targeting", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(targetingTypeProp);
        EditorGUILayout.PropertyField(castModeProp);

        var targetingType = (TargetingType)targetingTypeProp.enumValueIndex;

        if (targetingType == TargetingType.Point ||
           targetingType == TargetingType.Target)
        {
            EditorGUILayout.PropertyField(castRangeProp);
        }

        if ((CastMode)castModeProp.enumValueIndex == CastMode.Confirm)
        {
            EditorGUILayout.PropertyField(indicatorPrefabProp);
        }

        EditorGUILayout.Space();
    }

    private void DrawDelivery()
    {
        EditorGUILayout.LabelField("Delivery", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(deliveryTypeProp);
        var deliveryType = (DeliveryType)deliveryTypeProp.enumValueIndex;
        switch (deliveryType)
        {
            case DeliveryType.Instant:
                break;
            case DeliveryType.Projectile:
                EditorGUILayout.PropertyField(projectileProp);
                EditorGUILayout.PropertyField(impactVFXProp);
                break;
            case DeliveryType.Delayed:
                EditorGUILayout.PropertyField(delayProp);
                EditorGUILayout.PropertyField(telegraphVFXProp);
                EditorGUILayout.PropertyField(impactVFXProp);
                break;
            default:
                break;
        }
        EditorGUILayout.Space();

    }

    private void DrawImpact()
    {
        EditorGUILayout.LabelField("Impact", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(areaShapeProp);

        var areaShape = (AreaShape)areaShapeProp.enumValueIndex;

        switch (areaShape)
        {
            case AreaShape.None:
                break;

            case AreaShape.Sphere:
                EditorGUILayout.PropertyField(radiusProp);
                EditorGUILayout.PropertyField(targetLayersProp);
                break;

            case AreaShape.Cone:
                EditorGUILayout.PropertyField(radiusProp);
                EditorGUILayout.PropertyField(coneAngleProp);
                EditorGUILayout.PropertyField(targetLayersProp);
                break;
            default:
                break;
        }
        EditorGUILayout.Space();
    }
}
