using UnityEngine;
using System.Collections;

/*
 FPS Constructor - Weapons
 Copyrightï¿½ Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
/////////////////////////// CHANGEABLE BY USER ///////////////////////////
/*These variables can be changed by external scripts when necessary. 
*/
////////// Accuracy Variables //////////
/*Kickback variables: Kickback is the visual motion of the camera when firing.
*/
 //Vertical kickback per shot (degrees)
 //Horizontal kickback per shot (percent of vertical)
 //Maximum vertical kickback (degrees)
 //Delay between stopping firing and recoil decreasing
/*Spread variables: Spread (between 0 and 1) determines the accuracy of the weapon.
A spread of 0 means that the weapon will fire exactly towards the crosshair, and
a spread of 1 means that it will fire anywhere within 90 degrees of the crosshair.
*/
 //default spread of this weapon
 //Maximum spread of this weapon
 //When crouching, spread is multiplied by this
 //When prone, spread is multiplied by this
 //When walking, spread is multiplied by this
 //Standard increase in spread per shot
 //Increase in spread per shot when aiming
 //Default spread when aiming
 //Speed at which spread decreases when not firing
////////// Ammo variables //////////
 //Ammo left in the curent clip (ammo before next reload)
 //Shots per clip
 //Ammo used per shot
 //Number of spare clips (reloads) left
 //Maximum number of clips
 //Does this gun deplete clips whe reoading?
public enum ammoTypes
{
    byClip = 0,
    byBullet = 1
}

 //Does this weapon conserve ammo when reloading? (e.g. if you reload after shooting one bullet, does it use a whole clip)
////////// Fire Variables //////////
 //Sound that plays when firing
//Pitch of fire sound
 //Time in seconds between shots
 //Is this weapon automatic (can you hold down the fre button?)
 //Does this weapon's fire animation scale to fit the fire rate (generally used for non-automatic weapons)
 //Delay between hitting fire button and actually firing (can be used to sync firing with animation)
 //Sound that plays when firing
//Pitch of fire sound
//Burst fire
//note: burst fire doesn't work well with automatic weapons
 //does this wepon fire in bursts
 //shots per burst
 //time to fire full burst
////////// Reloading variables //////////
/*Progressive Reload is a different kind of reloading where the reload is broken into stages.
The first stage initializes the animation to get to the second stage. The second stage represents
reloading one shot, and will repeat as many times as necessary to reach a full clip unless
interrupted. Then the third stage returns the weapon to its standrad position. This is useful
for weapons like shotguns that reload by shells.
*/
 //Does this weapon use progressive reload?
 //Does this weapon's ammo reset to 0 when starting a reload? 
//(e.g. a revolver where the shells are discarded and replaced)
 //time in seconds for the first stage
 //time in seconds for the third stage
//the time for the second stage is just reloadTime
////////// Gun-Specific Variables //////////
 //Range of bullet raycast in meters
 //Force of bullet
 //Damage per bullet
 //Bullets per shot
 //penetration level of bullet
/* Damage falloff allows raycast weapons to do less damage at long distances
*/
 //Does this weapon use damage falloff?
 //Distance at which falloff begins to take effect
 //Distance at which falloff stops (minumum damage)
 //Coefficient for multiplying damage
 //Scaling value to change speed of falloff
////////// Launcher-Specific Variables //////////
 //The object to launch. This can be anything, as long as it has a rigidbody.
 //Initial speed of projectile, applied  forward
 //Number of projectiles fired
 //GameObject whose position the projectile is fired from (place this at the end of the weapon's barrel, generally)
////////// Tracer related variables //////////
/* Tracers are done using particle emitters.
*/
 //Tracer object. Must have a particle emitter attached
 //Activate a tracer evey x shots.
 //How long to simulate tracer before it appears
 //This isn't exposed, but can be tweaked if needed
////////// Sway //////////
 //Does the weapon sway?
 //How fast does the weapon sway when walking? (xy)
 //How much does the weapon sway when walking? (xy)
 //How fast does the weapon sway when sprinting? (xy)
 //How much does the weapon sway when sprinting? (xy)
 //How fast does the weapon sway when standing? (xy)
 //How much does the weapon sway when standing? (xy)
////////// Secondary Weapons //////////
 //Gunscript of secondary weapon (additional weapon script on this object)
 //Can primary and secondary weapon interrupt each other's actions
 //Is the secondary weapon fired with Mouse2?
 //How long does it take to switch to secondary (animation)?
 //How long does it take to switch from secondary (animation)?
////////// Charge weapon variables //////////
 //Minimum charge value at ahich the weapon can fire
 // Maximum charge value the weapon can have
 //current charge level of the weapon
 //Does this weapon have to fire when it hits max charge?
 //Sound to play when charging
 //Does the weapon automatically start charging again after a forced release?
//Specifically for hitscan charge weapons
 //Damage multiplier as charge increases
 //Ammo change as charge increases (add this per 1 charge level)
//////////Other variables//////////
 //Time in seconds that the player has been idle
 //Time in seconds of being idle which will cause the idle animation to play
 //Time to take out (switch to) weapon
 //Time to put away (switch from) weapon 
//////////Z KickBack//////////
 //Does this weapon use z kickback?
 //Rate of z kickback when firing
 //rate of return from z when not firing
 //maximum z kickback
//////////Avoidance//////////
//Avoidance is by default handled globall by the Avoidance Component. This just overrides its values for this weapon.
 //Does this weapon override global object avoidance values
//Shell Ejection
 //Does this weapon use shell ejection?
 //If it does, this gameobject provides the position where shells are instantiated
 //The shell prefab to instantiate
//Custom crosshair variables
 //Does the crosshair scale with accuracy?
 //Crosshair object to use
 //Default scale of the crosshair object
///////////////////////// END CHANGEABLE BY USER /////////////////////////
///////////////////////// Internal Variables /////////////////////////
/*These variables should not be modified directly, weither because it could compromise
the functioning of the package, or because changes will be overwritten.
*/
 // Is the weapon currently selected & activated
//Status
 //How far have we kicked back?
//Components
////////// Spray //////////
/* Spray weapons are meant to be a simple solution for something that can now be done better with a 
charge weapon.
*/
////////// Charge weapon variables //////////
 //Is this weapon a charge weapon?
//Gun Types
public enum gunTypes
{
    hitscan = 0,
    launcher = 1,
    melee = 2,
    spray = 3
}

[System.Serializable]
public partial class GunScript : MonoBehaviour
{
    public float kickbackAngle;
    public float xKickbackFactor;
    public float maxKickback;
    public float kickbackAim;
    public float crouchKickbackMod;
    public float proneKickbackMod;
    public float moveKickbackMod;
    private float curKickback;
    public float recoilDelay;
    public float standardSpread;
    public float maxSpread;
    public float crouchSpreadModifier;
    public float proneSpreadModifier;
    public float moveSpreadModifier;
    public float standardSpreadRate;
    public float aimSpreadRate;
    public float aimSpread;
    public float spDecRate;
    public float ammoLeft;
    public int ammoPerClip;
    public int ammoPerShot;
    public int clips;
    public int maxClips;
    public bool infiniteAmmo;
    public ammoTypes ammoType;
    public AudioClip fireSound;
    public float fireVolume;
    public float firePitch;
    public float fireRate;
    public bool autoFire;
    public bool fireAnim;
    public float delay;
    public AudioClip emptySound;
    public float emptyVolume;
    public float emptyPitch;
    public bool burstFire;
    public int burstCount;
    public float burstTime;
    public float reloadTime;
    public float emptyReloadTime;
    public bool addOneBullet;
    public float waitforReload;
    public bool progressiveReload;
    public bool progressiveReset;
    public float reloadInTime;
    public float reloadOutTime;
    public float range;
    public float force;
    public float damage;
    public int shotCount;
    public int penetrateVal;
    public bool hasFalloff;
    public float minFalloffDist;
    public float maxFalloffDist;
    public float falloffCoefficient;
    public float falloffDistanceScale;
    public Rigidbody projectile;
    public float initialSpeed;
    public int projectileCount;
    public GameObject launchPosition;
    public GameObject tracer;
    public int traceEvery;
    public float simulateTime;
    public float minDistForTracer;
    public bool sway;
    public Vector2 moveSwayRate;
    public Vector2 moveSwayAmplitude;
    public Vector2 runSwayRate;
    public Vector2 runAmplitude;
    public Vector2 idleSwayRate;
    public Vector2 idleAmplitude;
    public GunScript secondaryWeapon;
    public bool secondaryInterrupt;
    public bool secondaryFire;
    public float enterSecondaryTime;
    public float exitSecondaryTime;
    public float minCharge;
    public float maxCharge;
    public float chargeLevel;
    public bool forceFire;
    public AudioClip chargeLoop;
    public bool chargeAuto;
    public float chargeCoefficient;
    public float additionalAmmoPerCharge;
    public float idleTime;
    public float timeToIdle;
    public float takeOutTime;
    public float putAwayTime;
    public bool useZKickBack;
    public float kickBackZ;
    public float zRetRate;
    public float maxZ;
    public bool overrideAvoidance;
    public bool avoids;
    public Vector3 rot;
    public Vector3 pos;
    public float dist;
    public float minDist;
    public bool shellEjection;
    public GameObject ejectorPosition;
    public float ejectDelay;
    public GameObject shell;
    public bool scale;
    public GameObject crosshairObj;
    public float crosshairSize;
    public bool gunActive;
    private bool interruptPutAway;
    private bool progressiveReloading;
    public bool inDelay;
    private int m_LastFrameShot;
    public static bool reloading;
    public float nextFireTime;
    public static bool takingOut;
    public static bool puttingAway;
    public bool secondaryActive;
    public static float crosshairSpread;
    private float shotSpread;
    private float actualSpread;
    private float spreadRate;
    public bool isPrimaryWeapon;
    public bool aim;
    public bool aim2;
    private float pReloadTime;
    private bool stopReload;
    private Vector3 startPosition;
    public bool gunDisplayed;
    private float totalKickBack;
    public AmmoDisplay ammo;
    public SprintDisplay sprint;
    public WeaponDisplay wepDis;
    public static GameObject mainCam;
    public static GameObject weaponCam;
    private GunScript primaryWeapon;
    private GameObject player;
    public AimMode aim1;
    public MouseLookDBJS mouseY;
    public MouseLookDBJS mouseX;
    public bool reloadCancel;
    private float tempAmmo;
    public bool sprayOn;
    public GameObject sprayObj;
    public SprayScript sprayScript;
    public float deltaTimeCoefficient;
    public float forceFalloffCoefficient;
    public AudioClip loopSound;
    public AudioClip releaseSound;
    public float ammoPerSecond;
    public bool chargeWeapon;
    public bool chargeReleased;
    public bool chargeLocked;
    public gunTypes gunType;
    //Melee
    public bool hitBox;
    //Tracer related variables
    private int shotCountTracer;
    //Ammo Sharing
    public bool sharesAmmo;
    public bool shareLoadedAmmo;
    public int ammoSetUsed;
    public GameObject managerObject;
    public AmmoManager ammoManagerScript;
    //Effects
    public EffectsManager effectsManager;
    public CharacterMotorDB CM;
    //Inspector only variables
    public bool shotPropertiesFoldout;
    public bool firePropertiesFoldout;
    public bool accuracyFoldout;
    public bool altFireFoldout;
    public bool ammoReloadFoldout;
    public bool audioVisualFoldout;
    //Sway (Internal)
    public float swayStartTime;
    public Vector2 swayRate;
    public Vector2 swayAmplitude;
    public bool overwriteSway;
    private bool airborne;
    public virtual void Awake()
    {
        //if(gunActive){
        Component[] gos = this.GetComponentsInChildren(typeof(Renderer));
        foreach (Renderer go in gos)
        {
            go.enabled = false;
        }
        this.gunActive = false;
        //}
        this.startPosition = this.transform.localPosition;
        if (this.gunType == gunTypes.spray)
        {
            if (this.sprayObj)
            {
                this.sprayScript = (SprayScript) this.sprayObj.GetComponent(typeof(SprayScript));
                this.sprayScript.isActive = false;
            }
            else
            {
                Debug.LogWarning("Spray object is undefined; all spray weapons must have a spray object!");
            }
        }
        GunScript.crosshairSpread = 0;
        this.managerObject = GameObject.FindWithTag("Manager");
        this.ammoManagerScript = (AmmoManager) this.managerObject.GetComponent(typeof(AmmoManager));
        this.effectsManager = (EffectsManager) this.managerObject.GetComponent(typeof(EffectsManager));
        this.aim1 = (AimMode) this.GetComponentInChildren(typeof(AimMode));
        this.ammo = (AmmoDisplay) this.GetComponent(typeof(AmmoDisplay));
        this.sprint = (SprintDisplay) this.aim1.GetComponent(typeof(SprintDisplay));
        this.wepDis = (WeaponDisplay) this.GetComponent(typeof(WeaponDisplay));
        this.ammo.enabled = false;
        this.sprint.enabled = false;
        this.wepDis.enabled = false;
    }

    public virtual void Start()
    {
        GunScript.mainCam = PlayerWeapons.mainCam;
        GunScript.weaponCam = PlayerWeapons.weaponCam;
        this.player = PlayerWeapons.player;
        this.CM = (CharacterMotorDB) this.player.GetComponent(typeof(CharacterMotorDB));
        this.mouseY = (MouseLookDBJS) GunScript.weaponCam.GetComponent(typeof(MouseLookDBJS));
        this.mouseX = (MouseLookDBJS) this.player.GetComponent(typeof(MouseLookDBJS));
        if (this.maxSpread > 1)
        {
            this.maxSpread = 1;
        }
        this.inDelay = false;
        this.hitBox = false;
        if (this.sharesAmmo)
        {
            this.clips = this.ammoManagerScript.clipsArray[this.ammoSetUsed];
            this.maxClips = this.ammoManagerScript.maxClipsArray[this.ammoSetUsed];
            this.infiniteAmmo = this.ammoManagerScript.infiniteArray[this.ammoSetUsed];
        }
        if (!this.isPrimaryWeapon)
        {
            this.gunActive = false;
            object[] wpns = new object[0];
            wpns = this.GetComponents(typeof(GunScript));
            int p = 0;
            while (p < wpns.Length)
            {
                GunScript g = wpns[p] as GunScript;
                if (g.isPrimaryWeapon)
                {
                    this.primaryWeapon = g;
                }
                p++;
            }
        }
        if (!this.overwriteSway && this.sway)
        {
            CamSway overSway = CamSway.singleton;
            if (overSway != null)
            {
                this.runSwayRate = overSway.runSwayRate;
                this.moveSwayRate = overSway.moveSwayRate;
                this.idleSwayRate = overSway.idleSwayRate;
            }
        }
        this.curKickback = this.kickbackAngle;
        this.shotSpread = this.standardSpread;
        this.spreadRate = this.standardSpreadRate;
        this.ammoLeft = this.ammoPerClip;
        this.SendMessage("UpdateAmmo", this.ammoLeft, SendMessageOptions.DontRequireReceiver);
        this.SendMessage("UpdateClips", this.clips, SendMessageOptions.DontRequireReceiver);
        this.swayRate = this.moveSwayRate;
        this.swayAmplitude = this.moveSwayAmplitude;
    }

    public virtual void Aiming()
    {
        this.idleTime = 0;
        this.shotSpread = this.aimSpread;
        this.spreadRate = this.aimSpreadRate;
        this.curKickback = this.kickbackAim;
        if (CharacterMotorDB.crouching)
        {
            this.Crouching();
        }
        if (CharacterMotorDB.prone)
        {
            this.Prone();
        }
        if (CharacterMotorDB.walking)
        {
            this.Walking();
        }
        if (!this.CM.grounded)
        {
            this.Airborne();
        }
    }

    public virtual void Crouching()
    {
        if (this.aim1.aiming)
        {
            this.spreadRate = this.aimSpreadRate * this.crouchSpreadModifier;
            this.shotSpread = Mathf.Max(this.aimSpread * this.crouchSpreadModifier, this.shotSpread);
            this.curKickback = this.kickbackAim * this.crouchKickbackMod;
        }
        else
        {
            this.curKickback = this.kickbackAngle * this.crouchKickbackMod;
            this.spreadRate = this.standardSpreadRate * this.crouchSpreadModifier;
            this.shotSpread = Mathf.Max(this.standardSpread * this.crouchSpreadModifier, this.shotSpread);
        }
    }

    public virtual void Prone()
    {
        if (this.aim1.aiming)
        {
            this.curKickback = this.kickbackAim * this.proneKickbackMod;
            this.spreadRate = this.aimSpreadRate * this.proneSpreadModifier;
            this.shotSpread = Mathf.Max(this.aimSpread * this.proneSpreadModifier, this.shotSpread);
        }
        else
        {
            this.curKickback = this.kickbackAngle * this.proneKickbackMod;
            this.spreadRate = this.standardSpreadRate * this.proneSpreadModifier;
            this.shotSpread = Mathf.Max(this.standardSpread * this.proneSpreadModifier, this.shotSpread);
        }
    }

    public virtual void Walking()
    {
        if (this.aim1.aiming)
        {
            this.curKickback = this.kickbackAim * this.moveKickbackMod;
            this.spreadRate = this.aimSpreadRate * this.moveSpreadModifier;
            this.shotSpread = Mathf.Max(this.aimSpread * this.moveSpreadModifier, this.shotSpread);
        }
        else
        {
            this.curKickback = this.kickbackAngle * this.moveKickbackMod;
            this.spreadRate = this.standardSpreadRate * this.moveSpreadModifier;
            this.shotSpread = Mathf.Max(this.standardSpread * this.moveSpreadModifier, this.shotSpread);
        }
    }

    public virtual void StopWalking()
    {
        if (this.airborne)
        {
            return;
        }
        this.spreadRate = this.standardSpreadRate;
        this.curKickback = this.kickbackAngle;
        if (this.shotSpread < this.standardSpread)
        {
            this.shotSpread = this.standardSpread;
        }
        if (this.aim1.aiming)
        {
            this.curKickback = this.kickbackAim;
            this.spreadRate = this.aimSpreadRate;
            this.shotSpread = this.aimSpread;
        }
    }

    public virtual void Landed()
    {
        this.airborne = false;
        this.spreadRate = this.standardSpreadRate;
        this.curKickback = this.kickbackAngle;
        if (this.shotSpread < this.standardSpread)
        {
            this.shotSpread = this.standardSpread;
        }
        if (this.aim1.aiming)
        {
            this.curKickback = this.kickbackAim;
            this.spreadRate = this.aimSpreadRate;
            this.shotSpread = this.aimSpread;
        }
    }

    public virtual void Airborne()
    {
        this.airborne = true;
        if (this.aim1.aiming)
        {
            this.curKickback = this.kickbackAim * this.moveKickbackMod;
            this.spreadRate = this.aimSpreadRate * this.moveSpreadModifier;
            this.shotSpread = Mathf.Max(this.aimSpread * this.moveSpreadModifier, this.shotSpread);
        }
        else
        {
            this.curKickback = this.kickbackAngle * this.moveKickbackMod;
            this.spreadRate = this.standardSpreadRate * this.moveSpreadModifier;
            this.shotSpread = Mathf.Max(this.standardSpread * this.moveSpreadModifier, this.shotSpread);
        }
    }

    public virtual void StopAiming()
    {
        this.idleTime = 0;
        this.shotSpread = this.standardSpread;
        this.spreadRate = this.standardSpreadRate;
        this.curKickback = this.kickbackAngle;
        if (this.CM != null)
        {
            if (CharacterMotorDB.crouching)
            {
                this.Crouching();
            }
            if (CharacterMotorDB.prone)
            {
                this.Prone();
            }
            if (CharacterMotorDB.walking)
            {
                this.Walking();
            }
            if (!this.CM.grounded)
            {
                this.Airborne();
            }
        }
    }

    public virtual void Cooldown()
    {
        float targ = 0.0f;
        if (!this.gunActive)
        {
            return;
        }
        this.ReturnKickBackZ();
        if (this.aim1.aiming)
        {
            targ = this.aimSpread;
        }
        else
        {
            targ = this.standardSpread;
        }
        if (this.CM != null)
        {
            if (CharacterMotorDB.crouching)
            {
                targ = targ * this.crouchSpreadModifier;
            }
            if (CharacterMotorDB.prone)
            {
                targ = targ * this.proneSpreadModifier;
            }
            if (CharacterMotorDB.walking || !this.CM.grounded)
            {
                targ = targ * this.moveSpreadModifier;
            }
        }
        this.shotSpread = Mathf.Clamp(this.shotSpread - (this.spDecRate * Time.deltaTime), targ, this.maxSpread);
    }

    public virtual void Update()
    {
        if (this.progressiveReloading)
        {
            if (((this.ammoLeft < this.ammoPerClip) && (this.clips >= 1)) && !this.stopReload)
            {
                if (Time.time > this.pReloadTime)
                {
                    this.BroadcastMessage("ReloadAnim", this.reloadTime);
                    this.pReloadTime = Time.time + this.reloadTime;
                    this.ammoLeft++;
                    this.clips--;
                    this.SendMessage("UpdateAmmo", this.ammoLeft, SendMessageOptions.DontRequireReceiver);
                    this.SendMessage("UpdateClips", this.clips, SendMessageOptions.DontRequireReceiver);
                }
            }
            else
            {
                if (Time.time > this.pReloadTime)
                {
                    this.progressiveReloading = false;
                    PlayerWeapons.autoFire = this.autoFire;
                    this.stopReload = false;
                    this.BroadcastMessage("ReloadOut", this.reloadOutTime);
                    GunScript.reloading = false;
                    if (this.aim)
                    {
                        this.aim1.canAim = true;
                    }
                    this.aim1.canSprint = true;
                    //aim1.canSwitchWeaponAim = true;
                    this.ApplyToSharedAmmo();
                }
            }
        }
        if (this.actualSpread != this.shotSpread)
        {
            if (this.actualSpread > this.shotSpread)
            {
                this.actualSpread = Mathf.Clamp(this.actualSpread - (Time.deltaTime / 4), this.shotSpread, this.maxSpread);
            }
            else
            {
                this.actualSpread = Mathf.Clamp(this.actualSpread + (Time.deltaTime / 4), 0, this.shotSpread);
            }
        }
        if (this.gunActive)
        {
            this.idleTime = this.idleTime + Time.deltaTime;
            this.idleTime = 0;
            if (!PlayerWeapons.autoFire && this.autoFire)
            {
                this.SendMessageUpwards("FullAuto");
            }
            if (PlayerWeapons.autoFire && !this.autoFire)
            {
                this.SendMessageUpwards("SemiAuto");
            }
            if (!PlayerWeapons.charge && this.chargeWeapon)
            {
                this.SendMessageUpwards("Charge");
            }
            if (PlayerWeapons.charge && !this.chargeWeapon)
            {
                this.SendMessageUpwards("NoCharge");
            }
        }
    }

    public virtual void LateUpdate()
    {
        if ((((InputDB.GetButtonDown("Fire2") && (this.secondaryWeapon != null)) && !this.secondaryFire) && !this.aim1.aiming) && !Avoidance.collided)
        {
            if (!this.secondaryWeapon.gunActive)
            {
                this.StartCoroutine(this.ActivateSecondary());
            }
            else
            {
                if (this.secondaryWeapon.gunActive)
                {
                    this.StartCoroutine(this.ActivatePrimary());
                }
            }
        }
        if (this.gunActive)
        {
            if (this.idleTime > this.timeToIdle)
            {
                if (((this.CM != null) && !this.aim1.aiming) && !Avoidance.collided)
                {
                    this.BroadcastMessage("IdleAnim", SendMessageOptions.DontRequireReceiver);
                }
                this.idleTime = 0;
            }
            this.shotSpread = Mathf.Clamp(this.shotSpread, 0, this.maxSpread);
        }
        else
        {
            //crosshairSpread = actualSpread*180/weaponCam.GetComponent.<Camera>().fieldOfView*Screen.height;
            return;
        }
        if (((((this.CM != null) && CharacterMotorDB.walking) && !this.aim1.aiming) && this.sway) && !CharacterMotorDB.paused)
        {
            //if(swayStartTime > Time.time)
            //	swayStartTime = Time.time;
            this.WalkSway();
            this.idleTime = 0;
        }
        else
        {
            //swayStartTime = 999999999999999999;
            this.ResetPosition();
        }
        if ((this.chargeLevel > 0) && this.chargeWeapon)
        {
            if ((this.GetComponent<AudioSource>().clip != this.chargeLoop) || !this.GetComponent<AudioSource>().isPlaying)
            {
                this.GetComponent<AudioSource>().clip = this.chargeLoop;
                this.GetComponent<AudioSource>().loop = true;
                this.GetComponent<AudioSource>().Play();
            }
        }
        else
        {
            if (this.GetComponent<AudioSource>().clip == this.chargeLoop)
            {
                this.GetComponent<AudioSource>().Stop();
            }
        }
        // We shot this frame, enable the muzzle flash
        if (this.m_LastFrameShot == Time.frameCount)
        {
        }
        else
        {
            // Play sound
            if (this.GetComponent<AudioSource>())
            {
                this.GetComponent<AudioSource>().loop = false;
            }
        }
    }

    public virtual void FireAlt()
    {
        if (!this.isPrimaryWeapon)
        {
            this.AlignToSharedAmmo();
            this.gunActive = true;
            this.StartCoroutine(this.Fire());
            this.gunActive = false;
        }
    }

    public virtual void AlignToSharedAmmo()
    {
        if (this.sharesAmmo)
        {
            this.clips = this.ammoManagerScript.clipsArray[this.ammoSetUsed];
            this.maxClips = this.ammoManagerScript.maxClipsArray[this.ammoSetUsed];
            this.infiniteAmmo = this.ammoManagerScript.infiniteArray[this.ammoSetUsed];
            this.SendMessage("UpdateAmmo", this.ammoLeft, SendMessageOptions.DontRequireReceiver);
            this.SendMessage("UpdateClips", this.clips, SendMessageOptions.DontRequireReceiver);
        }
    }

    public virtual void ApplyToSharedAmmo()
    {
        if (this.sharesAmmo)
        {
            this.ammoManagerScript.clipsArray[this.ammoSetUsed] = this.clips;
            this.ammoManagerScript.maxClipsArray[this.ammoSetUsed] = this.maxClips;
            this.ammoManagerScript.infiniteArray[this.ammoSetUsed] = this.infiniteAmmo;
        }
    }

    public virtual void Fire2()
    {
        if (((this.isPrimaryWeapon && (this.secondaryWeapon != null)) && this.gunActive) && this.secondaryFire)
        {
            this.ApplyToSharedAmmo();
            this.secondaryWeapon.FireAlt();
        }
    }

    public virtual IEnumerator Fire()
    {
        this.idleTime = 0;
        if (((!this.gunActive || this.aim1.sprinting) || this.inDelay) || LockCursor.unPaused)
        {
            if ((gunTypes.spray != gunTypes.hitscan) && this.sprayOn)
            {
                if (this.GetComponent<AudioSource>())
                {
                    if (this.GetComponent<AudioSource>().clip == this.loopSound)
                    {
                        this.GetComponent<AudioSource>().Stop();
                    }
                    this.sprayOn = false;
                    this.sprayScript.ToggleActive(false);
                }
            }
            yield break;
        }
        //Melee attack
        if ((this.gunType == gunTypes.melee) && (this.nextFireTime < Time.time))
        {
            this.BroadcastMessage("FireMelee", this.delay, SendMessageOptions.DontRequireReceiver);
            this.nextFireTime = Time.time + this.fireRate;
            this.inDelay = true;
            this.hitBox = true;
            this.GetComponent<AudioSource>().clip = this.fireSound;
            this.GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(this.delay);
            this.inDelay = false;
            if (this.reloadTime > 0)
            {
                this.BroadcastMessage("ReloadMelee", this.reloadTime, SendMessageOptions.DontRequireReceiver);
            }
            this.hitBox = false;
            yield break;
        }
        //Prog reload cancel
        if (this.progressiveReloading && (this.ammoLeft > 0))
        {
            this.stopReload = true;
        }
        int b = 1; //variable to control burst fire
        //Can we fire?
        if (((((this.ammoLeft < this.ammoPerShot) || (this.nextFireTime > Time.time)) || !this.gunActive) || GunScript.reloading) || Avoidance.collided)
        {
            if ((this.ammoLeft < this.ammoPerShot) && !((((this.nextFireTime > Time.time) || !this.gunActive) || GunScript.reloading) || Avoidance.collided))
            {
                if (PlayerWeapons.autoReload && (this.clips > 0))
                {
                    this.StartCoroutine(this.Reload());
                }
                else
                {
                    if (this.isPrimaryWeapon)
                    {
                        this.BroadcastMessage("EmptyFireAnim");
                    }
                    else
                    {
                        this.BroadcastMessage("SecondaryEmptyFireAnim");
                    }
                    this.nextFireTime = Time.time + 0.3f;
                }
                if (!GunScript.reloading)
                {
                    PlayerWeapons.autoFire = false;
                    this.GetComponent<AudioSource>().pitch = this.emptyPitch;
                    this.GetComponent<AudioSource>().volume = this.emptyVolume;
                    this.GetComponent<AudioSource>().clip = this.emptySound;
                    this.GetComponent<AudioSource>().Play();
                }
            }
            if (this.gunType == gunTypes.spray)
            {
                this.sprayOn = false;
                this.sprayScript.ToggleActive(false);
            }
            yield break;
        }
        //KickBack
        this.KickBackZ();
        if (this.gunType != gunTypes.spray)
        {
            //Handle charging
            if (this.chargeWeapon)
            {
                if (((this.chargeLevel < this.maxCharge) && !this.chargeLocked) && this.gunActive)
                {
                    if (((this.ammoPerShot + (this.additionalAmmoPerCharge * this.chargeLevel)) >= this.ammoLeft) && (this.additionalAmmoPerCharge != 0))
                    {
                        this.chargeReleased = true;
                        this.chargeLocked = true;
                    }
                    else
                    {
                        this.chargeLevel = this.chargeLevel + Time.deltaTime;
                    }
                }
                else
                {
                    if (this.forceFire && (this.chargeLocked == false))
                    {
                        this.chargeReleased = true;
                        if (!this.chargeAuto)
                        {
                            this.chargeLocked = true;
                        }
                    }
                }
            }
            //Handle firing
            if (!this.chargeWeapon || (this.chargeWeapon && this.chargeReleased))
            {
                if (this.chargeWeapon)
                {
                    this.chargeReleased = false;
                }
                if (this.burstFire)
                {
                    b = this.burstCount;
                }
                else
                {
                    b = 1;
                }
                int i = 0;
                while (i < b)
                {
                    if (this.ammoLeft >= this.ammoPerShot)
                    {
                        this.StartCoroutine(this.FireShot());
                        if (this.chargeWeapon)
                        {
                            this.ammoLeft = this.ammoLeft - (this.ammoPerShot + Mathf.Floor(this.additionalAmmoPerCharge * this.chargeLevel));
                        }
                        else
                        {
                            this.ammoLeft = this.ammoLeft - this.ammoPerShot;
                        }
                        if (this.fireRate < this.delay)
                        {
                            this.nextFireTime = Time.time + this.delay;
                        }
                        else
                        {
                            this.nextFireTime = Time.time + this.fireRate;
                        }
                        if (((this.secondaryWeapon != null) && this.secondaryFire) && !this.secondaryWeapon.secondaryInterrupt)
                        {
                            if (this.fireRate < this.delay)
                            {
                                this.secondaryWeapon.nextFireTime = Time.time + this.delay;
                            }
                            else
                            {
                                this.secondaryWeapon.nextFireTime = Time.time + this.fireRate;
                            }
                        }
                        else
                        {
                            if ((this.secondaryFire && !this.secondaryInterrupt) && !this.isPrimaryWeapon)
                            {
                                if (this.fireRate < this.delay)
                                {
                                    this.primaryWeapon.nextFireTime = Time.time + this.delay;
                                }
                                else
                                {
                                    this.primaryWeapon.nextFireTime = Time.time + this.fireRate;
                                }
                            }
                        }
                        if (this.burstFire && (i < (b - 1)))
                        {
                            if ((this.burstTime / this.burstCount) < this.delay)
                            {
                                yield return new WaitForSeconds(this.delay);
                            }
                            else
                            {
                                yield return new WaitForSeconds(this.burstTime / this.burstCount);
                            }
                        }
                    }
                    i++;
                }
            }
        }
        else
        {
            if (this.gunType == gunTypes.spray)
            {
                this.FireSpray();
            }
        }
        this.ApplyToSharedAmmo();
        this.SendMessage("UpdateAmmo", this.ammoLeft, SendMessageOptions.DontRequireReceiver);
        this.SendMessage("UpdateClips", this.clips, SendMessageOptions.DontRequireReceiver);
        if ((this.ammoLeft <= 0) && PlayerWeapons.autoReload)
        {
            if (this.fireRate < this.delay)
            {
                yield return new WaitForSeconds(this.delay);
            }
            else
            {
                yield return new WaitForSeconds(this.fireRate);
            }
            this.StartCoroutine(this.Reload());
        }
    }

    //Kickback function which moves the gun transform backwards when called
    public virtual void KickBackZ()
    {
        if (!this.useZKickBack)
        {
            return;
        }
        float amt = Time.deltaTime * this.kickBackZ;
        amt = Mathf.Min(amt, this.maxZ - this.totalKickBack);
        this.transform.localPosition.z = this.transform.localPosition.z - amt;
        this.totalKickBack = this.totalKickBack + amt;
    }

    //Reset Kickback function which moves the gun transform forwards when called
    public virtual void ReturnKickBackZ()
    {
        float amt = Time.deltaTime * this.zRetRate;
        amt = Mathf.Min(amt, this.totalKickBack);
        this.transform.localPosition.z = this.transform.localPosition.z + amt;
        this.totalKickBack = this.totalKickBack - amt;
    }

    public virtual IEnumerator FireShot()
    {
        if (this.isPrimaryWeapon)
        {
            if ((this.fireAnim && !this.autoFire) && !this.burstFire)
            {
                this.BroadcastMessage("SingleFireAnim", this.fireRate, SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                this.BroadcastMessage("FireAnim", SendMessageOptions.DontRequireReceiver);
            }
        }
        else
        {
            if ((this.fireAnim && !this.autoFire) && !this.burstFire)
            {
                this.BroadcastMessage("SingleSecFireAnim", this.fireRate, SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                this.BroadcastMessage("SecondaryFireAnim", SendMessageOptions.DontRequireReceiver);
            }
        }
        if (this.shellEjection && !this.aim1.inScope)
        {
            this.StartCoroutine(this.EjectShell());
        }
        if (this.gunType == gunTypes.hitscan)
        {
            this.inDelay = true;
            yield return new WaitForSeconds(this.delay);
            this.inDelay = false;
            int i = 0;
            while (i < this.shotCount)
            {
                this.FireOneBullet();
                this.Kickback();
                i++;
            }
        }
        else
        {
            if (this.gunType == gunTypes.launcher)
            {
                this.inDelay = true;
                yield return new WaitForSeconds(this.delay);
                this.inDelay = false;
                int p = 0;
                while (p < this.projectileCount)
                {
                    this.FireOneProjectile();
                    p++;
                }
            }
        }
        this.m_LastFrameShot = Time.frameCount;
        this.shotSpread = Mathf.Clamp(this.shotSpread + this.spreadRate, 0, this.maxSpread);
        this.chargeLevel = 0;
        this.FireEffects();
    }

    public virtual void FireOneProjectile()
    {
        Vector3 direction = this.SprayDirection();
        Quaternion convert = Quaternion.LookRotation(direction);
        /*var layer1 = 1 << PlayerWeapons.playerLayer;
	var layer2 = 1 << 2;
  	var layerMask = layer1 | layer2;
  	layerMask = ~layerMask;*/
        Rigidbody instantiatedProjectile = null;
        Transform launchPos = null;
        //if(launchPosition != null && !Physics.Linecast(launchPosition.transform.position, weaponCam.transform.position, ~(PlayerWeapons.PW.RaycastsIgnore.value))){
        //	launchPos = launchPosition.transform;
        //} else {
        //	launchPos = weaponCam.transform;
        //}
        if (this.launchPosition != null)
        {
            launchPos = this.launchPosition.transform;
            instantiatedProjectile = UnityEngine.Object.Instantiate(this.projectile, launchPos.position, convert);
            instantiatedProjectile.velocity = instantiatedProjectile.transform.TransformDirection(new Vector3(0, 0, this.initialSpeed));
            instantiatedProjectile.transform.rotation = launchPos.transform.rotation;
            Physics.IgnoreCollision(instantiatedProjectile.GetComponent<Collider>(), this.transform.root.GetComponentInChildren<Collider>());
            Physics.IgnoreCollision(instantiatedProjectile.GetComponent<Collider>(), this.player.transform.GetComponentInChildren<Collider>());
            instantiatedProjectile.BroadcastMessage("ChargeLevel", this.chargeLevel, SendMessageOptions.DontRequireReceiver);
            this.Kickback();
        }
    }

    public virtual void FireOneBullet()
    {
        float hitDist = 0.0f;
        bool penetrate = true;
        int pVal = this.penetrateVal;
        /*var layer1 = 1 << PlayerWeapons.playerLayer;
	var layer2 = 1 << 2;
  	var layerMask = layer1 | layer2;
  	layerMask = ~layerMask;*/
        RaycastHit[] hits = null;
        //var direction = SprayDirection();
        if (!GunScript.mainCam)
        {
            GunScript.mainCam = PlayerWeapons.mainCam;
        }
        Camera camera = null;
        camera = GunScript.mainCam.GetComponent<Camera>();
        if (camera != null)
        {
            GunScript.mainCam = camera.gameObject;
        }
        Ray ray = GunScript.mainCam.GetComponent<Camera>().ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        ray.direction = this.SprayDirection();
        hits = Physics.RaycastAll(ray, this.range, ~PlayerWeapons.PW.RaycastsIgnore.value);
        float Speed = 100f;
        float dist = 1000;
        //Tracer
        this.shotCountTracer = this.shotCountTracer + 1;
        if (((this.tracer != null) && (this.traceEvery <= this.shotCountTracer)) && (this.traceEvery != 0))
        {
            ParticleSystem emitter = (ParticleSystem) this.tracer.GetComponent(typeof(ParticleSystem));
            this.shotCountTracer = 0;
            if (hits.Length > 0)
            {
                if (Vector3.Distance(hits[0].point, this.transform.position) >= this.minDistForTracer)
                {
                    this.tracer.transform.LookAt(hits[0].point);
                    this.tracer.transform.rotation = Quaternion.LookRotation(ray.direction);//(transform.position + 90 * direction));
                    emitter.startSpeed = Speed;
                    emitter.startLifetime = dist / Speed;
                    emitter.Play();
                }
            }
        }
        //else{
        //tracer.transform.rotation = Quaternion.LookRotation(ray.direction);//(transform.position + 90 * direction));
        //emitter.Play();//.Emit();
        //emitter.Simulate(simulateTime);
         //emitter.startSpeed = Speed;
         //emitter.startLifetime = dist / Speed;
        //}
        System.Array.Sort(hits, this.Comparison);
        //	 Did we hit anything?
        int i = 0;
        while (i < hits.Length)
        {
            RaycastHit hit = hits[i];
            BulletPenetration BP = (BulletPenetration) hit.transform.GetComponent(typeof(BulletPenetration));
            if (penetrate && !hit.collider.isTrigger)
            {
                if (BP == null)
                {
                    penetrate = false;
                }
                else
                {
                    if (pVal < BP.penetrateValue)
                    {
                        penetrate = false;
                    }
                    else
                    {
                        pVal = pVal - BP.penetrateValue;
                    }
                }
                //Apply charge if applicable
                float chargedDamage = this.damage;
                if (this.chargeWeapon)
                {
                    chargedDamage = this.damage * Mathf.Pow(this.chargeCoefficient, this.chargeLevel);
                }
                //Calculate damage falloff
                float finalDamage = chargedDamage;
                if (this.hasFalloff)
                {
                    hitDist = Vector3.Distance(hit.transform.position, this.transform.position);
                    if (hitDist > this.maxFalloffDist)
                    {
                        finalDamage = chargedDamage * Mathf.Pow(this.falloffCoefficient, (this.maxFalloffDist - this.minFalloffDist) / this.falloffDistanceScale);
                    }
                    else
                    {
                        if ((hitDist < this.maxFalloffDist) && (hitDist > this.minFalloffDist))
                        {
                            finalDamage = chargedDamage * Mathf.Pow(this.falloffCoefficient, (hitDist - this.minFalloffDist) / this.falloffDistanceScale);
                        }
                    }
                }
                // Send a damage message to the hit object
                /*var sendArray : Object[] = new Object[2];
			sendArray[0] = finalDamage;
			sendArray[1] = true;	*/
                hit.collider.SendMessageUpwards("ApplyDamagePlayer", finalDamage, SendMessageOptions.DontRequireReceiver);
                //hit.collider.SendMessageUpwards("Accuracy", SendMessageOptions.DontRequireReceiver);
                //And send a message to the decal manager, if the target uses decals
                if ((UseEffects) hit.transform.gameObject.GetComponent(typeof(UseEffects)))
                {
                    //The effectsManager needs five bits of information
                    Quaternion hitRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                    int hitSet = ((UseEffects) hit.transform.gameObject.GetComponent(typeof(UseEffects))).setIndex;
                    object[] hitInfo = hit.point;
                    this.effectsManager.SendMessage("ApplyDecal", hitInfo, SendMessageOptions.DontRequireReceiver);
                }
                //Calculate force falloff
                float finalForce = this.force;
                if (this.hasFalloff)
                {
                    if (hitDist > this.maxFalloffDist)
                    {
                        finalForce = finalForce * Mathf.Pow(this.forceFalloffCoefficient, (this.maxFalloffDist - this.minFalloffDist) / this.falloffDistanceScale);
                    }
                    else
                    {
                        if ((hitDist < this.maxFalloffDist) && (hitDist > this.minFalloffDist))
                        {
                            finalForce = finalForce * Mathf.Pow(this.forceFalloffCoefficient, (hitDist - this.minFalloffDist) / this.falloffDistanceScale);
                        }
                    }
                }
                // Apply a force to the rigidbody we hit
                if (hit.rigidbody)
                {
                    hit.rigidbody.AddForceAtPosition(finalForce * ray.direction, hit.point);
                }
            }
            i++;
        }
    }

    public virtual void FireSpray()
    {
        if (!this.sprayOn)
        {
            this.sprayOn = true;
            this.sprayScript.ToggleActive(true);
            this.GetComponent<AudioSource>().clip = this.fireSound;
            this.GetComponent<AudioSource>().Play();
        }
        if (((this.GetComponent<AudioSource>().clip == this.loopSound) && this.GetComponent<AudioSource>().isPlaying) && AimMode.sprintingPublic)
        {
            this.GetComponent<AudioSource>().Stop();
        }
        else
        {
            if ((this.GetComponent<AudioSource>() && !this.GetComponent<AudioSource>().isPlaying) && !AimMode.sprintingPublic)
            {
                this.GetComponent<AudioSource>().clip = this.loopSound;
                this.GetComponent<AudioSource>().loop = true;
                this.GetComponent<AudioSource>().Play();
            }
        }
        if (this.tempAmmo <= 0)
        {
            this.tempAmmo = 1;
            this.ammoLeft = this.ammoLeft - this.ammoPerShot;
        }
        else
        {
            this.tempAmmo = this.tempAmmo - (Time.deltaTime * this.deltaTimeCoefficient);
        }
    }

    public virtual void ReleaseFire(int key)
    {
        if (this.GetComponent<AudioSource>())
        {
            if (this.GetComponent<AudioSource>().isPlaying && (this.GetComponent<AudioSource>().clip == this.chargeLoop))
            {
                this.GetComponent<AudioSource>().Stop();
            }
        }
        if (this.sprayOn)
        {
            this.sprayScript.ToggleActive(false);
            this.sprayOn = false;
            if (this.GetComponent<AudioSource>())
            {
                this.GetComponent<AudioSource>().clip = this.releaseSound;
                this.GetComponent<AudioSource>().loop = false;
                this.GetComponent<AudioSource>().Play();
            }
        }
        if (this.chargeWeapon)
        {
            if (this.chargeLocked)
            {
                this.chargeLocked = false;
                this.chargeLevel = 0;
            }
            else
            {
                if (this.chargeLevel > this.minCharge)
                {
                    this.chargeReleased = true;
                    this.StartCoroutine(this.Fire());
                }
                else
                {
                    this.chargeLevel = 0;
                }
            }
        }
    }

    public virtual int Comparison(RaycastHit x, RaycastHit y)
    {
        return Mathf.Sign(x.distance - y.distance);
    }

    public virtual Vector3 SprayDirection()
    {
        float vx = (1 - (2 * Random.value)) * this.actualSpread;
        float vy = (1 - (2 * Random.value)) * this.actualSpread;
        float vz = 1f;
        return GunScript.weaponCam.transform.TransformDirection(new Vector3(vx, vy, vz));
    }

    public virtual Vector3 SprayDirection(Vector3 dir)
    {
        float vx = (1 - (2 * Random.value)) * this.actualSpread;
        float vy = (1 - (2 * Random.value)) * this.actualSpread;
        float vz = (1 - (2 * Random.value)) * this.actualSpread;
        return dir + new Vector3(vx, vy, vz);
    }

    public virtual IEnumerator Reload()
    {
        bool tempEmpty = false;
        if ((((this.ammoLeft >= this.ammoPerClip) || (this.clips <= 0)) || !this.gunActive) || Avoidance.collided)
        {
            yield break;
        }
        this.reloadCancel = false;
        this.idleTime = 0;
        this.aim1.canSprint = PlayerWeapons.PW.reloadWhileSprinting;
        if (this.progressiveReload)
        {
            this.StartCoroutine(this.ProgReload());
            yield break;
        }
        if (GunScript.reloading)
        {
            yield break;
        }
        //aim1.canSwitchWeaponAim = false;
        if (this.aim1.canAim)
        {
            this.aim1.canAim = false;
            this.aim = true;
        }
        if (this.gunType == gunTypes.spray)
        {
            if (this.GetComponent<AudioSource>())
            {
                if ((this.GetComponent<AudioSource>().clip == this.loopSound) && this.GetComponent<AudioSource>().isPlaying)
                {
                    this.GetComponent<AudioSource>().Stop();
                }
            }
        }
        GunScript.reloading = true;
        if (this.secondaryWeapon != null)
        {
            GunScript.reloading = true;
        }
        else
        {
            if (!this.isPrimaryWeapon)
            {
                GunScript.reloading = true;
            }
        }
        yield return new WaitForSeconds(this.waitforReload);
        if (this.reloadCancel)
        {
            yield break;
        }
        if (this.isPrimaryWeapon)
        {
            this.BroadcastMessage("ReloadAnimEarly", SendMessageOptions.DontRequireReceiver);
            if (this.ammoLeft >= this.ammoPerShot)
            {
                tempEmpty = false;
                this.BroadcastMessage("ReloadAnim", this.reloadTime, SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                tempEmpty = true;
                this.BroadcastMessage("ReloadEmpty", this.emptyReloadTime, SendMessageOptions.DontRequireReceiver);
            }
        }
        else
        {
            this.BroadcastMessage("SecondaryReloadAnimEarly", SendMessageOptions.DontRequireReceiver);
            if (this.ammoLeft >= this.ammoPerShot)
            {
                tempEmpty = false;
                this.BroadcastMessage("SecondaryReloadAnim", this.reloadTime, SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                tempEmpty = true;
                this.BroadcastMessage("SecondaryReloadEmpty", this.emptyReloadTime, SendMessageOptions.DontRequireReceiver);
            }
        }
        // Wait for reload time first - then add more bullets!
        if (this.ammoLeft > this.ammoPerShot)
        {
            yield return new WaitForSeconds(this.reloadTime);
        }
        else
        {
            yield return new WaitForSeconds(this.emptyReloadTime);
        }
        if (this.reloadCancel)
        {
            yield break;
        }
        GunScript.reloading = false;
        if (this.secondaryWeapon != null)
        {
            GunScript.reloading = false;
        }
        else
        {
            if (!this.isPrimaryWeapon)
            {
                GunScript.reloading = false;
            }
        }
        // We have a clip left reload
        if (this.ammoType == ammoTypes.byClip)
        {
            if (this.clips > 0)
            {
                if (!this.infiniteAmmo)
                {
                    this.clips--;
                }
                this.ammoLeft = this.ammoPerClip;
            }
        }
        else
        {
            if (this.ammoType == ammoTypes.byBullet)
            {
                if (this.clips > 0)
                {
                    if (this.clips > this.ammoPerClip)
                    {
                        if (!this.infiniteAmmo)
                        {
                            this.clips = this.clips - (this.ammoPerClip - this.ammoLeft);
                        }
                        this.ammoLeft = this.ammoPerClip;
                    }
                    else
                    {
                        float ammoVal = Mathf.Clamp(this.ammoPerClip, this.clips, this.ammoLeft + this.clips);
                        if (!this.infiniteAmmo)
                        {
                            this.clips = this.clips - (ammoVal - this.ammoLeft);
                        }
                        this.ammoLeft = ammoVal;
                    }
                }
            }
        }
        if (!tempEmpty && this.addOneBullet)
        {
            if ((this.ammoType == ammoTypes.byBullet) && (this.clips > 0))
            {
                this.ammoLeft = this.ammoLeft + 1;
                this.clips = this.clips - 1;
            }
        }
        if (this.aim)
        {
            this.aim1.canAim = true;
        }
        this.aim1.canSprint = true;
        //aim1.canSwitchWeaponAim = true;
        this.SendMessage("UpdateAmmo", this.ammoLeft, SendMessageOptions.DontRequireReceiver);
        this.SendMessage("UpdateClips", this.clips, SendMessageOptions.DontRequireReceiver);
        this.ApplyToSharedAmmo();
        PlayerWeapons.autoFire = this.autoFire;
    }

    public virtual void StopReloading()
    {
        GunScript.reloading = false;
        if (this.secondaryWeapon != null)
        {
            GunScript.reloading = false;
        }
        else
        {
            if (!this.isPrimaryWeapon)
            {
                GunScript.reloading = false;
            }
        }
        this.progressiveReloading = false;
        this.aim1.canSprint = true;
        PlayerWeapons.autoFire = this.autoFire;
        if (this.aim)
        {
            this.aim1.canAim = true;
        }
    }

    public virtual IEnumerator ProgReload()
    {
        if (GunScript.reloading)
        {
            yield break;
        }
        //aim1.canSwitchWeaponAim = false;
        if (this.aim1.canAim)
        {
            this.aim1.canAim = false;
            this.aim = true;
        }
        this.BroadcastMessage("ReloadIn", this.reloadInTime);
        yield return new WaitForSeconds(this.reloadInTime);
        if (this.reloadCancel)
        {
            yield break;
        }
        if (this.progressiveReset)
        {
            this.clips = this.clips + this.ammoLeft;
            this.ammoLeft = 0;
        }
        this.progressiveReloading = true;
        GunScript.reloading = true;
        if (((this.secondaryWeapon != null) && this.secondaryFire) && !this.secondaryWeapon.secondaryInterrupt)
        {
            GunScript.reloading = true;
        }
        else
        {
            if ((this.secondaryFire && !this.secondaryInterrupt) && !this.isPrimaryWeapon)
            {
                GunScript.reloading = false;
            }
        }
    }

    public virtual float GetBulletsLeft()
    {
        return this.ammoLeft;
    }

    public virtual IEnumerator SelectWeapon()
    {
        this.AlignToSharedAmmo();
        this.idleTime = 0;
        if (!this.isPrimaryWeapon || GunScript.puttingAway)
        {
            yield break;
        }
        if (!GunScript.mainCam)
        {
            GunScript.mainCam = PlayerWeapons.mainCam;
        }
        this.SetCrosshair();
        if (this.overrideAvoidance)
        {
            Avoidance.SetValues(this.rot, this.pos, this.dist, this.minDist, this.avoids);
        }
        else
        {
            Avoidance.SetValues();
        }
        if (this.secondaryWeapon != null)
        {
            this.secondaryWeapon.gunActive = false;
            this.secondaryWeapon.secondaryActive = false;
            this.BroadcastMessage("AimPrimary", SendMessageOptions.DontRequireReceiver);
        }
        if (!GunScript.takingOut && !this.gunActive)
        {
            Component[] gos = this.GetComponentsInChildren(typeof(Renderer));
            foreach (Renderer go in gos)
            {
                go.enabled = true;
            }
            this.wepDis.enabled = true;
            AimMode.canSwitchWeaponAim = false;
            //BroadcastMessage("TakeOutAnim", takeOutTime, SendMessageOptions.DontRequireReceiver);
            //mainCam.SendMessage("TakeOutAnim", takeOutTime, SendMessageOptions.DontRequireReceiver);
            GunScript.takingOut = true;
            this.interruptPutAway = true;
            yield return new WaitForSeconds(this.takeOutTime);
            if (GunScript.puttingAway)
            {
                yield break;
            }
            //	return;
            SmartCrosshair.crosshair = true;
            this.gunActive = true;
            GunScript.takingOut = false;
            AimMode.canSwitchWeaponAim = true;
            this.ammo.enabled = true;
            this.sprint.enabled = true;
            this.wepDis.Select();
            this.SendMessage("UpdateAmmo", this.ammoLeft, SendMessageOptions.DontRequireReceiver);
            this.SendMessage("UpdateClips", this.clips, SendMessageOptions.DontRequireReceiver);
            this.NormalSpeed();
            if (gos.Length > 0)
            {
                if (gos[0].GetComponent<Renderer>().enabled == false)
                {
                    foreach (Renderer go in gos)
                    {
                        go.enabled = true;
                    }
                }
            }
            if ((PlayerWeapons.autoReload && (this.ammoLeft <= 0)) && (this.gunType != gunTypes.melee))
            {
                this.StartCoroutine(this.Reload());
            }
        }
    }

    public virtual IEnumerator DeselectWeapon()
    {
        if (this.GetComponent<AudioSource>())
        {
            if ((this.GetComponent<AudioSource>().clip == this.loopSound) && this.GetComponent<AudioSource>().isPlaying)
            {
                this.GetComponent<AudioSource>().Stop();
            }
        }
        this.chargeLevel = 0;
        this.reloadCancel = true;
        GunScript.reloading = false;
        if (!this.gunActive)
        {
            yield break;
        }
        this.StopReloading();
        this.interruptPutAway = false;
        GunScript.puttingAway = true;
        GunScript.takingOut = false;
        this.ammo.enabled = false;
        this.sprint.enabled = false;
        this.wepDis.enabled = false;
        AimMode.canSwitchWeaponAim = false;
        //BroadcastMessage("PutAwayAnim", putAwayTime, SendMessageOptions.DontRequireReceiver);
        //mainCam.SendMessage("PutAwayAnim", putAwayTime, SendMessageOptions.DontRequireReceiver);
        this.gunActive = false;
        SmartCrosshair.crosshair = false;
        yield return new WaitForSeconds(this.putAwayTime);
        GunScript.puttingAway = false;
        /*if(takingOut || interruptPutAway){
		return;		
	}*/
        this.SendMessageUpwards("ActivateWeapon");
        Component[] gos = this.GetComponentsInChildren(typeof(Renderer));
        foreach (Renderer go in gos)
        {
            go.enabled = false;
        }
    }

    public virtual void DeselectInstant()
    {
        if (this.GetComponent<AudioSource>())
        {
            if ((this.GetComponent<AudioSource>().clip == this.loopSound) && this.GetComponent<AudioSource>().isPlaying)
            {
                this.GetComponent<AudioSource>().Stop();
            }
        }
        this.chargeLevel = 0;
        if (!this.gunActive)
        {
            return;
        }
        GunScript.takingOut = false;
        this.ammo.enabled = false;
        this.sprint.enabled = false;
        this.wepDis.enabled = false;
        this.gunActive = false;
        SmartCrosshair.crosshair = false;
        //SendMessageUpwards("ActivateWeapon");
        Component[] gos = this.GetComponentsInChildren(typeof(Renderer));
        foreach (Renderer go in gos)
        {
            go.enabled = false;
        }
        this.BroadcastMessage("DeselectWeapon");
    }

    public virtual void EditorSelect()
    {
        this.gunActive = true;
        Component[] gos = this.GetComponentsInChildren(typeof(Renderer));
        foreach (Renderer go in gos)
        {
            go.enabled = true;
        }
    }

    public virtual void EditorDeselect()
    {
        this.gunActive = false;
        Component[] gos = this.GetComponentsInChildren(typeof(Renderer));
        foreach (Renderer go in gos)
        {
            go.enabled = false;
        }
    }

    public virtual void WalkSway()
    {
        Vector3 curVect = default(Vector3);
        int speed = ((CharacterController) this.CM.GetComponent(typeof(CharacterController))).velocity.magnitude;
        if (speed < 0.2f)
        {
            this.ResetPosition();
            return;
        }
        if (!this.sway || !this.gunActive)
        {
            return;
        }
        //sine function for motion
        float t = Time.time - CamSway.singleton.swayStartTime;
        /*if(CM.crouching){
		swayRate = moveSwayRate*CM.movement.maxCrouchSpeed/CM.movement.defaultForwardSpeed;
	} else if (CM.prone) {
		swayRate = moveSwayRate*CM.movement.maxProneSpeed/CM.movement.defaultForwardSpeed;
	} else if (AimMode.sprintingPublic) {
		swayRate = runSwayRate;
	} else {
		swayRate = moveSwayRate;
	}*/
        curVect.x = (this.swayAmplitude.x * Mathf.Sin((this.swayRate.x * t) + (this.idleSwayRate.x / 2))) * Mathf.Sin((this.swayRate.x * t) + (this.idleSwayRate.x / 2));
        curVect.y = Mathf.Abs(this.swayAmplitude.y * Mathf.Sin((this.swayRate.y * t) + (this.idleSwayRate.y / 2)));
        curVect.x = curVect.x - (this.swayAmplitude.x / 2);
        curVect.y = curVect.y - (this.swayAmplitude.y / 2);
        //offset from start position
        curVect = curVect + this.startPosition;
        float s = new Vector3(PlayerWeapons.CM.movement.velocity.x, 0, PlayerWeapons.CM.movement.velocity.z).magnitude / 14;
        //move towards target
        this.transform.localPosition.x = Mathf.Lerp(this.transform.localPosition.x, curVect.x, (Time.deltaTime * this.swayRate.x) * s);
        this.transform.localEulerAngles.z = Mathf.LerpAngle(this.transform.localEulerAngles.z, -curVect.x, Time.deltaTime * s);
        this.transform.localPosition.y = Mathf.Lerp(this.transform.localPosition.y, curVect.y, (Time.deltaTime * this.swayRate.y) * s);
        this.transform.localEulerAngles.x = Mathf.LerpAngle(this.transform.localEulerAngles.x, -curVect.y, (Time.deltaTime * this.swayRate.y) * s);
    }

    public virtual void ResetPosition()
    {
        Vector3 curVect = default(Vector3);
        if (((this.transform.localPosition == this.startPosition) && !this.sway) || !this.gunActive)
        {
            return;
        }
        float rate = 0.15f * Time.deltaTime;
        if (this.sway && !this.aim1.aiming)
        {
            //sine function for idle motion
            curVect.x = this.idleAmplitude.x * Mathf.Sin(this.idleSwayRate.x * Time.time);
            curVect.y = this.idleAmplitude.y * Mathf.Sin(this.idleSwayRate.y * Time.time);
            curVect.x = curVect.x - (this.idleAmplitude.x / 2);
            curVect.y = curVect.y - (this.idleAmplitude.y / 2);
            //offset from start position
            curVect = curVect + this.startPosition;
        }
        else
        {
            curVect = this.startPosition;
        }
        //move towards target
        if (this.CM != null)
        {
            this.transform.localPosition.x = Mathf.Lerp(this.transform.localPosition.x, curVect.x, Time.deltaTime * this.swayRate.x);
            this.transform.localEulerAngles.z = Mathf.LerpAngle(this.transform.localEulerAngles.z, curVect.x, Time.deltaTime * this.swayRate.x);
            this.transform.localPosition.y = Mathf.Lerp(this.transform.localPosition.y, curVect.y, Time.deltaTime * this.swayRate.y);
            this.transform.localEulerAngles.x = Mathf.LerpAngle(this.transform.localEulerAngles.x, curVect.y, Time.deltaTime * this.swayRate.y);
        }
    }

    public virtual void Sprinting()
    {
        if (!this.gunActive)
        {
            return;
        }
        this.idleTime = 0;
        PlayerWeapons.sprinting = true;
        this.swayRate = this.runSwayRate;
        this.swayAmplitude = this.runAmplitude;
        //Only affects charge weapons
        if (this.chargeWeapon)
        {
            this.chargeLocked = true;
            this.chargeLevel = 0;
        }
    }

    public virtual void NormalSpeed()
    {
        if (this.airborne)
        {
            return;
        }
        PlayerWeapons.sprinting = false;
        if (this.secondaryWeapon != null)
        {
            if (this.isPrimaryWeapon && this.secondaryWeapon.secondaryActive)
            {
                return;
            }
        }
        if (!this.isPrimaryWeapon && !this.secondaryActive)
        {
            return;
        }
        this.swayRate = this.moveSwayRate;
        if (CharacterMotorDB.crouching)
        {
            this.swayRate = (this.moveSwayRate * this.CM.movement.maxCrouchSpeed) / this.CM.movement.defaultForwardSpeed;
        }
        else
        {
            if (CharacterMotorDB.prone)
            {
                this.swayRate = (this.moveSwayRate * this.CM.movement.maxProneSpeed) / this.CM.movement.defaultForwardSpeed;
            }
        }
        this.swayAmplitude = this.moveSwayAmplitude;
        //gunActive = true;
        //Only affects charge weapons
        if (this.chargeWeapon)
        {
            this.chargeLocked = false;
            this.chargeLevel = 0;
        }
    }

    public virtual void Kickback()
    {
        if ((this.mouseX != null) && (this.mouseY != null))
        {
            this.mouseY.offsetY = this.curKickback;
            this.mouseY.maxKickback = this.maxKickback;
            this.mouseX.offsetX = this.curKickback * this.xKickbackFactor;//*Random.value;
            this.mouseX.maxKickback = this.maxKickback;
            if (this.mouseY.offsetY < this.mouseY.maxKickback)
            {
                this.mouseY.resetDelay = this.recoilDelay;
            }
            if (Mathf.Abs(this.mouseX.offsetX) < this.mouseX.maxKickback)
            {
                this.mouseX.resetDelay = this.recoilDelay;
            }
        }
    }

    public virtual IEnumerator ActivateSecondary()
    {
        if (((this.secondaryWeapon == null) || this.secondaryFire) || GunScript.reloading)
        {
            yield break;
        }
        this.AlignToSharedAmmo();
        if (this.gunActive)
        {
            SmartCrosshair.crosshair = false;
            this.gunActive = false;
            this.BroadcastMessage("EnterSecondary", this.enterSecondaryTime);
            yield return new WaitForSeconds(this.enterSecondaryTime);
            SmartCrosshair.crosshair = true;
            this.secondaryWeapon.gunActive = true;
            this.secondaryActive = true;
            this.secondaryWeapon.SetCrosshair();
            this.SendMessage("UpdateAmmo", this.secondaryWeapon.ammoLeft, SendMessageOptions.DontRequireReceiver);
            this.SendMessage("UpdateClips", this.secondaryWeapon.clips, SendMessageOptions.DontRequireReceiver);
            this.BroadcastMessage("AimSecondary", SendMessageOptions.DontRequireReceiver);
        }
    }

    public virtual void SetCrosshair()
    {
        if (this.crosshairObj != null)
        {
            ((SmartCrosshair) GunScript.weaponCam.GetComponent(typeof(SmartCrosshair))).SetCrosshair();
            SmartCrosshair.cObj = this.crosshairObj;
            SmartCrosshair.cSize = this.crosshairSize;
            SmartCrosshair.scl = this.scale;
            SmartCrosshair.sclRef = this.maxSpread;
            SmartCrosshair.ownTexture = true;
        }
        else
        {
            this.SendMessageUpwards("DefaultCrosshair");
        }
    }

    public virtual IEnumerator ActivatePrimary()
    {
        this.AlignToSharedAmmo();
        if (GunScript.reloading)
        {
            yield break;
        }
        if (!this.gunActive)
        {
            this.secondaryWeapon.gunActive = false;
            this.secondaryActive = false;
            SmartCrosshair.crosshair = false;
            this.BroadcastMessage("ExitSecondary", this.exitSecondaryTime);
            yield return new WaitForSeconds(this.exitSecondaryTime);
            SmartCrosshair.crosshair = true;
            this.gunActive = true;
            this.SetCrosshair();
            this.SendMessage("UpdateAmmo", this.ammoLeft, SendMessageOptions.DontRequireReceiver);
            this.SendMessage("UpdateClips", this.clips, SendMessageOptions.DontRequireReceiver);
            this.BroadcastMessage("AimPrimary", SendMessageOptions.DontRequireReceiver);
        }
    }

    public virtual IEnumerator EjectShell()
    {
        yield return new WaitForSeconds(this.ejectDelay);
        GameObject instantiatedProjectile1 = UnityEngine.Object.Instantiate(this.shell, this.ejectorPosition.transform.position, this.ejectorPosition.transform.rotation);
    }

    public virtual void FireEffects()
    {
        bool scoped = ((AimMode) this.transform.Find("AimObject").GetComponent(typeof(AimMode))).inScope;
        if (!scoped)
        {
            this.BroadcastMessage("MuzzleFlash", this.isPrimaryWeapon, SendMessageOptions.DontRequireReceiver);
        }
        if (this.fireSound == null)
        {
            return;
        }
        //Play Audio
        GameObject audioObj = new GameObject("GunShot");
        audioObj.transform.position = this.transform.position;
        audioObj.transform.parent = this.transform;
        ((TimedObjectDestructorDB) audioObj.AddComponent(typeof(TimedObjectDestructorDB))).timeOut = this.fireSound.length + 0.1f;
        AudioSource aO = (AudioSource) audioObj.AddComponent(typeof(AudioSource));
        aO.clip = this.fireSound;
        aO.volume = this.fireVolume;
        aO.pitch = this.firePitch;
        aO.Play();
        aO.loop = false;
        aO.rolloffMode = AudioRolloffMode.Linear;
    }

    //Returns primary gunscript on this weapon
    public virtual GunScript GetPrimaryGunScript()
    {
        if (this.isPrimaryWeapon)
        {
            return this;
        }
        else
        {
            return this.primaryWeapon;
        }
    }

    public GunScript()
    {
        this.xKickbackFactor = 0.5f;
        this.maxKickback = 15;
        this.crouchKickbackMod = 0.6f;
        this.proneKickbackMod = 0.35f;
        this.moveKickbackMod = 1.3f;
        this.recoilDelay = 0.11f;
        this.standardSpread = 0.1f;
        this.maxSpread = 0.25f;
        this.crouchSpreadModifier = 0.7f;
        this.proneSpreadModifier = 0.4f;
        this.moveSpreadModifier = 1.5f;
        this.standardSpreadRate = 0.05f;
        this.aimSpreadRate = 0.01f;
        this.aimSpread = 0.01f;
        this.spDecRate = 0.05f;
        this.ammoPerClip = 40;
        this.ammoPerShot = 1;
        this.clips = 20;
        this.maxClips = 20;
        this.ammoType = ammoTypes.byClip;
        this.fireVolume = 1;
        this.firePitch = 1;
        this.fireRate = 0.05f;
        this.emptyVolume = 1;
        this.emptyPitch = 1;
        this.burstCount = 1;
        this.burstTime = 0.5f;
        this.reloadTime = 0.5f;
        this.emptyReloadTime = 0.4f;
        this.reloadInTime = 0.5f;
        this.reloadOutTime = 0.5f;
        this.range = 100f;
        this.force = 10f;
        this.damage = 5f;
        this.shotCount = 6;
        this.penetrateVal = 1;
        this.minFalloffDist = 10;
        this.maxFalloffDist = 100;
        this.falloffCoefficient = 1;
        this.falloffDistanceScale = 4;
        this.initialSpeed = 20f;
        this.projectileCount = 1;
        this.simulateTime = 0.02f;
        this.minDistForTracer = 2;
        this.moveSwayRate = new Vector2(2.5f, 5);
        this.moveSwayAmplitude = new Vector2(0.04f, 0.01f);
        this.runSwayRate = new Vector2(4.5f, 0.9f);
        this.runAmplitude = new Vector2(0.04f, 0.04f);
        this.idleSwayRate = new Vector2(2, 1);
        this.idleAmplitude = new Vector2(0.002f, 0.001f);
        this.enterSecondaryTime = 0.5f;
        this.exitSecondaryTime = 0.5f;
        this.maxCharge = 10;
        this.chargeCoefficient = 1.1f;
        this.timeToIdle = 7;
        this.takeOutTime = 0.6f;
        this.putAwayTime = 0.6f;
        this.useZKickBack = true;
        this.kickBackZ = 2;
        this.zRetRate = 1;
        this.maxZ = 0.3f;
        this.avoids = true;
        this.dist = 2;
        this.minDist = 1.5f;
        this.m_LastFrameShot = -1;
        this.spreadRate = 0.05f;
        this.isPrimaryWeapon = true;
        this.tempAmmo = 1;
        this.deltaTimeCoefficient = 1;
        this.forceFalloffCoefficient = 0.99f;
        this.gunType = gunTypes.hitscan;
    }

}