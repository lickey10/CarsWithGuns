using UnityEngine;
using System.Collections;

public class levelSelection : MonoBehaviour 
{
	
	Transform centerOfBox;
	float scale=0;
	float dist;
	/// <summary>
	/// How quickly the scaling should be done.
	/// </summary>
	float speed=3.5f;
	/// <summary>
	/// The maximum size an individual box could gain when comes to the center of the screen.
	/// </summary>
	public static float maxScaleSize=3.0f;
	/// <summary>
	/// The maximum size an individual box could gain when comes to the center of the screen.
	/// </summary>
	public static float minScaleSize=2.0f;
	public MonoBehaviour ScriptWithMethod;
	public string MethodToInvoke;
	
	// Use this for initialization
	void Start ()
	{
		//Taking reference of the center of the box gameObject
		centerOfBox= LevelBoxMove.centerOfBox;
	}
	RaycastHit hit=new RaycastHit();
	// Update is called once per frame
	void Update ()
	{
		if(centerOfBox==null)
			return;
		dist=centerOfBox.position.x-transform.position.x;
		scale=maxScaleSize/Mathf.Abs(dist);
		scale=Mathf.Clamp(scale,minScaleSize,maxScaleSize);//performing scaling on behalf of the box posiiton

		transform.localScale=Vector3.MoveTowards(transform.localScale,new Vector3(scale,scale,0.01f),Time.deltaTime*speed);
		
		if(Input.GetMouseButtonDown(0))
		{
			Ray ray=new Ray(centerOfBox.position,centerOfBox.forward);
			
			//Whether we hit this box or not
			if(Physics.Raycast(ray,out hit))
			{
				if(hit.transform.name.Equals(this.transform.name))
				{
					 ScriptWithMethod.SendMessage(MethodToInvoke);
				}
			}
		}
		
	}
	
}
