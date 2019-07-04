using UnityEngine;
using System.Collections;

[System.Serializable]
//This is the central piece of the Inventory System.
 //The content of the Inventory
 //The maximum number of items the Player can carry.
 //If this is turned on the Inventory script will output the base of what it's doing to the Console window.
 //Keep track of the InventoryDisplay script.
 //The object the unactive items are going to be parented to. In most cases this is going to be the Inventory object itself.
[UnityEngine.AddComponentMenu("Inventory/Inventory")]
public partial class Inventory : MonoBehaviour
{
    public Transform[] Contents;
    public int MaxContent;
    public bool DebugMode;
    private InventoryDisplay playersInvDisplay;
    public static Transform itemHolderObject;
    private int items;
    //Handle components and assign the itemHolderObject.
    public virtual void Awake()
    {
        Inventory.itemHolderObject = this.gameObject.transform;
        this.playersInvDisplay = (InventoryDisplay) this.GetComponent(typeof(InventoryDisplay));
        if (this.playersInvDisplay == null)
        {
            Debug.LogError(("No Inventory Display script was found on " + this.transform.name) + " but an Inventory script was.");
            Debug.LogError("Unless a Inventory Display script is added the Inventory won't show. Add it to the same gameobject as the Inventory for maximum performance");
        }
    }

    //Add an item to the inventory.
    public virtual void AddItem(Transform Item)
    {
        object[] newContents = this.Contents;
        newContents.Add(Item);
        this.Contents = newContents.ToBuiltin(typeof(Transform)); //Array to unity builtin array
        if (this.DebugMode)
        {
            Debug.Log(Item.name + " has been added to inventroy");
        }
        //Tell the InventoryDisplay to update the list.
        if (this.playersInvDisplay != null)
        {
            this.playersInvDisplay.UpdateInventoryList();
        }
    }

    //Removed an item from the inventory (IT DOESN'T DROP IT).
    public virtual void RemoveItem(Transform Item)
    {
        object[] newContents = this.Contents;
        int index = 0;
        bool shouldend = false;
        foreach (Transform i in newContents) //Loop through the Items in the Inventory:
        {
            if (i == Item) //When a match is found, remove the Item.
            {
                newContents.RemoveAt(index);
                shouldend = true;
            }
            //No need to continue running through the loop since we found our item.
            index++;
            if (shouldend) //Exit the loop
            {
                this.Contents = newContents.ToBuiltin(typeof(Transform));
                if (this.DebugMode)
                {
                    Debug.Log(Item.name + " has been removed from inventroy");
                }
                if (this.playersInvDisplay != null)
                {
                    this.playersInvDisplay.UpdateInventoryList();
                }
                return;
            }
        }
    }

    //Dropping an Item from the Inventory
    public virtual void DropItem(Item item)
    {
        this.gameObject.SendMessage("PlayDropItemSound", SendMessageOptions.DontRequireReceiver); //Play sound
        bool makeDuplicate = false;
        if (item.stack == 1) //Drop item
        {
            this.RemoveItem(item.transform);
        }
        else
        {
             //Drop from stack
            item.stack = item.stack - 1;
            makeDuplicate = true;
        }
        item.DropMeFromThePlayer(makeDuplicate); //Calling the drop function + telling it if the object is stacked or not.
        if (this.DebugMode)
        {
            Debug.Log(item.name + " has been dropped");
        }
    }

    //This will tell you everything that is in the inventory.
    public virtual void DebugInfo()
    {
        Debug.Log("Inventory Debug - Contents");
        this.items = 0;
        foreach (Transform i in this.Contents)
        {
            this.items++;
            Debug.Log(i.name);
        }
        Debug.Log(("Inventory contains " + this.items) + " Item(s)");
    }

    //Drawing an 'S' in the scene view on top of the object the Inventory is attached to stay organized.
    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawIcon(new Vector3(this.transform.position.x, this.transform.position.y + 2.3f, this.transform.position.z), "InventoryGizmo.png", true);
    }

    public Inventory()
    {
        this.MaxContent = 12;
    }

}