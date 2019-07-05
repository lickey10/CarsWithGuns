using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class SprintDisplay : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 Copyright© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
    [UnityEngine.HideInInspector]
    public AimMode aim;
    public virtual void Start()
    {
        this.aim = (AimMode) this.GetComponent(typeof(AimMode));
    }

    public virtual void OnGUI()
    {
        if (((this.aim == null) || (this.aim.GunScript1 == null)) || !this.aim.GunScript1.gunActive)
        {
            return;
        }
        if ((this.aim.scopeTexture != null) && this.aim.inScope)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), this.aim.scopeTexture, ScaleMode.StretchToFill);
        }
        //temp is the percentage of sprint remaining
        float temp = AimMode.sprintNum / this.aim.sprintDuration;
        //baselength is the length of the bar at full sprint
        float baseLength = Screen.width * 0.5f;
        //ypos is the y position of the sprint bar
        float yPos = Screen.height;
        yPos = yPos - (Screen.height / 10);
        //tempLength is the length of the sprint bar we want to display
        float tempLength = Mathf.Clamp(baseLength * temp, baseLength * 0.03f, baseLength);
        //only display the bar if we are sprinting, and don't display if sprint is full
        if (this.aim.sprinting || (AimMode.sprintNum < this.aim.sprintDuration))
        {
            //display the sprint bar - change this to modify the display.
            GUI.Box(new Rect((Screen.width - baseLength) / 2, yPos, tempLength, 10), "");
        }
    }

}