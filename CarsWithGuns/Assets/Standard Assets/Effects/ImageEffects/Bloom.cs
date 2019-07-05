using UnityEngine;
using System.Collections;

[System.Serializable]
[UnityEngine.ExecuteInEditMode]
[UnityEngine.RequireComponent(typeof(Camera))]
[UnityEngine.AddComponentMenu("Image Effects/Bloom (4.0, HDR, Lens Flares)")]
public partial class Bloom : PostEffectsBase
{
    public enum LensFlareStyle
    {
        Ghosting = 0,
        Anamorphic = 1,
        Combined = 2
    }


    public enum TweakMode
    {
        Basic = 0,
        Complex = 1
    }


    public enum HDRBloomMode
    {
        Auto = 0,
        On = 1,
        Off = 2
    }


    public enum BloomScreenBlendMode
    {
        Screen = 0,
        Add = 1
    }


    public enum BloomQuality
    {
        Cheap = 0,
        High = 1
    }


    public Bloom.TweakMode tweakMode;
    public Bloom.BloomScreenBlendMode screenBlendMode;
    public Bloom.HDRBloomMode hdr;
    private bool doHdr;
    public float sepBlurSpread;
    public Bloom.BloomQuality quality;
    public float bloomIntensity;
    public float bloomThreshhold;
    public Color bloomThreshholdColor;
    public int bloomBlurIterations;
    public int hollywoodFlareBlurIterations;
    public float flareRotation;
    public Bloom.LensFlareStyle lensflareMode;
    public float hollyStretchWidth;
    public float lensflareIntensity;
    public float lensflareThreshhold;
    public float lensFlareSaturation;
    public Color flareColorA;
    public Color flareColorB;
    public Color flareColorC;
    public Color flareColorD;
    public float blurWidth;
    public Texture2D lensFlareVignetteMask;
    public Shader lensFlareShader;
    private Material lensFlareMaterial;
    public Shader screenBlendShader;
    private Material screenBlend;
    public Shader blurAndFlaresShader;
    private Material blurAndFlaresMaterial;
    public Shader brightPassFilterShader;
    private Material brightPassFilterMaterial;
    public override bool CheckResources()
    {
        this.CheckSupport(false);
        this.screenBlend = this.CheckShaderAndCreateMaterial(this.screenBlendShader, this.screenBlend);
        this.lensFlareMaterial = this.CheckShaderAndCreateMaterial(this.lensFlareShader, this.lensFlareMaterial);
        this.blurAndFlaresMaterial = this.CheckShaderAndCreateMaterial(this.blurAndFlaresShader, this.blurAndFlaresMaterial);
        this.brightPassFilterMaterial = this.CheckShaderAndCreateMaterial(this.brightPassFilterShader, this.brightPassFilterMaterial);
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
        // screen blend is not supported when HDR is enabled (will cap values)
        this.doHdr = false;
        if (this.hdr == HDRBloomMode.Auto)
        {
            this.doHdr = (source.format == RenderTextureFormat.ARGBHalf) && this.GetComponent<Camera>().allowHDR;
        }
        else
        {
            this.doHdr = this.hdr == HDRBloomMode.On;
        }
        this.doHdr = this.doHdr && this.supportHDRTextures;
        Bloom.BloomScreenBlendMode realBlendMode = this.screenBlendMode;
        if (this.doHdr)
        {
            realBlendMode = BloomScreenBlendMode.Add;
        }
        RenderTextureFormat rtFormat = this.doHdr ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.Default;
        RenderTexture halfRezColor = RenderTexture.GetTemporary(source.width / 2, source.height / 2, 0, rtFormat);
        RenderTexture quarterRezColor = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, rtFormat);
        RenderTexture secondQuarterRezColor = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, rtFormat);
        RenderTexture thirdQuarterRezColor = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, rtFormat);
        float widthOverHeight = (1f * source.width) / (1f * source.height);
        float oneOverBaseSize = 1f / 512f;
        // downsample
        if (this.quality > BloomQuality.Cheap)
        {
            Graphics.Blit(source, halfRezColor, this.screenBlend, 2);
            Graphics.Blit(halfRezColor, secondQuarterRezColor, this.screenBlend, 2);
            Graphics.Blit(secondQuarterRezColor, quarterRezColor, this.screenBlend, 6);
        }
        else
        {
            Graphics.Blit(source, halfRezColor);
            Graphics.Blit(halfRezColor, quarterRezColor, this.screenBlend, 6);
        }
        // cut colors (threshholding)			
        this.BrightFilter(this.bloomThreshhold * this.bloomThreshholdColor, quarterRezColor, secondQuarterRezColor);
        // blurring
        if (this.bloomBlurIterations < 1)
        {
            this.bloomBlurIterations = 1;
        }
        else
        {
            if (this.bloomBlurIterations > 10)
            {
                this.bloomBlurIterations = 10;
            }
        }
        int iter = 0;
        while (iter < this.bloomBlurIterations)
        {
            float spreadForPass = (1f + (iter * 0.25f)) * this.sepBlurSpread;
            this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(0f, spreadForPass * oneOverBaseSize, 0f, 0f));
            Graphics.Blit(secondQuarterRezColor, thirdQuarterRezColor, this.blurAndFlaresMaterial, 4);
            if (this.quality > BloomQuality.Cheap)
            {
                this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4((spreadForPass / widthOverHeight) * oneOverBaseSize, 0f, 0f, 0f));
                Graphics.Blit(thirdQuarterRezColor, secondQuarterRezColor, this.blurAndFlaresMaterial, 4);
                if (iter == 0)
                {
                    Graphics.Blit(secondQuarterRezColor, quarterRezColor);
                }
                else
                {
                    Graphics.Blit(secondQuarterRezColor, quarterRezColor, this.screenBlend, 10);
                }
            }
            else
            {
                this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4((spreadForPass / widthOverHeight) * oneOverBaseSize, 0f, 0f, 0f));
                Graphics.Blit(thirdQuarterRezColor, secondQuarterRezColor, this.blurAndFlaresMaterial, 4);
            }
            iter++;
        }
        if (this.quality > BloomQuality.Cheap)
        {
            Graphics.Blit(quarterRezColor, secondQuarterRezColor, this.screenBlend, 6);
        }
        // lens flares: ghosting, anamorphic or both (ghosted anamorphic flares) 
        if (this.lensflareIntensity > Mathf.Epsilon)
        {
            if (this.lensflareMode == (Bloom.LensFlareStyle) 0)
            {
                this.BrightFilter(this.lensflareThreshhold, secondQuarterRezColor, thirdQuarterRezColor);
                if (this.quality > BloomQuality.Cheap)
                {
                    // smooth a little
                    this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(0f, 1.5f / (1f * quarterRezColor.height), 0f, 0f));
                    Graphics.Blit(thirdQuarterRezColor, quarterRezColor, this.blurAndFlaresMaterial, 4);
                    this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(1.5f / (1f * quarterRezColor.width), 0f, 0f, 0f));
                    Graphics.Blit(quarterRezColor, thirdQuarterRezColor, this.blurAndFlaresMaterial, 4);
                }
                // no ugly edges!
                this.Vignette(0.975f, thirdQuarterRezColor, thirdQuarterRezColor);
                this.BlendFlares(thirdQuarterRezColor, secondQuarterRezColor);
            }
            else
            {
                //Vignette (0.975f, thirdQuarterRezColor, thirdQuarterRezColor);	
                //DrawBorder(thirdQuarterRezColor, screenBlend, 8);
                float flareXRot = 1f * Mathf.Cos(this.flareRotation);
                float flareyRot = 1f * Mathf.Sin(this.flareRotation);
                float stretchWidth = ((this.hollyStretchWidth * 1f) / widthOverHeight) * oneOverBaseSize;
                float stretchWidthY = this.hollyStretchWidth * oneOverBaseSize;
                this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(flareXRot, flareyRot, 0f, 0f));
                this.blurAndFlaresMaterial.SetVector("_Threshhold", new Vector4(this.lensflareThreshhold, 1f, 0f, 0f));
                this.blurAndFlaresMaterial.SetVector("_TintColor", (new Vector4(this.flareColorA.r, this.flareColorA.g, this.flareColorA.b, this.flareColorA.a) * this.flareColorA.a) * this.lensflareIntensity);
                this.blurAndFlaresMaterial.SetFloat("_Saturation", this.lensFlareSaturation);
                Graphics.Blit(thirdQuarterRezColor, quarterRezColor, this.blurAndFlaresMaterial, 2);
                Graphics.Blit(quarterRezColor, thirdQuarterRezColor, this.blurAndFlaresMaterial, 3);
                this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(flareXRot * stretchWidth, flareyRot * stretchWidth, 0f, 0f));
                this.blurAndFlaresMaterial.SetFloat("_StretchWidth", this.hollyStretchWidth);
                Graphics.Blit(thirdQuarterRezColor, quarterRezColor, this.blurAndFlaresMaterial, 1);
                this.blurAndFlaresMaterial.SetFloat("_StretchWidth", this.hollyStretchWidth * 2f);
                Graphics.Blit(quarterRezColor, thirdQuarterRezColor, this.blurAndFlaresMaterial, 1);
                this.blurAndFlaresMaterial.SetFloat("_StretchWidth", this.hollyStretchWidth * 4f);
                Graphics.Blit(thirdQuarterRezColor, quarterRezColor, this.blurAndFlaresMaterial, 1);
                iter = 0;
                while (iter < this.hollywoodFlareBlurIterations)
                {
                    stretchWidth = ((this.hollyStretchWidth * 2f) / widthOverHeight) * oneOverBaseSize;
                    this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(stretchWidth * flareXRot, stretchWidth * flareyRot, 0f, 0f));
                    Graphics.Blit(quarterRezColor, thirdQuarterRezColor, this.blurAndFlaresMaterial, 4);
                    this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(stretchWidth * flareXRot, stretchWidth * flareyRot, 0f, 0f));
                    Graphics.Blit(thirdQuarterRezColor, quarterRezColor, this.blurAndFlaresMaterial, 4);
                    iter++;
                }
                if (this.lensflareMode == (Bloom.LensFlareStyle) 1)
                {
                    this.AddTo(1f, quarterRezColor, secondQuarterRezColor);
                }
                else
                {
                    // "combined" lens flares													
                    this.Vignette(1f, quarterRezColor, thirdQuarterRezColor);
                    this.BlendFlares(thirdQuarterRezColor, quarterRezColor);
                    this.AddTo(1f, quarterRezColor, secondQuarterRezColor);
                }
            }
        }
        int blendPass = (int) realBlendMode;
        //if(Mathf.Abs(chromaticBloom) < Mathf.Epsilon) 
        //	blendPass += 4;
        this.screenBlend.SetFloat("_Intensity", this.bloomIntensity);
        this.screenBlend.SetTexture("_ColorBuffer", source);
        if (this.quality > BloomQuality.Cheap)
        {
            Graphics.Blit(secondQuarterRezColor, halfRezColor);
            Graphics.Blit(halfRezColor, destination, this.screenBlend, blendPass);
        }
        else
        {
            Graphics.Blit(secondQuarterRezColor, destination, this.screenBlend, blendPass);
        }
        RenderTexture.ReleaseTemporary(halfRezColor);
        RenderTexture.ReleaseTemporary(quarterRezColor);
        RenderTexture.ReleaseTemporary(secondQuarterRezColor);
        RenderTexture.ReleaseTemporary(thirdQuarterRezColor);
    }

    private void AddTo(float intensity_, RenderTexture from, RenderTexture to)
    {
        this.screenBlend.SetFloat("_Intensity", intensity_);
        Graphics.Blit(from, to, this.screenBlend, 9);
    }

    private void BlendFlares(RenderTexture from, RenderTexture to)
    {
        this.lensFlareMaterial.SetVector("colorA", new Vector4(this.flareColorA.r, this.flareColorA.g, this.flareColorA.b, this.flareColorA.a) * this.lensflareIntensity);
        this.lensFlareMaterial.SetVector("colorB", new Vector4(this.flareColorB.r, this.flareColorB.g, this.flareColorB.b, this.flareColorB.a) * this.lensflareIntensity);
        this.lensFlareMaterial.SetVector("colorC", new Vector4(this.flareColorC.r, this.flareColorC.g, this.flareColorC.b, this.flareColorC.a) * this.lensflareIntensity);
        this.lensFlareMaterial.SetVector("colorD", new Vector4(this.flareColorD.r, this.flareColorD.g, this.flareColorD.b, this.flareColorD.a) * this.lensflareIntensity);
        Graphics.Blit(from, to, this.lensFlareMaterial);
    }

    private void BrightFilter(float thresh, RenderTexture from, RenderTexture to)
    {
        this.brightPassFilterMaterial.SetVector("_Threshhold", new Vector4(thresh, thresh, thresh, thresh));
        Graphics.Blit(from, to, this.brightPassFilterMaterial, 0);
    }

    private void BrightFilter(Color threshColor, RenderTexture from, RenderTexture to)
    {
        this.brightPassFilterMaterial.SetVector("_Threshhold", threshColor);
        Graphics.Blit(from, to, this.brightPassFilterMaterial, 1);
    }

    private void Vignette(float amount, RenderTexture from, RenderTexture to)
    {
        if (this.lensFlareVignetteMask)
        {
            this.screenBlend.SetTexture("_ColorBuffer", this.lensFlareVignetteMask);
            Graphics.Blit(from == to ? null : from, to, this.screenBlend, from == to ? 7 : 3);
        }
        else
        {
            if (from != to)
            {
                Graphics.Blit(from, to);
            }
        }
    }

    public Bloom()
    {
        this.screenBlendMode = BloomScreenBlendMode.Add;
        this.hdr = HDRBloomMode.Auto;
        this.sepBlurSpread = 2.5f;
        this.quality = BloomQuality.High;
        this.bloomIntensity = 0.5f;
        this.bloomThreshhold = 0.5f;
        this.bloomThreshholdColor = Color.white;
        this.bloomBlurIterations = 2;
        this.hollywoodFlareBlurIterations = 2;
        this.lensflareMode = (Bloom.LensFlareStyle) 1;
        this.hollyStretchWidth = 2.5f;
        this.lensflareThreshhold = 0.3f;
        this.lensFlareSaturation = 0.75f;
        this.flareColorA = new Color(0.4f, 0.4f, 0.8f, 0.75f);
        this.flareColorB = new Color(0.4f, 0.8f, 0.8f, 0.75f);
        this.flareColorC = new Color(0.8f, 0.4f, 0.8f, 0.75f);
        this.flareColorD = new Color(0.8f, 0.4f, 0f, 0.75f);
        this.blurWidth = 1f;
    }

}