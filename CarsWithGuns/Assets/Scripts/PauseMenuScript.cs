using UnityEngine;
using System.Collections;

public class PauseMenuScript : MonoBehaviour {
	public GUIStyle customGuiStyle;
	public Texture2D ContinueButtonImage;
	public Texture2D QuitButtonImage;
    public GameObject ScrollingMenu;

    int logoX;
	int logoY;
	bool displayMenu = false;

	void OnStart()
	{
		customGuiStyle = new GUIStyle();
		
		customGuiStyle.font = (Font)Resources.Load("Fonts/advlit");
		customGuiStyle.active.textColor = Color.red; // not working
		customGuiStyle.hover.textColor = Color.blue; // not working
		customGuiStyle.normal.textColor = Color.green;
		customGuiStyle.fontSize = 50;
		
		customGuiStyle.stretchWidth = true; // ---
		customGuiStyle.stretchHeight = true; // not working, since backgrounds aren't showing
	}

	void Update()
	{
		//toggle pause menu
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKey(KeyCode.Menu))
		{
			//bool isPaused = transform.gameObject.GetComponent<generate>().PauseGame(true);
			bool isPaused = gamestate.Instance.SetGamePaused(!gamestate.Instance.GetGamePaused());
			
			if (isPaused)
			{
				displayMenu = true;

                Time.timeScale = 0;
                gamestate.Instance.SetGamePaused(true);

                try {
					//AudioSource backgroundMusic = (AudioSource)GameObject.Find("backgroud music").GetComponent<AudioSource>();
					//backgroundMusic.Pause ();
					//backgroundMusic.transform.localScale = new Vector3(backgroundMusic.time,0,0);

                    AudioListener.pause = true;

                } catch (System.Exception ex) {
					//oops, no audio found
				}
			}
			else
			{
				//AudioSource backgroundMusic = (AudioSource)GameObject.Find("backgroud music").GetComponent<AudioSource>();
				//backgroundMusic.Play ();
				//backgroundMusic.time = backgroundMusic.transform.localScale.x;

                AudioListener.pause = false;

                displayMenu = false;
			}
		}
	}

	void OnGUI()
	{
		if(displayMenu)
		{
			GUI.depth = 10;

			GUI.Box(new Rect((Screen.width - 400)/2, (Screen.height-150) / 2, 400, 250),"", new GUIStyle(GUI.skin.box));

			logoX = (Screen.width - 300 ) / 2;
			logoY = (Screen.height - 450) / 2;

			customGuiStyle.font = (Font)Resources.Load("Fonts/advlit");
			customGuiStyle.active.textColor = Color.red; // not working
			customGuiStyle.hover.textColor = Color.blue; // not working
			customGuiStyle.fontSize = 60;
			customGuiStyle.alignment = TextAnchor.MiddleCenter;

			//paused
			//drop shadow
			customGuiStyle.normal.textColor = Color.black;
			GUI.Box(new Rect( (Screen.width-450)/2+3, (Screen.height-200) / 2+3, 450, 30 ), "Paused!" ,customGuiStyle);
			
			customGuiStyle.normal.textColor = Color.green;
			GUI.Box(new Rect( (Screen.width-450)/2, (Screen.height-200) / 2, 450, 30 ), "Paused!" ,customGuiStyle);

			if(GUI.Button(new Rect((Screen.width-200)/2, (Screen.height-100) / 2, 200, 100),ContinueButtonImage,new GUIStyle()))
			{
				Time.timeScale = 1;
				gamestate.Instance.SetGamePaused(false);
				displayMenu = false;

				try {
					AudioSource backgroundMusic = (AudioSource)GameObject.Find("backgroud music").GetComponent<AudioSource>();
					backgroundMusic.Play ();
					backgroundMusic.time = backgroundMusic.transform.localScale.x;
				} catch (System.Exception ex) {
					//oops, no audio found
				}
			}

            if (GUI.Button(new Rect((Screen.width - 100) / 2, (Screen.height + 50) / 2, 100, 100), "Cars", customGuiStyle))
            {
                displayMenu = false;

                Instantiate(ScrollingMenu, new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, GameObject.FindGameObjectWithTag("Player").transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z), GameObject.FindGameObjectWithTag("Player").transform.rotation);
                //GameObject.FindGameObjectWithTag("ScrollingMenu").SetActive(true);
                GameObject.FindGameObjectWithTag("MenuCamera").GetComponent<Camera>().enabled = true;
                GameObject.FindGameObjectWithTag("MenuCameraMain").GetComponent<Camera>().enabled = true;
            }

            if (GUI.Button(new Rect((Screen.width-200)/2, (Screen.height+200) / 2, 200, 100),QuitButtonImage,new GUIStyle()))
			{
				Application.Quit();
			}

			GUI.depth = 0;
		}
	}
}
