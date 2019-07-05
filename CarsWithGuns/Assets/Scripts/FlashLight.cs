using UnityEngine;
using System.Collections;

[System.Serializable]
//BATTERY LIFE BRIGHTNESS//////////
 /////// PIC  ///////////
 // draw the background:
// draw the filled-in part:
[UnityEngine.RequireComponent(typeof(AudioSource))]
public partial class FlashLight : MonoBehaviour
{
    public Light flashlightLightSource;
    public bool lightOn;
    public float lightDrain;
    private static float batteryLife;
    public float maxBatteryLife;
    private static float batteryPower;
    public float barDisplay;
    public Vector2 pos;
    public Vector2 size;
    public Texture2D progressBarEmpty;
    public Texture2D progressBarFull;
    public AudioClip soundTurnOn;
    public AudioClip soundTurnOff;
    public virtual void Start()
    {
        FlashLight.batteryLife = this.maxBatteryLife;
        this.flashlightLightSource = (Light) this.GetComponent(typeof(Light));
    }

    public static void AlterEnergy(int amount)
    {
        FlashLight.batteryLife = Mathf.Clamp(FlashLight.batteryLife + FlashLight.batteryPower, 0, 100);
    }

    public virtual void Update()
    {
        if (this.lightOn && (FlashLight.batteryLife >= 0))
        {
            FlashLight.batteryLife = FlashLight.batteryLife - (Time.deltaTime * this.lightDrain);
        }
        if (this.lightOn && (FlashLight.batteryLife <= 0.4f))
        {
            this.flashlightLightSource.GetComponent<Light>().intensity = 5;
        }
        if (this.lightOn && (FlashLight.batteryLife <= 0.3f))
        {
            this.flashlightLightSource.GetComponent<Light>().intensity = 4;
        }
        if (this.lightOn && (FlashLight.batteryLife <= 0.2f))
        {
            this.flashlightLightSource.GetComponent<Light>().intensity = 3;
        }
        if (this.lightOn && (FlashLight.batteryLife <= 0.1f))
        {
            this.flashlightLightSource.GetComponent<Light>().intensity = 2;
        }
        if (this.lightOn && (FlashLight.batteryLife <= 0))
        {
            this.flashlightLightSource.GetComponent<Light>().intensity = 0;
        }
        this.barDisplay = FlashLight.batteryLife;
        if (FlashLight.batteryLife <= 0)
        {
            FlashLight.batteryLife = 0;
            this.lightOn = false;
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            this.toggleFlashlight();
            this.toggleFlashlightSFX();
            if (this.lightOn)
            {
                this.lightOn = false;
            }
            else
            {
                if (!this.lightOn && (FlashLight.batteryLife >= 0))
                {
                    this.lightOn = true;
                }
            }
        }
    }

    public virtual void OnGUI()
    {
        GUI.BeginGroup(new Rect(this.pos.x, this.pos.y, this.size.x, this.size.y));
        GUI.Box(new Rect(0, 0, this.size.x, this.size.y), this.progressBarEmpty);
        GUI.BeginGroup(new Rect(0, 0, this.size.x * this.barDisplay, this.size.y));
        GUI.Box(new Rect(0, 0, this.size.x, this.size.y), this.progressBarFull);
        GUI.EndGroup();
        GUI.EndGroup();
    }

    public virtual void toggleFlashlight()
    {
        if (this.lightOn)
        {
            this.flashlightLightSource.enabled = false;
        }
        else
        {
            this.flashlightLightSource.enabled = true;
        }
    }

    public virtual void toggleFlashlightSFX()
    {
        if (this.flashlightLightSource.enabled)
        {
            this.GetComponent<AudioSource>().clip = this.soundTurnOn;
        }
        else
        {
            this.GetComponent<AudioSource>().clip = this.soundTurnOff;
        }
        this.GetComponent<AudioSource>().Play();
    }

    public FlashLight()
    {
        this.lightOn = true;
        this.lightDrain = 0.1f;
        this.maxBatteryLife = 2f;
        this.pos = new Vector2(20, 40);
        this.size = new Vector2(60, 20);
    }

    static FlashLight()
    {
        FlashLight.batteryPower = 1;
    }

}