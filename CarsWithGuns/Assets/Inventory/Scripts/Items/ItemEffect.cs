using UnityEngine;
using System.Collections;

[System.Serializable]
//This script allows you to insert code when the Item is used (clicked on in the inventory).
[UnityEngine.AddComponentMenu("Inventory/Items/Item Effect")]
[UnityEngine.RequireComponent(typeof(Item))]
public partial class ItemEffect : MonoBehaviour
{
    public bool deleteOnUse;
    private Inventory playersInv;
    private Item item;
    //This is where we find the components we need
    public virtual void Awake()
    {
        this.playersInv = (Inventory) UnityEngine.Object.FindObjectOfType(typeof(Inventory)); //finding the players inv.
        if (this.playersInv == null)
        {
            Debug.LogWarning(("No 'Inventory' found in game. The Item " + this.transform.name) + " has been disabled for pickup (canGet = false).");
        }
        this.item = (Item) this.GetComponent(typeof(Item));
    }

    //This is called when the object should be used.
    public virtual void UseEffect()
    {
        Debug.LogWarning("<INSERT CUSTOM ACTION HERE>"); //INSERT CUSTOM CODE HERE!
        //Play a sound
        this.playersInv.gameObject.SendMessage("PlayDropItemSound", SendMessageOptions.DontRequireReceiver);
        //This will delete the item on use or remove 1 from the stack (if stackable).
        if (this.deleteOnUse == true)
        {
            this.DeleteUsedItem();
        }
    }

    //This takes care of deletion
    public virtual void DeleteUsedItem()
    {
        if (this.item.stack == 1) //Remove item
        {
            this.playersInv.RemoveItem(this.gameObject.transform);
        }
        else
        {
             //Remove from stack
            this.item.stack = this.item.stack - 1;
        }
        Debug.Log(this.item.name + " has been deleted on use");
    }

    public ItemEffect()
    {
        this.deleteOnUse = true;
    }

}