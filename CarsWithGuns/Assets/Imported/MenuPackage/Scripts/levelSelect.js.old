@script AddComponentMenu("MenuPackage/LevelSelect")

var speed: float=0.5;
var hit: RaycastHit;
var ray: Ray;
var loadingPlane:Transform;
var faderTxtr: GUITexture;
static var fadeTime:float=0.3;
static var fadeAlpha:float=0.7;
var flag: boolean=false;

function Start()
{	
    //Fading texture used for fading in and out
	faderTxtr= GameObject.Find("faderTexture").GetComponent(GUITexture) as GUITexture;
	//Loading texture you see whenever any scene is loading
	//Initially it must be deactive
	if(loadingPlane.gameObject.active)
		loadingPlane.gameObject.active=false;
}


function FixedUpdate()
{
    /*
    The aplha of the fading texture is altered depending on the condition whether you want to fade in
    or fade out. So in the next condition it is being checked that if the alpha of the texture is reached to
    the half of its value then load the scene, in order to give it an effect of fading in and flag determines
    that a load scene button has been clicked
    */
    
	if(faderTxtr.color.a==(fadeAlpha/2) && flag)
	{	    
	    Application.LoadLevel(hit.transform.name);
	}
	
	/*The next condition is to detect a mouse event in order to load a desired scene*/
	if(Input.GetMouseButtonDown(0))
	{
	
		ray=Camera.main.ScreenPointToRay(Input.mousePosition);
		/*
		Raycast determines whether a collider has been hit and then load only that particular scene
		for which the raycast is registered
		*/
		if(Physics.Raycast(ray,hit) && hit.transform.name==transform.name)
		{
			hit.transform.GetComponent.<Renderer>().material.color=Color.red;
			loadingPlane.gameObject.active=true;
			Fade.use.Alpha(faderTxtr, 0.0, fadeAlpha, fadeTime, EaseType.In);
			flag=true;	
		}
	}
	/*Same above but for the touch events*/
	for(var touch in Input.touches)
	{	
		if(touch.phase==TouchPhase.Began)
		{		
			ray=Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
			
			if(Physics.Raycast(ray,hit) && hit.transform.name==transform.name)
			{
				hit.transform.GetComponent.<Renderer>().material.color=Color.red;
				loadingPlane.gameObject.active=true;
				flag=true;
			}
		}
		
		if(touch.phase==TouchPhase.Ended)
		{		
			//renderer.material.color=Color.white;
		}
	}
}

