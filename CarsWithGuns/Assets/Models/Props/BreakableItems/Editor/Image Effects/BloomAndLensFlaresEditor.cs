using UnityEditor;
using System.Collections;

[System.Serializable]
[UnityEditor.CustomEditor(typeof(error))]
public class BloomAndLensFlaresEditor : Editor
{
    public SerializedProperty tweakMode;
    public SerializedProperty screenBlendMode;
    public SerializedObject serObj;
    public SerializedProperty sepBlurSpread;
    public SerializedProperty useSrcAlphaAsMask;
    public SerializedProperty bloomIntensity;
    public SerializedProperty bloomThreshhold;
    public SerializedProperty bloomBlurIterations;
    public SerializedProperty lensflares;
    public SerializedProperty hollywoodFlareBlurIterations;
    public SerializedProperty lensflareMode;
    public SerializedProperty hollyStretchWidth;
    public SerializedProperty lensflareIntensity;
    public SerializedProperty lensflareThreshhold;
    public SerializedProperty flareColorA;
    public SerializedProperty flareColorB;
    public SerializedProperty flareColorC;
    public SerializedProperty flareColorD;
    public SerializedProperty blurWidth;
    public virtual void OnEnable()
    {
        this.serObj = new SerializedObject(this.target);
        this.screenBlendMode = this.serObj.FindProperty("screenBlendMode");
        this.sepBlurSpread = this.serObj.FindProperty("sepBlurSpread");
        this.useSrcAlphaAsMask = this.serObj.FindProperty("useSrcAlphaAsMask");
        this.bloomIntensity = this.serObj.FindProperty("bloomIntensity");
        this.bloomThreshhold = this.serObj.FindProperty("bloomThreshhold");
        this.bloomBlurIterations = this.serObj.FindProperty("bloomBlurIterations");
        this.lensflares = this.serObj.FindProperty("lensflares");
        this.lensflareMode = this.serObj.FindProperty("lensflareMode");
        this.hollywoodFlareBlurIterations = this.serObj.FindProperty("hollywoodFlareBlurIterations");
        this.hollyStretchWidth = this.serObj.FindProperty("hollyStretchWidth");
        this.lensflareIntensity = this.serObj.FindProperty("lensflareIntensity");
        this.lensflareThreshhold = this.serObj.FindProperty("lensflareThreshhold");
        this.flareColorA = this.serObj.FindProperty("flareColorA");
        this.flareColorB = this.serObj.FindProperty("flareColorB");
        this.flareColorC = this.serObj.FindProperty("flareColorC");
        this.flareColorD = this.serObj.FindProperty("flareColorD");
        this.blurWidth = this.serObj.FindProperty("blurWidth");
        this.tweakMode = this.serObj.FindProperty("tweakMode");
    }

    public override void OnInspectorGUI()
    {
        this.serObj.Update();
        UnityEngine.GUILayout.Label(this.useSrcAlphaAsMask.floatValue < 0.1f ? "Current settings ignore color buffer alpha" : "Current settings use color buffer alpha as a glow mask", EditorStyles.miniBoldLabel);
        EditorGUILayout.PropertyField(this.tweakMode, new UnityEngine.GUIContent("Mode"));
        EditorGUILayout.PropertyField(this.screenBlendMode, new UnityEngine.GUIContent("Blend mode"));
        EditorGUILayout.Separator();
        // some genral tweak needs
        EditorGUILayout.PropertyField(this.bloomIntensity, new UnityEngine.GUIContent("Intensity"));
        this.bloomThreshhold.floatValue = EditorGUILayout.Slider("Threshhold", this.bloomThreshhold.floatValue, -0.05f, 1f);
        if (1 == this.tweakMode.intValue)
        {
            this.bloomBlurIterations.intValue = EditorGUILayout.IntSlider("Blur iterations", this.bloomBlurIterations.intValue, 1, 4);
        }
        else
        {
            this.bloomBlurIterations.intValue = 1;
        }
        this.sepBlurSpread.floatValue = EditorGUILayout.Slider("Blur spread", this.sepBlurSpread.floatValue, 0.1f, 10f);
        if (1 == this.tweakMode.intValue)
        {
            this.useSrcAlphaAsMask.floatValue = EditorGUILayout.Slider(new UnityEngine.GUIContent("Use alpha mask", "How much should the image alpha values (deifned by all materials, colors and textures alpha values define the bright (blooming/glowing) areas of the image"), this.useSrcAlphaAsMask.floatValue, 0f, 1f);
        }
        else
        {
            this.useSrcAlphaAsMask.floatValue = 0f;
        }
        if (1 == this.tweakMode.intValue)
        {
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(this.lensflares, new UnityEngine.GUIContent("Cast lens flares"));
            if (this.lensflares.boolValue)
            {
                // further lens flare tweakings
                if (0 != this.tweakMode.intValue)
                {
                    EditorGUILayout.PropertyField(this.lensflareMode, new UnityEngine.GUIContent(" Mode"));
                }
                else
                {
                    this.lensflareMode.enumValueIndex = 0;
                }
                EditorGUILayout.PropertyField(this.lensflareIntensity, new UnityEngine.GUIContent("Local intensity"));
                this.lensflareThreshhold.floatValue = EditorGUILayout.Slider("Local threshhold", this.lensflareThreshhold.floatValue, 0f, 1f);
                EditorGUILayout.Separator();
                if (this.lensflareMode.intValue == 0)
                {
                    // ghosting	
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(this.flareColorA, new UnityEngine.GUIContent(" 1st Color"));
                    EditorGUILayout.PropertyField(this.flareColorB, new UnityEngine.GUIContent(" 2nd Color"));
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(this.flareColorC, new UnityEngine.GUIContent(" 3rd Color"));
                    EditorGUILayout.PropertyField(this.flareColorD, new UnityEngine.GUIContent(" 4th Color"));
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    if (this.lensflareMode.intValue == 1)
                    {
                        // hollywood
                        EditorGUILayout.PropertyField(this.hollyStretchWidth, new UnityEngine.GUIContent(" Stretch width"));
                        this.hollywoodFlareBlurIterations.intValue = EditorGUILayout.IntSlider(" Blur iterations", this.hollywoodFlareBlurIterations.intValue, 1, 4);
                        EditorGUILayout.PropertyField(this.flareColorA, new UnityEngine.GUIContent(" Tint Color"));
                    }
                    else
                    {
                        if (this.lensflareMode.intValue == 2)
                        {
                            // both
                            EditorGUILayout.PropertyField(this.hollyStretchWidth, new UnityEngine.GUIContent(" Stretch width"));
                            this.hollywoodFlareBlurIterations.intValue = EditorGUILayout.IntSlider(" Blur iterations", this.hollywoodFlareBlurIterations.intValue, 1, 4);
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.PropertyField(this.flareColorA, new UnityEngine.GUIContent(" 1st Color"));
                            EditorGUILayout.PropertyField(this.flareColorB, new UnityEngine.GUIContent(" 2nd Color"));
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.PropertyField(this.flareColorC, new UnityEngine.GUIContent(" 3rd Color"));
                            EditorGUILayout.PropertyField(this.flareColorD, new UnityEngine.GUIContent(" 4th Color"));
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                }
            }
        }
        else
        {
            this.lensflares.boolValue = false;
        }
        this.serObj.ApplyModifiedProperties();
    }

}