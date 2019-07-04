using UnityEngine;
using System.Collections;

[System.Serializable]
[UnityEngine.AddComponentMenu("MenuPackage/SplashScene")]
public partial class splash : MonoBehaviour
{
    /*This scene acts as a splash screen 
providing menuscene to load user preferences for sound
*/
    public Texture txtr;
    public virtual void FixedUpdate()
    {
         //After 5seconds MenuScene will be loaded
        if (Time.realtimeSinceStartup > 0.5f)
        {
            Application.LoadLevel("MenuScene");
        }
    }

    public virtual void OnGUI()
    {
         //To draw any texture as a Splash Screen
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), this.txtr, ScaleMode.StretchToFill, true);
    }

}