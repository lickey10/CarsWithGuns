using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class HighlightFX : MonoBehaviour
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
    public string objectName;
    public string description;
    public virtual void HighlightOn()
    {
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
            string s = "(Tab) to Use";
            Vector2 pos = Camera.main.WorldToScreenPoint(this.transform.position);
            GUI.Box(new Rect(pos.x - 77.5f, ((Screen.height - pos.y) - (Screen.height / 4)) - 52.5f, 155, 105), (((this.objectName + "\n \n") + this.description) + "\n") + s);
        }
    }

}