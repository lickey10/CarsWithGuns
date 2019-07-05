using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class SlotInfo : MonoBehaviour
{
    // Helper Class for configuring which weapon classes are allowed in which slots
    public string[] slotName;
    public int[] allowed;
    public virtual void clearAllowed(int slot)
    {
        this.allowed[slot] = 0;
    }

    public virtual void setAllowed(int slot, weaponClasses wc, bool b)
    {
        if (b)
        {
            this.allowed[slot] = this.allowed[slot] | (1 << UnityScript.Lang.UnityBuiltins.parseInt((int) wc));
        }
        else
        {
            this.allowed[slot] = this.allowed[slot] & ~(1 << UnityScript.Lang.UnityBuiltins.parseInt((int) wc));
        }
    }

    public virtual bool isWCAllowed(int slot, weaponClasses wc)
    {
        bool ret = false;
        if ((this.allowed[slot] & (1 << UnityScript.Lang.UnityBuiltins.parseInt((int) wc))) != 0)
        {
            ret = true;
        }
        return ret;
    }

    public virtual bool isWeaponAllowed(int slot, WeaponInfo w)
    {
        bool ret = false;
        if ((this.allowed[slot] & (1 << UnityScript.Lang.UnityBuiltins.parseInt((int) w.weaponClass))) != 0)
        {
            ret = true;
        }
        return ret;
    }

}