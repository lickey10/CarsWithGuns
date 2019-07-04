using UnityEngine;
using System.Collections;

[System.Serializable]
[UnityEngine.AddComponentMenu("MenuPackage/loadingScene")]
public partial class loading : MonoBehaviour
{
    /*This scene acts as a loading screen 
*/
    public Texture txtr;//The loading texture which will be displayed when loading scene appears
    public virtual void Update()
    {
        if (Time.timeSinceLevelLoad > 0.5f)
        {
            Application.LoadLevel(Application.loadedLevel + 1);
        }
    }

    public virtual void OnGUI()
    {
         //Actually draws the texture here
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), this.txtr, ScaleMode.StretchToFill, true);
    }

}