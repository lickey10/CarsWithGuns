using UnityEngine;
using System.Collections;

[System.Serializable]
[UnityEngine.AddComponentMenu("MenuPackage/MenuSettings")]
public partial class settings : MonoBehaviour
{
    //This is the script where GameSettings like sound,etc are provided
    public GUISkin guiskin;
    public GUIContent gc;
    public GameObject cam;
    public GameObject background;
    private bool toggleTxt;
    private string txt;
    public virtual void Awake()
    {
        if (PlayerPrefs.GetString("sound").Equals("true"))
        {
            this.toggleTxt = false;
            this.txt = "Sound On";
        }
        else
        {
            if (PlayerPrefs.GetString("sound").Equals("false"))
            {
                this.toggleTxt = true;
                this.txt = "Sound Off";
            }
        }
    }

    public virtual void OnGUI()//end of GUI
    {
        GUI.skin = this.guiskin;
        this.toggleTxt = GUI.Toggle(new Rect(50, 50, MenuScript.buttonWidth, MenuScript.buttonHeight), this.toggleTxt, this.txt);
        this.toggle();
        //It is for the Options available in the MainMenu
        if (GUI.Button(new Rect(50, 100, MenuScript.buttonWidth, MenuScript.buttonHeight), "Back"))
        {
             //It disables the Settings script
            Behaviour setting = this.GetComponent("settings") as Behaviour;
            setting.enabled = false;
            //And enables the MenuScript
            Behaviour menu = this.GetComponent("MenuScript") as Behaviour;
            menu.enabled = true;
        }
    }

    public virtual void toggle()
    {
         /*You can also use this function when you have a toggle button of EZGUI*/
        if (this.toggleTxt)
        {
            this.txt = "Sound Off";
            PlayerPrefs.SetString("sound", "false");
            if (this.background != null)
            {
                this.background.GetComponent<AudioSource>().mute = true;//Pausing BG Music
            }
            AudioListener.volume = 0;//Muting SFX           
        }
        else
        {
            this.txt = "Sound On";
            PlayerPrefs.SetString("sound", "true");
            if (this.background != null)
            {
                this.background.GetComponent<AudioSource>().mute = false;//Resuming BG Music
            }
            AudioListener.volume = 1;//Unmuting SFX
        }
    }

    public settings()
    {
        this.txt = "Sound On";
    }

}