using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class SelectableWeapon : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 Copyright� Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
 For additional information contact us info@dastardlybanana.com.
*/
    private bool selected;
    public GameObject weapon;
    [UnityEngine.HideInInspector]
    public WeaponInfo WeaponInfo;
    public bool isEquipped;
    public int ammo;
    //@HideInInspector
    public bool[] upgradesApplied;
    public virtual void Start()
    {
        this.WeaponInfo = this.weapon.GetComponent("WeaponInfo") as WeaponInfo;
    }

    public virtual void Interact()
    {
        PickupWeapon.Pickup(this.gameObject);
    }

    public virtual void select(bool a)
    {
        this.SendMessage("HighlightOn", SendMessageOptions.DontRequireReceiver);
        this.isEquipped = a;
    }

    //Apply any additional effects to the gunscript
    public virtual void Apply(GunScript g)
    {
        g.ammoLeft = this.ammo;
        if (!PlayerWeapons.saveUpgradesToDrops)
        {
            return;
        }
        int i = 0;
        while (i < this.WeaponInfo.upgrades.Length)
        {
            if (i >= this.upgradesApplied.Length)
            {
                this.WeaponInfo.upgrades[i].RemoveUpgradeInstant();
                goto Label_for_80;
            }
            if (this.upgradesApplied[i])
            {
                this.WeaponInfo.upgrades[i].ApplyUpgradeInstant();
            }
            else
            {
                this.WeaponInfo.upgrades[i].RemoveUpgradeInstant();
            }
            Label_for_80:
            i++;
        }
    }

    public virtual void PopulateDrop()
    {
        this.WeaponInfo = this.weapon.GetComponent("WeaponInfo") as WeaponInfo;
        if (this.WeaponInfo.gun != null)
        {
            this.ammo = this.WeaponInfo.gun.ammoLeft;
        }
        else
        {
            this.ammo = 0;
        }
        if (!PlayerWeapons.saveUpgradesToDrops)
        {
            return;
        }
        this.upgradesApplied = new bool[this.WeaponInfo.upgrades.Length];
        int i = 0;
        while (i < this.WeaponInfo.upgrades.Length)
        {
            this.upgradesApplied[i] = this.WeaponInfo.upgrades[i].applied;
            i++;
        }
    }

}