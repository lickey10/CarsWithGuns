@script AddComponentMenu("MenuPackage/loadingScene")
/*This scene acts as a loading screen 
*/

var txtr: Texture;//The loading texture which will be displayed when loading scene appears
function Update ()
{
    if(Time.timeSinceLevelLoad>0.5)
        Application.LoadLevel(Application.loadedLevel+1);	
}


function OnGUI()
{
    //Actually draws the texture here
	GUI.DrawTexture(Rect(0,0,Screen.width,Screen.height),txtr,ScaleMode.StretchToFill,true);
}
