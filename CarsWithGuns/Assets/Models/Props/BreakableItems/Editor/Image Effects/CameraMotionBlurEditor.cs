using UnityEditor;
using System.Collections;

[System.Serializable]
[UnityEditor.CustomEditor(typeof(error))]
public class CameraMotionBlurEditor : Editor
{
    public SerializedObject serObj;
    public SerializedProperty filterType;
    public SerializedProperty preview2;
    public SerializedProperty previewScale;
    public SerializedProperty movementScale;
    public SerializedProperty rotationScale;
    public SerializedProperty maxVelocity;
    public SerializedProperty minVelocity;
    public SerializedProperty maxNumSamples;
    public SerializedProperty velocityScale;
    public SerializedProperty velocityDownsample;
    public SerializedProperty noiseTexture;
    public SerializedProperty showVelocity;
    public SerializedProperty showVelocityScale;
    public SerializedProperty excludeLayers;
    //var dynamicLayers : SerializedProperty;
    public virtual void OnEnable()
    {
        this.serObj = new SerializedObject(this.target);
        this.filterType = this.serObj.FindProperty("filterType");
        this.preview2 = this.serObj.FindProperty("preview");
        this.previewScale = this.serObj.FindProperty("previewScale");
        this.movementScale = this.serObj.FindProperty("movementScale");
        this.rotationScale = this.serObj.FindProperty("rotationScale");
        this.maxVelocity = this.serObj.FindProperty("maxVelocity");
        this.minVelocity = this.serObj.FindProperty("minVelocity");
        this.maxNumSamples = this.serObj.FindProperty("maxNumSamples");
        this.excludeLayers = this.serObj.FindProperty("excludeLayers");
        //dynamicLayers = serObj.FindProperty ("dynamicLayers");
        this.velocityScale = this.serObj.FindProperty("velocityScale");
        this.velocityDownsample = this.serObj.FindProperty("velocityDownsample");
        this.noiseTexture = this.serObj.FindProperty("noiseTexture");
    }

    public override void OnInspectorGUI()
    {
        this.serObj.Update();
        EditorGUILayout.LabelField("Simulates camera based motion blur", EditorStyles.miniLabel);
        EditorGUILayout.PropertyField(this.filterType, new UnityEngine.GUIContent("Technique"));
        if ((this.filterType.enumValueIndex == 3) && !(this.target as CameraMotionBlur).Dx11Support())
        {
            EditorGUILayout.HelpBox("DX11 mode not supported (need shader model 5)", MessageType.Info);
        }
        EditorGUILayout.PropertyField(this.velocityScale, new UnityEngine.GUIContent(" Velocity Scale"));
        if (this.filterType.enumValueIndex >= 2)
        {
            EditorGUILayout.LabelField(" Tile size used during reconstruction filter:", EditorStyles.miniLabel);
            EditorGUILayout.PropertyField(this.maxVelocity, new UnityEngine.GUIContent("  Velocity Max"));
        }
        else
        {
            EditorGUILayout.PropertyField(this.maxVelocity, new UnityEngine.GUIContent(" Velocity Max"));
        }
        EditorGUILayout.PropertyField(this.minVelocity, new UnityEngine.GUIContent(" Velocity Min"));
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Technique Specific");
        if (this.filterType.enumValueIndex == 0)
        {
            // portal style motion blur
            EditorGUILayout.PropertyField(this.rotationScale, new UnityEngine.GUIContent(" Camera Rotation"));
            EditorGUILayout.PropertyField(this.movementScale, new UnityEngine.GUIContent(" Camera Movement"));
        }
        else
        {
            // "plausible" blur or cheap, local blur
            EditorGUILayout.PropertyField(this.excludeLayers, new UnityEngine.GUIContent(" Exclude Layers"));
            EditorGUILayout.PropertyField(this.velocityDownsample, new UnityEngine.GUIContent(" Velocity Downsample"));
            this.velocityDownsample.intValue = this.velocityDownsample.intValue < 1 ? 1 : this.velocityDownsample.intValue;
            if (this.filterType.enumValueIndex >= 2) // only display jitter for reconstruction
            {
                EditorGUILayout.PropertyField(this.noiseTexture, new UnityEngine.GUIContent(" Sample Jitter"));
            }
            if (this.filterType.enumValueIndex > 2) // DX11
            {
                this.maxNumSamples.intValue = EditorGUILayout.IntSlider(" Max Sample Count", this.maxNumSamples.intValue, 6, 32);
            }
        }
        EditorGUILayout.Separator();
        EditorGUILayout.PropertyField(this.preview2, new UnityEngine.GUIContent("Preview"));
        if (this.preview2.boolValue)
        {
            EditorGUILayout.PropertyField(this.previewScale, new UnityEngine.GUIContent(" Preview Scale"));
        }
        this.serObj.ApplyModifiedProperties();
    }

}