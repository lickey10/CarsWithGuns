#pragma strict
@script AddComponentMenu("MenuPackage/PauseGame")

/*The real script for which you paid $5*/

var guiskin: GUISkin;//Skin for GUI
private var flag:boolean=false;
var background: GameObject;
private var guiEnable: boolean=true;
var arr: Object[];
private var toggleTxt : boolean = false;
private var txt: String="Sound On";
/*
  Please note that: never allow PlayOnAwake feature on any sound to true.
  The reason behind that is that it would play the sound irrespective of the fact that the user has swtiched off
  the music in the MainMenu
 */


function Awake()
{	
	arr=GameObject.FindSceneObjectsOfType(MonoBehaviour);//Getting references of all those gameObjects which have scripts on them
    //Changing the boolean value of toggle button according to the PlayerPrefs.
    //Now you see a slight change in assigning the values. Whenever the sound is true, the value of toggleTxt is false and vice-versa.
    //it is because of the reason that when the value is false you won't see a cross mark against the toggle button but on true you would.
    //So just to give it a better feel of switching the sound on/off, i used that approach.
    
    if(PlayerPrefs.GetString("sound").Equals("true"))
    {        
		toggleTxt=false;//setting the value of toggleTxt as false whenever the sound is true
		txt="Sound On";
    }
    else if(PlayerPrefs.GetString("sound").Equals("false"))
    {        
		toggleTxt=true;
		txt="Sound Off";
    }
}



function OnGUI ()
{
	GUI.skin=guiskin;
	
	//guiEnable is taken for one thing when you pause the menu all buttons other than those available in the
	//the pause menu gets disabled so that user won't be able to click or touch them

	GUI.enabled=guiEnable;
	
	if(GUI.Button(Rect((Screen.width-MenuScript.buttonWidth),0,MenuScript.buttonWidth,MenuScript.buttonHeight),"Menu"))
	{
	    //Calling the function to pause the game
		OnPause();
	}
	
	if(GUI.Button(Rect((Screen.width-(2*MenuScript.buttonWidth)),0,MenuScript.buttonWidth,MenuScript.buttonHeight),"Restart"))
	{
		restart();
	}
	GUI.enabled=guiEnable;

	
	GUI.enabled=true;
	if(flag==true)
	{
		// Register the window. This window gets generated on pause
		var windowRect = GUI.Window (0, Rect((Screen.width/2-150),(Screen.height/2-150),300,300), winpause, "Pause Menu");
		guiEnable=false;//Disabling other buttons here
	}
	GUI.enabled=true;
	
}



// Make the contents of the window
function winpause (windowID : int)
{
   toggleTxt=GUI.Toggle(Rect(100, 90, MenuScript.buttonWidth, MenuScript.buttonHeight), toggleTxt, txt);
   toggle();  
  
	
	if(GUI.Button(Rect(100,150,MenuScript.buttonWidth,MenuScript.buttonHeight),"Resume"))
	{		
		resume();
	}
	
	if(GUI.Button(Rect(100,200,MenuScript.buttonWidth,MenuScript.buttonHeight),"Level Select"))
	{		
		Application.LoadLevel("levelSelect");
	}
	
	if(GUI.Button(Rect(100,250,MenuScript.buttonWidth,MenuScript.buttonHeight),"Menu"))
	{		
		menu();
	}
	
 	GUI.DragWindow();

}
/*This function gets fired automatically when the game loses focus*/
function OnApplicationPause (bool:boolean)
{
	if(bool)
		StartCoroutine("pause","true");	
}

/*The function where the pause fucntionality is actually implemented*/
function pause(str:String)
{
    //It contains the references of all those gameObjects which contain one or many scripts on them.
    
	arr=GameObject.FindSceneObjectsOfType(MonoBehaviour);
	//str is true so we need to disable all the scripts so that they won't execute during the pause.
	//You can avoid this by avoiding the use of Update() and OnGUI() and simply altering TimeScale from 0 to 1 and vice-versa
	if(str.Equals("true"))
	{
		for(var script in arr)
		{
			if((script as MonoBehaviour).name!=this.gameObject.name)
				(script as MonoBehaviour).enabled=false;
		}

		Time.timeScale=0;
		if(background!=null)
			background.GetComponent.<AudioSource>().mute=true;//Pausing BG Music
		AudioListener.volume=0;//Muting SFX
		flag=true;
	}
	else if(str.Equals("false"))
	{
	//Enabling all the scripts on resume
		for(var script in arr)
		{
			if((script as MonoBehaviour).name!=this.gameObject.name)
				(script as MonoBehaviour).enabled=true;
		}
		
		/*We have to go with the user preferences on resume so determining whether to swtich on or off the music*/
		//If the sound is On during the PauseMenu
		if(PlayerPrefs.GetString("sound")=="true")
		{			
		    if(background!=null)
			    background.GetComponent.<AudioSource>().mute=false;//Pausing BG Music
		    AudioListener.volume=1;//Muting SFX
		}
		//If the sound is Off during the PauseMenu
		else if(PlayerPrefs.GetString("sound")=="false")
		{
			if(background!=null)
			    background.GetComponent.<AudioSource>().mute=true;//Pausing BG Music
		    AudioListener.volume=0;//Muting SFX
		}
		
		Time.timeScale=1;		
		flag=false;
		guiEnable=true;
	}
	StopAllCoroutines();
}


function resume()
{
	StartCoroutine("pause","false");
}

function restart()
{
	Application.LoadLevel(Application.loadedLevel);
}

function menu()
{
	Application.LoadLevel("MenuScene");
}

function OnPause()
{
	OnApplicationPause(true);
}

function toggle()
{
    /*You can also use this function when you have a toggle button of EZGUI*/
    if(toggleTxt)
    {
        txt="Sound Off";
        PlayerPrefs.SetString("sound","false");
        
    }
    
    else
    {
        txt="Sound On";
        PlayerPrefs.SetString("sound","true");
    }
}

