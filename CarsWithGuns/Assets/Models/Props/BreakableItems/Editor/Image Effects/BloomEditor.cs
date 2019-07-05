using UnityEngine;
using UnityEditor;
using System.Collections;

[System.Serializable]
[UnityEditor.CustomEditor(typeof(error))]
public class BloomEditor : Editor
{
    public SerializedProperty tweakMode;
    public SerializedProperty screenBlendMode;
    public SerializedObject serObj;
    public SerializedProperty hdr;
    public SerializedProperty quality;
    public SerializedProperty sepBlurSpread;
    public SerializedProperty bloomIntensity;
    public SerializedProperty bloomThreshholdColor;
    public SerializedProperty bloomThreshhold;
    public SerializedProperty bloomBlurIterations;
    public SerializedProperty hollywoodFlareBlurIterations;
    public SerializedProperty lensflareMode;
    public SerializedProperty hollyStretchWidth;
    public SerializedProperty lensflareIntensity;
    public SerializedProperty flareRotation;
    public SerializedProperty lensFlareSaturation;
    public SerializedProperty lensflareThreshhold;
    public SerializedProperty flareColorA;
    public SerializedProperty flareColorB;
    public SerializedProperty flareColorC;
    public SerializedProperty flareColorD;
    public SerializedProperty blurWidth;
    public SerializedProperty lensFlareVignetteMask;
    public virtual void OnEnable()
    {
        this.serObj = new SerializedObject(this.target);
        this.screenBlendMode = this.serObj.FindProperty("screenBlendMode");
        this.hdr = this.serObj.FindProperty("hdr");
        this.quality = this.serObj.FindProperty("quality");
        this.sepBlurSpread = this.serObj.FindProperty("sepBlurSpread");
        this.bloomIntensity = this.serObj.FindProperty("bloomIntensity");
        this.bloomThreshhold = this.serObj.FindProperty("bloomThreshhold");
        this.bloomThreshholdColor = this.serObj.FindProperty("bloomThreshholdColor");
        this.bloomBlurIterations = this.serObj.FindProperty("bloomBlurIterations");
        this.lensflareMode = this.serObj.FindProperty("lensflareMode");
        this.hollywoodFlareBlurIterations = this.serObj.FindProperty("hollywoodFlareBlurIterations");
        this.hollyStretchWidth = this.serObj.FindProperty("hollyStretchWidth");
        this.lensflareIntensity = this.serObj.FindProperty("lensflareIntensity");
        this.lensflareThreshhold = this.serObj.FindProperty("lensflareThreshhold");
        this.lensFlareSaturation = this.serObj.FindProperty("lensFlareSaturation");
        this.flareRotation = this.serObj.FindProperty("flareRotation");
        this.flareColorA = this.serObj.FindProperty("flareColorA");
        this.flareColorB = this.serObj.FindProperty("flareColorB");
        this.flareColorC = this.serObj.FindProperty("flareColorC");
        this.flareColorD = this.serObj.FindProperty("flareColorD");
        this.blurWidth = this.serObj.FindProperty("blurWidth");
        this.lensFlareVignetteMask = this.serObj.FindProperty("lensFlareVignetteMask");
        this.tweakMode = this.serObj.FindProperty("tweakMode");
    }

    public override void OnInspectorGUI()
    {
        this.serObj.Update();
        EditorGUILayout.LabelField("Glow and Lens Flares for bright screen pixels", EditorStyles.miniLabel);
        EditorGUILayout.PropertyField(this.quality, new GUIContent("Quality", "High quality preserves high frequencies with bigger blurs and uses a better blending and down-/upsampling"));
        EditorGUILayout.Separator();
        EditorGUILayout.PropertyField(this.tweakMode, new GUIContent("Mode"));
        EditorGUILayout.PropertyField(this.screenBlendMode, new GUIContent("Blend"));
        EditorGUILayout.PropertyField(this.hdr, new GUIContent("HDR"));
        EditorGUILayout.Separator();
        // display info text when screen blend mode cannot be used
        Camera cam = (this.target as Bloom).GetComponent<Camera>();
        if (cam != null)
        {
            if ((this.screenBlendMode.enumValueIndex == 0) && ((cam.allowHDR && (this.hdr.enumValueIndex == 0)) || (this.hdr.enumValueIndex == 1)))
            {
                EditorGUILayout.HelpBox("Screen blend is not supported in HDR. Using 'Add' instead.", MessageType.Info);
            }
        }
        EditorGUILayout.PropertyField(this.bloomIntensity, new GUIContent("Intensity"));
        this.bloomThreshhold.floatValue = EditorGUILayout.Slider("Threshhold", this.bloomThreshhold.floatValue, -0.05f, 4f);
        if (1 == this.tweakMode.intValue)
        {
            EditorGUILayout.PropertyField(this.bloomThreshholdColor, new GUIContent(" RGB Threshhold"));
        }
        EditorGUILayout.Separator();
        this.bloomBlurIterations.intValue = EditorGUILayout.IntSlider("Blur Iterations", this.bloomBlurIterations.intValue, 1, 4);
        this.sepBlurSpread.floatValue = EditorGUILayout.Slider(" Sample Distance", this.sepBlurSpread.floatValue, 0.1f, 10f);
        EditorGUILayout.Separator();
        if (1 == this.tweakMode.intValue)
        {
            // further lens flare tweakings
            if (0 != this.tweakMode.intValue)
            {
                EditorGUILayout.PropertyField(this.lensflareMode, new GUIContent("Lens Flares"));
            }
            else
            {
                this.lensflareMode.enumValueIndex = 0;
            }
            EditorGUILayout.PropertyField(this.lensflareIntensity, new GUIContent(" Local Intensity", "0 disables lens flares entirely (optimization)"));
            this.lensflareThreshhold.floatValue = EditorGUILayout.Slider("  Threshhold", this.lensflareThreshhold.floatValue, 0f, 4f);
            if (Mathf.Abs(this.lensflareIntensity.floatValue) > Mathf.Epsilon)
            {
                if (this.lensflareMode.intValue == 0)
                {
                    // ghosting	
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(this.flareColorA, new GUIContent(" 1st Color"));
                    EditorGUILayout.PropertyField(this.flareColorB, new GUIContent(" 2nd Color"));
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(this.flareColorC, new GUIContent(" 3rd Color"));
                    EditorGUILayout.PropertyField(this.flareColorD, new GUIContent(" 4th Color"));
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    if (this.lensflareMode.intValue == 1)
                    {
                        // hollywood
                        EditorGUILayout.PropertyField(this.hollyStretchWidth, new GUIContent(" Stretch width"));
                        EditorGUILayout.PropertyField(this.flareRotation, new GUIContent(" Rotation"));
                        this.hollywoodFlareBlurIterations.intValue = EditorGUILayout.IntSlider(" Blur Iterations", this.hollywoodFlareBlurIterations.intValue, 1, 4);
                        EditorGUILayout.PropertyField(this.lensFlareSaturation, new GUIContent(" Saturation"));
                        EditorGUILayout.PropertyField(this.flareColorA, new GUIContent(" Tint Color"));
                    }
                    else
                    {
                        if (this.lensflareMode.intValue == 2)
                        {
                            // both
                            EditorGUILayout.PropertyField(this.hollyStretchWidth, new GUIContent(" Stretch width"));
                            this.hollywoodFlareBlurIterations.intValue = EditorGUILayout.IntSlider(" Blur Iterations", this.hollywoodFlareBlurIterations.intValue, 1, 4);
                            EditorGUILayout.PropertyField(this.lensFlareSaturation, new GUIContent(" Saturation"));
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.PropertyField(this.flareColorA, new GUIContent(" 1st Color"));
                            EditorGUILayout.PropertyField(this.flareColorB, new GUIContent(" 2nd Color"));
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.PropertyField(this.flareColorC, new GUIContent(" 3rd Color"));
                            EditorGUILayout.PropertyField(this.flareColorD, new GUIContent(" 4th Color"));
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                }
                EditorGUILayout.PropertyField(this.lensFlareVignetteMask, new GUIContent(" Mask", "This mask is needed to prevent lens flare artifacts"));
            }
        }
        this.serObj.ApplyModifiedProperties();
    }

}