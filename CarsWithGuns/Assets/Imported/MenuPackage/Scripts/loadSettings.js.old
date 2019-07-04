@script AddComponentMenu("MenuPackage/loadSettings")

//Background is the gameObject that contains background audio clip
var background: GameObject;


function OnLevelWasLoaded(level: int)
{	
	Resources.UnloadUnusedAssets();//To free memory which was being used by any unused asset
	
	/*
	Now as the name of this script suggests that it is for loading all the settings which were chosen by the user in the main menu.
	So what i did, i used PlayerPrefs to store the user preferences for sound and as i am using two images i.e., MusicOn and MusicOff
	so i check them based on the Key 'im' to load images and 'sound' to check whether to swtich on or off the music
	*/
	
	//Checking whether the key exists. For the first time it doesn't exist
	if(PlayerPrefs.HasKey("sound")==false)
	{
		if(background!=null)
			background.GetComponent.<AudioSource>().mute=false;
		AudioListener.volume=1;
		PlayerPrefs.SetString("sound","true");		
	}
	
	else
	{
	  if(PlayerPrefs.GetString("sound")=="true")//Whether the music is swtiched on
		{		    
			if(background!=null)
			    background.GetComponent.<AudioSource>().mute=false;
		    AudioListener.volume=1;
		}
		else if(PlayerPrefs.GetString("sound")=="false")
		{			
			if(background!=null)
			    background.GetComponent.<AudioSource>().mute=true;
		    AudioListener.volume=0;
		}		
	}
	Time.timeScale=1;
}

var faderTxtr: GUITexture;//you can drag the prefab into your scene and then assign it in the inspector view

function Awake()
{	
    //To position and scale the texture in order to fill up the whole screen, no matter of whatever size it is
    if(faderTxtr)
    {
	    faderTxtr.pixelInset.width=Screen.width;
	    faderTxtr.pixelInset.height=Screen.height;
	    faderTxtr.pixelInset.x=-(Screen.width/2);
	    faderTxtr.pixelInset.y=-(Screen.height/2);
	}
}

function Start()
{
    //To load the scene with fading effect
    Fade.use.Alpha(faderTxtr, 1.0, 0.0, 1.0);
}


