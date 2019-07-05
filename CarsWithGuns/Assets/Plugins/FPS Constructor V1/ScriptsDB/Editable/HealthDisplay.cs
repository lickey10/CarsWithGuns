using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class HealthDisplay : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 Copyright© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
    public PlayerHealth playerHealth;
    public virtual void Start()
    {
        this.playerHealth = (PlayerHealth) this.GetComponent(typeof(PlayerHealth));
    }

    public virtual void OnGUI()
    {
        GUI.Box(new Rect(10, Screen.height - 30, 100, 20), "Health: " + Mathf.Round(this.playerHealth.health));
    }

}