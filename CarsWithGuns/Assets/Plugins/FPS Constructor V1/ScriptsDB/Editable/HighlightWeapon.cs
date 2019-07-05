using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class HighlightWeapon : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 Copyright© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
 For additional information contact us info@dastardlybanana.com.
*/
    // Example script display effects when a pickupable weapon is highlighted. 
    // It implements the HighlightOn() and HighlightOff() functions which are called when the systems sends those 
    private bool selected;
    private SelectableWeapon info;
    private bool equipped;
    public virtual void Start()
    {
        this.info = (SelectableWeapon) this.GetComponent(typeof(SelectableWeapon));
    }

    public virtual void HighlightOn()
    {
        this.equipped = PickupWeapon.CheckWeapons(this.gameObject);
        this.selected = true;
    }

    public virtual void HighlightOff()
    {
        this.selected = false;
    }

    public virtual void OnGUI()
    {
        GUI.skin.box.wordWrap = true;
        if (this.selected && !DBStoreController.inStore)
        {
            string s = "(Tab) to Select";
            if (this.equipped)
            {
                s = "(Already Equipped)";
            }
            Vector2 pos = Camera.main.WorldToScreenPoint(this.transform.position);
            GUI.Box(new Rect(pos.x - 77.5f, ((Screen.height - pos.y) - (Screen.height / 4)) - 52.5f, 155, 105), (((this.info.WeaponInfo.gunName + "\n \n") + this.info.WeaponInfo.gunDescription) + "\n") + s);
        }
    }

}