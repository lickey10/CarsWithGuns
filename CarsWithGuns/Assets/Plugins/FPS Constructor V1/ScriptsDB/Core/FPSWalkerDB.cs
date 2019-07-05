using UnityEngine;
using System.Collections;

[System.Serializable]
/*
 FPS Constructor - Weapons
 Copyright© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
// We are grounded, so recalculate movedirection directly from axes
// Apply gravity
// Move the controller
[UnityEngine.RequireComponent(typeof(CharacterController))]
public partial class FPSWalkerDB : MonoBehaviour
{
    public float speed;
    public float aimSpeed;
    public float sprintSpeed;
    private bool canSprint;
    public float sprintJumpSpeed;
    public float normSpeed;
    public float crouchSpeed;
    public float crouchDeltaHeight;
    public float jumpSpeed;
    public float normJumpSpeed;
    public float gravity;
    private GameObject mainCamera;
    private GameObject weaponCamera;
    public static bool crouching;
    private bool stopCrouching;
    private float standardCamHeight;
    private float crouchingCamHeight;
    private Vector3 moveDirection;
    public static bool grounded;
    public static bool walking;
    public static float notWalkingTime;
    private float stopWalkingTime;
    public virtual void Start()
    {
        this.speed = this.normSpeed;
        this.mainCamera = GameObject.FindWithTag("MainCamera");
        this.weaponCamera = GameObject.FindWithTag("WeaponCamera");
        FPSWalkerDB.crouching = false;
        this.standardCamHeight = this.weaponCamera.transform.localPosition.y;
        this.crouchingCamHeight = this.standardCamHeight - this.crouchDeltaHeight;
    }

    public virtual void Update()
    {
        if (!FPSWalkerDB.walking)
        {
            FPSWalkerDB.notWalkingTime = Time.time - this.stopWalkingTime;
        }
        else
        {
            FPSWalkerDB.notWalkingTime = 0;
        }
        if (this.weaponCamera.transform.localPosition.y > this.standardCamHeight)
        {
            this.weaponCamera.transform.localPosition.y = this.standardCamHeight;
        }
        else
        {
            if (this.weaponCamera.transform.localPosition.y < this.crouchingCamHeight)
            {
                this.weaponCamera.transform.localPosition.y = this.crouchingCamHeight;
            }
        }
        if (FPSWalkerDB.grounded)
        {
            if (InputDB.GetButtonDown("Crouch"))
            {
                if (FPSWalkerDB.crouching)
                {
                    this.stopCrouching = true;
                    this.NormalSpeed();
                    return;
                }
                if (!FPSWalkerDB.crouching)
                {
                    this.Crouch();
                }
            }
        }
        if (FPSWalkerDB.crouching)
        {
            if (this.weaponCamera.transform.localPosition.y > this.crouchingCamHeight)
            {
                this.weaponCamera.transform.localPosition.y = Mathf.Clamp(this.weaponCamera.transform.localPosition.y - ((this.crouchDeltaHeight * Time.deltaTime) * 8), this.crouchingCamHeight, this.standardCamHeight);
            }
        }
        else
        {
            if (this.weaponCamera.transform.localPosition.y < this.standardCamHeight)
            {
                this.weaponCamera.transform.localPosition.y = Mathf.Clamp(this.weaponCamera.transform.localPosition.y + ((this.standardCamHeight * Time.deltaTime) * 8), 0, this.standardCamHeight);
            }
        }
    }

    public virtual void FixedUpdate()
    {
        if (FPSWalkerDB.grounded && PlayerWeapons.canMove)
        {
            this.moveDirection = new Vector3(InputDB.GetAxis("Horizontal"), 0, InputDB.GetAxis("Vertical"));
            this.moveDirection = this.transform.TransformDirection(this.moveDirection);
            this.moveDirection = this.moveDirection * this.speed;
            if (InputDB.GetButton("Jump"))
            {
                this.moveDirection.y = this.jumpSpeed;
                if (FPSWalkerDB.crouching)
                {
                    this.stopCrouching = true;
                    this.NormalSpeed();
                }
            }
        }
        this.moveDirection.y = this.moveDirection.y - (this.gravity * Time.deltaTime);
        CharacterController controller = (CharacterController) this.GetComponent(typeof(CharacterController));
        CollisionFlags flags = controller.Move(this.moveDirection * Time.deltaTime);
        FPSWalkerDB.grounded = (flags & CollisionFlags.CollidedBelow) != (CollisionFlags) 0;
        if (((Mathf.Abs(this.moveDirection.x) > 0) && FPSWalkerDB.grounded) || ((Mathf.Abs(this.moveDirection.z) > 0) && FPSWalkerDB.grounded))
        {
            if (!FPSWalkerDB.walking)
            {
                FPSWalkerDB.walking = true;
                this.BroadcastMessage("Walking", SendMessageOptions.DontRequireReceiver);
            }
        }
        else
        {
            if (FPSWalkerDB.walking)
            {
                FPSWalkerDB.walking = false;
                this.stopWalkingTime = Time.time;
                this.BroadcastMessage("StopWalking", SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    public virtual void Aiming()
    {
        this.speed = this.aimSpeed;
    }

    public virtual void Crouch()
    {
        this.speed = this.crouchSpeed;
        ((CharacterController) this.GetComponent(typeof(CharacterController))).height = ((CharacterController) this.GetComponent(typeof(CharacterController))).height - this.crouchDeltaHeight;
        ((CharacterController) this.GetComponent(typeof(CharacterController))).center = ((CharacterController) this.GetComponent(typeof(CharacterController))).center - new Vector3(0, this.crouchDeltaHeight / 2, 0);
        FPSWalkerDB.crouching = true;
    }

    public virtual void NormalSpeed()
    {
        if (this.stopCrouching)
        {
            FPSWalkerDB.crouching = false;
            ((CharacterController) this.GetComponent(typeof(CharacterController))).height = ((CharacterController) this.GetComponent(typeof(CharacterController))).height + this.crouchDeltaHeight;
            this.BroadcastMessage("StopWalking", SendMessageOptions.DontRequireReceiver);
            ((CharacterController) this.GetComponent(typeof(CharacterController))).center = ((CharacterController) this.GetComponent(typeof(CharacterController))).center + new Vector3(0, this.crouchDeltaHeight / 2, 0);
            this.stopCrouching = false;
        }
        else
        {
            if (FPSWalkerDB.crouching)
            {
                this.speed = this.crouchSpeed;
                return;
            }
        }
        this.speed = this.normSpeed;
        this.jumpSpeed = this.normJumpSpeed;
    }

    public virtual void Sprinting()
    {
        if (FPSWalkerDB.crouching)
        {
            FPSWalkerDB.crouching = false;
            ((BoxCollider) this.GetComponent(typeof(BoxCollider))).size = ((BoxCollider) this.GetComponent(typeof(BoxCollider))).size + new Vector3(0, this.crouchDeltaHeight, 0);
            ((BoxCollider) this.GetComponent(typeof(BoxCollider))).center = ((BoxCollider) this.GetComponent(typeof(BoxCollider))).center + new Vector3(0, this.crouchDeltaHeight, 0);
        }
        if (this.canSprint)
        {
            this.speed = this.sprintSpeed;
            this.jumpSpeed = this.sprintJumpSpeed;
        }
    }

    public FPSWalkerDB()
    {
        this.speed = 6f;
        this.aimSpeed = 2f;
        this.sprintSpeed = 10f;
        this.canSprint = true;
        this.sprintJumpSpeed = 8f;
        this.normSpeed = 6f;
        this.crouchSpeed = 6f;
        this.crouchDeltaHeight = 1f;
        this.jumpSpeed = 8f;
        this.normJumpSpeed = 8f;
        this.gravity = 20f;
        this.moveDirection = Vector3.zero;
    }

}