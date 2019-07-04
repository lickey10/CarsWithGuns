using UnityEngine;
using System.Collections;

[System.Serializable]
//The Character window (CSheet).
 //This is where the Weapons are going to go (be parented too). In my case it's the "Melee" gameobject.
 //This is the built in Array that stores the Items equipped. You can change this to static if you want to access it from another script.
 //This determines how many slots the character has (Head, Legs, Weapon and so on) and the text on each slot.
 //This list will contain where all buttons, equipped or not will be and SHOULD HAVE THE SAME NUMBER OF cells as the ArmorSlot array.
 //The size of the character window.
 //Do we want to use the customPosition variable to define where on the screen the Character window will appear.
 //The custom position of the Character window.
 //This is where you can add a custom GUI skin or use the one included (CSheetSkin) under the Resources folder.
 //Can the Character window be dragged?
 //The key to toggle the Character window on and of.
 //If this is enabled, debug.logs will print out information when something happens (equipping items etc.).
 //Helps with turning the CharacterSheet on and off.
 //Keeping track of our character window.
//These are keeping track of components such as equipmentEffects and Audio.
 //Refers to the Inventory script.
[UnityEngine.AddComponentMenu("Inventory/Character Sheet")]
[UnityEngine.RequireComponent(typeof(Inventory))]
public partial class Character : MonoBehaviour
{
    public Transform WeaponSlot;
    private Item[] ArmorSlot;
    public string[] ArmorSlotName;
    public Rect[] buttonPositions;
    public Vector2 windowSize;
    public bool useCustomPosition;
    public Vector2 customPosition;
    public GUISkin cSheetSkin;
    public bool canBeDragged;
    public KeyCode onOffButton;
    public bool DebugMode;
    public static bool csheet;
    private Rect windowRect;
    private Inventory playersinv;
    private bool equipmentEffectIs;
    private InvAudio invAudio;
    private bool invDispKeyIsSame;
    //Assign the differnet components to variables and other "behind the scenes" stuff.
    public virtual void Awake()
    {
        this.playersinv = (Inventory) this.GetComponent(typeof(Inventory));
        if (this.useCustomPosition == false)
        {
             //windowRect = Rect(Screen.width-windowSize.x-70,Screen.height-windowSize.y-(162.5+70*2),windowSize.x,windowSize.y);
             //center window
            this.windowRect = new Rect((Screen.width - this.windowSize.x) / 2, (Screen.height - this.windowSize.y) - (162.5f + (70 * 2)), this.windowSize.x, this.windowSize.y);
        }
        else
        {
            this.windowRect = new Rect(this.customPosition.x, this.customPosition.y, this.windowSize.x, this.windowSize.y);
        }
        this.invAudio = (InvAudio) this.GetComponent(typeof(InvAudio));
        if (((InventoryDisplay) this.GetComponent(typeof(InventoryDisplay))).onOffButton == this.onOffButton)
        {
            this.invDispKeyIsSame = true;
        }
    }

    //Take care of the array lengths.
    public virtual void Start()
    {
        this.ArmorSlot = new Item[this.ArmorSlotName.Length];
        if (this.buttonPositions.Length != this.ArmorSlotName.Length)
        {
            Debug.LogError(("The variables on the Character script attached to " + this.transform.name) + " are not set up correctly. There needs to be an equal amount of slots on 'ArmorSlotName' and 'buttonPositions'.");
        }
    }

    //Checking if we already have somthing equipped
    public virtual bool CheckSlot(int tocheck)
    {
        bool toreturn = false;
        if (this.ArmorSlot[tocheck] != null)
        {
            toreturn = true;
        }
        return toreturn;
    }

    //Using the item. If we assign a slot, we already know where to equip it.
    public virtual void UseItem(Item i, int slot, bool autoequip)
    {
        if (i.isEquipment)
        {
            //This is in case we dbl click the item, it will auto equip it. REMEMBER TO MAKE THE ITEM TYPE AND THE SLOT YOU WANT IT TO BE EQUIPPED TO HAVE THE SAME NAME.
            if (autoequip)
            {
                int index = 0; //Keeping track of where we are in the list.
                int equipto = 0; //Keeping track of where we want to be.
                foreach (string a in this.ArmorSlotName)
                {
                    if (a == i.itemType) //if the name is the same as the armor type.
                    {
                        equipto = index; //We aim for that slot.
                    }
                    index++; //We move on to the next slot.
                }
                this.EquipItem(i, equipto);
            }
            else
            {
                 //If we dont auto equip it then it means we must of tried to equip it to a slot so we make sure the item can be equipped to that slot.
                if (i.itemType == this.ArmorSlotName[slot]) //If types match.
                {
                    this.EquipItem(i, slot); //Equip the item to the slot.
                }
            }
        }
        if (this.DebugMode)
        {
            Debug.Log(i.name + " has been used");
        }
    }

    //Equip an item to a slot.
    public virtual void EquipItem(Item i, int slot)
    {
        if (i.itemType == this.ArmorSlotName[slot]) //If the item can be equipped there:
        {
            if (this.CheckSlot(slot)) //If theres an item equipped to that slot we unequip it first:
            {
                this.UnequipItem(this.ArmorSlot[slot]);
                this.ArmorSlot[slot] = null;
            }
            this.ArmorSlot[slot] = i; //When we find the slot we set it to the item.
            this.gameObject.SendMessage("PlayEquipSound", SendMessageOptions.DontRequireReceiver); //Play sound
            //We tell the Item to handle EquipmentEffects (if any).
            if (((EquipmentEffect) i.GetComponent(typeof(EquipmentEffect))) != null)
            {
                this.equipmentEffectIs = true;
                ((EquipmentEffect) i.GetComponent(typeof(EquipmentEffect))).EquipmentEffectToggle(this.equipmentEffectIs);
            }
            //If the item is also a weapon we call the PlaceWeapon function.
            if (i.isAlsoWeapon == true)
            {
                if (i.equippedWeaponVersion != null)
                {
                    this.PlaceWeapon(i);
                }
                else
                {
                    Debug.LogError("Remember to assign the equip weapon variable!");
                }
            }
            if (this.DebugMode)
            {
                Debug.Log(i.name + " has been equipped");
            }
            this.playersinv.RemoveItem(i.transform); //We remove the item from the inventory
        }
    }

    //Unequip an item.
    public virtual void UnequipItem(Item i)
    {
        this.gameObject.SendMessage("PlayPickUpSound", SendMessageOptions.DontRequireReceiver); //Play sound
        //We tell the Item to disable EquipmentEffects (if any).
        if (!(i.equipmentEffect == null))
        {
            this.equipmentEffectIs = false;
            ((EquipmentEffect) i.GetComponent(typeof(EquipmentEffect))).EquipmentEffectToggle(this.equipmentEffectIs);
        }
        //If it's a weapon we call the RemoveWeapon function.
        if (i.itemType == "Weapon")
        {
            this.RemoveWeapon(i);
        }
        if (this.DebugMode)
        {
            Debug.Log(i.name + " has been unequipped");
        }
        this.playersinv.AddItem(i.transform);
    }

    //Places the weapon in the hand of the Player.
    public virtual void PlaceWeapon(Item item)
    {
        Transform Clone = UnityEngine.Object.Instantiate(item.equippedWeaponVersion, this.WeaponSlot.position, this.WeaponSlot.rotation);
        Clone.name = item.equippedWeaponVersion.name;
        Clone.transform.parent = this.WeaponSlot;
        if (this.DebugMode)
        {
            Debug.Log(item.name + " has been placed as weapon");
        }
    }

    //Removes the weapon from the hand of the Player.
    public virtual void RemoveWeapon(Item item)
    {
        if (item.equippedWeaponVersion != null)
        {
            UnityEngine.Object.Destroy(this.WeaponSlot.Find("" + item.equippedWeaponVersion.name).gameObject);
            if (this.DebugMode)
            {
                Debug.Log(item.name + " has been removed as weapon");
            }
        }
    }

    public virtual void Update()
    {
         //This will turn the character sheet on and off.
        if (Input.GetKeyDown(this.onOffButton))
        {
            if (Character.csheet)
            {
                Character.csheet = false;
                if (this.invDispKeyIsSame != true)
                {
                    this.gameObject.SendMessage("ChangedState", false, SendMessageOptions.DontRequireReceiver); //Play sound
                    this.gameObject.SendMessage("PauseGame", false, SendMessageOptions.DontRequireReceiver); //StopPauseGame/EnableMouse/ShowMouse
                }
            }
            else
            {
                Character.csheet = true;
                if (this.invDispKeyIsSame != true)
                {
                    this.gameObject.SendMessage("ChangedState", true, SendMessageOptions.DontRequireReceiver); //Play sound
                    this.gameObject.SendMessage("PauseGame", true, SendMessageOptions.DontRequireReceiver); //PauseGame/DisableMouse/HideMouse
                }
            }
        }
    }

    //Draw the Character Window
    public virtual void OnGUI()
    {
        GUI.skin = this.cSheetSkin; //Use the cSheetSkin variable.
        if (Character.csheet) //If the csheet is opened up.
        {
             //Make a window that shows what's in the csheet called "Character" and update the position and size variables from the window variables.
            this.windowRect = GUI.Window(1, this.windowRect, this.DisplayCSheetWindow, "Character");
        }
    }

    //This will display the character sheet and handle the buttons.
    public virtual void DisplayCSheetWindow(int windowID)
    {
        if (this.canBeDragged == true)
        {
            GUI.DragWindow(new Rect(0, 0, 10000, 30)); //The window is dragable.
        }
        int index = 0;
        foreach (Item a in this.ArmorSlot) //Loop through the ArmorSlot array.
        {
            if (a == null)
            {
                if (GUI.Button(this.buttonPositions[index], this.ArmorSlotName[index])) //If we click this button (that has no item equipped):
                {
                    InventoryDisplay id = (InventoryDisplay) this.GetComponent(typeof(InventoryDisplay));
                    if (InventoryDisplay.itemBeingDragged != null) //If we are dragging an item:
                    {
                        this.EquipItem(InventoryDisplay.itemBeingDragged, index); //Equip the Item.
                        id.ClearDraggedItem();//Stop dragging the item.
                    }
                }
            }
            else
            {
                if (GUI.Button(this.buttonPositions[index], this.ArmorSlot[index].itemIcon)) //If we click this button (that has an item equipped):
                {
                    InventoryDisplay id2 = (InventoryDisplay) this.GetComponent(typeof(InventoryDisplay));
                    if (InventoryDisplay.itemBeingDragged != null) //If we are dragging an item:
                    {
                        this.EquipItem(InventoryDisplay.itemBeingDragged, index); //Equip the Item.
                        id2.ClearDraggedItem(); //Stop dragging the item.
                    }
                    else
                    {
                        if (this.playersinv.Contents.Length < this.playersinv.MaxContent) //If there is room in the inventory:
                        {
                            this.UnequipItem(this.ArmorSlot[index]); //Unequip the Item.
                            this.ArmorSlot[index] = null; //Clear the slot.
                            id2.ClearDraggedItem(); //Stop dragging the Item.
                        }
                        else
                        {
                            if (this.DebugMode)
                            {
                                Debug.Log(("Could not unequip " + this.ArmorSlot[index].name) + " since the inventory is full");
                            }
                        }
                    }
                }
            }
            index++;
        }
    }

    public Character()
    {
        this.windowSize = new Vector2(375, 300);
        this.customPosition = new Vector2(70, 70);
        this.canBeDragged = true;
        this.onOffButton = KeyCode.I;
        this.windowRect = new Rect(100, 100, 200, 300);
    }

}