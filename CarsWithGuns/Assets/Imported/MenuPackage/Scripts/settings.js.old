@script AddComponentMenu("MenuPackage/MenuSettings")

//This is the script where GameSettings like sound,etc are provided

var guiskin: GUISkin;
var gc: GUIContent;
var cam: GameObject;
var background:GameObject;
private var toggleTxt : boolean = false;
private var txt: String="Sound On";

function Awake()
{
    if(PlayerPrefs.GetString("sound").Equals("true"))
    {        
		toggleTxt=false;
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
    
    toggleTxt=GUI.Toggle(Rect(50, 50, MenuScript.buttonWidth, MenuScript.buttonHeight), toggleTxt, txt);
    toggle();  
	//It is for the Options available in the MainMenu
	
	if(GUI.Button(Rect(50,100,MenuScript.buttonWidth,MenuScript.buttonHeight),"Back"))
	{	
	    //It disables the Settings script
		var setting: Behaviour= GetComponent("settings") as Behaviour;
		setting.enabled=false;
		//And enables the MenuScript
		var menu: Behaviour= GetComponent("MenuScript") as Behaviour;
		menu.enabled=true;		
		
	}	
	
}//end of GUI

function toggle()
{
    /*You can also use this function when you have a toggle button of EZGUI*/
    if(toggleTxt)
    {
        txt="Sound Off";
        PlayerPrefs.SetString("sound","false");
        if(background!=null)
		    background.GetComponent.<AudioSource>().mute=true;//Pausing BG Music
		AudioListener.volume=0;//Muting SFX           
    }
    
    else
    {
        txt="Sound On";
        PlayerPrefs.SetString("sound","true");
        if(background!=null)
		   background.GetComponent.<AudioSource>().mute=false;//Resuming BG Music
		AudioListener.volume=1;//Unmuting SFX
    }
}