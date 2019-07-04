using UnityEngine;
using System.Collections;

public class DrawGizmo : MonoBehaviour 
{
	public float cubeSize=0.1f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnDrawGizmos()
	{
		Gizmos.DrawCube(this.transform.localPosition,new Vector3(cubeSize,cubeSize,cubeSize));
	}
}
