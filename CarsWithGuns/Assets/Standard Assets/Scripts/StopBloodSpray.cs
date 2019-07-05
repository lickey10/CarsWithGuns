using UnityEngine;
using System.Collections;

public class StopBloodSpray : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("stopEmitter", 1); //In 3 seconds, runs StopEmitter function
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void stopEmitter()
    {
        gameObject.GetComponent<ParticleSystem>().Play();// .emit = false; //tell the emitter to stop.

    }
}
