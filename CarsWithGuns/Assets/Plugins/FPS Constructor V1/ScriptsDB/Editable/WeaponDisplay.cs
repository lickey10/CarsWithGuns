using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class WeaponDisplay : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 Copyright© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
 For additional information contact us info@dastardlybanana.com.
*/
    // Sample script to display weapon info when a weapon is selected.
    private bool display;
    private float endTime;
    private GameObject weapon;
    private WeaponInfo WeaponInfo;
    public float displayTime;
    public virtual void Start()
    {
        this.WeaponInfo = (WeaponInfo) this.GetComponent("WeaponInfo");
        this.display = false;
    }

    public virtual void Select()
    {
        this.display = true;
        this.endTime = Time.time + this.displayTime;
    }

    public virtual void OnGUI()
    {
        if (this.display && (Time.time != 0f))
        {
            if (Time.time > this.endTime)
            {
                this.display = false;
            }
            GUI.Box(new Rect(Screen.width - 255, Screen.height - 85, 135, 75), ((this.WeaponInfo.gunName + "\n") + this.WeaponInfo.gunDescription) + "\n");
        }
    }

    public WeaponDisplay()
    {
        this.displayTime = 2;
    }

}