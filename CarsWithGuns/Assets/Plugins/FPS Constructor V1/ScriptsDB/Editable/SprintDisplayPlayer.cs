using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class SprintDisplayPlayer : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 Copyright© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
    [UnityEngine.HideInInspector]
    public PlayerSprint aim;
    public virtual void Start()
    {
        this.aim = (PlayerSprint) this.GetComponent(typeof(PlayerSprint));
    }

    public virtual void OnGUI()
    {
        if (!this.aim.weaponsInactive)
        {
            return;
        }
        //temp is the percentage of sprint remaining
        float temp = AimMode.sprintNum / this.aim.values.sprintDuration;
        //baselength is the length of the bar at full sprint
        float baseLength = Screen.width * 0.5f;
        //ypos is the y position of the sprint bar
        float yPos = Screen.height;
        yPos = yPos - (Screen.height / 10);
        //tempLength is the length of the sprint bar we want to display
        float tempLength = Mathf.Clamp(baseLength * temp, baseLength * 0.03f, baseLength);
        //only display the bar if we are sprinting, and don't display if sprint is full
        if (PlayerSprint.sprinting || (AimMode.sprintNum < this.aim.values.sprintDuration))
        {
            //display the sprint bar - change this to modify the display.
            GUI.Box(new Rect((Screen.width - baseLength) / 2, yPos, tempLength, 10), "");
        }
    }

}