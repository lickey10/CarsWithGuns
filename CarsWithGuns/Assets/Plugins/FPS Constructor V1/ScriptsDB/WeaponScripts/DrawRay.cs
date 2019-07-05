using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class DrawRay : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 Copyright© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
    public bool needsSelection;
    public bool negative;
    public virtual void OnDrawGizmosSelected()
    {
        if (this.needsSelection)
        {
            Gizmos.color = Color.white;
            Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 10));
            Gizmos.DrawWireSphere(pos, 0.05f);
        }
    }

    public virtual void OnDrawGizmos()
    {
        if (!this.needsSelection)
        {
            Gizmos.color = Color.white;
            Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 10));
            Gizmos.DrawWireSphere(pos, 0.05f);
        }
    }

}