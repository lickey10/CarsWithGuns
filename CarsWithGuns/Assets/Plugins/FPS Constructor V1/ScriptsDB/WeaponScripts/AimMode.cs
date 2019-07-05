using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class AimMode : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 Copyrightï¿½ Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
    /* AimMode controls weapon positioning, aiming, sprinting */
    /////////////////////////// CHANGEABLE BY USER ///////////////////////////
    public Texture scopeTexture; //Currently used scope texture
    public bool overrideSprint;
    public int sprintDuration; //how long can we sprint for?
    public float sprintAddStand; //how quickly does sprint replenish when idle?
    public float sprintAddWalk; //how quickly does sprint replenish when moving?
    public float sprintMin; //What is the minimum value ofsprint at which we can begin sprinting?
    public float recoverDelay; //how much time after sprinting does it take to start recovering sprint?
    public float exhaustedDelay; //how much time after sprinting to exhaustion does it take to start recovering sprint?
    public bool crosshairWhenAiming;
    //Changes to the following variable will only be reflected when AimPrimary() or AimSecondary() are called
    //Calling either while aiming is not suggested
    //Zoom info for secondary weapon
    public float zoomFactor2;
    public bool scoped2;
    public bool sightsZoom2;
    //Zoom info for primary weapon
    public float zoomFactor1;
    public bool scoped1;
    public bool sightsZoom1;
    //Set of positions and rotations used for primary weapon
    public Vector3 aimPosition1;
    public Vector3 aimRotation1;
    public Vector3 hipPosition1;
    public Vector3 hipRotation1;
    public Vector3 sprintPosition1;
    public Vector3 sprintRotation1;
    //Set of positions and rotations used for secondary weapon
    public Vector3 aimPosition2;
    public Vector3 aimRotation2;
    public Vector3 hipPosition2;
    public Vector3 hipRotation2;
    public Vector3 sprintPosition2;
    public Vector3 sprintRotation2;
    private float aimStartTime;
    ///////////////////////// END CHANGEABLE BY USER /////////////////////////
    ///////////////////////// Internal Variables /////////////////////////
    /*These variables should not be modified directly, weither because it could compromise
the functioning of the package, or because changes will be overwritten or otherwise
ignored.
*/
    public Texture st169; //scope texture for 16 : 9 aspect ration
    public Texture st1610; // 16 :10
    public Texture st43; // 4 : 3
    public Texture st54; // 5 : 4
    private GameObject player; //Player object
    public bool scoped; //Does this weapon use a scope?
    private float scopeTime; //Time when we should be in scope
    public bool sightsZoom; //Does this weapon zoom when aiming? (Not scoped)
    public bool inScope; //Are we currently scoped?
    public bool aim; //Does the primary weapon aim?
    public bool secondaryAim; //Does the Secondaey weapon Aim?
    public bool canAim; //does the weapon currently aim?
    public bool aiming; //are we currently aiming?
    public static bool sprintingPublic; //are we currently sprinting?
    public bool sprinting;
    public bool canSprint; //can the player sprint currently?
    private Vector3 deltaAngle;
    private bool selected;
    public static float sprintNum;
    public float aimRate;
    public float sprintRate;
    public float retRate;
    private GameObject cmra;
    private GameObject wcmra;
    public float zoomFactor; //how much does this zoom in when aiming? (currently)
    //Set of positions and rotations used
    public Vector3 aimPosition;
    public Vector3 aimRotation;
    public Vector3 hipPosition;
    public Vector3 hipRotation;
    public Vector3 sprintPosition;
    public Vector3 sprintRotation;
    public float rotationSpeed;
    public CharacterController controller;
    private bool zoomed;
    public static bool canSwitchWeaponAim;
    public static bool staticAiming;
    public bool hasSecondary;
    public GunScript GunScript1;
    private Vector3 curVect;
    private float sprintEndTime;
    private CharacterMotorDB CM;
    public static bool exhausted;
    private bool switching;
    private Vector3 startPosition;
    private Vector3 startRotation;
    private float moveProgress;
    public static float staticRate;
    public virtual void Start()
    {
        if (this.aimRate <= 0)
        {
            this.aimRate = 0.3f;
        }
        if (!this.overrideSprint)
        {
            //Get sprint info form MovementValues
            this.sprintDuration = MovementValues.singleton.sprintDuration;
            this.sprintAddStand = MovementValues.singleton.sprintAddStand;
            this.sprintAddWalk = MovementValues.singleton.sprintAddWalk;
            this.sprintMin = MovementValues.singleton.sprintMin;
            this.recoverDelay = MovementValues.singleton.recoverDelay;
            this.exhaustedDelay = MovementValues.singleton.exhaustedDelay;
        }
        this.AimPrimary();
        this.cmra = PlayerWeapons.mainCam;
        this.wcmra = PlayerWeapons.weaponCam;
        this.player = PlayerWeapons.player;
        AimMode.sprintNum = this.sprintDuration;
        this.canSprint = true;
        this.aiming = false;
        this.sprinting = false;
        this.controller = (CharacterController) this.player.GetComponent(typeof(CharacterController));
        if (this.zoomFactor == 0)
        {
            this.zoomFactor = 1;
        }
        this.AspectCheck();
        this.CM = (CharacterMotorDB) GameObject.FindWithTag("Player").GetComponent(typeof(CharacterMotorDB));
    }

    public virtual void AspectCheck()
    {
        if ((this.cmra.GetComponent<Camera>().aspect == 1.6f) && (this.st1610 != null))
        {
            this.scopeTexture = this.st1610;
        }
        else
        {
            if ((Mathf.Round(this.cmra.GetComponent<Camera>().aspect) == 2) && (this.st169 != null))
            {
                this.scopeTexture = this.st169;
            }
            else
            {
                if ((this.cmra.GetComponent<Camera>().aspect == 1.25f) && (this.st54 != null))
                {
                    this.scopeTexture = this.st54;
                }
                else
                {
                    if ((Mathf.Round(this.cmra.GetComponent<Camera>().aspect) == 1) && (this.st43 != null))
                    {
                        this.scopeTexture = this.st43;
                    }
                }
            }
        }
    }

    public virtual void Update()
    {
        float tempSprintTime = 0.0f;
        if ((this.GunScript1 == null) || !this.GunScript1.gunActive)
        {
            if (this.transform.localPosition != this.hipPosition)
            {
                this.transform.localPosition = this.hipPosition;
            }
            if (this.transform.localEulerAngles != this.hipRotation)
            {
                this.transform.localEulerAngles = this.hipRotation;
            }
            this.sprinting = false;
            return;
        }
        AimMode.staticAiming = this.aiming;
        //Replenish Sprint time
        if ((this.controller != null) && (this.controller.velocity.magnitude == 0))
        {
            tempSprintTime = this.sprintEndTime;
        }
        if (((AimMode.sprintNum < this.sprintDuration) && !this.sprinting) && (Time.time > tempSprintTime))
        {
            if ((this.controller != null) && (this.controller.velocity.magnitude == 0))
            {
                AimMode.sprintNum = Mathf.Clamp(AimMode.sprintNum + (this.sprintAddStand * Time.deltaTime), 0, this.sprintDuration);
            }
            else
            {
                AimMode.sprintNum = Mathf.Clamp(AimMode.sprintNum + (this.sprintAddWalk * Time.deltaTime), 0, this.sprintDuration);
            }
        }
        if (AimMode.sprintNum > this.sprintMin)
        {
            AimMode.exhausted = false;
        }
        //Turn on scope if it is time
        if ((this.inScope && !this.aiming) || (this.zoomed && !this.aiming))
        {
            this.inScope = false;
            this.zoomed = false;
            Component[] gos = this.GetComponentsInChildren(typeof(Renderer));
            foreach (Renderer go in gos)
            {
                if (go.name != "muzzle_flash")
                {
                    go.enabled = true;
                }
            }
        }
         //Reset Camera
        //if(!aiming && cmra.GetComponent.<Camera>().fieldOfView != PlayerWeapons.fieldOfView)
        //{
        //		if(sightsZoom1 && !scoped){
        //		    cmra.GetComponent.<Camera>().fieldOfView = Mathf.Lerp(cmra.GetComponent.<Camera>().fieldOfView, PlayerWeapons.fieldOfView, Mathf.SmoothStep(0,1,1 - (aimStartTime-Time.time)/aimRate));
        //		    if(wcmra.GetComponent.<Camera>() != null)
        //			    wcmra.GetComponent.<Camera>().fieldOfView = Mathf.Lerp(wcmra.GetComponent.<Camera>().fieldOfView, PlayerWeapons.fieldOfView, Mathf.SmoothStep(0,1,1 - (aimStartTime-Time.time)/aimRate));
        //		} else {
        //		    cmra.GetComponent.<Camera>().fieldOfView = PlayerWeapons.fieldOfView;
        //		    if(wcmra.GetComponent.<Camera>() != null)
        //			    wcmra.GetComponent.<Camera>().fieldOfView = PlayerWeapons.fieldOfView;
        //		}
        //	}
        //	staticRate = aimRate;
        //    //aiming
        //	if (InputDB.GetButton("Aim") && canAim && PlayerWeapons.canAim && selected && !sprinting /*&& !GunScript1.sprint*/ && Avoidance.canAim){	
        //		if (!aiming){
        //			aimStartTime = Time.time + aimRate;
        //			scopeTime = Time.time + aimRate;
        //			aiming = true;
        //			canSwitchWeaponAim = false;
        //			startPosition = transform.localPosition;
        //			startRotation = transform.localEulerAngles;
        //			curVect= aimPosition-transform.localPosition;
        //			player.BroadcastMessage("Aiming", zoomFactor, SendMessageOptions.DontRequireReceiver);
        //		}
        //		//Align to position
        //		GunToRotation(aimRotation, aimRate);
        //		if (aiming){
        //			transform.localPosition = Vector3.Slerp(startPosition, aimPosition, Mathf.SmoothStep(0,1,1 - (aimStartTime-Time.time)/aimRate));
        //		}
        //		//Turn on scope if it's time
        //		if (scoped && selected && Time.time >= scopeTime && !inScope){
        //			inScope = true;
        //			var go = GetComponentsInChildren(Renderer);
        //			for( var g : Renderer in go){
        //				if (g.gameObject.name != "Sparks")
        //					g.enabled=false;
        //			}
        //			cmra.GetComponent.<Camera>().fieldOfView = PlayerWeapons.fieldOfView/zoomFactor;
        //		}
        //		//Otherwise if sights zoom then zoom in camera
        //		if (sightsZoom && selected && !zoomed && !scoped){
        //			cmra.GetComponent.<Camera>().fieldOfView = Mathf.Lerp(cmra.GetComponent.<Camera>().fieldOfView, PlayerWeapons.fieldOfView/zoomFactor, Mathf.SmoothStep(0,1,1 - (aimStartTime-Time.time)/aimRate));
        //			wcmra.GetComponent.<Camera>().fieldOfView = Mathf.Lerp(wcmra.GetComponent.<Camera>().fieldOfView, PlayerWeapons.fieldOfView/zoomFactor, Mathf.SmoothStep(0,1,1 - (aimStartTime-Time.time)/aimRate));
        //			if(cmra.GetComponent.<Camera>().fieldOfView == PlayerWeapons.fieldOfView/zoomFactor){
        //				zoomed = true;
        //			}
        //		}
        //    //sprinting
        //	}else if(CM != null && InputDB.GetButton("Sprint")&& !InputDB.GetButton("Aim") && canSprint && PlayerWeapons.canSprint && selected && !aiming && CM.grounded && !exhausted  && (controller.velocity.magnitude > CM.movement.minSprintSpeed || (/*CM.prone || */CM.crouching))){
        //		sprintNum = Mathf.Clamp(sprintNum - Time.deltaTime, 0, sprintDuration);
        //		aiming = false;
        //		if (!sprinting){
        //			aimStartTime = Time.time + sprintRate;
        //			curVect= sprintPosition-transform.localPosition;
        //			sprinting = true;			
        //			player.BroadcastMessage("Sprinting", SendMessageOptions.DontRequireReceiver);
        //			canSwitchWeaponAim = false;	
        //			startPosition = transform.localPosition;
        //			startRotation = transform.localEulerAngles;
        //		}
        //		//Align to position
        //		transform.localPosition = Vector3.Slerp(startPosition, sprintPosition, Mathf.SmoothStep(0,1,1 - (aimStartTime-Time.time)/sprintRate));
        //		GunToRotation(sprintRotation, sprintRate);
        //		//Check if we're out of sprint
        //		if(sprintNum <= 0){
        //			exhausted = true;
        //			sprintEndTime = Time.time + recoverDelay;
        //	}
        //    //returning to normal		
        //	} else {
        //		if ((aiming || sprinting || switching)){
        //			if(sprinting){
        //				sprintEndTime = Time.time + recoverDelay;
        //				player.BroadcastMessage("StopSprinting", SendMessageOptions.DontRequireReceiver);
        //			}
        //			switching = false;
        //			aimStartTime = Time.time + retRate;
        //			startPosition = transform.localPosition;
        //			startRotation = transform.localEulerAngles;
        //			sprinting = false;
        //			canSwitchWeaponAim = true;
        //			curVect= hipPosition-transform.localPosition;
        //			SendMessageUpwards("NormalSpeed", SendMessageOptions.DontRequireReceiver);
        //			if(aiming){
        //				aiming = false;
        //				player.BroadcastMessage("StopAiming", SendMessageOptions.DontRequireReceiver);
        //			}
        //		}
        //		//Align to position
        //		transform.localPosition = Vector3.Slerp(startPosition, hipPosition, Mathf.SmoothStep(0,1,1 - (aimStartTime-Time.time)/retRate));
        //		GunToRotation(hipRotation, retRate);
        //	}
        AimMode.staticAiming = this.aiming;
        AimMode.sprintingPublic = this.sprinting;
    }

    public virtual void DeselectWeapon()
    {
        this.selected = false;
        this.inScope = false;
        this.aiming = false;
    }

    public virtual void SelectWeapon()
    {
        this.selected = true;
        this.aiming = false;
        SmartCrosshair.displayWhenAiming = this.crosshairWhenAiming;
    }

    public virtual void AimPrimary()
    {
        this.aimPosition = this.aimPosition1;
        this.aimRotation = this.aimRotation1;
        this.hipPosition = this.hipPosition1;
        this.hipRotation = this.hipRotation1;
        this.sprintPosition = this.sprintPosition1;
        this.sprintRotation = this.sprintRotation1;
        this.curVect = this.hipPosition - this.transform.localPosition;
        this.GetGunScript(0);
        this.zoomFactor = this.zoomFactor1;
        this.scoped = this.scoped1;
        this.sightsZoom = this.sightsZoom1;
        this.canAim = this.aim;
        this.switching = true;
    }

    public virtual void AimSecondary()
    {
        this.aimPosition = this.aimPosition2;
        this.aimRotation = this.aimRotation2;
        this.hipPosition = this.hipPosition2;
        this.hipRotation = this.hipRotation2;
        this.sprintPosition = this.sprintPosition2;
        this.sprintRotation = this.sprintRotation2;
        this.curVect = this.hipPosition - this.transform.localPosition;
        this.GetGunScript(1);
        this.zoomFactor = this.zoomFactor2;
        this.scoped = this.scoped2;
        this.sightsZoom = this.sightsZoom2;
        this.canAim = this.secondaryAim;
        this.switching = true;
    }

    public virtual void GetGunScript(int n)
    {
        Component[] GunScripts = this.transform.parent.GetComponents(typeof(GunScript));
        foreach (GunScript gs in GunScripts)
        {
            if ((n == 0) && gs.isPrimaryWeapon)
            {
                this.GunScript1 = gs;
            }
            else
            {
                if ((n == 1) && !gs.isPrimaryWeapon)
                {
                    this.GunScript1 = gs;
                }
            }
        }
    }

    public virtual void GunToRotation(Vector3 v3, float rate)
    {
        this.transform.localEulerAngles.x = Mathf.LerpAngle(this.startRotation.x, v3.x, Mathf.SmoothStep(0, 1, 1 - ((this.aimStartTime - Time.time) / rate)));
        this.transform.localEulerAngles.y = Mathf.LerpAngle(this.startRotation.y, v3.y, Mathf.SmoothStep(0, 1, 1 - ((this.aimStartTime - Time.time) / rate)));
        this.transform.localEulerAngles.z = Mathf.LerpAngle(this.startRotation.z, v3.z, Mathf.SmoothStep(0, 1, 1 - ((this.aimStartTime - Time.time) / rate)));
    }

    public AimMode()
    {
        this.sprintDuration = 5;
        this.sprintAddStand = 1;
        this.sprintAddWalk = 0.3f;
        this.sprintMin = 1;
        this.recoverDelay = 0.7f;
        this.exhaustedDelay = 1;
        this.zoomFactor2 = 1;
        this.zoomFactor1 = 1;
        this.aim = true;
        this.secondaryAim = true;
        this.aimRate = 3;
        this.sprintRate = 0.4f;
        this.retRate = 0.4f;
        this.zoomFactor = 1;
        this.rotationSpeed = 180;
        this.hasSecondary = true;
    }

    static AimMode()
    {
        AimMode.canSwitchWeaponAim = true;
    }

}