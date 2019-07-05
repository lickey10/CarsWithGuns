using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class FadeLightInstant : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 CopyrightÂ© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
    public float delay;
    public float fadeTime;
    private float fadeSpeed;
    private float intensity;
    public float startIntensity;
    private Color color;
    private bool active1;
    public virtual void Start()
    {
        if (this.GetComponent<Light>() == null)
        {
            UnityEngine.Object.Destroy(this);
            return;
        }
    }

    public virtual void Update()
    {
        if (!this.active1)
        {
            return;
        }
        if (this.delay > 0f)
        {
            this.delay = this.delay - Time.deltaTime;
        }
        else
        {
            if (this.intensity > 0f)
            {
                this.intensity = this.intensity - (this.fadeSpeed * Time.deltaTime);
                this.GetComponent<Light>().intensity = this.intensity;
            }
        }
    }

    public virtual void Activate()
    {
        this.GetComponent<Light>().intensity = this.startIntensity;
        this.intensity = this.GetComponent<Light>().intensity;
        this.active1 = true;
        if (this.fadeTime > 0f)
        {
            this.fadeSpeed = this.intensity / this.fadeTime;
        }
        else
        {
            this.fadeSpeed = this.intensity;
        }
    }

    public FadeLightInstant()
    {
        this.startIntensity = 6;
    }

}