using UnityEngine;
using System.Collections;

[System.Serializable]
[UnityEngine.AddComponentMenu("MenuPackage/PlayAgainScene")]
public partial class playAgain : MonoBehaviour
{
    public Texture txt;
    public string sceneToLoad;
    public GUISkin guiskin;
    public virtual void OnGUI()
    {
        GUI.skin = this.guiskin;
        //Displaying the GameOver texture
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), this.txt, ScaleMode.StretchToFill, true);
        if (GUI.Button(new Rect(Screen.width / 2, 10, MenuScript.buttonWidth, MenuScript.buttonHeight), "Play Again"))
        {
            Application.LoadLevel(this.sceneToLoad);
        }
    }

}