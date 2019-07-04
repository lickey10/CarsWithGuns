@script AddComponentMenu("MenuPackage/PlayAgainScene")

var txt: Texture;
var sceneToLoad: String;
var guiskin: GUISkin;
function OnGUI ()
{
	GUI.skin=guiskin;
	
	//Displaying the GameOver texture
	GUI.DrawTexture(Rect(0,0,Screen.width,Screen.height),txt,ScaleMode.StretchToFill,true);
	
	if(GUI.Button(Rect(Screen.width/2,10,MenuScript.buttonWidth,MenuScript.buttonHeight),"Play Again"))
	{
		Application.LoadLevel(sceneToLoad);
	}
}