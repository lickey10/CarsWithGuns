using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class CamSway : MonoBehaviour
{
    public Vector2 moveSwayRate;
    public Vector2 moveSwayAmplitude;
    public Vector2 runSwayRate;
    public Vector2 runSwayAmplitude;
    public float swayStartTime;
    public Vector2 idleSwayRate;
    public Vector2 idleAmplitude;
    private Vector3 val;
    private Vector3 lastVal;
    private Vector2 swayRate;
    private Vector2 swayAmplitude;
    public static CamSway singleton;
    public Vector3 curJostle;
    public Vector3 lastJostle;
    public static Vector3 jostleAmt;
    public virtual void Awake()
    {
        CamSway.singleton = this;
    }

    public virtual void WalkSway()
    {
        Vector3 curVect = default(Vector3);
        if (this.swayStartTime > Time.time)
        {
            this.swayStartTime = Time.time;
        }
        CharacterMotorDB CM = PlayerWeapons.CM;
        int speed = ((CharacterController) CM.GetComponent(typeof(CharacterController))).velocity.magnitude;
        //Jostle
        this.lastJostle = this.curJostle;
        this.curJostle = Vector3.Lerp(this.curJostle, CamSway.jostleAmt, Time.deltaTime * 16);
        CamSway.jostleAmt = Vector3.Lerp(CamSway.jostleAmt, Vector3.zero, Time.deltaTime * 4);
        this.transform.localPosition = this.transform.localPosition + ((this.curJostle - this.lastJostle) * 15);
        if (speed < 0.2f)
        {
            this.ResetPosition();
            return;
        }
        //sine function for motion
        float t = Time.time - this.swayStartTime;
        this.swayAmplitude = this.moveSwayAmplitude;
        if (CharacterMotorDB.crouching)
        {
            this.swayRate = (this.moveSwayRate * CM.movement.maxCrouchSpeed) / CM.movement.defaultForwardSpeed;
        }
        else
        {
            if (CharacterMotorDB.prone)
            {
                this.swayRate = (this.moveSwayRate * CM.movement.maxProneSpeed) / CM.movement.defaultForwardSpeed;
            }
            else
            {
                if (AimMode.sprintingPublic)
                {
                    this.swayRate = this.runSwayRate;
                    this.swayAmplitude = this.runSwayAmplitude;
                }
                else
                {
                    this.swayRate = this.moveSwayRate;
                }
            }
        }
        curVect.x = this.swayAmplitude.x * Mathf.Sin(this.swayRate.x * t);//*Mathf.Sin(swayRate.x*speed/14*t);
        curVect.y = Mathf.Abs(this.swayAmplitude.y * Mathf.Sin(this.swayRate.y * t));
        curVect.x = curVect.x - (this.swayAmplitude.x / 2);
        curVect.y = curVect.y - (this.swayAmplitude.y / 2);
        //Move
        this.lastVal = this.val;
        this.val.x = Mathf.Lerp(this.val.x, curVect.x, Time.deltaTime * this.swayRate.x);
        this.transform.localEulerAngles.z = Mathf.LerpAngle(this.transform.localEulerAngles.z, -curVect.x * 0.5f, Time.deltaTime * this.swayRate.x);
        this.val.y = Mathf.Lerp(this.val.y, curVect.y, Time.deltaTime * this.swayRate.y);
        this.transform.localEulerAngles.x = Mathf.LerpAngle(this.transform.localEulerAngles.x, -curVect.y * 0.5f, Time.deltaTime * this.swayRate.y);
        //transform.localPosition.x = Vector3.Lerp(transform.localPosition.x, curVect.x, Time.deltaTime*swayRate.x);
        this.transform.localPosition.x = this.transform.localPosition.x + (this.val.x - this.lastVal.x);
        this.transform.localPosition.y = this.transform.localPosition.y + (this.val.y - this.lastVal.y);
    }

    public virtual void ResetPosition()
    {
        this.swayStartTime = 9999999999999;
        if (this.transform.localPosition == new Vector3(0, 0, 0))
        {
            return;
        }
        //Move
        this.lastVal = this.val;
        this.val.x = Mathf.Lerp(this.val.x, 0, Time.deltaTime * this.idleSwayRate.x);
        this.transform.localEulerAngles.z = Mathf.LerpAngle(this.transform.localEulerAngles.z, 0, Time.deltaTime * this.idleSwayRate.x);
        this.val.y = Mathf.Lerp(this.val.y, 0, Time.deltaTime * this.idleSwayRate.y);
        this.transform.localEulerAngles.x = Mathf.LerpAngle(this.transform.localEulerAngles.x, 0, Time.deltaTime * this.idleSwayRate.y);
        this.transform.localPosition.x = this.transform.localPosition.x + (this.val.x - this.lastVal.x);
        this.transform.localPosition.y = this.transform.localPosition.y + (this.val.y - this.lastVal.y);
    }

    public virtual void Update()
    {
        if ((!AimMode.staticAiming && PlayerWeapons.CM.grounded) && !CharacterMotorDB.paused)// && CM.walking){
        {
            this.WalkSway();
        }
        else
        {
            this.ResetPosition();
        }
    }

}