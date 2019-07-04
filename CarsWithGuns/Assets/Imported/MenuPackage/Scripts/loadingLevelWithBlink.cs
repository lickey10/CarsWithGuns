using UnityEngine;
using System.Collections;
[AddComponentMenu ("MenuPackage/Load Level With Blinking Texture")]

public class loadingLevelWithBlink : MonoBehaviour 
{
	public float speed;
	bool firstTime=false;
	Color c=new Color(1,1,1,1);
	public GUITexture target;
	
	// Use this for initialization
	void Start () 
	{
		StartCoroutine(LoadGame());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator LoadGame()
    {
        AsyncOperation async = Application.LoadLevelAsync(Application.loadedLevel+1);//this method loads level asynchoronously
        while (!async.isDone)//while the next level is not loaded do animate the following
        {
			if(!firstTime && c.a!=0)
			{
				c.a=Mathf.MoveTowards(c.a,0,Time.deltaTime*speed);
				target.color=c;
			}
			else if(!firstTime && c.a==0)
			{
				firstTime=true;
			}
			
			if(firstTime && c.a!=1.0f)
			{
				c=Vector4.MoveTowards(c,new Vector4(1,1,1,1),Time.deltaTime*speed*2);
				target.color=c;
			}
			else
				firstTime=false;
		
			yield return 0;
        
		}//end of while
		
	}//end of LoadGame()
}
