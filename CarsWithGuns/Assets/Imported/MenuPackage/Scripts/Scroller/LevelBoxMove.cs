using UnityEngine;
using System.Collections;

public class LevelBoxMove : MonoBehaviour
{
	/// <summary>
	/// Reference of this gameObject.
	/// </summary>
    Transform box;
    Vector2 startPos;
    Vector2 endPos;
	
	Touch touchInput;
	
	public float speed=4;
	float distanceToMove=1;
	public float gap=3.0f;
	
	/// <summary>
	/// The minimum and maximum limits beyond which the box will restore to it's previous position.
	/// </summary>
	float minLimit=-3.7f;
	float maxLimit=2.5f;
	
	public float zValueOfBox=5;
	public float yValueOfBox=1;
	
	
	public Transform[] LevelBoxes;
	
	/// <summary>
	/// The center of box used to find out which level box is currently at the center of the screen.
	/// </summary>
	public static Transform centerOfBox;
	float nextDist;
	float prevPos;
	float firstBoxPos;
	float lastBoxPos;
	
	float startTime=0;
	float endTime=0;
	
	
	// Use this for initialization
	void Awake ()
    {
        box = this.transform;
		temp=box.position;
		temp.z=zValueOfBox;
		temp.y=yValueOfBox;
		box.position=temp;
		
		//We want the center of box to be static and not movable with other level boxes
		centerOfBox=this.transform.Find("center");
		
		if(centerOfBox)
			centerOfBox.parent=null;
		
		minLimit=box.position.x-(gap*(LevelBoxes.Length));
		maxLimit=box.position.x+(gap*(LevelBoxes.Length));
		prevPos=box.position.x;
	}
	
	void Start()
	{
		//Taking references of first and last level box
		firstBoxPos=LevelBoxes[0].position.x;
		lastBoxPos=LevelBoxes[(LevelBoxes.Length-1)].position.x;
	}
	
	Vector3 temp;
	float direction;
	bool isHit=false;
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.touchCount==1)
		{
			Touch touch=Input.GetTouch(0);
				
			if(touch.phase==TouchPhase.Began)
			{
				Ray ray= Camera.main.ScreenPointToRay(touch.position);
				RaycastHit hit=new RaycastHit();
				
				//Whether we hit this box or not
				if(Physics.Raycast(ray,out hit))
				{
					if(hit.transform.name.Equals(this.transform.name))
					{
						isHit=true;
						StartCoroutine("OnDown",new Vector3(touch.position.x,touch.position.y,6));
					}
				}
			}

			if(touch.phase==TouchPhase.Moved)
			{
				if(isHit)
					StartCoroutine("OnDrag",new Vector3(touch.position.x,touch.position.y,6));
			}

			if(touch.phase==TouchPhase.Ended)
			{
				if(isHit)
				{
					isHit=false;
					StartCoroutine("OnEnded",new Vector3(touch.position.x,touch.position.y,6));
				}
				
			}

		}//end of foreach
	}
	
	float curBoxPos;

    void OnMouseDown()
    {
		//If no touch means we are dealing with mouse. This was provided so you could test the code remotely on UnityRemote
		if(Input.touchCount==0)
			StartCoroutine("OnDown",Input.mousePosition);
    }

    void OnMouseDrag()
    {
		if(Input.touchCount==0)
			StartCoroutine("OnDrag",Input.mousePosition);
    }

    void OnMouseUp()
    {
		if(Input.touchCount==0)
			StartCoroutine("OnEnded",Input.mousePosition);
    }
	
	void OnDown(Vector3 thisPos)
	{
		startPos=thisPos;
		startTime=Time.time;
	}
	
	void OnDrag(Vector3 thisPos)
	{
		endPos=thisPos;
		
		//Direction finds out whether our box was moved towards left or right.
		//As we are dealing with x axis only so we can ignore directions in other axes.
		direction=Mathf.Sign(endPos.x-startPos.x);
		
		if(direction==-1 && Vector3.Distance(startPos,endPos)>1)
		{
			temp=box.position;
			distanceToMove=box.position.x-gap;
			temp.x=Mathf.MoveTowards(temp.x,distanceToMove,Time.deltaTime*speed);
			temp.x=Mathf.Clamp(temp.x,minLimit,maxLimit);
			box.position=temp;
		}
		
		else if(direction==1 && Vector3.Distance(startPos,endPos)>1)
		{
			temp=box.position;
			distanceToMove=box.position.x+gap;
			temp.x=Mathf.MoveTowards(temp.x,distanceToMove,Time.deltaTime*speed);
			temp.x=Mathf.Clamp(temp.x,minLimit,gap);
			box.position=temp;
		}
		startPos=endPos;
		
	}
	
	IEnumerator OnEnded(Vector3 thisPos)
	{
		endPos=thisPos;
		endTime=Time.time;
		curBoxPos= prevPos;
		
		direction=Mathf.Sign(endPos.x-startPos.x);
		
		//On Ended we check whether it was a swipe gesture or user has just ended the touch or mouse
		if(Vector3.Distance(endPos,startPos)>0.1f && (endTime-startTime)<1.0f)
		{
			if(direction==-1)
			{			
				if(rayForCenter.LevelBox.position.x==lastBoxPos)
				{
					nextDist=-lastBoxPos;
				}
				
				else
				{
					nextDist=curBoxPos-gap;
				}
				
				nextDist=Mathf.Clamp(nextDist,(minLimit+gap),lastBoxPos);
			}
			
			else if(direction==1 && Vector3.Distance(endPos,startPos)>0.5f)
			{
				if(rayForCenter.LevelBox.position.x==firstBoxPos)
				{
					nextDist=0;
				}
				
				else
				{
					nextDist=curBoxPos+gap;
				}
				
				nextDist=Mathf.Clamp(nextDist,minLimit,firstBoxPos);
			}
			
			
		}
		
		else
		{
			//if it wasn't a swipe gesture then put the theme box which is nearest to the center of the box.
			nextDist=-rayForCenter.LevelBox.localPosition.x;
		}
		
		
		temp=Vector3.zero;
		temp.x=nextDist;
		temp.y=yValueOfBox;
		temp.z=zValueOfBox;
		
		
		//To store the nearest level box to the center of this position
		while(true)
		{
			if(box.position.x==temp.x)
			{
				prevPos=-rayForCenter.LevelBox.localPosition.x;				
				break;
			}
			
			box.position=Vector3.MoveTowards(box.position,temp,Time.deltaTime*speed);
			
			yield return 0;
		}
	}//end of OnEnded
	
}
