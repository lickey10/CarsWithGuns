using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class TapToInteract : MonoBehaviour
{
    //This script handles touches. When the user touches the screen, the script determines what game object the player was trying to touch,
    //and passes it the information about that touch
    public LayerMask mask;
    public float interactRange;
    public virtual void Update()
    {
        this.CheckForTouch();
    }

    public virtual void CheckForTouch()
    {
        Ray ray = default(Ray);
         //Iterate through all touches
        int i = 0;
        while (i < Input.touches.Length)
        {
             //If a touch just began
            if (Input.touches[i].phase == TouchPhase.Began)
            {
                GameObject target = null;
                ray = Camera.main.ScreenPointToRay(Input.touches[i].position);
                target = this.ReturnRaycastHit(ray);
                if (target != null)
                {
                    target.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
                }
            }
            i++;
        }
    }

    public virtual GameObject ReturnRaycastHit(Ray ray)
    {
        RaycastHit hit = default(RaycastHit);
        //NOTE: May want to change to raycast all + use layer info
        if (Physics.Raycast(ray, out hit, this.interactRange, this.mask))
        {
            return hit.transform.gameObject;
        }
        else
        {
            return null;
        }
    }

    public TapToInteract()
    {
        this.interactRange = 9;
    }

}