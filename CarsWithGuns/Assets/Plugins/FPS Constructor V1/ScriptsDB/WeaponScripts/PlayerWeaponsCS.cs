using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponsCS : MonoBehaviour
{
    public GameObject[] weapons = new GameObject[0]; //Array of equipped weapons (accessible by number keys)
public int selectedWeapon = 0; //index of currently selected weapon in array
bool reloadWhileSprinting = false;
float displayTime = 1; //How long slot-related message is shown for
float sensitivity = 13; //Sensitivity of mouse
bool inverted = false;  //Is the mouse y-axis inverted
float interactDistance = 5; //How far can an object be to be interacted with
private Transform lastHit; //the last object we hit
LayerMask interactMask; //Mask for raycast

//Settings
static bool autoReload = true; //Do guns automatically reload when emptied?
static float fieldOfView = 60; //Base field of view for cameras
static int playerLayer = 8;
static int ignorePlayerLayer = 8;
static bool canSwapSameWeapon = true; //Can we pick up multiples of the same weapon? (Replacing it)
static bool saveUpgradesToDrops = true;
    LayerMask RaycastsIgnore; //Layers that gun raycasts hit


//Control Variables
static bool canMove = true;
static bool canSprint = true;
static bool canLook = true; //Can the player look around?
static bool canFire = true;
static bool canAim = true;
static bool canCrouch = true;
static bool doesIdle = true;
static bool canInteract = true;
static bool canSwitchWeapon = true;

//Status
static bool hidden = false;
static bool sprinting = false;

//Don't change
static GameObject player;
static CharacterController controller;
//static CharacterMotorDB CM;
//static GameObject weaponCam;
//static GameObject mainCam;
//static bool autoFire;
//static bool charge = false;
//private bool canKickback = true;
//private float emptyMessageTime;
//private string emptyMessage;
//private bool displayMessage = false;
//static bool playerActive = true;
//static PlayerWeapons PW = new PlayerWeapons(); //Singleton object

//    void Start()
//    {
//        // Select the first weapon
//        //playerActive = true;
//        ActivateWeapon();
//    }

//    void Awake()
//    {
//        if (PW)
//            Debug.LogError("Too many instances of PlayerWeapons! There should only be one per scene");
//        PW = null;
//        PW = this;
//        weaponCam = GameObject.FindWithTag("WeaponCamera");
//        mainCam = GameObject.FindWithTag("MainCamera");
//        player = GameObject.FindWithTag("Player");
//        CM = player.GetComponent(CharacterMotorDB);
//        controller = player.GetComponent(CharacterController);
//        hidden = false;

//        SetSensitivity();
//    }

//    void SetSensitivity()
//    {
//        //transform.parent.GetComponent(MouseLookDBJS).sensitivityStandardX = sensitivity;
//        if (inverted)
//            sensitivity *= -1;
//        //this.GetComponent(MouseLookDBJS).sensitivityStandardY = sensitivity;
//        //sensitivity = Mathf.Abs(sensitivity);

//    }

//    void LateUpdate()
//    {
//        if (!playerActive)
//            return;
//        if (Input.GetButtonDown("Fire1") && canKickback)
//        {
//            canKickback = false;
//        }
//        else if (Input.GetButtonUp("Fire1"))
//        {
//            canKickback = true;
//            gameObject.BroadcastMessage("ReleaseFire", 1, SendMessageOptions.DontRequireReceiver);
//        }
//        if (weapons[selectedWeapon] != null)
//            if (/*!InputDB.GetButton ("Fire1") || */Time.time > weapons[selectedWeapon].GetComponent(GunScript).nextFireTime)
//            {
//                BroadcastMessage("Cooldown");
//            }
//        if (Input.GetButtonUp("Aim"))
//        {
//            gameObject.SendMessageUpwards("ReleaseFire", 2, SendMessageOptions.DontRequireReceiver);
//        }
//    }

//    void Update()
//    {
//        if (!playerActive)
//            return;

//        /************************Interact****************************/
//        if (interactDistance > 0)
//        {
//            //Set up ray
//            Ray ray;
//            RaycastHit hit;
//            ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0));
//            if (Physics.Raycast(ray, hit, interactDistance, interactMask))
//            {
//                //We hit something new
//                if (lastHit != hit.transform)
//                {
//                    //Last object isn't still highlighted
//                    if (lastHit)
//                        lastHit.SendMessage("HighlightOff", SendMessageOptions.DontRequireReceiver);
//                    //New one is
//                    lastHit = hit.transform;
//                    lastHit.SendMessage("HighlightOn", SendMessageOptions.DontRequireReceiver);
//                }
//            }
//            else if (lastHit != null)
//            {
//                //We hit nothing, but still have a object highlighted, so unhighlight it
//                lastHit.SendMessage("HighlightOff", SendMessageOptions.DontRequireReceiver);
//                lastHit = null;
//            }

//            //Interact
//            if (InputDB.GetButtonDown("Interact") && lastHit != null)
//            {
//                lastHit.SendMessage("Interact", this.gameObject, SendMessageOptions.DontRequireReceiver);
//            }
//        }
//        /**********************Fire & Reload******************************/

//        // Did the user press fire?
//        if (InputDB.GetButton("Fire1") && (autoFire || charge) && canFire)
//        {
//            var touch : Touch;

//#if !MOBILE_INPUT
//            transform.root.BroadcastMessage("Fire", SendMessageOptions.DontRequireReceiver);
//#else
//                for (var i: int = 0; i < Input.touchCount; ++i) 
//                {
//                    touch = Input.GetTouch(i);

//                    if (touch.phase == TouchPhase.Began) 
//                    {
//                        if (touch.position.x > Screen.width/2) {
//                            transform.root.BroadcastMessage("Fire", SendMessageOptions.DontRequireReceiver);
//                        }
//                    }
//                }

//                if(Input.mousePosition.x > Screen.width/2)
//                    transform.root.BroadcastMessage("Fire", SendMessageOptions.DontRequireReceiver);
//#endif
//        }
//        else if (InputDB.GetButtonDown("Fire1") && canFire)
//        {
//#if !MOBILE_INPUT
//            transform.root.BroadcastMessage("Fire", SendMessageOptions.DontRequireReceiver);
//#else
//            for (var x: int = 0; x < Input.touchCount; ++x) 
//            {
//                touch = Input.GetTouch(x);

//                if (touch.phase == TouchPhase.Began) 
//                 {
//                    if (touch.position.x > Screen.width/2) {
//                        transform.root.BroadcastMessage("Fire", SendMessageOptions.DontRequireReceiver);
//                    }
//                }
//            }
//#endif
//        }
//        if (InputDB.GetButton("Fire2") && canFire && (autoFire || charge))
//        {
//            BroadcastMessage("Fire2", SendMessageOptions.DontRequireReceiver);
//        }
//        else if (InputDB.GetButtonDown("Fire2") && canFire)
//        {
//            BroadcastMessage("Fire2", SendMessageOptions.DontRequireReceiver);
//        }

//        if (InputDB.GetButtonDown("Reload"))
//        {
//            BroadcastMessage("Reload", SendMessageOptions.DontRequireReceiver);
//        }

//        /*************************Weapon Switching***************************/

//        if (!AimMode.canSwitchWeaponAim || hidden || !canSwitchWeapon || Avoidance.collided)
//        {
//            return;
//        }

//        if (InputDB.GetButtonDown("SelectWeapon"))
//        {
//            var temp : int = WeaponSelector.selectedWeapon;
//            if (weapons.length > temp && (selectedWeapon != temp || weapons[selectedWeapon] == null) && temp >= 0)
//            {
//                if (weapons[temp] != null)
//                {
//                    if (!weapons[temp].gameObject.GetComponent(WeaponInfo).locked)
//                    {
//                        SelectWeapon(temp);
//                        selectedWeapon = temp;
//                        displayMessage = false;
//                    }
//                    else
//                    {
//                        SlotEmptyMessage(temp + 1);
//                    }
//                }
//                else
//                {
//                    SlotEmptyMessage(temp + 1);
//                }
//            }
//        }

//    }
}
