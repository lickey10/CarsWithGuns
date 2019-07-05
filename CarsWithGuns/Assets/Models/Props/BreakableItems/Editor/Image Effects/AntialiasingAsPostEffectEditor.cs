using UnityEditor;
using System.Collections;

[System.Serializable]
[UnityEditor.CustomEditor(typeof(error))]
public class AntialiasingAsPostEffectEditor : Editor
{
    public SerializedObject serObj;
    public SerializedProperty mode;
    public SerializedProperty showGeneratedNormals;
    public SerializedProperty offsetScale;
    public SerializedProperty blurRadius;
    public SerializedProperty dlaaSharp;
    public virtual void OnEnable()
    {
        this.serObj = new SerializedObject(this.target);
        this.mode = this.serObj.FindProperty("mode");
        this.showGeneratedNormals = this.serObj.FindProperty("showGeneratedNormals");
        this.offsetScale = this.serObj.FindProperty("offsetScale");
        this.blurRadius = this.serObj.FindProperty("blurRadius");
        this.dlaaSharp = this.serObj.FindProperty("dlaaSharp");
    }

    public override void OnInspectorGUI()
    {
        this.serObj.Update();
        UnityEngine.GUILayout.Label("Various luminance based fullscreen Antialiasing techniques", EditorStyles.miniBoldLabel);
        EditorGUILayout.PropertyField(this.mode, new UnityEngine.GUIContent("AA Technique"));
        if (this.mode.enumValueIndex == AAMode.NFAA)
        {
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(this.offsetScale, new UnityEngine.GUIContent("Edge Detect Ofs"));
            EditorGUILayout.PropertyField(this.blurRadius, new UnityEngine.GUIContent("Blur Radius"));
            EditorGUILayout.PropertyField(this.showGeneratedNormals, new UnityEngine.GUIContent("Show Normals"));
        }
        else
        {
            if (this.mode.enumValueIndex == AAMode.DLAA)
            {
                EditorGUILayout.Separator();
                EditorGUILayout.PropertyField(this.dlaaSharp, new UnityEngine.GUIContent("Sharp"));
            }
        }
        this.serObj.ApplyModifiedProperties();
    }

}