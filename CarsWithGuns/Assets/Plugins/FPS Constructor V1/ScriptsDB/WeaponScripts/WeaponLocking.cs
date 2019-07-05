using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class WeaponLocking : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 Copyright� Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
    public bool isLocked;
    public virtual void Lock()
    {
        this.isLocked = true;
    }

    public virtual void Unlock()
    {
        this.isLocked = false;
    }

}