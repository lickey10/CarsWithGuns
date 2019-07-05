using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NextHandler : MonoBehaviour {
    
    public UILabel ButtonLabel;
    public GameObject[] ObjectsThatNeedToHide;
    public GameObject[] ObjectsThatNeedToShow;

    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void NextClicked()
    {
        if(ButtonLabel.text == "Start!")
        {
            GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
            scripts.GetComponent<gamestart>().StartGame();
            return;
        }

        ButtonLabel.text = "Start!";

        foreach(GameObject go in ObjectsThatNeedToHide)
        {
            go.SetActive(false);
        }

        foreach (GameObject go in ObjectsThatNeedToShow)
        {
            go.SetActive(true);
        }
    }
}
