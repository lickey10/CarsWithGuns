using UnityEngine;
using System.Collections;

[System.Serializable]
[UnityEngine.AddComponentMenu("MenuPackage/loadSettings")]
public partial class loadSettings : MonoBehaviour
{
    //Background is the gameObject that contains background audio clip
    public GameObject background;
    public virtual void OnLevelWasLoaded(int level)
    {
        Resources.UnloadUnusedAssets();//To free memory which was being used by any unused asset
        /*
	Now as the name of this script suggests that it is for loading all the settings which were chosen by the user in the main menu.
	So what i did, i used PlayerPrefs to store the user preferences for sound and as i am using two images i.e., MusicOn and MusicOff
	so i check them based on the Key 'im' to load images and 'sound' to check whether to swtich on or off the music
	*/
        //Checking whether the key exists. For the first time it doesn't exist
        if (PlayerPrefs.HasKey("sound") == false)
        {
            if (this.background != null)
            {
                this.background.GetComponent<AudioSource>().mute = false;
            }
            AudioListener.volume = 1;
            PlayerPrefs.SetString("sound", "true");
        }
        else
        {
            if (PlayerPrefs.GetString("sound") == "true")//Whether the music is swtiched on
            {
                if (this.background != null)
                {
                    this.background.GetComponent<AudioSource>().mute = false;
                }
                AudioListener.volume = 1;
            }
            else
            {
                if (PlayerPrefs.GetString("sound") == "false")
                {
                    if (this.background != null)
                    {
                        this.background.GetComponent<AudioSource>().mute = true;
                    }
                    AudioListener.volume = 0;
                }
            }
        }
        Time.timeScale = 1;
    }

    public GUITexture faderTxtr;//you can drag the prefab into your scene and then assign it in the inspector view
    public virtual void Awake()
    {
         //To position and scale the texture in order to fill up the whole screen, no matter of whatever size it is
        if (this.faderTxtr)
        {
            this.faderTxtr.pixelInset.width = Screen.width;
            this.faderTxtr.pixelInset.height = Screen.height;
            this.faderTxtr.pixelInset.x = -(Screen.width / 2);
            this.faderTxtr.pixelInset.y = -(Screen.height / 2);
        }
    }

    public virtual void Start()
    {
         //To load the scene with fading effect
        this.StartCoroutine(Fade.use.Alpha(this.faderTxtr, 1f, 0f, 1f));
    }

}