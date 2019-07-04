using UnityEngine;
using System.Collections;

[System.Serializable]
//Displaying the Inventory.
//Variables for dragging:
 //This refers to the 'Item' script when dragging.
 //Where on the screen we are dragging our Item.
//The size of the item icon we are dragging.
//Variables for the window:
 //The size of the Inventory window.
 //Do we want to use the customPosition variable to define where on the screen the Inventory window will appear?
 // The custom position of the Inventory window.
 //The size of the item icons.
//Variables for updating the inventory
//This can be used to update the Inventory with a certain delay rather than updating it every time the OnGUI is called.
//This is only useful if you are expanding on the Inventory System cause by default Inventory has a system for only updating when needed (when an item is added or removed).
 //Last time we updated the display.
 //The updated inventory array.
//More variables for the window:
 //If inv is opened.
 //Keeping track of the Inventory window.
 //This is where you can add a custom GUI skin or use the one included (InventorySkin) under the Resources folder.
 //This will leave so many pixels between the edge of the window (x = horizontal and y = vertical).
 //Can the Inventory window be dragged?
 //The button that turns the Inventory window on and off.
//Keeping track of components.
[UnityEngine.AddComponentMenu("Inventory/Inventory Display")]
[UnityEngine.RequireComponent(typeof(Inventory))]
public partial class InventoryDisplay : MonoBehaviour
{
    public static Item itemBeingDragged;
    private Vector2 draggedItemPosition;
    private Vector2 draggedItemSize;
    public Vector2 windowSize;
    public bool useCustomPosition;
    public Vector2 customPosition;
    public Vector2 itemIconSize;
    public int updateListDelay;
    private float lastUpdate;
    private Transform[] UpdatedList;
    public static bool displayInventory;
    private Rect windowRect;
    public GUISkin invSkin;
    public Vector2 Offset;
    public bool canBeDragged;
    public KeyCode onOffButton;
    private Inventory associatedInventory;
    private bool cSheetFound;
    private Character cSheet;
    //Store components and adjust the window position.
    public virtual void Awake()
    {
        if (this.useCustomPosition == false)
        {
             //windowRect=Rect(Screen.width-windowSize.x-70,Screen.height-windowSize.y-70,windowSize.x,windowSize.y);
             //center window
            this.windowRect = new Rect((Screen.width - this.windowSize.x) / 2, (Screen.height - this.windowSize.y) - 70, this.windowSize.x, this.windowSize.y);
        }
        else
        {
            this.windowRect = new Rect(this.customPosition.x, this.customPosition.y, this.windowSize.x, this.windowSize.y);
        }
        this.associatedInventory = (Inventory) this.GetComponent(typeof(Inventory));//keepin track of the inventory script
        if (((Character) this.GetComponent(typeof(Character))) != null)
        {
            this.cSheetFound = true;
            this.cSheet = (Character) this.GetComponent(typeof(Character));
        }
        else
        {
            Debug.LogError("No Character script was found on this object. Attaching one allows for functionality such as equipping items.");
            this.cSheetFound = false;
        }
    }

    //Update the inv list
    public virtual void UpdateInventoryList()//Debug.Log("Inventory Updated");
    {
        this.UpdatedList = this.associatedInventory.Contents;
    }

    public virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //Pressed escape
        {
            this.ClearDraggedItem(); //Get rid of the dragged item.
        }
        if (Input.GetMouseButtonDown(1)) //Pressed right mouse
        {
            this.ClearDraggedItem(); //Get rid of the dragged item.
        }
        //Turn the Inventory on and off and handle audio + pausing the game.
        if (Input.GetKeyDown(this.onOffButton))
        {
            if (InventoryDisplay.displayInventory)
            {
                InventoryDisplay.displayInventory = false;
                this.gameObject.SendMessage("ChangedState", false, SendMessageOptions.DontRequireReceiver);
                this.gameObject.SendMessage("PauseGame", false, SendMessageOptions.DontRequireReceiver); //StopPauseGame/EnableMouse/ShowMouse
            }
            else
            {
                InventoryDisplay.displayInventory = true;
                this.gameObject.SendMessage("ChangedState", true, SendMessageOptions.DontRequireReceiver);
                this.gameObject.SendMessage("PauseGame", true, SendMessageOptions.DontRequireReceiver); //PauseGame/DisableMouse/HideMouse
            }
        }
        //Making the dragged icon update its position
        if (InventoryDisplay.itemBeingDragged != null)
        {
             //Give it a 15 pixel space from the mouse pointer to allow the Player to click stuff and not hit the button we are dragging.
            this.draggedItemPosition.y = (Screen.height - Input.mousePosition.y) + 15;
            this.draggedItemPosition.x = Input.mousePosition.x + 15;
        }
        //Updating the list by delay
        if (Time.time > this.lastUpdate)
        {
            this.lastUpdate = Time.time + this.updateListDelay;
            this.UpdateInventoryList();
        }
    }

    //Drawing the Inventory window
    public virtual void OnGUI()
    {
        GUI.skin = this.invSkin; //Use the invSkin
        if (InventoryDisplay.itemBeingDragged != null) //If we are dragging an Item, draw the button on top:
        {
            GUI.depth = 3;
            GUI.Button(new Rect(this.draggedItemPosition.x, this.draggedItemPosition.y, this.draggedItemSize.x, this.draggedItemSize.y), InventoryDisplay.itemBeingDragged.itemIcon);
            GUI.depth = 0;
        }
        //If the inventory is opened up we create the Inventory window:
        if (InventoryDisplay.displayInventory)
        {
            this.windowRect = GUI.Window(0, this.windowRect, this.DisplayInventoryWindow, "Inventory");
        }
    }

    //Setting up the Inventory window
    public virtual void DisplayInventoryWindow(int windowID)
    {
        if (this.canBeDragged == true)
        {
            GUI.DragWindow(new Rect(0, 0, 10000, 30)); //the window to be able to be dragged
        }
        float currentX = 0 + this.Offset.x; //Where to put the first items.
        float currentY = 18 + this.Offset.y; //Im setting the start y position to 18 to give room for the title bar on the window.
        foreach (Transform i in this.UpdatedList)
        {
            Item item = (Item) i.GetComponent(typeof(Item));
            if (this.cSheetFound) //CSheet was found (recommended)
            {
                if (GUI.Button(new Rect(currentX, currentY, this.itemIconSize.x, this.itemIconSize.y), item.itemIcon))
                {
                    bool dragitem = true; //Incase we stop dragging an item we dont want to redrag a new one.
                    if (InventoryDisplay.itemBeingDragged == item) //We clicked the item, then clicked it again
                    {
                        if (this.cSheetFound)
                        {
                            ((Character) this.GetComponent(typeof(Character))).UseItem(item, 0, true); //We use the item.
                        }
                        this.ClearDraggedItem(); //Stop dragging
                        dragitem = false; //Dont redrag
                    }
                    if (Event.current.button == 0) //Check to see if it was a left click
                    {
                        if (dragitem)
                        {
                            if (item.isEquipment == true) //If it's equipment
                            {
                                InventoryDisplay.itemBeingDragged = item; //Set the item being dragged.
                                this.draggedItemSize = this.itemIconSize; //We set the dragged icon size to our item button size.
                                //We set the position:
                                this.draggedItemPosition.y = (Screen.height - Input.mousePosition.y) - 15;
                                this.draggedItemPosition.x = Input.mousePosition.x + 15;
                            }
                            else
                            {
                                ((ItemEffect) i.GetComponent(typeof(ItemEffect))).UseEffect(); //It's not equipment so we just use the effect.
                            }
                        }
                    }
                    else
                    {
                        if (Event.current.button == 1) //If it was a right click we want to drop the item.
                        {
                            this.associatedInventory.DropItem(item);
                        }
                    }
                }
            }
            else
            {
                 //No CSheet was found (not recommended)
                if (GUI.Button(new Rect(currentX, currentY, this.itemIconSize.x, this.itemIconSize.y), item.itemIcon))
                {
                    if ((Event.current.button == 0) && (item.isEquipment != true)) //Check to see if it was a left click.
                    {
                        ((ItemEffect) i.GetComponent(typeof(ItemEffect))).UseEffect(); //Use the effect of the item.
                    }
                    else
                    {
                        if (Event.current.button == 1) //If it was a right click we want to drop the item.
                        {
                            this.associatedInventory.DropItem(item);
                        }
                    }
                }
            }
            if (item.stackable) //If the item can be stacked:
            {
                GUI.Label(new Rect(currentX, currentY, this.itemIconSize.x, this.itemIconSize.y), "" + item.stack, "Stacks"); //Showing the number (if stacked).
            }
            currentX = currentX + this.itemIconSize.x;
            if (((currentX + this.itemIconSize.x) + this.Offset.x) > this.windowSize.x) //Make new row
            {
                currentX = this.Offset.x; //Move it back to its startpoint wich is 0 + offsetX.
                currentY = currentY + this.itemIconSize.y; //Move it down a row.
                if (((currentY + this.itemIconSize.y) + this.Offset.y) > this.windowSize.y) //If there are no more room for rows we exit the loop.
                {
                    return;
                }
            }
        }
    }

    //If we are dragging an item, we will clear it.
    public virtual void ClearDraggedItem()
    {
        InventoryDisplay.itemBeingDragged = null;
    }

    public InventoryDisplay()
    {
        this.windowSize = new Vector2(375, 162.5f);
        this.customPosition = new Vector2(70, 400);
        this.itemIconSize = new Vector2(60f, 60f);
        this.updateListDelay = 9999;
        this.windowRect = new Rect(200, 200, 108, 130);
        this.Offset = new Vector2(7, 12);
        this.canBeDragged = true;
        this.onOffButton = KeyCode.I;
    }

}