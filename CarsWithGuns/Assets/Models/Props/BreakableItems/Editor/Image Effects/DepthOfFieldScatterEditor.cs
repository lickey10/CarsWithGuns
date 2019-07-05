using UnityEditor;
using System.Collections;

[System.Serializable]
[UnityEditor.CustomEditor(typeof(error))]
public class DepthOfFieldScatterEditor : Editor
{
    public SerializedObject serObj;
    public SerializedProperty visualizeFocus;
    public SerializedProperty focalLength;
    public SerializedProperty focalSize;
    public SerializedProperty aperture;
    public SerializedProperty focalTransform;
    public SerializedProperty maxBlurSize;
    public SerializedProperty highResolution;
    public SerializedProperty blurType;
    public SerializedProperty blurSampleCount;
    public SerializedProperty nearBlur;
    public SerializedProperty foregroundOverlap;
    public SerializedProperty dx11BokehThreshhold;
    public SerializedProperty dx11SpawnHeuristic;
    public SerializedProperty dx11BokehTexture;
    public SerializedProperty dx11BokehScale;
    public SerializedProperty dx11BokehIntensity;
    public virtual void OnEnable()
    {
        this.serObj = new SerializedObject(this.target);
        this.visualizeFocus = this.serObj.FindProperty("visualizeFocus");
        this.focalLength = this.serObj.FindProperty("focalLength");
        this.focalSize = this.serObj.FindProperty("focalSize");
        this.aperture = this.serObj.FindProperty("aperture");
        this.focalTransform = this.serObj.FindProperty("focalTransform");
        this.maxBlurSize = this.serObj.FindProperty("maxBlurSize");
        this.highResolution = this.serObj.FindProperty("highResolution");
        this.blurType = this.serObj.FindProperty("blurType");
        this.blurSampleCount = this.serObj.FindProperty("blurSampleCount");
        this.nearBlur = this.serObj.FindProperty("nearBlur");
        this.foregroundOverlap = this.serObj.FindProperty("foregroundOverlap");
        this.dx11BokehThreshhold = this.serObj.FindProperty("dx11BokehThreshhold");
        this.dx11SpawnHeuristic = this.serObj.FindProperty("dx11SpawnHeuristic");
        this.dx11BokehTexture = this.serObj.FindProperty("dx11BokehTexture");
        this.dx11BokehScale = this.serObj.FindProperty("dx11BokehScale");
        this.dx11BokehIntensity = this.serObj.FindProperty("dx11BokehIntensity");
    }

    public override void OnInspectorGUI()
    {
        this.serObj.Update();
        EditorGUILayout.LabelField("Simulates camera lens defocus", EditorStyles.miniLabel);
        UnityEngine.GUILayout.Label("Focal Settings");
        EditorGUILayout.PropertyField(this.visualizeFocus, new UnityEngine.GUIContent(" Visualize"));
        EditorGUILayout.PropertyField(this.focalLength, new UnityEngine.GUIContent(" Focal Distance"));
        EditorGUILayout.PropertyField(this.focalSize, new UnityEngine.GUIContent(" Focal Size"));
        EditorGUILayout.PropertyField(this.focalTransform, new UnityEngine.GUIContent(" Focus on Transform"));
        EditorGUILayout.PropertyField(this.aperture, new UnityEngine.GUIContent(" Aperture"));
        EditorGUILayout.Separator();
        EditorGUILayout.PropertyField(this.blurType, new UnityEngine.GUIContent("Defocus Type"));
        if (!(this.target as DepthOfFieldScatter).Dx11Support() && (this.blurType.enumValueIndex > 0))
        {
            EditorGUILayout.HelpBox("DX11 mode not supported (need shader model 5)", MessageType.Info);
        }
        if (this.blurType.enumValueIndex < 1)
        {
            EditorGUILayout.PropertyField(this.blurSampleCount, new UnityEngine.GUIContent(" Sample Count"));
        }
        EditorGUILayout.PropertyField(this.maxBlurSize, new UnityEngine.GUIContent(" Max Blur Distance"));
        EditorGUILayout.PropertyField(this.highResolution, new UnityEngine.GUIContent(" High Resolution"));
        EditorGUILayout.Separator();
        EditorGUILayout.PropertyField(this.nearBlur, new UnityEngine.GUIContent("Near Blur"));
        EditorGUILayout.PropertyField(this.foregroundOverlap, new UnityEngine.GUIContent("  Overlap Size"));
        EditorGUILayout.Separator();
        if (this.blurType.enumValueIndex > 0)
        {
            UnityEngine.GUILayout.Label("DX11 Bokeh Settings");
            EditorGUILayout.PropertyField(this.dx11BokehTexture, new UnityEngine.GUIContent(" Bokeh Texture"));
            EditorGUILayout.PropertyField(this.dx11BokehScale, new UnityEngine.GUIContent(" Bokeh Scale"));
            EditorGUILayout.PropertyField(this.dx11BokehIntensity, new UnityEngine.GUIContent(" Bokeh Intensity"));
            EditorGUILayout.PropertyField(this.dx11BokehThreshhold, new UnityEngine.GUIContent(" Min Luminance"));
            EditorGUILayout.PropertyField(this.dx11SpawnHeuristic, new UnityEngine.GUIContent(" Spawn Heuristic"));
        }
        this.serObj.ApplyModifiedProperties();
    }

}