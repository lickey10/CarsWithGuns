using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
		if (other.gameObject.GetComponent<Collider>() != null)
			GetComponent<Health>().ApplyDamage(100);
           //SendMessageUpwards("ApplyDamage", 50, SendMessageOptions.DontRequireReceiver);
    }
}
