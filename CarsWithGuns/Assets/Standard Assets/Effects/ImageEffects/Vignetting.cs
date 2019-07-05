using UnityEngine;
using System.Collections;

[System.Serializable]
[UnityEngine.ExecuteInEditMode]
[UnityEngine.RequireComponent(typeof(Camera))]
[UnityEngine.AddComponentMenu("Image Effects/Vignette and Chromatic Aberration")]
public partial class Vignetting : PostEffectsBase
{
    public enum AberrationMode
    {
        Simple = 0,
        Advanced = 1
    }


    public Vignetting.AberrationMode mode;
    public float intensity; // intensity == 0 disables pre pass (optimization)
    public float chromaticAberration;
    public float axialAberration;
    public float blur; // blur == 0 disables blur pass (optimization)
    public float blurSpread;
    public float luminanceDependency;
    public Shader vignetteShader;
    private Material vignetteMaterial;
    public Shader separableBlurShader;
    private Material separableBlurMaterial;
    public Shader chromAberrationShader;
    private Material chromAberrationMaterial;
    public override bool CheckResources()
    {
        this.CheckSupport(false);
        this.vignetteMaterial = this.CheckShaderAndCreateMaterial(this.vignetteShader, this.vignetteMaterial);
        this.separableBlurMaterial = this.CheckShaderAndCreateMaterial(this.separableBlurShader, this.separableBlurMaterial);
        this.chromAberrationMaterial = this.CheckShaderAndCreateMaterial(this.chromAberrationShader, this.chromAberrationMaterial);
        if (!this.isSupported)
        {
            this.ReportAutoDisable();
        }
        return this.isSupported;
    }

    public virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (this.CheckResources() == false)
        {
            Graphics.Blit(source, destination);
            return;
        }
        bool doPrepass = (Mathf.Abs(this.blur) > 0f) || (Mathf.Abs(this.intensity) > 0f);
        float widthOverHeight = (1f * source.width) / (1f * source.height);
        float oneOverBaseSize = 1f / 512f;
        RenderTexture color = null;
        RenderTexture halfRezColor = null;
        RenderTexture secondHalfRezColor = null;
        if (doPrepass)
        {
            color = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
            if (Mathf.Abs(this.blur) > 0f)
            {
                halfRezColor = RenderTexture.GetTemporary((int) (source.width / 2f), (int) (source.height / 2f), 0, source.format);
                secondHalfRezColor = RenderTexture.GetTemporary((int) (source.width / 2f), (int) (source.height / 2f), 0, source.format);
                Graphics.Blit(source, halfRezColor, this.chromAberrationMaterial, 0);
                int i = 0;
                while (i < 2) // maybe make iteration count tweakable
                {
                    this.separableBlurMaterial.SetVector("offsets", new Vector4(0f, this.blurSpread * oneOverBaseSize, 0f, 0f));
                    Graphics.Blit(halfRezColor, secondHalfRezColor, this.separableBlurMaterial);
                    this.separableBlurMaterial.SetVector("offsets", new Vector4((this.blurSpread * oneOverBaseSize) / widthOverHeight, 0f, 0f, 0f));
                    Graphics.Blit(secondHalfRezColor, halfRezColor, this.separableBlurMaterial);
                    i++;
                }
            }
            this.vignetteMaterial.SetFloat("_Intensity", this.intensity); // intensity for vignette
            this.vignetteMaterial.SetFloat("_Blur", this.blur); // blur intensity
            this.vignetteMaterial.SetTexture("_VignetteTex", halfRezColor); // blurred texture
            Graphics.Blit(source, color, this.vignetteMaterial, 0); // prepass blit: darken & blur corners
        }
        this.chromAberrationMaterial.SetFloat("_ChromaticAberration", this.chromaticAberration);
        this.chromAberrationMaterial.SetFloat("_AxialAberration", this.axialAberration);
        this.chromAberrationMaterial.SetFloat("_Luminance", 1f / (Mathf.Epsilon + this.luminanceDependency));
        if (doPrepass)
        {
            color.wrapMode = TextureWrapMode.Clamp;
        }
        else
        {
            source.wrapMode = TextureWrapMode.Clamp;
        }
        Graphics.Blit(doPrepass ? color : source, destination, this.chromAberrationMaterial, this.mode == AberrationMode.Advanced ? 2 : 1);
        if (color)
        {
            RenderTexture.ReleaseTemporary(color);
        }
        if (halfRezColor)
        {
            RenderTexture.ReleaseTemporary(halfRezColor);
        }
        if (secondHalfRezColor)
        {
            RenderTexture.ReleaseTemporary(secondHalfRezColor);
        }
    }

    public Vignetting()
    {
        this.mode = AberrationMode.Simple;
        this.intensity = 0.375f;
        this.chromaticAberration = 0.2f;
        this.axialAberration = 0.5f;
        this.blurSpread = 0.75f;
        this.luminanceDependency = 0.25f;
    }

}
 /* And Chromatic Aberration */