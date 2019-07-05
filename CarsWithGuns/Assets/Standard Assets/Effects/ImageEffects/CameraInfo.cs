using UnityEngine;
using System.Collections;

// pseudo image effect that displays useful info for your image effects
[System.Serializable]
[UnityEngine.ExecuteInEditMode]
[UnityEngine.RequireComponent(typeof(Camera))]
[UnityEngine.AddComponentMenu("Image Effects/Camera Info")]
public partial class CameraInfo : MonoBehaviour
{
    // display current depth texture mode
    public DepthTextureMode currentDepthMode;
    // render path
    public RenderingPath currentRenderPath;
    // number of official image fx used
    public int recognizedPostFxCount;
    public virtual void Start()
    {
        this.UpdateInfo();
    }

    public virtual void Update()
    {
        if (this.currentDepthMode != this.GetComponent<Camera>().depthTextureMode)
        {
            this.GetComponent<Camera>().depthTextureMode = this.currentDepthMode;
        }
        if (this.currentRenderPath != this.GetComponent<Camera>().actualRenderingPath)
        {
            this.GetComponent<Camera>().renderingPath = this.currentRenderPath;
        }
        this.UpdateInfo();
    }

    public virtual void UpdateInfo()
    {
        this.currentDepthMode = this.GetComponent<Camera>().depthTextureMode;
        this.currentRenderPath = this.GetComponent<Camera>().actualRenderingPath;
        PostEffectsBase[] fx = this.gameObject.GetComponents<PostEffectsBase>();
        int fxCount = 0;
        foreach (PostEffectsBase post in fx)
        {
            if (post.enabled)
            {
                fxCount++;
            }
        }
        this.recognizedPostFxCount = fxCount;
    }

}