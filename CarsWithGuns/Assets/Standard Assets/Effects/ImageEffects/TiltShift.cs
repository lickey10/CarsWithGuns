using UnityEngine;
using System.Collections;

[System.Serializable]
[UnityEngine.ExecuteInEditMode]
[UnityEngine.RequireComponent(typeof(Camera))]
[UnityEngine.AddComponentMenu("Image Effects/Tilt shift")]
public partial class TiltShift : PostEffectsBase
{
    public Shader tiltShiftShader;
    private Material tiltShiftMaterial;
    public int renderTextureDivider;
    public int blurIterations;
    public bool enableForegroundBlur;
    public int foregroundBlurIterations;
    public float maxBlurSpread;
    public float focalPoint;
    public float smoothness;
    public bool visualizeCoc;
    // these values will be automatically determined
    private float start01;
    private float distance01;
    private float end01;
    private float curve;
    public override bool CheckResources()
    {
        this.CheckSupport(true);
        this.tiltShiftMaterial = this.CheckShaderAndCreateMaterial(this.tiltShiftShader, this.tiltShiftMaterial);
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
        float widthOverHeight = (1f * source.width) / (1f * source.height);
        float oneOverBaseSize = 1f / 512f;
        // clamp some values
        this.renderTextureDivider = this.renderTextureDivider < 1 ? 1 : this.renderTextureDivider;
        this.renderTextureDivider = this.renderTextureDivider > 4 ? 4 : this.renderTextureDivider;
        this.blurIterations = this.blurIterations < 1 ? 0 : this.blurIterations;
        this.blurIterations = this.blurIterations > 4 ? 4 : this.blurIterations;
        // automagically calculate parameters based on focalPoint
        float focalPoint01 = this.GetComponent<Camera>().WorldToViewportPoint((this.focalPoint * this.GetComponent<Camera>().transform.forward) + this.GetComponent<Camera>().transform.position).z / this.GetComponent<Camera>().farClipPlane;
        this.distance01 = focalPoint01;
        this.start01 = 0f;
        this.end01 = 1f;
        this.start01 = Mathf.Min(focalPoint01 - Mathf.Epsilon, this.start01);
        this.end01 = Mathf.Max(focalPoint01 + Mathf.Epsilon, this.end01);
        this.curve = this.smoothness * this.distance01;
        // resources
        RenderTexture cocTex = RenderTexture.GetTemporary(source.width, source.height, 0);
        RenderTexture cocTex2 = RenderTexture.GetTemporary(source.width, source.height, 0);
        RenderTexture lrTex1 = RenderTexture.GetTemporary(source.width / this.renderTextureDivider, source.height / this.renderTextureDivider, 0);
        RenderTexture lrTex2 = RenderTexture.GetTemporary(source.width / this.renderTextureDivider, source.height / this.renderTextureDivider, 0);
        // coc		
        this.tiltShiftMaterial.SetVector("_SimpleDofParams", new Vector4(this.start01, this.distance01, this.end01, this.curve));
        this.tiltShiftMaterial.SetTexture("_Coc", cocTex);
        if (this.enableForegroundBlur)
        {
            Graphics.Blit(source, cocTex, this.tiltShiftMaterial, 0);
            Graphics.Blit(cocTex, lrTex1); // downwards (only really needed if lrTex resolution is different)
            int fgBlurIter = 0;
            while (fgBlurIter < this.foregroundBlurIterations)
            {
                this.tiltShiftMaterial.SetVector("offsets", new Vector4(0f, (this.maxBlurSpread * 0.75f) * oneOverBaseSize, 0f, 0f));
                Graphics.Blit(lrTex1, lrTex2, this.tiltShiftMaterial, 3);
                this.tiltShiftMaterial.SetVector("offsets", new Vector4(((this.maxBlurSpread * 0.75f) / widthOverHeight) * oneOverBaseSize, 0f, 0f, 0f));
                Graphics.Blit(lrTex2, lrTex1, this.tiltShiftMaterial, 3);
                fgBlurIter++;
            }
            Graphics.Blit(lrTex1, cocTex2, this.tiltShiftMaterial, 7); // upwards (only really needed if lrTex resolution is different)
            this.tiltShiftMaterial.SetTexture("_Coc", cocTex2);
        }
        else
        {
            RenderTexture.active = cocTex;
            GL.Clear(false, true, Color.black);
        }
        // combine coc's
        Graphics.Blit(source, cocTex, this.tiltShiftMaterial, 5);
        this.tiltShiftMaterial.SetTexture("_Coc", cocTex);
        // downsample & blur
        Graphics.Blit(source, lrTex2);
        int iter = 0;
        while (iter < this.blurIterations)
        {
            this.tiltShiftMaterial.SetVector("offsets", new Vector4(0f, (this.maxBlurSpread * 1f) * oneOverBaseSize, 0f, 0f));
            Graphics.Blit(lrTex2, lrTex1, this.tiltShiftMaterial, 6);
            this.tiltShiftMaterial.SetVector("offsets", new Vector4(((this.maxBlurSpread * 1f) / widthOverHeight) * oneOverBaseSize, 0f, 0f, 0f));
            Graphics.Blit(lrTex1, lrTex2, this.tiltShiftMaterial, 6);
            iter++;
        }
        this.tiltShiftMaterial.SetTexture("_Blurred", lrTex2);
        Graphics.Blit(source, destination, this.tiltShiftMaterial, this.visualizeCoc ? 4 : 1);
        RenderTexture.ReleaseTemporary(cocTex);
        RenderTexture.ReleaseTemporary(cocTex2);
        RenderTexture.ReleaseTemporary(lrTex1);
        RenderTexture.ReleaseTemporary(lrTex2);
    }

    public TiltShift()
    {
        this.renderTextureDivider = 2;
        this.blurIterations = 2;
        this.enableForegroundBlur = true;
        this.foregroundBlurIterations = 2;
        this.maxBlurSpread = 1.5f;
        this.focalPoint = 30f;
        this.smoothness = 1.65f;
        this.distance01 = 0.2f;
        this.end01 = 1f;
        this.curve = 1f;
    }

}