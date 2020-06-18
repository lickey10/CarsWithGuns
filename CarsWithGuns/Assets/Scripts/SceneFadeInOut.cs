using UnityEngine;

using System.Collections;


public class SceneFadeInOut : MonoBehaviour
	
{
	public float fadeSpeed = 1.5f;          // Speed that the screen fades to and from black.
	
	private bool sceneStarting = true;      // Whether or not the scene is still fading in.
	
	public UnityEngine.UI.Text guiTextre;
	
	
	
	void Awake ()
		
	{
		
		// Set the texture so that it is the the size of the screen and covers it.
		
		//GetComponent<UnityEngine.UI.Image>().pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
		
	}
	
	
	
	
	
	void Update ()
		
	{
		
		// If the scene is starting...
		
		if(sceneStarting)
			
			// ... call the StartScene function.
			
			StartScene();
		
	}
	
	
	
	
	
	public void FadeToClear ()
		
	{
		
		// Lerp the colour of the texture between itself and transparent.
		
		GetComponent<UnityEngine.UI.Image>().color = Color.Lerp(GetComponent<UnityEngine.UI.Image>().color, Color.clear, fadeSpeed * Time.deltaTime);
		
	}
	
	
	
	
	
	public void FadeToBlack ()
		
	{
		
		// Lerp the colour of the texture between itself and black.
		
		GetComponent<UnityEngine.UI.Image>().color = Color.Lerp(GetComponent<UnityEngine.UI.Image>().color, Color.black, fadeSpeed * Time.deltaTime);
		
	}
	
	
	
	
	
	void StartScene ()
		
	{
		
		// Fade the texture to clear.
		
		FadeToClear();
		
		
		
		// If the texture is almost clear...
		
		if(GetComponent<UnityEngine.UI.Image>().color.a <= 0.05f)
			
		{
			
			// ... set the colour to clear and disable the GUITexture.
			
			GetComponent<UnityEngine.UI.Image>().color = Color.clear;
			
			GetComponent<UnityEngine.UI.Image>().enabled = false;
			
			
			
			// The scene is no longer starting.
			
			sceneStarting = false;
			
		}
		
	}
	
	
	
	
	
	public void EndScene (string name)
		
	{
		
		// Make sure the texture is enabled.
		
		GetComponent<UnityEngine.UI.Image>().enabled = true;
		
		
		
		// Start fading towards black.
		
		FadeToBlack();
		
		
		
		// If the screen is almost black...
		
		if(GetComponent<UnityEngine.UI.Image>().color.a >= 0.95f)
			
			// ... reload the level.
			
			Application.LoadLevel(name);
		
	}
	
}