using UnityEngine;
using System.Collections;

public class rayForCenter : MonoBehaviour 
{
	Ray ray;
	RaycastHit hit=new RaycastHit();
	
	public static Transform LevelBox;
	
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		ray.origin=transform.position;
		ray.direction=Vector3.forward;

		if(Physics.Raycast(ray,out hit,10))
		{
			LevelBox=hit.transform;
			//print(hit.transform.name);
		}
	}
}
