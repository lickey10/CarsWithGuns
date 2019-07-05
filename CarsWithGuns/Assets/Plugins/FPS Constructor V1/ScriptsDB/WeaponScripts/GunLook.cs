using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class GunLook : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 CopyrightÂ© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
    ///This is similar to mouseLook, but is only for visual sway on weapons
    private float sensitivityX;
    private float sensitivityY;
    public float sensitivityStandardX;
    public float sensitivityStandardZ;
    public float sensitivityStandardY;
    public float sensitivityAimingX;
    public float sensitivityAimingZ;
    public float sensitivityAimingY;
    public float retSensitivity;
    [UnityEngine.HideInInspector]
    public float minimumX;
    [UnityEngine.HideInInspector]
    public float maximumX;
    public float xRange;
    public float xRangeAim;
    public float zRange;
    public float zRangeAim;
    private float actualZRange;
    private float sensitivityZ;
    public float yRange;
    public float yRangeAim;
    public float zMoveRange;
    public float zMoveSensitivity;
    public float zMoveAdjustSpeed;
    public float xMoveRange;
    public float xMoveSensitivity;
    public float xMoveAdjustSpeed;
    public float xAirMoveRange;
    public float xAirMoveSensitivity;
    public float xAirAdjustSpeed;
    public float zPosMoveRange;
    public float zPosMoveSensitivity;
    public float zPosAdjustSpeed;
    public float xPosMoveRange;
    public float xPosMoveSensitivity;
    public float xPosAdjustSpeed;
    private float minimumY;
    private float maximumY;
    //added by dw to pause camera when in store
    [UnityEngine.HideInInspector]
    public bool freeze;
    [UnityEngine.HideInInspector]
    public float rotationX;
    [UnityEngine.HideInInspector]
    public float rotationY;
    public float rotationZ;
    private Vector3 startPos;
    private Vector3 lastOffset;
    private Vector3 posOffset;
    private float curZ;
    private float curX;
    private float curX2;
    private float lastZ;
    private float lastX;
    private float tX;
    public bool useLookMotion;
    public bool useWalkMotion;
    public bool lookMotionOpen;
    public bool walkMotionOpen;
    private Quaternion originalRotation;
    public static Vector3 jostleAmt;
    public Vector3 curJostle;
    public Vector3 lastJostle;
    private Vector3 targetPosition;
    private Vector3 curTarget;
    private Vector3 lastTarget;
    public virtual void Freeze()
    {
        this.freeze = true;
    }

    public virtual void UnFreeze()
    {
        this.freeze = false;
    }

    public virtual void Update()
    {
        Quaternion xQuaternion = default(Quaternion);
        Quaternion yQuaternion = default(Quaternion);
        float zVal = 0.0f;
        float xVal = 0.0f;
        float xVal2 = 0.0f;
        if (this.freeze || !PlayerWeapons.playerActive)
        {
            return;
        }
        if (this.retSensitivity > 0)
        {
            this.retSensitivity = this.retSensitivity * -1;
        }
        if (this.useLookMotion && PlayerWeapons.canLook)
        {
            // Read the mouse input axis
            this.rotationX = this.rotationX + (InputDB.GetAxis("Mouse X") * this.sensitivityX);
            this.rotationY = this.rotationY + (InputDB.GetAxis("Mouse Y") * this.sensitivityY);
            this.rotationZ = this.rotationZ + (InputDB.GetAxis("Mouse X") * this.sensitivityZ);
            this.rotationX = GunLook.ClampAngle(this.rotationX, this.minimumX, this.maximumX);
            this.rotationY = GunLook.ClampAngle(this.rotationY, this.minimumY, this.maximumY);
            this.rotationZ = GunLook.ClampAngle(this.rotationZ, -this.actualZRange, this.actualZRange);
            if (Mathf.Abs(Input.GetAxis("Mouse X")) < 0.05f)
            {
                if (this.sensitivityX > 0)
                {
                    this.rotationX = this.rotationX - (((this.rotationX * Time.deltaTime) * this.retSensitivity) * 7);
                    this.rotationZ = this.rotationZ - (((this.rotationZ * Time.deltaTime) * this.retSensitivity) * 7);
                    this.rotationY = this.rotationY + (((this.rotationY * Time.deltaTime) * this.retSensitivity) * 7);
                }
                else
                {
                    this.rotationX = this.rotationX + (((this.rotationX * Time.deltaTime) * this.retSensitivity) * 7);
                    this.rotationZ = this.rotationZ + (((this.rotationZ * Time.deltaTime) * this.retSensitivity) * 7);
                    this.rotationY = this.rotationY + (((this.rotationY * Time.deltaTime) * this.retSensitivity) * 7);
                }
            }
            xQuaternion = Quaternion.AngleAxis(this.rotationX, Vector3.up);
            Quaternion zQuaternion = Quaternion.AngleAxis(this.rotationZ, Vector3.forward);
            yQuaternion = Quaternion.AngleAxis(this.rotationY, Vector3.left);
            this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, ((this.originalRotation * xQuaternion) * yQuaternion) * zQuaternion, Time.deltaTime * 10);
        }
        if (this.useWalkMotion)
        {
            //Velocity-based changes
            Vector3 relVelocity = this.transform.InverseTransformDirection(PlayerWeapons.CM.movement.velocity);
            this.lastOffset = this.posOffset;
            float s = new Vector3(PlayerWeapons.CM.movement.velocity.x, 0, PlayerWeapons.CM.movement.velocity.z).magnitude / 14;
            if (!AimMode.staticAiming)
            {
                float xPos = Mathf.Clamp(relVelocity.x * this.xPosMoveSensitivity, -this.xPosMoveRange * s, this.xPosMoveRange * s);
                this.posOffset.x = Mathf.Lerp(this.posOffset.x, xPos, Time.deltaTime * this.xPosAdjustSpeed);// + startPos.x;
                float zPos = Mathf.Clamp(relVelocity.z * this.zPosMoveSensitivity, -this.zPosMoveRange * s, this.zPosMoveRange * s);
                this.posOffset.z = Mathf.Lerp(this.posOffset.z, zPos, Time.deltaTime * this.zPosAdjustSpeed);// + startPos.z;
            }
            else
            {
                this.posOffset.x = Mathf.Lerp(this.posOffset.x, 0, (Time.deltaTime * this.xPosAdjustSpeed) * 3);// + startPos.x;
                this.posOffset.z = Mathf.Lerp(this.posOffset.z, 0, (Time.deltaTime * this.zPosAdjustSpeed) * 3);// + startPos.z;
            }
            //Apply Jostle
            this.lastJostle = this.curJostle;
            this.curJostle = Vector3.Lerp(this.curJostle, GunLook.jostleAmt, Time.deltaTime * 10);
            GunLook.jostleAmt = Vector3.Lerp(GunLook.jostleAmt, Vector3.zero, Time.deltaTime * 3);
            this.lastTarget = this.curTarget;
            this.curTarget = Vector3.Lerp(this.curTarget, this.posOffset, Time.deltaTime * 8);
            this.transform.localPosition = this.transform.localPosition + (this.curTarget - this.lastTarget);
            this.transform.localPosition = this.transform.localPosition + (this.curJostle - this.lastJostle);
        }
    }

    public virtual void Start()
    {
         // Make the rigid body not change rotation
        if (this.GetComponent<Rigidbody>())
        {
            this.GetComponent<Rigidbody>().freezeRotation = true;
        }
        this.originalRotation = this.transform.localRotation;
        this.startPos = this.transform.localPosition;
        this.StopAiming();
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
        {
            angle = angle + 360f;
        }
        if (angle > 360f)
        {
            angle = angle - 360f;
        }
        return Mathf.Clamp(angle, min, max);
    }

    public virtual void Aiming()
    {
        this.sensitivityX = this.sensitivityAimingX;
        this.sensitivityY = this.sensitivityAimingY;
        this.minimumX = -this.xRangeAim;
        this.maximumX = this.xRangeAim;
        this.minimumY = -this.yRangeAim;
        this.maximumY = this.yRangeAim;
        this.sensitivityZ = this.sensitivityAimingZ;
        this.actualZRange = this.zRangeAim;
    }

    public virtual void StopAiming()
    {
        this.sensitivityX = this.sensitivityStandardX;
        this.sensitivityY = this.sensitivityStandardY;
        this.minimumX = -this.xRange;
        this.maximumX = this.xRange;
        this.minimumY = -this.yRange;
        this.maximumY = this.yRange;
        this.sensitivityZ = this.sensitivityStandardZ;
        this.actualZRange = this.zRange;
    }

    public GunLook()
    {
        this.sensitivityX = 15f;
        this.sensitivityY = 15f;
        this.sensitivityStandardX = 15f;
        this.sensitivityStandardZ = 15f;
        this.sensitivityStandardY = 15f;
        this.sensitivityAimingX = 15f;
        this.sensitivityAimingZ = 15f;
        this.sensitivityAimingY = 15f;
        this.retSensitivity = -0.5f;
        this.minimumX = 5f;
        this.maximumX = 3f;
        this.xRange = 5f;
        this.xRangeAim = 3f;
        this.zRange = 5f;
        this.zRangeAim = 3f;
        this.yRange = 5f;
        this.yRangeAim = 3f;
        this.zMoveRange = 10;
        this.zMoveSensitivity = 0.5f;
        this.zMoveAdjustSpeed = 4;
        this.xMoveRange = 10;
        this.xMoveSensitivity = 0.5f;
        this.xMoveAdjustSpeed = 4;
        this.xAirMoveRange = 10;
        this.xAirMoveSensitivity = 0.5f;
        this.xAirAdjustSpeed = 4;
        this.zPosMoveRange = 0.13f;
        this.zPosMoveSensitivity = 0.5f;
        this.zPosAdjustSpeed = 4;
        this.xPosMoveRange = 0.13f;
        this.xPosMoveSensitivity = 0.5f;
        this.xPosAdjustSpeed = 4;
        this.minimumY = -60f;
        this.maximumY = 60f;
        this.useLookMotion = true;
        this.useWalkMotion = true;
    }

}