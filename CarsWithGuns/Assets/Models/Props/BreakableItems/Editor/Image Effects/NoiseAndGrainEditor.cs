using UnityEditor;
using System.Collections;

[System.Serializable]
[UnityEditor.CustomEditor(typeof(error))]
public class NoiseAndGrainEditor : Editor
{
    public SerializedObject serObj;
    public SerializedProperty intensityMultiplier;
    public SerializedProperty generalIntensity;
    public SerializedProperty blackIntensity;
    public SerializedProperty whiteIntensity;
    public SerializedProperty midGrey;
    public SerializedProperty dx11Grain;
    public SerializedProperty softness;
    public SerializedProperty monochrome;
    public SerializedProperty intensities;
    public SerializedProperty tiling;
    public SerializedProperty monochromeTiling;
    public SerializedProperty noiseTexture;
    public SerializedProperty filterMode;
    public virtual void OnEnable()
    {
        this.serObj = new SerializedObject(this.target);
        this.intensityMultiplier = this.serObj.FindProperty("intensityMultiplier");
        this.generalIntensity = this.serObj.FindProperty("generalIntensity");
        this.blackIntensity = this.serObj.FindProperty("blackIntensity");
        this.whiteIntensity = this.serObj.FindProperty("whiteIntensity");
        this.midGrey = this.serObj.FindProperty("midGrey");
        this.dx11Grain = this.serObj.FindProperty("dx11Grain");
        this.softness = this.serObj.FindProperty("softness");
        this.monochrome = this.serObj.FindProperty("monochrome");
        this.intensities = this.serObj.FindProperty("intensities");
        this.tiling = this.serObj.FindProperty("tiling");
        this.monochromeTiling = this.serObj.FindProperty("monochromeTiling");
        this.noiseTexture = this.serObj.FindProperty("noiseTexture");
        this.filterMode = this.serObj.FindProperty("filterMode");
    }

    public override void OnInspectorGUI()
    {
        this.serObj.Update();
        EditorGUILayout.LabelField("Overlays animated noise patterns", EditorStyles.miniLabel);
        EditorGUILayout.PropertyField(this.dx11Grain, new UnityEngine.GUIContent("DirectX 11 Grain"));
        if (this.dx11Grain.boolValue && !(this.target as NoiseAndGrain).Dx11Support())
        {
            EditorGUILayout.HelpBox("DX11 mode not supported (need shader model 5)", MessageType.Info);
        }
        EditorGUILayout.PropertyField(this.monochrome, new UnityEngine.GUIContent("Monochrome"));
        EditorGUILayout.Separator();
        EditorGUILayout.PropertyField(this.intensityMultiplier, new UnityEngine.GUIContent("Intensity Multiplier"));
        EditorGUILayout.PropertyField(this.generalIntensity, new UnityEngine.GUIContent(" General"));
        EditorGUILayout.PropertyField(this.blackIntensity, new UnityEngine.GUIContent(" Black Boost"));
        EditorGUILayout.PropertyField(this.whiteIntensity, new UnityEngine.GUIContent(" White Boost"));
        this.midGrey.floatValue = EditorGUILayout.Slider(new UnityEngine.GUIContent(" Mid Grey (for Boost)"), this.midGrey.floatValue, 0f, 1f);
        if (this.monochrome.boolValue == false)
        {
            UnityEngine.Color c = new UnityEngine.Color(this.intensities.vector3Value.x, this.intensities.vector3Value.y, this.intensities.vector3Value.z, 1f);
            c = EditorGUILayout.ColorField(new UnityEngine.GUIContent(" Color Weights"), c);
            this.intensities.vector3Value.x = c.r;
            this.intensities.vector3Value.y = c.g;
            this.intensities.vector3Value.z = c.b;
        }
        if (!this.dx11Grain.boolValue)
        {
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Noise Shape");
            EditorGUILayout.PropertyField(this.noiseTexture, new UnityEngine.GUIContent(" Texture"));
            EditorGUILayout.PropertyField(this.filterMode, new UnityEngine.GUIContent(" Filter"));
        }
        else
        {
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Noise Shape");
        }
        this.softness.floatValue = EditorGUILayout.Slider(new UnityEngine.GUIContent(" Softness"), this.softness.floatValue, 0f, 0.99f);
        if (!this.dx11Grain.boolValue)
        {
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Advanced");
            if (this.monochrome.boolValue == false)
            {
                this.tiling.vector3Value.x = EditorGUILayout.FloatField(new UnityEngine.GUIContent(" Tiling (Red)"), this.tiling.vector3Value.x);
                this.tiling.vector3Value.y = EditorGUILayout.FloatField(new UnityEngine.GUIContent(" Tiling (Green)"), this.tiling.vector3Value.y);
                this.tiling.vector3Value.z = EditorGUILayout.FloatField(new UnityEngine.GUIContent(" Tiling (Blue)"), this.tiling.vector3Value.z);
            }
            else
            {
                EditorGUILayout.PropertyField(this.monochromeTiling, new UnityEngine.GUIContent(" Tiling"));
            }
        }
        this.serObj.ApplyModifiedProperties();
    }

}