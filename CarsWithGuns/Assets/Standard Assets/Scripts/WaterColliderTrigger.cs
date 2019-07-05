using UnityEngine;
using System.Collections;

public class WaterColliderTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerExit(Collider other)
    {
        if (other.tag.Contains("Player"))
        { 
            Collider waterCollider = this.transform.parent.GetComponentInParent<Collider>();
            waterCollider.isTrigger = true;
        }
    }
}
