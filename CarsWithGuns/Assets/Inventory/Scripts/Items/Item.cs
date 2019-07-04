using UnityEngine;
using System.Collections;

[System.Serializable]
 //The Icon.
 //If we can pick up the Item.
 //This will let us equip the item to specific slots. Ex: Head, Shoulder, or whatever we set up. If the item is equipment (or weapon) this needs to match a slot to work properly.
 //Is it stackable? If yes then items with the same itemType will be stacked.
 //How many Items each stack can have before creating a new one. Remember that the Items that should be stacked should have the same itemType.
 //This is how many stack counts this Item will take up.
 //Can the Item be equipped? This includes weapons.
 //Is the Item also a Weapon? This only works with isEquipment set to true.
//This is the object we will instantiate in the Players hand.
//We use this so we can have two versions of the weapon. One for picking up and one for using.
 //The button that picks up the item
//These will store information about usefull components.
[UnityEngine.AddComponentMenu("Inventory/Items/Item")]
public partial class Item : MonoBehaviour
{
    public Texture2D itemIcon;
    public bool canGet;
    public string itemType;
    public bool stackable;
    public int maxStack;
    public int stack;
    public bool isEquipment;
    public bool isAlsoWeapon;
    public Transform equippedWeaponVersion;
    public object equipmentEffect;
    public KeyCode PickupButton;
    public static Inventory playersinv;
    private bool FPPickUpFound;
    private GameObject clone;
    //Here we find the components we need.
    public virtual void Awake()
    {
        Item.playersinv = (Inventory) UnityEngine.Object.FindObjectOfType(typeof(Inventory)); //finding the players inv.
        if (Item.playersinv == null)
        {
            this.canGet = false;
            Debug.LogWarning(("No 'Inventory' found in game. The Item " + this.transform.name) + " has been disabled for pickup (canGet = false).");
        }
        else
        {
            this.gameObject.SendMessage("RetrievePlayer", Item.playersinv, SendMessageOptions.DontRequireReceiver);
        }
        if ((this.isEquipment == false) && (((ItemEffect) this.GetComponent(typeof(ItemEffect))) == null))
        {
            Debug.LogError(this.gameObject.name + " is not equipment so please assign an ItemEffect script to it");
        }
        if (((FirstPersonPickUp) this.GetComponent(typeof(FirstPersonPickUp))) != null)
        {
            this.FPPickUpFound = true;
        }
        else
        {
            if (((FirstPersonPickUp) this.transform.GetComponentInChildren(typeof(FirstPersonPickUp))) != null)
            {
                this.FPPickUpFound = true;
            }
        }
    }

    public virtual void Update()
    {
         //check to see if we pick up the item
        if (Input.GetKeyDown(this.PickupButton))
        {
             //If the 'FirstPersonPickUp' script is not attached we want to pick up the item.
            if (this.FPPickUpFound == false)
            {
                this.PickUpItem();
            }
        }
    }

    //When you click an item
    //function OnMouseDown()
    //{
    //	//If the 'FirstPersonPickUp' script is not attached we want to pick up the item.
    //	if (FPPickUpFound == false)
    //	{
    //		PickUpItem();
    //	}
    //}
    //Picking up the Item.
    public virtual void PickUpItem()
    {
        bool getit = true;
        if (this.canGet)//if its getable or hasnt been gotten.
        {
            Item.playersinv.gameObject.SendMessage("PlayPickUpSound", SendMessageOptions.DontRequireReceiver); //Play sound
            if (this.stackable)
            {
                Item locatedit = null;
                foreach (Transform t in Item.playersinv.Contents)
                {
                    if (t.name == this.transform.name)//if the item we wanna stack this on has the same name
                    {
                        Item i = (Item) t.GetComponent(typeof(Item));
                        if (i.stack < i.maxStack)
                        {
                            locatedit = i;
                        }
                    }
                }
                if (locatedit != null)//if we have a stack to stack it to!
                {
                    getit = false;
                    locatedit.stack = locatedit.stack + 1;
                    UnityEngine.Object.Destroy(this.gameObject);
                }
                else
                {
                    getit = true;
                }
            }
            //If we can get it and the inventory isn't full.
            if (getit && (Item.playersinv.Contents.Length < Item.playersinv.MaxContent))
            {
                Item.playersinv.AddItem(this.transform);
                this.MoveMeToThePlayer(Inventory.itemHolderObject);//moves the object, to the player
            }
            else
            {
                if (Item.playersinv.Contents.Length >= Item.playersinv.MaxContent)
                {
                    Debug.Log("Inventory is full");
                }
            }
        }
    }

    //Moves the item to the Players 'itemHolderObject' and disables it. In most cases this will just be the Inventory object.
    public virtual void MoveMeToThePlayer(Transform itemHolderObject)
    {
        this.canGet = false;
        //gameObject.SetActive(false);	It's normally best to disable the individual components so we can keep item effects and update functions alive.
        if (((MeshRenderer) this.GetComponent(typeof(MeshRenderer))) != null)
        {
            ((MeshRenderer) this.GetComponent(typeof(MeshRenderer))).enabled = false;
        }
        if (((Collider) this.GetComponent(typeof(Collider))) != null)
        {
            ((Collider) this.GetComponent(typeof(Collider))).enabled = false;
        }
        //GetComponent("Item").enabled = false;
        (this.GetComponent("Item") as MonoBehaviour).enabled = true;
        this.transform.parent = itemHolderObject;
        this.transform.localPosition = Vector3.zero;
    }

    //Drops the Item from the Inventory.
    public virtual void DropMeFromThePlayer(bool makeDuplicate)
    {
        if (makeDuplicate == false) //We use this if the object is not stacked and so we can just drop it.
        {
            this.canGet = true;
            this.gameObject.SetActive(true);
            if (((MeshRenderer) this.GetComponent(typeof(MeshRenderer))) != null)
            {
                ((MeshRenderer) this.GetComponent(typeof(MeshRenderer))).enabled = true;
            }
            if (((Collider) this.GetComponent(typeof(Collider))) != null)
            {
                ((Collider) this.GetComponent(typeof(Collider))).enabled = true;
            }
            //GetComponent("Item").enabled = true;
            (this.GetComponent("Item") as MonoBehaviour).enabled = true;
            this.transform.parent = null;
            this.StartCoroutine(this.DelayPhysics());
        }
        else
        {
             //If the object is stacked we need to make a clone of it and drop the clone instead.
            this.canGet = true;
            this.clone = UnityEngine.Object.Instantiate(this.gameObject, this.transform.position, this.transform.rotation);
            this.canGet = false;
            this.clone.SetActive(true);
            if (((MeshRenderer) this.clone.GetComponent(typeof(MeshRenderer))) != null)
            {
                ((MeshRenderer) this.clone.GetComponent(typeof(MeshRenderer))).enabled = true;
            }
            if (((Collider) this.clone.GetComponent(typeof(Collider))) != null)
            {
                ((Collider) this.clone.GetComponent(typeof(Collider))).enabled = true;
            }
            //clone.GetComponent("Item").enabled = true;
            (this.GetComponent("Item") as MonoBehaviour).enabled = true;
            this.clone.transform.parent = null;
            this.clone.name = this.gameObject.name;
        }
    }

    public virtual IEnumerator DelayPhysics()
    {
        if ((Item.playersinv.transform.parent.GetComponent<Collider>() != null) && (this.GetComponent<Collider>() != null))
        {
            Physics.IgnoreCollision(Item.playersinv.transform.parent.GetComponent<Collider>(), this.GetComponent<Collider>(), true);
            yield return new WaitForSeconds(1);
            Physics.IgnoreCollision(Item.playersinv.transform.parent.GetComponent<Collider>(), this.GetComponent<Collider>(), false);
        }
    }

    //Drawing an 'I' icon on top of the Item in the scene to keep organised.
    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawIcon(new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z), "ItemGizmo.png", true);
    }

    public Item()
    {
        this.canGet = true;
        this.maxStack = 20;
        this.stack = 1;
        this.isEquipment = true;
        this.PickupButton = KeyCode.Tab;
    }

}