using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour {
    public Object SceneToLoad = null;
    public GameObject Fader = null;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeScene()
    {
        if (Fader != null)
            Fader.SetActive(true);

        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneToLoad.name);
        
    }
}
