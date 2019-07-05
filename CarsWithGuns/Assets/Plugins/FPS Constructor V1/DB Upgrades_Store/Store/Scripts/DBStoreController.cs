using UnityEngine;
using System.Collections;

//FPS Constructor - Weapons
//Copyright� Dastardly Banana Productions 2010
//This script is distributed exclusively through ActiveDen and it's use is restricted to the terms of the ActiveDen 
//licensing agreement.
//
// Questions should be addressed to info@dastardlybanana.com
//
[System.Serializable]
public class WeaponClassArrayType : object
{
    public string weaponClass;
    public WeaponInfo[] WeaponInfoArray;
}
[System.Serializable]
public partial class DBStoreController : MonoBehaviour
{
    public static bool storeActive;
    public static bool canActivate;
    public static DBStoreController singleton;
    public float balance; // Store account balance 
    //var scrollPosition : Vector2;
    [UnityEngine.HideInInspector]
    public WeaponInfo[] WeaponInfoArray;
    [UnityEngine.HideInInspector]
    public WeaponClassArrayType[] WeaponInfoByClass;
    [UnityEngine.HideInInspector]
    public string[] weaponClassNames;
    [UnityEngine.HideInInspector]
    public string[] weaponClassNamesPopulated;
    [UnityEngine.HideInInspector]
    public PlayerWeapons playerW;
    [UnityEngine.HideInInspector]
    public GameObject nullWeapon; //there must be one null weapon as a placeholder to put in an empty slot.
    [UnityEngine.HideInInspector]
    public SlotInfo slotInfo;
    public bool canExitWhileEmpty;
    public static bool inStore;
    public virtual void Initialize()
    {
        DBStoreController.singleton = this;
        this.playerW = ((PlayerWeapons) UnityEngine.Object.FindObjectOfType(typeof(PlayerWeapons))) as PlayerWeapons;
        this.slotInfo = ((SlotInfo) UnityEngine.Object.FindObjectOfType(typeof(SlotInfo))) as SlotInfo;
        this.WeaponInfoArray = ((WeaponInfo[]) UnityEngine.Object.FindObjectsOfType(typeof(WeaponInfo))) as WeaponInfo[];
        foreach (WeaponInfo w in this.WeaponInfoArray)
        {
            if (w.weaponClass == weaponClasses.Null)
            {
                this.nullWeapon = w.gameObject;
            }
        }
        this.setupWeaponClassNames();
        this.setupWeaponInfoByClass();
    }

    public virtual int getNumOwned(int slot)
    {
        //will use the slot info later to restrict count
        int n = 0;
        int i = 0;
        while (i < this.WeaponInfoArray.Length)
        {
            if (this.WeaponInfoArray[i].owned && this.slotInfo.isWeaponAllowed(slot, this.WeaponInfoArray[i]))
            {
                n++;
            }
            i++;
        }
        return n;
    }

    public virtual string[] getWeaponNamesOwned(int slot)
    {
        string[] names = new string[this.getNumOwned(slot)];
        int n = 0;
        int i = 0;
        while (i < this.WeaponInfoArray.Length)
        {
            if (this.WeaponInfoArray[i].owned && this.slotInfo.isWeaponAllowed(slot, this.WeaponInfoArray[i]))
            {
                names[n] = this.WeaponInfoArray[i].gunName;
                n++;
            }
            i++;
        }
        return names;
    }

    public virtual WeaponInfo[] getWeaponsOwned(int slot)
    {
        WeaponInfo[] w = new WeaponInfo[this.getNumOwned(slot)];
        int n = 0;
        int i = 0;
        while (i < this.WeaponInfoArray.Length)
        {
            if (this.WeaponInfoArray[i].owned && this.slotInfo.isWeaponAllowed(slot, this.WeaponInfoArray[i]))
            {
                w[n] = this.WeaponInfoArray[i];
                n++;
            }
            i++;
        }
        return w;
    }

    public virtual void Update()
    {
        if (InputDB.GetButtonDown("Store"))
        {
            if (((!DBStoreController.storeActive && DBStoreController.canActivate) && !GunScript.takingOut) && !GunScript.puttingAway)
            {
                this.activateStore();
            }
            else
            {
                if (DBStoreController.storeActive)
                {
                    this.deActivateStore();
                }
            }
        }
    }

    public virtual void setupWeaponClassNames()
    {
        string[] names = null;
        object[] nameArray = new object[0];
        foreach (weaponClasses w in System.Enum.GetValues(typeof(weaponClasses)))
        {
            nameArray.Push(w.ToString().Replace("_", " "));
        }
        this.weaponClassNames = nameArray.ToBuiltin(typeof(string));
    }

    //Organize Weapon Information by Weapon Class for use within the GUI
    //Note: the code assumes the last weapon class is "Null" so the array is one element shorter than the number of
    //		weapon classes.
    public virtual void setupWeaponInfoByClass()
    {
        //check to see how many Weapon Classes have one or more weapons
        int n = 0;
        int i = 0;
        while (i < (this.weaponClassNames.Length - 1))
        {
            int j = 0;
            while (j < this.WeaponInfoArray.Length)
            {
                if (this.WeaponInfoArray[j].weaponClass == (weaponClasses) i)
                {
                    n++;
                    break;
                }
                j++;
            }
            i++;
        }
        this.weaponClassNamesPopulated = new string[n];
        this.WeaponInfoByClass = new WeaponClassArrayType[n]; // size array to hold all non-Null weapon classes with at least one weapon
        n = 0;
        i = 0;
        while (i < (this.weaponClassNames.Length - 1))
        {
            object[] arr = new object[0];
            j = 0;
            while (j < this.WeaponInfoArray.Length)
            {
                if (this.WeaponInfoArray[j].weaponClass == (weaponClasses) i)
                {
                    arr.Push(this.WeaponInfoArray[j]);
                }
                j++;
            }
            if (arr.Length > 0)
            {
                this.WeaponInfoByClass[n] = new WeaponClassArrayType();
                this.WeaponInfoByClass[n].WeaponInfoArray = arr.ToBuiltin(typeof(WeaponInfo));
                this.WeaponInfoByClass[n].weaponClass = this.weaponClassNames[i];
                this.weaponClassNamesPopulated[n] = this.weaponClassNames[i];
                n++;
            }
            i++;
        }
    }

    public virtual void activateStore()
    {
        PlayerWeapons.HideWeaponInstant();
        Time.timeScale = 0;
        //	GUIController.state = GUIStates.store;
        DBStoreController.inStore = true;
        //BroadcastMessage("Unlock");
        DBStoreController.storeActive = true;
        Screen.lockCursor = false;
        LockCursor.canLock = false;
        //playerW.BroadcastMessage("DeselectWeapon"); //turn off graphics/weapons on entering store
        GameObject player = GameObject.FindWithTag("Player");
        player.BroadcastMessage("Freeze", SendMessageOptions.DontRequireReceiver);
    }

    public virtual void deActivateStore()
    {
        if ((PlayerWeapons.HasEquipped() <= 0) && !this.canExitWhileEmpty)
        {
            return;
        }
        DBStoreController.storeActive = false;
        DBStoreController.inStore = false;
        //GUIController.state = GUIStates.playing;
        //BroadcastMessage("Lock");
        Time.timeScale = 1;
        Screen.lockCursor = true;
        LockCursor.canLock = true;
        PlayerWeapons.ShowWeapon();
        //playerW.SelectWeapon(playerW.selectedWeapon); // activate graphics on selected weapon
        GameObject player = GameObject.FindWithTag("Player");
        player.BroadcastMessage("UnFreeze", SendMessageOptions.DontRequireReceiver);
    }

    public virtual void equipWeapon(WeaponInfo g, int slot)
    {
        //if the weapon is equipped in another slot, unequip it
        if (slot < 0)
        {
            return;
        }
        int i = 0;
        while (i < this.playerW.weapons.Length)
        {
            if (g.gameObject == this.playerW.weapons[i])
            {
                this.unEquipWeapon(g, i);
            }
            i++;
        }
        if (this.playerW.weapons[this.playerW.selectedWeapon] == null)
        {
            this.playerW.selectedWeapon = slot;
        }
        this.playerW.BroadcastMessage("DeselectWeapon", SendMessageOptions.DontRequireReceiver);
        GunScript tempGScript = ((GunScript) g.gameObject.GetComponent(typeof(GunScript))).GetPrimaryGunScript();
        tempGScript.ammoLeft = tempGScript.ammoPerClip;
        this.playerW.SetWeapon(g.gameObject, slot);
    }

    public virtual void unEquipWeapon(WeaponInfo g, int slot)
    {
        this.playerW.weapons[slot] = null;
    }

    public virtual void buyUpgrade(WeaponInfo g, Upgrade u)
    {
        this.withdraw(u.buyPrice);
        u.owned = true;
        g.ApplyUpgrade();
    }

    public virtual void buyWeapon(WeaponInfo g)
    {
        this.withdraw(g.buyPrice);
        g.owned = true;
        g.ApplyUpgrade();
        this.equipWeapon(g, this.autoEquipWeapon(g, false));
    }

    public virtual void BuyAmmo(WeaponInfo g)
    {
        this.withdraw(g.ammoPrice);
        GunScript temp = ((GunScript) g.gameObject.GetComponent(typeof(GunScript))).GetPrimaryGunScript();
        temp.clips = Mathf.Min(temp.clips + temp.ammoPerClip, temp.maxClips);
        temp.ApplyToSharedAmmo();
    }

    public virtual void sellWeapon(WeaponInfo g)
    {
        if (!g.canBeSold)
        {
            return;
        }
        int i = 0;
        while (i < g.upgrades.Length)
        {
            g.upgrades[i].owned = false;
            g.upgrades[i].RemoveUpgrade();
            i++;
        }
        this.DropWeapon(g);
        this.deposit(g.sellPriceUpgraded);
        g.owned = false;
    }

    public virtual float getBalance()
    {
        return this.balance;
    }

    //Function to deposit money - returns the new balance
    public virtual float deposit(float amt)
    {
        this.balance = this.balance + amt;
        return this.balance;
    }

    // Function to withdraw money  - returns amount withdrawn 
    // You can't withdraw more than the balance.
    public virtual float withdraw(float amt)
    {
        if (amt <= this.balance)
        {
            this.balance = this.balance - amt;
            return amt;
        }
        else
        {
            float oldBalance = this.balance;
            this.balance = 0;
            return oldBalance;
        }
    }

    public virtual int autoEquipWeapon(WeaponInfo w, bool auto)
    {
        //Slot the weapon is equipped in, -1 if not equipped
        int slot = -1;
        //find the first empty slot that can hold w
        int i = 0;
        while (i < this.playerW.weapons.Length)
        {
            if (this.slotInfo.isWeaponAllowed(i, w) && (this.playerW.weapons[i] == null))
            {
                this.equipWeapon(w, i);
                slot = i;
            }
            if (slot >= 0)
            {
                return slot;
            }
            i++;
        }
        if (!auto)
        {
            return slot;
        }
        if (this.slotInfo.isWeaponAllowed(this.playerW.selectedWeapon, w))
        {
            //equipWeapon(w,i);
            slot = this.playerW.selectedWeapon;
            return slot;
        }
        i = 0;
        while (i < this.playerW.weapons.Length)
        {
            if (this.slotInfo.isWeaponAllowed(i, w))
            {
                //equipWeapon(w,i);
                slot = i;
            }
            if (slot >= 0)
            {
                return slot;
            }
            i++;
        }
        return slot;
    }

    public virtual int autoEquipWeaponWithReplacement(WeaponInfo w, bool auto)
    {
        //Slot the weapon is equipped in, -1 if not equipped
        int slot = -1;
        //See if the weapon is already equipped and can be replaced
        int i = 0;
        while (i < PlayerWeapons.PW.weapons.Length)
        {
            if (this.slotInfo.isWeaponAllowed(i, w) && ((this.playerW.weapons[i] == null) || (this.playerW.weapons[i].gameObject == w.gameObject)))
            {
                this.equipWeapon(w, i);
                slot = i;
            }
            if (slot >= 0)
            {
                return slot;
            }
            i++;
        }
        //find the first empty slot that can hold w
        i = 0;
        while (i < this.playerW.weapons.Length)
        {
            if (this.slotInfo.isWeaponAllowed(i, w) && (PlayerWeapons.PW.weapons[i] == null))
            {
                //equipWeapon(w,i);
                slot = i;
            }
            if (slot >= 0)
            {
                return slot;
            }
            i++;
        }
        if (!auto)
        {
            return slot;
        }
        if (this.slotInfo.isWeaponAllowed(PlayerWeapons.PW.selectedWeapon, w))
        {
            //equipWeapon(w,i);
            slot = this.playerW.selectedWeapon;
            return slot;
        }
        i = 0;
        while (i < PlayerWeapons.PW.weapons.Length)
        {
            if (this.slotInfo.isWeaponAllowed(i, w))
            {
                //equipWeapon(w,i);
                slot = i;
            }
            if (slot >= 0)
            {
                return slot;
            }
            i++;
        }
        return slot;
    }

    //Drops weapon at given index in Weapons[]
    public virtual void DropWeapon(WeaponInfo g)
    {
        //Weapon Drop
        int wep = -1;
        int i = 0;
        while (i < this.playerW.weapons.Length)
        {
            if (this.playerW.weapons[i].gameObject == g.gameObject)
            {
                wep = i;
                break;
            }
            i++;
        }
        if (wep < 0)
        {
            return;
        }
        this.playerW.weapons[wep] = null;
        //Ceck if we have a weapon to switch to
        int prevWeapon = -1;
        i = wep - 1;
        while (i >= 0)
        {
            if (this.playerW.weapons[i] != null)
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
            while (i < this.playerW.weapons.Length)
            {
                if (this.playerW.weapons[i] != null)
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
        this.playerW.selectedWeapon = prevWeapon;
        this.playerW.SelectWeapon(prevWeapon);
    }

    static DBStoreController()
    {
        DBStoreController.canActivate = true;
    }

}