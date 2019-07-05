using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class PickupWeapon : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 Copyright© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
 For additional information contact us info@dastardlybanana.com.
*/
    public RaycastHit hit;
    public Ray ray;
    //@HideInInspector
    //static var selectedWeapon: GameObject;
    //@HideInInspector
    public static GameObject lastSelectedWeapon;
    //@HideInInspector
    public static PlayerWeapons playerWeapons;
    public bool throwWeapons;
    public float throwForce;
    public float throwTorque;
    public bool destroyPickups;
    public static DBStoreController store;
    public static PickupWeapon singleton;
    public virtual void Start()
    {
        PickupWeapon.playerWeapons = PlayerWeapons.PW;
        PickupWeapon.store = (DBStoreController) UnityEngine.Object.FindObjectOfType(typeof(DBStoreController));
        PickupWeapon.singleton = this;
    }

    public static bool CheckWeapons(GameObject selectedWeapon)
    {
        int i = 0;
        while (i < PickupWeapon.playerWeapons.weapons.Length)
        {
            if (PickupWeapon.playerWeapons.weapons[i] == ((SelectableWeapon) selectedWeapon.GetComponent(typeof(SelectableWeapon))).weapon)
            {
                return true;
            }
            i++;
        }
        return false;
    }

    //Drops weapon at given index in Weapons[]
    public static void DropWeapon(int wep)
    {
        //Weapon Drop
        //Ceck if we have a weapon to switch to
        int prevWeapon = -1;
        int i = wep - 1;
        while (i >= 0)
        {
            if (PickupWeapon.playerWeapons.weapons[i] != null)
            {
                prevWeapon = i;
                break;
            }
            i--;
        }
        int nextWeapon = -1;
        if (prevWeapon == -1)
        {
            i = wep + 1;
            while (i < PickupWeapon.playerWeapons.weapons.Length)
            {
                if (PickupWeapon.playerWeapons.weapons[i] != null)
                {
                    nextWeapon = i;
                    break;
                }
                i++;
            }
            prevWeapon = nextWeapon;
            if (nextWeapon == -1)
            {
                return;
            }
        }
        PickupWeapon.DropObject(wep, null);
        PickupWeapon.playerWeapons.selectedWeapon = prevWeapon;
        PickupWeapon.playerWeapons.SelectWeapon(prevWeapon);
    }

    //Swaps out currently selected for given one, dropping currently selected weapon
    public static void Pickup(GameObject selectedWeapon)
    {
        int theSlot = 0;
        if (GunScript.takingOut)
        {
            return;
        }
        bool hasFlag = false;
        if (!PlayerWeapons.canSwapSameWeapon)
        {
            int i = 0;
            while (i < PickupWeapon.playerWeapons.weapons.Length)
            {
                if (PickupWeapon.playerWeapons.weapons[i] == ((SelectableWeapon) selectedWeapon.GetComponent(typeof(SelectableWeapon))).weapon)
                {
                    return;
                }
                i++;
            }
        }
        if (PickupWeapon.playerWeapons.weapons[PickupWeapon.playerWeapons.selectedWeapon] == ((SelectableWeapon) selectedWeapon.GetComponent(typeof(SelectableWeapon))).weapon)
        {
            hasFlag = true;
        }
        WeaponInfo selectedWeaponInfo = (WeaponInfo) ((SelectableWeapon) selectedWeapon.GetComponent(typeof(SelectableWeapon))).weapon.GetComponent(typeof(WeaponInfo));
        //Get applicable slot
        theSlot = PickupWeapon.store.autoEquipWeaponWithReplacement(selectedWeaponInfo, true);
        if ((theSlot < 0) && (!PickupWeapon.playerWeapons.weapons[PickupWeapon.playerWeapons.selectedWeapon] == null))
        {
            return;
        }
        //We now own the weapon
        if (selectedWeaponInfo != null)
        {
            selectedWeaponInfo.owned = true;
            selectedWeaponInfo.locked = false;
        }
        PickupWeapon.DropObject(theSlot, selectedWeapon);
        GunScript gscript = null;
        if (PickupWeapon.playerWeapons.weapons[PickupWeapon.playerWeapons.selectedWeapon] != null)
        {
            gscript = ((GunScript) PickupWeapon.playerWeapons.weapons[PickupWeapon.playerWeapons.selectedWeapon].GetComponent(typeof(GunScript))).GetPrimaryGunScript();
        }
        //Get new weapon
        if (hasFlag)
        {
            gscript = ((GunScript) PickupWeapon.playerWeapons.weapons[PickupWeapon.playerWeapons.selectedWeapon].GetComponent(typeof(GunScript))).GetPrimaryGunScript();
            ((SelectableWeapon) selectedWeapon.GetComponent(typeof(SelectableWeapon))).Apply(gscript);
            PickupWeapon.playerWeapons.weapons[PickupWeapon.playerWeapons.selectedWeapon].BroadcastMessage("DeselectInstant");
            PickupWeapon.playerWeapons.ActivateWeapon();
        }
        else
        {
            //playerWeapons.weapons[playerWeapons.selectedWeapon].BroadcastMessage("SelectWeapon");
            PickupWeapon.playerWeapons.weapons[theSlot] = ((SelectableWeapon) selectedWeapon.GetComponent(typeof(SelectableWeapon))).weapon;
            PickupWeapon.playerWeapons.selectedWeapon = theSlot;
            gscript = ((GunScript) PickupWeapon.playerWeapons.weapons[PickupWeapon.playerWeapons.selectedWeapon].GetComponent(typeof(GunScript))).GetPrimaryGunScript();
            ((SelectableWeapon) selectedWeapon.GetComponent(typeof(SelectableWeapon))).Apply(gscript);
            PickupWeapon.playerWeapons.ActivateWeapon();
        }
        if (PickupWeapon.singleton.destroyPickups)
        {
            UnityEngine.Object.Destroy(selectedWeapon);
        }
    }

    public static void DropObject(int index, GameObject selectedWeapon)
    {
        //Deselect old weapon
        if (PickupWeapon.playerWeapons.weapons[PickupWeapon.playerWeapons.selectedWeapon])
        {
            if (((SelectableWeapon) selectedWeapon.GetComponent(typeof(SelectableWeapon))) != null)
            {
                if (PickupWeapon.playerWeapons.weapons[PickupWeapon.playerWeapons.selectedWeapon] != ((SelectableWeapon) selectedWeapon.GetComponent(typeof(SelectableWeapon))).weapon)
                {
                    PickupWeapon.playerWeapons.weapons[PickupWeapon.playerWeapons.selectedWeapon].gameObject.BroadcastMessage("DeselectWeapon");
                }
            }
        }
        //Weapon Drop
        if (PickupWeapon.playerWeapons.weapons[index] != null)
        {
            GameObject dropObj = ((WeaponInfo) PickupWeapon.playerWeapons.weapons[index].GetComponent(typeof(WeaponInfo))).drops;
            if (dropObj != null)
            {
                GameObject temp = UnityEngine.Object.Instantiate(dropObj, new Vector3(PickupWeapon.singleton.transform.position.x, PickupWeapon.singleton.transform.position.y - 1, PickupWeapon.singleton.transform.position.z), Quaternion.identity);
                ((SelectableWeapon) temp.GetComponent(typeof(SelectableWeapon))).weapon = PickupWeapon.playerWeapons.weapons[index];
                ((SelectableWeapon) temp.GetComponent(typeof(SelectableWeapon))).PopulateDrop();
                if (PickupWeapon.singleton.throwWeapons || (selectedWeapon == null))
                {
                    ((Rigidbody) temp.GetComponent(typeof(Rigidbody))).AddForce(PickupWeapon.singleton.transform.forward * PickupWeapon.singleton.throwForce, ForceMode.Impulse);
                    ((Rigidbody) temp.GetComponent(typeof(Rigidbody))).AddTorque(PickupWeapon.singleton.transform.forward * PickupWeapon.singleton.throwTorque, ForceMode.Impulse);
                }
                else
                {
                    Vector3 pos = selectedWeapon.transform.position;
                    temp.transform.position = new Vector3(pos.x, pos.y + 0.4f, pos.z);
                }
            }
        }
    }

    public PickupWeapon()
    {
        this.throwWeapons = true;
    }

}