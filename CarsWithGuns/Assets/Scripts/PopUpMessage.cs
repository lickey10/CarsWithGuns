using Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopUpMessage : MonoBehaviour {
    public GUIStyle customGuiStyle;
    public string Message = "";
    public string Description = "";
    public Object SceneToLoad = null;
    
    public GameObject[] DoorsToOpen = null;
    private Transform target;
    private bool showMessage = false;
    private bool doorOpen = false;
    private bool showLoading = false;
    

    // Use this for initialization
    void Start () {
        customGuiStyle = new GUIStyle();

        customGuiStyle.font = (Font)Resources.Load("Fonts/advlit");
        customGuiStyle.active.textColor = Color.red; // not working
        customGuiStyle.hover.textColor = Color.blue; // not working
        customGuiStyle.normal.textColor = Color.green;
        customGuiStyle.fontSize = 50;
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    protected virtual void OnGUI()
    {
        if(showMessage)
        {
            GUI.skin.box.wordWrap = true;
            string s = "(Tab) to Interact";

            customGuiStyle.normal.textColor = Color.white;
            Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
            GUI.Box(new Rect(pos.x - 77.5f, Screen.height - pos.y - (Screen.height / 4) - 52.5f, 155, 105), Message + "\n \n" + Description + "\n" + s);
        }
        else if (showLoading)
        {
            GUI.skin.box.wordWrap = true;
            
            customGuiStyle.normal.textColor = Color.white;
            Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
            //GUI.TextArea.Box(new Rect(pos.x - 77.5f, Screen.height - pos.y - (Screen.height / 4) - 52.5f, 155, 75), "Loading...");
        }
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            showMessage = true;
        }
    }

    void OnTriggerExit(Collider otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            showMessage = false;
        }
    }

    void Interact()
    {
        if (!doorOpen)
        {
            if (DoorsToOpen.Length > 0)
            {
                foreach (GameObject doorToOpen in DoorsToOpen)
                {
                    iTweenEvent.GetEvent(doorToOpen, "openDoor").Play();
                }

                doorOpen = true;
            }
            else
            {
                Teleport();
                doorOpen = true;
            }
        }

        showMessage = false;
    }

    public void Teleport()
    {
        if (SceneToLoad != null)
        {
            showLoading = true;
            SceneManager.LoadScene(SceneToLoad.name);

            //GameObject screenManager = GameObject.FindWithTag("ScreenManager");
            //ScreenManager sceneManager = screenManager.GetComponent<ScreenManager>();
            //StartCoroutine(sceneManager.LoadSceneAsync2(SceneToLoad.name));
        }
    }
}
