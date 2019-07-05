using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class JumpCrouchEffects : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 Copyright© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
    public float jumpHeight;
    public float crouchHeight;
    public float proneHeight;
    public float crouchSpeed;
    public float jumpAdjustSpeed;
    public float landingHeight;
    public float landAdjustSpeed;
    private bool airborne;
    private bool landingAdjusted;
    private float targetHeight;
    private bool aim;
    private float speed;
    private CharacterMotorDB CM;
    private float aimSpeed;
    public virtual void Update()
    {
        if (!this.CM.grounded)
        {
            this.targetHeight = this.jumpHeight;
            this.airborne = true;
            this.speed = this.jumpAdjustSpeed;
        }
        else
        {
            if (this.airborne)
            {
                this.airborne = false;
                this.targetHeight = this.landingHeight;
                this.landingAdjusted = false;
                this.speed = this.landAdjustSpeed;
            }
            else
            {
                if (CharacterMotorDB.crouching && this.landingAdjusted)
                {
                    this.targetHeight = this.crouchHeight;
                    this.speed = this.crouchSpeed;
                }
                else
                {
                    if (CharacterMotorDB.prone && this.landingAdjusted)
                    {
                        this.targetHeight = this.proneHeight;
                        this.speed = this.crouchSpeed;
                    }
                    else
                    {
                        if (this.landingAdjusted)
                        {
                            this.targetHeight = 0;
                            this.speed = this.crouchSpeed;
                        }
                    }
                }
            }
        }
        if (this.aim && this.landingAdjusted)
        {
            this.targetHeight = 0;
            this.speed = this.crouchSpeed * 2;
        }
        this.transform.localPosition.y = Mathf.Lerp(this.transform.localPosition.y, this.targetHeight, Time.deltaTime * this.speed);
        if (Mathf.Abs(this.transform.localPosition.y - this.targetHeight) < 0.1f)
        {
            this.landingAdjusted = true;
        }
    }

    public virtual void Start()
    {
        this.CM = (CharacterMotorDB) GameObject.FindWithTag("Player").GetComponent(typeof(CharacterMotorDB));
        this.targetHeight = 0;
    }

    public virtual void Aiming()
    {
        this.aim = true;
    }

    public virtual void StopAiming()
    {
        this.aim = false;
    }

    public JumpCrouchEffects()
    {
        this.jumpHeight = 0.15f;
        this.crouchHeight = -0.1f;
        this.proneHeight = -0.2f;
        this.crouchSpeed = 1;
        this.jumpAdjustSpeed = 1;
        this.landingHeight = -0.06f;
        this.landAdjustSpeed = 1;
        this.landingAdjusted = true;
        this.aimSpeed = 1;
    }

}