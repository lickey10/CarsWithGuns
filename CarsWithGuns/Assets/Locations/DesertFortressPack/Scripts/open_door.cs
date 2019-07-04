using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class open_door : MonoBehaviour
{
    //Instruction:
    //Make an empty game object and call it "Door"
    //Rename your 3D door model to "Body"
    //Parent a "Body" object to "Door"
    //Make sure thet a "Door" object is in left down corner of "Body" object. The place where a Door Hinge need be
    //Add a box collider to "Door" object and make it much bigger then the "Body" model, mark it trigger
    //Assign this script to a "Door" game object that have box collider with trigger enabled
    //Press "f" to open the door and "g" to close the door
    //Make sure the main character is tagged "player"
    // Smothly open a door
    public float smooth;
    public float DoorOpenAngle;
    public float DoorCloseAngle;
    public bool open;
    public bool enter;
    //Main function
    public virtual void Update()
    {
        if (this.open == true)
        {
            Quaternion target = Quaternion.Euler(this.transform.localRotation.x, this.DoorOpenAngle, this.transform.localRotation.z);
            // Dampen towards the target rotation
            this.transform.localRotation = Quaternion.Slerp(this.transform.localRotation, target, Time.deltaTime * this.smooth);
        }
        if (this.open == false)
        {
            Quaternion target1 = Quaternion.Euler(this.transform.localRotation.x, this.DoorCloseAngle, this.transform.localRotation.z);
            // Dampen towards the target rotation
            this.transform.localRotation = Quaternion.Slerp(this.transform.localRotation, target1, Time.deltaTime * this.smooth);
        }
        if (this.enter == true)
        {
            if (Input.GetKeyDown("f"))
            {
                this.open = !this.open;
            }
        }
    }

    //Activate the Main function when player is near the door
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Trigger Enter");
            this.enter = true;
        }
    }

    //Deactivate the Main function when player is go away from door
    public virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Trigger Exit");
            this.enter = false;
        }
    }

    public open_door()//@youtube.com/user/maksimum654321
    {
        this.smooth = 2f;
        this.DoorOpenAngle = 80f;
    }

}