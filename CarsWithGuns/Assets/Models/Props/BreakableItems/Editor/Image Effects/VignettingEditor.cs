using UnityEditor;
using System.Collections;

[System.Serializable]
[UnityEditor.CustomEditor(typeof(error))]
public class VignettingEditor : Editor
{
    public SerializedObject serObj;
    public SerializedProperty mode;
    public SerializedProperty intensity; // intensity == 0 disables pre pass (optimization)
    public SerializedProperty chromaticAberration;
    public SerializedProperty axialAberration;
    public SerializedProperty blur; // blur == 0 disables blur pass (optimization)
    public SerializedProperty blurSpread;
    public SerializedProperty luminanceDependency;
    public virtual void OnEnable()
    {
        this.serObj = new SerializedObject(this.target);
        this.mode = this.serObj.FindProperty("mode");
        this.intensity = this.serObj.FindProperty("intensity");
        this.chromaticAberration = this.serObj.FindProperty("chromaticAberration");
        this.axialAberration = this.serObj.FindProperty("axialAberration");
        this.blur = this.serObj.FindProperty("blur");
        this.blurSpread = this.serObj.FindProperty("blurSpread");
        this.luminanceDependency = this.serObj.FindProperty("luminanceDependency");
    }

    public override void OnInspectorGUI()
    {
        this.serObj.Update();
        EditorGUILayout.LabelField("Simulates camera (lens) artifacts known as 'Vignette' and 'Aberration'", EditorStyles.miniLabel);
        EditorGUILayout.PropertyField(this.intensity, new UnityEngine.GUIContent("Vignetting"));
        EditorGUILayout.PropertyField(this.blur, new UnityEngine.GUIContent(" Blurred Corners"));
        if (this.blur.floatValue > 0f)
        {
            EditorGUILayout.PropertyField(this.blurSpread, new UnityEngine.GUIContent(" Blur Distance"));
        }
        EditorGUILayout.Separator();
        EditorGUILayout.PropertyField(this.mode, new UnityEngine.GUIContent("Aberration Mode"));
        if (this.mode.intValue > 0)
        {
            EditorGUILayout.PropertyField(this.chromaticAberration, new UnityEngine.GUIContent("  Tangential Aberration"));
            EditorGUILayout.PropertyField(this.axialAberration, new UnityEngine.GUIContent("  Axial Aberration"));
            this.luminanceDependency.floatValue = EditorGUILayout.Slider("  Contrast Dependency", this.luminanceDependency.floatValue, 0.001f, 1f);
        }
        else
        {
            EditorGUILayout.PropertyField(this.chromaticAberration, new UnityEngine.GUIContent(" Chromatic Aberration"));
        }
        this.serObj.ApplyModifiedProperties();
    }

}