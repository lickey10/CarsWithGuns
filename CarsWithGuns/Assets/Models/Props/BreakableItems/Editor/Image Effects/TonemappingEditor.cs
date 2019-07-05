using UnityEngine;
using UnityEditor;
using System.Collections;

[System.Serializable]
[UnityEditor.CustomEditor(typeof(error))]
public class TonemappingEditor : Editor
{
    public SerializedObject serObj;
    public SerializedProperty type;
    // CURVE specific parameter
    public SerializedProperty remapCurve;
    public SerializedProperty exposureAdjustment;
    // REINHARD specific parameter
    public SerializedProperty middleGrey;
    public SerializedProperty white;
    public SerializedProperty adaptionSpeed;
    public SerializedProperty adaptiveTextureSize;
    public virtual void OnEnable()
    {
        this.serObj = new SerializedObject(this.target);
        this.type = this.serObj.FindProperty("type");
        this.remapCurve = this.serObj.FindProperty("remapCurve");
        this.exposureAdjustment = this.serObj.FindProperty("exposureAdjustment");
        this.middleGrey = this.serObj.FindProperty("middleGrey");
        this.white = this.serObj.FindProperty("white");
        this.adaptionSpeed = this.serObj.FindProperty("adaptionSpeed");
        this.adaptiveTextureSize = this.serObj.FindProperty("adaptiveTextureSize");
    }

    public override void OnInspectorGUI()
    {
        this.serObj.Update();
        GUILayout.Label("Mapping HDR to LDR ranges since 1982", EditorStyles.miniLabel);
        Camera cam = (this.target as Tonemapping).GetComponent<Camera>();
        if (cam != null)
        {
            if (!cam.allowHDR)
            {
                EditorGUILayout.HelpBox("The camera is not HDR enabled. This will likely break the Tonemapper.", MessageType.Warning);
            }
            else
            {
                if (!(this.target as Tonemapping).validRenderTextureFormat)
                {
                    EditorGUILayout.HelpBox("The input to Tonemapper is not in HDR. Make sure that all effects prior to this are executed in HDR.", MessageType.Warning);
                }
            }
        }
        EditorGUILayout.PropertyField(this.type, new GUIContent("Technique"));
        if (this.type.enumValueIndex == Tonemapping.TonemapperType.UserCurve)
        {
            EditorGUILayout.PropertyField(this.remapCurve, new GUIContent("Remap curve", "Specify the mapping of luminances yourself"));
        }
        else
        {
            if (this.type.enumValueIndex == Tonemapping.TonemapperType.SimpleReinhard)
            {
                EditorGUILayout.PropertyField(this.exposureAdjustment, new GUIContent("Exposure", "Exposure adjustment"));
            }
            else
            {
                if (this.type.enumValueIndex == Tonemapping.TonemapperType.Hable)
                {
                    EditorGUILayout.PropertyField(this.exposureAdjustment, new GUIContent("Exposure", "Exposure adjustment"));
                }
                else
                {
                    if (this.type.enumValueIndex == Tonemapping.TonemapperType.Photographic)
                    {
                        EditorGUILayout.PropertyField(this.exposureAdjustment, new GUIContent("Exposure", "Exposure adjustment"));
                    }
                    else
                    {
                        if (this.type.enumValueIndex == Tonemapping.TonemapperType.OptimizedHejiDawson)
                        {
                            EditorGUILayout.PropertyField(this.exposureAdjustment, new GUIContent("Exposure", "Exposure adjustment"));
                        }
                        else
                        {
                            if (this.type.enumValueIndex == Tonemapping.TonemapperType.AdaptiveReinhard)
                            {
                                EditorGUILayout.PropertyField(this.middleGrey, new GUIContent("Middle grey", "Middle grey defines the average luminance thus brightening or darkening the entire image."));
                                EditorGUILayout.PropertyField(this.white, new GUIContent("White", "Smallest luminance value that will be mapped to white"));
                                EditorGUILayout.PropertyField(this.adaptionSpeed, new GUIContent("Adaption Speed", "Speed modifier for the automatic adaption"));
                                EditorGUILayout.PropertyField(this.adaptiveTextureSize, new GUIContent("Texture size", "Defines the amount of downsamples needed."));
                            }
                            else
                            {
                                if (this.type.enumValueIndex == Tonemapping.TonemapperType.AdaptiveReinhardAutoWhite)
                                {
                                    EditorGUILayout.PropertyField(this.middleGrey, new GUIContent("Middle grey", "Middle grey defines the average luminance thus brightening or darkening the entire image."));
                                    EditorGUILayout.PropertyField(this.adaptionSpeed, new GUIContent("Adaption Speed", "Speed modifier for the automatic adaption"));
                                    EditorGUILayout.PropertyField(this.adaptiveTextureSize, new GUIContent("Texture size", "Defines the amount of downsamples needed."));
                                }
                            }
                        }
                    }
                }
            }
        }
        GUILayout.Label("All following effects will use LDR color buffers", EditorStyles.miniBoldLabel);
        this.serObj.ApplyModifiedProperties();
    }

}