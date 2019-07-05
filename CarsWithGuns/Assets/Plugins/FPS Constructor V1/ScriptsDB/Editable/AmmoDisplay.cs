using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class AmmoDisplay : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 Copyright� Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
 For additional information contact us info@dastardlybanana.com.
*/
    [UnityEngine.HideInInspector]
    public int bulletsLeft;
    [UnityEngine.HideInInspector]
    public int clips;
    [UnityEngine.HideInInspector]
    public bool display; //used by system
    public bool show; //used by user
    [UnityEngine.HideInInspector]
    public string clipDisplay;
    private object[] gunScripts;
    private GunScript gunScriptSecondary;
    private GunScript gunScript;
    public virtual void Start()
    {
        //This is AmmoDisplay getting all of the GunScripts from this weapon, then saving the primary and secondary.
        this.gunScripts = this.GetComponents(typeof(GunScript));
        GunScript g = null;
        int i = 0;
        while (i < this.gunScripts.Length)
        {
            g = this.gunScripts[i] as GunScript;
            if (g.isPrimaryWeapon)
            {
                this.gunScript = g;
            }
            i++;
        }
        int q = 0;
        while (q < this.gunScripts.Length)
        {
            g = this.gunScripts[q] as GunScript;
            if (!g.isPrimaryWeapon)
            {
                if (this.gunScript.secondaryWeapon == g)
                {
                    this.gunScriptSecondary = g;
                }
            }
            q++;
        }
    }

    public virtual void reapply()
    {
        //This is AmmoDisplay getting all of the GunScripts from this weapon, then saving the primary and secondary.
        this.gunScripts = this.GetComponents(typeof(GunScript));
        GunScript g = null;
        int i = 0;
        while (i < this.gunScripts.Length)
        {
            g = this.gunScripts[i] as GunScript;
            if (g.isPrimaryWeapon)
            {
                this.gunScript = g;
            }
            i++;
        }
        int q = 0;
        while (q < this.gunScripts.Length)
        {
            g = this.gunScripts[q] as GunScript;
            if (!g.isPrimaryWeapon)
            {
                if (this.gunScript.secondaryWeapon == g)
                {
                    this.gunScriptSecondary = g;
                }
            }
            q++;
        }
    }

    public virtual void OnGUI()
    {
        if (!(this.display && this.show))
        {
            return;
        }
        //Decide whether or not to show clips depending on if the guns have infinite ammo
        //This will have to be modified if you change the display
        string clipDisplay = null;
        string clipDisplay2 = null;
        if (!this.gunScript.infiniteAmmo)
        {
            clipDisplay = "/" + this.gunScript.clips;
        }
        else
        {
            clipDisplay = "";
        }
        if ((this.gunScriptSecondary != null) && !this.gunScriptSecondary.infiniteAmmo)
        {
            clipDisplay2 = "/" + this.gunScriptSecondary.clips;
        }
        else
        {
            clipDisplay2 = "";
        }
        //This is where you'll want to edit to make your own ammo display
        if (this.gunScriptSecondary != null)
        {
            //If there is a secondary weapon, display it's ammo along with the main weapon's
            GUI.Box(new Rect(Screen.width - 110, Screen.height - 55, 100, 20), ("Ammo: " + Mathf.Round(this.gunScript.ammoLeft)) + clipDisplay);
            GUI.Box(new Rect(Screen.width - 80, Screen.height - 30, 70, 20), ("Alt: " + Mathf.Round(this.gunScriptSecondary.ammoLeft)) + clipDisplay2);
        }
        else
        {
            //Otherwise just display the main weapon's ammo
            GUI.Box(new Rect(Screen.width - 110, Screen.height - 30, 100, 20), ("Ammo: " + Mathf.Round(this.gunScript.ammoLeft)) + clipDisplay);
        }
    }

    public virtual void SelectWeapon()
    {
        this.display = true;
    }

    public virtual void DeselectWeapon()
    {
        this.display = false;
    }

    public AmmoDisplay()
    {
        this.display = true;
        this.show = true;
        this.gunScripts = new object[0];
    }

}