using UnityEngine;
using System.Collections;

[System.Serializable]
[UnityEngine.AddComponentMenu("MenuPackage/LoadMenu")]
public partial class loadMenu : MonoBehaviour
{
    public GUISkin guskin;
    public virtual void OnGUI()
    {
        GUI.skin = this.guskin;
        if (GUI.Button(new Rect(Screen.width - 70, 0, 70, 50), "Menu"))
        {
             //Just loads the Main Menu
            Application.LoadLevel("MenuScene");
        }
    }

}