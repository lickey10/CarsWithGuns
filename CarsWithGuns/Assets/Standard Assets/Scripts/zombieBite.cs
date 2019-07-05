using UnityEngine;
using System.Collections;

public class zombieBite : MonoBehaviour {
	public int DamageValue = 1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Friend")
			other.SendMessageUpwards("ApplyDamage",DamageValue,SendMessageOptions.DontRequireReceiver);
	}

	void OnTriggerStay(Collider other) {
		if(other.tag == "Friend")
			other.SendMessageUpwards("ApplyDamage",DamageValue,SendMessageOptions.DontRequireReceiver);
	}
}
