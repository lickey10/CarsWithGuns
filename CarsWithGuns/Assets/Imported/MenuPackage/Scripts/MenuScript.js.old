@script AddComponentMenu("MenuPackage/MainMenuScript")

var guiskin: GUISkin;
private var guienable:boolean=true;
//Just provide any value here and other buttons will be set accordingly
static var buttonWidth: int=100;//static width of all the buttons
static var buttonHeight: int=50;//static height of all the buttons
var faderTxtr: GUITexture;
var fadeTime:float=0.3;
var fadeAlpha:float=0.7;
var flag:boolean=false;

function Awake()
{
    //If you have come from pause menu the the TimeScale has to be reset to 1
    Time.timeScale=1;
}


function OnGUI ()
{
	GUI.skin=guiskin;
     /*
    The aplha of the fading texture is altered depending on the condition whether you want to fade in
    or fade out. So in the next condition it is being checked that if the alpha of the texture is reached to
    the half of its value then load the scene, in order to give it an effect of fading in
    */
	if(faderTxtr.color.a==(fadeAlpha/2))
	{
	    //loading the actual scene here after fading effect
	    Application.LoadLevel(2);
	}
			
	if(GUI.Button(Rect(50,50,buttonWidth,buttonHeight),"Play Game"))
	{
	    //Not loading the scene here but just fading in
	    Fade.use.Alpha(faderTxtr, 0.0, fadeAlpha, fadeTime, EaseType.In);
	}	
	
	if(GUI.Button(Rect(250,50,150,buttonHeight),"Level Scroll Box"))
	{
		Application.LoadLevel("_ThemeSelection");
	    //Not loading the scene here but just fading in
	    //Fade.use.Alpha(faderTxtr, 0.0, fadeAlpha, fadeTime, EaseType.In);
	}
	
	/*
	In the main menu you could see there are two buttons available there
	1. PlayGame
	2. Options
	
	PlayGame loads the scene next to it which is available in the BuildSettings
	
	Options button in such a way that when you click on it Settings script gets enabled and if you click Back
	then MenuScript is enabled again
	*/
	
	if(GUI.Button(Rect(50,150,buttonWidth,buttonHeight),"Options"))
	{	
	    //Disabling MenuScript here
		var op: Behaviour= GetComponent("MenuScript") as Behaviour;
		op.enabled=false;
		
		//Enabling Settings Script here
		var setting: Behaviour= GetComponent("settings") as Behaviour;
		setting.enabled=true;
	}	
   	
   
}//end of GUI




