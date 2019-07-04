using UnityEngine;
using System.Collections;

[System.Serializable]
[UnityEngine.AddComponentMenu("MenuPackage/PauseGame")]
public partial class pauseGame : MonoBehaviour
{
    /*The real script for which you paid $5*/
    public GUISkin guiskin;//Skin for GUI
    private bool flag;
    public GameObject background;
    private bool guiEnable;
    public object[] arr;
    private bool toggleTxt;
    private string txt;
    /*
  Please note that: never allow PlayOnAwake feature on any sound to true.
  The reason behind that is that it would play the sound irrespective of the fact that the user has swtiched off
  the music in the MainMenu
 */
    public virtual void Awake()
    {
        this.arr = GameObject.FindSceneObjectsOfType(typeof(MonoBehaviour));//Getting references of all those gameObjects which have scripts on them
        //Changing the boolean value of toggle button according to the PlayerPrefs.
        //Now you see a slight change in assigning the values. Whenever the sound is true, the value of toggleTxt is false and vice-versa.
        //it is because of the reason that when the value is false you won't see a cross mark against the toggle button but on true you would.
        //So just to give it a better feel of switching the sound on/off, i used that approach.
        if (PlayerPrefs.GetString("sound").Equals("true"))
        {
            this.toggleTxt = false;//setting the value of toggleTxt as false whenever the sound is true
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

    public virtual void OnGUI()
    {
        GUI.skin = this.guiskin;
        //guiEnable is taken for one thing when you pause the menu all buttons other than those available in the
        //the pause menu gets disabled so that user won't be able to click or touch them
        GUI.enabled = this.guiEnable;
        if (GUI.Button(new Rect(Screen.width - MenuScript.buttonWidth, 0, MenuScript.buttonWidth, MenuScript.buttonHeight), "Menu"))
        {
             //Calling the function to pause the game
            this.OnPause();
        }
        if (GUI.Button(new Rect(Screen.width - (2 * MenuScript.buttonWidth), 0, MenuScript.buttonWidth, MenuScript.buttonHeight), "Restart"))
        {
            this.restart();
        }
        GUI.enabled = this.guiEnable;
        GUI.enabled = true;
        if (this.flag == true)
        {
             // Register the window. This window gets generated on pause
            Rect windowRect = GUI.Window(0, new Rect((Screen.width / 2) - 150, (Screen.height / 2) - 150, 300, 300), this.winpause, "Pause Menu");
            this.guiEnable = false;//Disabling other buttons here
        }
        GUI.enabled = true;
    }

    // Make the contents of the window
    public virtual void winpause(int windowID)
    {
        this.toggleTxt = GUI.Toggle(new Rect(100, 90, MenuScript.buttonWidth, MenuScript.buttonHeight), this.toggleTxt, this.txt);
        this.toggle();
        if (GUI.Button(new Rect(100, 150, MenuScript.buttonWidth, MenuScript.buttonHeight), "Resume"))
        {
            this.resume();
        }
        if (GUI.Button(new Rect(100, 200, MenuScript.buttonWidth, MenuScript.buttonHeight), "Level Select"))
        {
            Application.LoadLevel("levelSelect");
        }
        if (GUI.Button(new Rect(100, 250, MenuScript.buttonWidth, MenuScript.buttonHeight), "Menu"))
        {
            this.menu();
        }
        GUI.DragWindow();
    }

    /*This function gets fired automatically when the game loses focus*/
    public virtual void OnApplicationPause(bool @bool)
    {
        if (@bool)
        {
            this.StartCoroutine("pause", "true");
        }
    }

    /*The function where the pause fucntionality is actually implemented*/
    public virtual void pause(string str)
    {
         //It contains the references of all those gameObjects which contain one or many scripts on them.
        this.arr = GameObject.FindSceneObjectsOfType(typeof(MonoBehaviour));
        //str is true so we need to disable all the scripts so that they won't execute during the pause.
        //You can avoid this by avoiding the use of Update() and OnGUI() and simply altering TimeScale from 0 to 1 and vice-versa
        if (str.Equals("true"))
        {
            foreach (object script in this.arr)
            {
                if ((script as MonoBehaviour).name != this.gameObject.name)
                {
                    (script as MonoBehaviour).enabled = false;
                }
            }
            Time.timeScale = 0;
            if (this.background != null)
            {
                this.background.GetComponent<AudioSource>().mute = true;//Pausing BG Music
            }
            AudioListener.volume = 0;//Muting SFX
            this.flag = true;
        }
        else
        {
            if (str.Equals("false"))
            {
                //Enabling all the scripts on resume
                foreach (object script in this.arr)
                {
                    if ((script as MonoBehaviour).name != this.gameObject.name)
                    {
                        (script as MonoBehaviour).enabled = true;
                    }
                }
                /*We have to go with the user preferences on resume so determining whether to swtich on or off the music*/
                //If the sound is On during the PauseMenu
                if (PlayerPrefs.GetString("sound") == "true")
                {
                    if (this.background != null)
                    {
                        this.background.GetComponent<AudioSource>().mute = false;//Pausing BG Music
                    }
                    AudioListener.volume = 1;//Muting SFX
                }
                else
                {
                    //If the sound is Off during the PauseMenu
                    if (PlayerPrefs.GetString("sound") == "false")
                    {
                        if (this.background != null)
                        {
                            this.background.GetComponent<AudioSource>().mute = true;//Pausing BG Music
                        }
                        AudioListener.volume = 0;//Muting SFX
                    }
                }
                Time.timeScale = 1;
                this.flag = false;
                this.guiEnable = true;
            }
        }
        this.StopAllCoroutines();
    }

    public virtual void resume()
    {
        this.StartCoroutine("pause", "false");
    }

    public virtual void restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public virtual void menu()
    {
        Application.LoadLevel("MenuScene");
    }

    public virtual void OnPause()
    {
        this.OnApplicationPause(true);
    }

    public virtual void toggle()
    {
         /*You can also use this function when you have a toggle button of EZGUI*/
        if (this.toggleTxt)
        {
            this.txt = "Sound Off";
            PlayerPrefs.SetString("sound", "false");
        }
        else
        {
            this.txt = "Sound On";
            PlayerPrefs.SetString("sound", "true");
        }
    }

    public pauseGame()
    {
        this.guiEnable = true;
        this.txt = "Sound On";
    }

}