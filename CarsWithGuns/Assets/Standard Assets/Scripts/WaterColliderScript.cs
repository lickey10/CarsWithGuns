using UnityEngine;
using System.Collections;

public class WaterColliderScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Collider waterCollider = this.GetComponentInParent<Collider>();
        waterCollider.isTrigger = false;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerExit(Collider other)
    {
        if(other.tag.Contains("Car GO") && other.transform.position.y > transform.position.y)
            StartCoroutine(pause());
    }

    IEnumerator pause()
    {
        yield return new WaitForSeconds(3);

        Collider waterCollider = this.GetComponentInParent<Collider>();
        waterCollider.isTrigger = false;
    }
}
