using UnityEngine;
using System.Collections;

public class Break_Object_CS : MonoBehaviour {

	[ Header ( "Broken object settings" ) ]
	[ Tooltip ( "Prefab of the broken object." ) ] public GameObject brokenObject ;
	[ Tooltip ( "lag time for spawning the broken object. (Sec)" ) ] public float lag = 1.0f ;
    public int HitPoints = 10;
    public int ObjectScore = 10;

	Transform thisTransform ;

	void Start () {
		thisTransform = transform ;
	}

	void OnJointBreak () {
		StartCoroutine ( "Broken" ) ;
	}

	void OnTriggerEnter (Collider other) {
        if (!other.isTrigger)
        {
            HitPoints -= 10;

            if (HitPoints <= 0)
                StartCoroutine("Broken");
        }
	}

	IEnumerator Broken () {
         
        yield return new WaitForSeconds ( lag ) ;
		if ( brokenObject ) {
            //Point PointScript = gameObject.AddComponent<Point>();
            //PointScript.Points = ObjectScore;
            Instantiate ( brokenObject , thisTransform.position , thisTransform.rotation ) ;
            SendMessageUpwards("ApplyDamage", 50.0F,SendMessageOptions.DontRequireReceiver);
        }
		Destroy ( gameObject ) ;
	}
}
