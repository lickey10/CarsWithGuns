using UnityEngine;
using System.Collections;

[System.Serializable]
[UnityEngine.ExecuteInEditMode]
[UnityEngine.RequireComponent(typeof(Camera))]
[UnityEngine.AddComponentMenu("Image Effects/Contrast Enhance (Unsharp Mask)")]
public partial class ContrastEnhance : PostEffectsBase
{
    public float intensity;
    public float threshhold;
    private Material separableBlurMaterial;
    private Material contrastCompositeMaterial;
    public float blurSpread;
    public Shader separableBlurShader;
    public Shader contrastCompositeShader;
    public override bool CheckResources()
    {
        this.CheckSupport(false);
        this.contrastCompositeMaterial = this.CheckShaderAndCreateMaterial(this.contrastCompositeShader, this.contrastCompositeMaterial);
        this.separableBlurMaterial = this.CheckShaderAndCreateMaterial(this.separableBlurShader, this.separableBlurMaterial);
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
        RenderTexture halfRezColor = RenderTexture.GetTemporary((int) (source.width / 2f), (int) (source.height / 2f), 0);
        RenderTexture quarterRezColor = RenderTexture.GetTemporary((int) (source.width / 4f), (int) (source.height / 4f), 0);
        RenderTexture secondQuarterRezColor = RenderTexture.GetTemporary((int) (source.width / 4f), (int) (source.height / 4f), 0);
        // ddownsample
        Graphics.Blit(source, halfRezColor);
        Graphics.Blit(halfRezColor, quarterRezColor);
        // blur
        this.separableBlurMaterial.SetVector("offsets", new Vector4(0f, (this.blurSpread * 1f) / quarterRezColor.height, 0f, 0f));
        Graphics.Blit(quarterRezColor, secondQuarterRezColor, this.separableBlurMaterial);
        this.separableBlurMaterial.SetVector("offsets", new Vector4((this.blurSpread * 1f) / quarterRezColor.width, 0f, 0f, 0f));
        Graphics.Blit(secondQuarterRezColor, quarterRezColor, this.separableBlurMaterial);
        // composite
        this.contrastCompositeMaterial.SetTexture("_MainTexBlurred", quarterRezColor);
        this.contrastCompositeMaterial.SetFloat("intensity", this.intensity);
        this.contrastCompositeMaterial.SetFloat("threshhold", this.threshhold);
        Graphics.Blit(source, destination, this.contrastCompositeMaterial);
        RenderTexture.ReleaseTemporary(halfRezColor);
        RenderTexture.ReleaseTemporary(quarterRezColor);
        RenderTexture.ReleaseTemporary(secondQuarterRezColor);
    }

    public ContrastEnhance()
    {
        this.intensity = 0.5f;
        this.blurSpread = 1f;
    }

}