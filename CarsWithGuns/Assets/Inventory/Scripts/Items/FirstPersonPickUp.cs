using UnityEngine;
using System.Collections;

[System.Serializable]
//Assign this script to an Item if you want to pick it up in First Person. If this script is not attached the Item can only be picked up when clicking on it with the mouse.
 //The skin to use. Default one is 'OtherSkin' under the 'Resources' folder.
 //The button to press when picking up the item.
 //The distance from where the Item can be picked up. Remember that this is relative to the center of the Item and the center of the Player.
//These store information about the Item, if we can pick it up, the Player and the distance to the Player.
[UnityEngine.AddComponentMenu("Inventory/Items/First Person Pick Up")]
[UnityEngine.RequireComponent(typeof(Item))]
public partial class FirstPersonPickUp : MonoBehaviour
{
    public GUISkin InstructionBoxSkin;
    private KeyCode ButtonToPress;
    public float PickUpDistance;
    public bool DisplayPickupMessage;
    public string PickupMessage;
    private bool canPickUp;
    private Item theItem;
    private Transform thePlayer;
    private float dist;
    //This is where we find the usefull information which we can later access.
    public virtual void Awake()
    {
        this.theItem = (Item) this.GetComponent(typeof(Item));
        this.ButtonToPress = this.theItem.PickupButton;
        if (this.InstructionBoxSkin == null)
        {
            this.InstructionBoxSkin = (GUISkin) Resources.Load("OtherSkin", typeof(GUISkin));
        }
        if (this.DisplayPickupMessage && (this.PickupMessage.length == 0))
        {
            this.PickupMessage = ((("Press " + this.ButtonToPress.ToString()) + " to pick up ") + this.transform.name) + ".";
        }
    }

    public virtual void RetrievePlayer(Inventory theInv)
    {
        this.thePlayer = theInv.transform.parent;
    }

    public virtual void OnGUI()
    {
         //This is where we draw a box telling the Player how to pick up the item.
        GUI.skin = this.InstructionBoxSkin;
        GUI.color = new Color(1, 1, 1, 0.7f);
        if (this.DisplayPickupMessage && (this.canPickUp == true))
        {
            if (this.transform.name.Length <= 7)
            {
                GUI.Box(new Rect((Screen.width * 0.5f) - (165 * 0.5f), 200, 165, 22), this.PickupMessage);
            }
            else
            {
                GUI.Box(new Rect((Screen.width * 0.5f) - (185 * 0.5f), 200, 185, 22), this.PickupMessage);
            }
        }
    }

    public virtual void Update()
    {
        if (this.thePlayer != null)
        {
             //This is where we enable and disable the Players ability to pick up the item based on the distance to the player.
            this.dist = Vector3.Distance(this.thePlayer.position, this.transform.position);
            if (this.dist <= this.PickUpDistance)
            {
                this.canPickUp = true;
            }
            else
            {
                this.canPickUp = false;
            }
            //This is where we allow the player to press the ButtonToPress to pick up the item.
            if (Input.GetKeyDown(this.ButtonToPress) && (this.canPickUp == true))
            {
                this.theItem.PickUpItem();
            }
        }
    }

    //This is just for drawing the sphere in the scene view for easy testing.
    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, this.PickUpDistance);
    }

    public FirstPersonPickUp()
    {
        this.ButtonToPress = KeyCode.E;
        this.PickUpDistance = 1.7f;
        this.DisplayPickupMessage = true;
        this.PickupMessage = "";
        this.dist = 9999f;
    }

}