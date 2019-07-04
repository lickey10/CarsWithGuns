@script AddComponentMenu("MenuPackage/SplashScene")

/*This scene acts as a splash screen 
providing menuscene to load user preferences for sound
*/
var txtr: Texture;

function FixedUpdate()
{	
	//After 5seconds MenuScene will be loaded
	if(Time.realtimeSinceStartup>0.5)
	{
		Application.LoadLevel("MenuScene");
	}
}
function OnGUI()
{
	//To draw any texture as a Splash Screen
	GUI.DrawTexture(Rect(0,0,Screen.width,Screen.height),txtr,ScaleMode.StretchToFill,true);
}