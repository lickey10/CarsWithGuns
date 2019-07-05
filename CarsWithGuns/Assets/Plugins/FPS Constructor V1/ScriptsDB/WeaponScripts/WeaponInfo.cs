using UnityEngine;
using System.Collections;

/*
 FPS Constructor - Weapons
 Copyright� Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
// You can change the WeaponClasses enum to define your own Weapon Classes, 
// "Null" must be the last value in the enum and should be applied to one empty weapon object.
// The store will replace any underscores with a space in the display name. Sniper_Rifle will be displayed as "Sniper Rifle"
public enum weaponClasses
{
    Sidearm = 0,
    Primary = 1,
    Special = 2,
    Null = 3
}

[System.Serializable]
public partial class WeaponInfo : MonoBehaviour
{
    public GameObject drops;
    public bool owned;
    public bool locked;
    public weaponClasses weaponClass;
    [UnityEngine.HideInInspector]
    public string weaponClassName;
    public string gunDescription;
    public string lockedDescription;
    public string gunName;
    public int buyPrice;
    public int ammoPrice;
    public float sellPrice;
    [UnityEngine.HideInInspector]
    public float sellPriceUpgraded;
    public Texture icon; //Icon should be X by Y pixels for store.
    [UnityEngine.HideInInspector]
    public bool[] upgradesApplied;
    [UnityEngine.HideInInspector]
    public Upgrade[] upgrades;
    private Upgrade[] storeUpgrades;
    public bool canBeSold; //Can this weapon be sold (it's often best to have one base weapon which cannot)
    [UnityEngine.HideInInspector]
    public GunScript gun;
    private GunScript[] guns;
    public virtual void Awake()
    {
        this.gun = this.getPrimaryGunscript();
        this.upgrades = this.GetComponentsInChildren(typeof(Upgrade)).ToBuiltin(typeof(Upgrade));
        object[] tempArr = new object[0];
        this.upgradesApplied = new bool[this.upgrades.Length];
        // Initialize array of applied;
        int i = 0;
        while (i < this.upgrades.Length)
        {
            this.upgradesApplied[i] = this.upgrades[i].applied;
            if (this.upgrades[i].showInStore)
            {
                tempArr.Push(this.upgrades[i]);
            }
            i++;
        }
        this.storeUpgrades = tempArr.ToBuiltin(typeof(Upgrade));
        // Create Display string for gunClass 
        this.weaponClassName = this.weaponClass.ToString().Replace("_", " ");
    }

    public virtual GunScript getPrimaryGunscript()
    {
        this.guns = this.GetComponents(typeof(GunScript)).ToBuiltin(typeof(GunScript));
        int i = 0;
        while (i < this.guns.Length)
        {
            if (this.guns[i].isPrimaryWeapon)
            {
                return this.guns[i];
            }
            i++;
        }
        return null;
    }

    public virtual Upgrade[] getUpgrades()
    {
        return this.storeUpgrades;
    }

    public virtual bool[] getUpgradesApplied()
    {
        return this.upgradesApplied;
    }

    public virtual void ApplyUpgrade()
    {
        float tmpPrice = 0.0f;
        tmpPrice = this.sellPrice;
        int i = 0;
        while (i < this.upgrades.Length)
        {
            if (this.upgrades[i].owned)
            {
                tmpPrice = tmpPrice + this.upgrades[i].sellPrice;
            }
            i++;
        }
        this.sellPriceUpgraded = tmpPrice;
    }

    public virtual void updateApplied()
    {
        int i = 0;
        while (i < this.upgrades.Length)
        {
            this.upgradesApplied[i] = this.upgrades[i].applied;
            i++;
        }
    }

    public WeaponInfo()
    {
        this.lockedDescription = "Weapon Locked";
        this.canBeSold = true;
    }

}