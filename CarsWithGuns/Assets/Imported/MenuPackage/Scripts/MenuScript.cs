using UnityEngine;
using System.Collections;

[System.Serializable]
[UnityEngine.AddComponentMenu("MenuPackage/MainMenuScript")]
public partial class MenuScript : MonoBehaviour
{
    public GUISkin guiskin;
    private bool guienable;
    //Just provide any value here and other buttons will be set accordingly
    public static int buttonWidth;//static width of all the buttons
    public static int buttonHeight;//static height of all the buttons
    public GUITexture faderTxtr;
    public float fadeTime;
    public float fadeAlpha;
    public bool flag;
    public virtual void Awake()
    {
         //If you have come from pause menu the the TimeScale has to be reset to 1
        Time.timeScale = 1;
    }

    public virtual void OnGUI()//end of GUI
    {
        GUI.skin = this.guiskin;
        /*
    The aplha of the fading texture is altered depending on the condition whether you want to fade in
    or fade out. So in the next condition it is being checked that if the alpha of the texture is reached to
    the half of its value then load the scene, in order to give it an effect of fading in
    */
        if (this.faderTxtr.color.a == (this.fadeAlpha / 2))
        {
             //loading the actual scene here after fading effect
            Application.LoadLevel(2);
        }
        if (GUI.Button(new Rect(50, 50, MenuScript.buttonWidth, MenuScript.buttonHeight), "Play Game"))
        {
             //Not loading the scene here but just fading in
            this.StartCoroutine(Fade.use.Alpha(this.faderTxtr, 0f, this.fadeAlpha, this.fadeTime, EaseType.In));
        }
        if (GUI.Button(new Rect(250, 50, 150, MenuScript.buttonHeight), "Level Scroll Box"))
        {
            Application.LoadLevel("_ThemeSelection");
        }
        //Not loading the scene here but just fading in
        //Fade.use.Alpha(faderTxtr, 0.0, fadeAlpha, fadeTime, EaseType.In);
        /*
	In the main menu you could see there are two buttons available there
	1. PlayGame
	2. Options
	
	PlayGame loads the scene next to it which is available in the BuildSettings
	
	Options button in such a way that when you click on it Settings script gets enabled and if you click Back
	then MenuScript is enabled again
	*/
        if (GUI.Button(new Rect(50, 150, MenuScript.buttonWidth, MenuScript.buttonHeight), "Options"))
        {
             //Disabling MenuScript here
            Behaviour op = this.GetComponent("MenuScript") as Behaviour;
            op.enabled = false;
            //Enabling Settings Script here
            Behaviour setting = this.GetComponent("settings") as Behaviour;
            setting.enabled = true;
        }
    }

    public MenuScript()
    {
        this.guienable = true;
        this.fadeTime = 0.3f;
        this.fadeAlpha = 0.7f;
    }

    static MenuScript()
    {
        MenuScript.buttonWidth = 100;
        MenuScript.buttonHeight = 50;
    }

}