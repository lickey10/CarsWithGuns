using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation
/// To make an FPS style character:
/// - Create a capsule.
/// - Add a rigid body to the capsule
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSWalker script to the capsule
/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
//[AddComponentMenu("Camera-Control/Mouse Look")]
public enum RotationAxes
{
    MouseX = 0,
    MouseY = 1
}

[System.Serializable]
public partial class MouseLookDBJS : MonoBehaviour
{
    public RotationAxes axes;
    [UnityEngine.HideInInspector]
    public float sensitivityX;
    [UnityEngine.HideInInspector]
    public float sensitivityY;
    [UnityEngine.HideInInspector]
    public float sensitivityStandardX;
    [UnityEngine.HideInInspector]
    public float sensitivityStandardY;
    [UnityEngine.HideInInspector]
    public float offsetY;
    [UnityEngine.HideInInspector]
    public float offsetX;
    [UnityEngine.HideInInspector]
    public float totalOffsetY;
    [UnityEngine.HideInInspector]
    public float totalOffsetX;
    [UnityEngine.HideInInspector]
    public float resetSpeed;
    [UnityEngine.HideInInspector]
    public float resetDelay;
    [UnityEngine.HideInInspector]
    public float maxKickback;
    [UnityEngine.HideInInspector]
    public float xDecrease;
    public float minimumX;
    public float maximumX;
    public float minimumY;
    public float maximumY;
    public bool smooth;
    public float smoothFactor;
    public object[] smoothIterations;
    public int iterations;
    private Quaternion tRotation;
    public float idleSway;
    private int minStored;
    private int maxStored;
    //added by dw to pause camera when in store
    [UnityEngine.HideInInspector]
    public static bool freeze;
    [UnityEngine.HideInInspector]
    public bool individualFreeze;
    [UnityEngine.HideInInspector]
    public float rotationX;
    [UnityEngine.HideInInspector]
    public float rotationY;
    [UnityEngine.HideInInspector]
    public Quaternion originalRotation;
    private Quaternion[] temp;
    private Quaternion smoothRotation;
    public virtual void Freeze()
    {
        MouseLookDBJS.freeze = true;
    }

    public virtual void UnFreeze()
    {
        MouseLookDBJS.freeze = false;
    }

    public virtual void SetRotation(Vector3 target)//rotationY = target.x;
    {
        this.rotationX = target.y;
    }

    public virtual void Update()
    {
        Quaternion xQuaternion = default(Quaternion);
        Quaternion yQuaternion = default(Quaternion);
        float offsetVal = 0.0f;
        float xDecrease = 0.0f;
        if ((MouseLookDBJS.freeze || !PlayerWeapons.canLook) || this.individualFreeze)
        {
            return;
        }
        if (this.axes == RotationAxes.MouseX)
        {
            this.rotationX = this.rotationX + (InputDB.GetAxis("Mouse X") * this.sensitivityX);
            if (this.totalOffsetX > 0)
            {
                xDecrease = Mathf.Clamp(this.resetSpeed * Time.deltaTime, 0, this.totalOffsetX);
            }
            else
            {
                xDecrease = Mathf.Clamp(this.resetSpeed * -Time.deltaTime, this.totalOffsetX, 0);
            }
            if (this.resetDelay > 0)
            {
                xDecrease = 0;
                this.resetDelay = Mathf.Clamp(this.resetDelay - Time.deltaTime, 0, this.resetDelay);
            }
            if (Random.value < 0.5f)
            {
                this.offsetX = this.offsetX * -1;
            }
            if (((this.totalOffsetX < this.maxKickback) && (this.totalOffsetX >= 0)) || ((this.totalOffsetX > -this.maxKickback) && (this.totalOffsetX <= 0)))
            {
                this.totalOffsetX = this.totalOffsetX + this.offsetX;
            }
            else
            {
                //offsetX = 0;
                this.resetDelay = this.resetDelay * 0.5f;
            }
            this.rotationX = (MouseLookDBJS.ClampAngle(this.rotationX, this.minimumX, this.maximumX) + this.offsetX) - xDecrease;
            if ((Input.GetAxis("Mouse X") * this.sensitivityX) < 0)
            {
                this.totalOffsetX = this.totalOffsetX + (Input.GetAxis("Mouse X") * this.sensitivityX);
            }
            this.rotationX = this.rotationX + (Mathf.Sin(Time.time) * this.idleSway);
            this.totalOffsetX = this.totalOffsetX - xDecrease;
            if (this.totalOffsetX < 0)
            {
                this.totalOffsetX = 0;
            }
            xQuaternion = Quaternion.AngleAxis(this.rotationX, Vector3.up);
            this.tRotation = this.originalRotation * xQuaternion;
            offsetVal = Mathf.Clamp(this.totalOffsetX * this.smoothFactor, 1, this.smoothFactor);
            if (this.smooth)
            {
                this.transform.localRotation = Quaternion.Slerp(this.transform.localRotation, this.tRotation, ((Time.deltaTime * 25) / this.smoothFactor) * offsetVal);
            }
            else
            {
                this.transform.localRotation = this.tRotation;
            }
        }
        else
        {
            this.rotationY = this.rotationY + (InputDB.GetAxis("Mouse Y") * this.sensitivityY);
            float yDecrease = Mathf.Clamp(this.resetSpeed * Time.deltaTime, 0, this.totalOffsetY);
            if (this.resetDelay > 0)
            {
                yDecrease = 0;
                this.resetDelay = Mathf.Clamp(this.resetDelay - Time.deltaTime, 0, this.resetDelay);
            }
            if (this.totalOffsetY < this.maxKickback)
            {
                this.totalOffsetY = this.totalOffsetY + this.offsetY;
            }
            else
            {
                this.offsetY = 0;
                this.resetDelay = this.resetDelay * 0.5f;
            }
            this.rotationY = (MouseLookDBJS.ClampAngle(this.rotationY, this.minimumY, this.maximumY) + this.offsetY) - yDecrease;
            if ((Input.GetAxis("Mouse Y") * this.sensitivityY) < 0)
            {
                this.totalOffsetY = this.totalOffsetY + (Input.GetAxis("Mouse Y") * this.sensitivityY);
            }
            this.rotationY = this.rotationY + (Mathf.Sin(Time.time) * this.idleSway);
            this.totalOffsetY = this.totalOffsetY - yDecrease;
            if (this.totalOffsetY < 0)
            {
                this.totalOffsetY = 0;
            }
            yQuaternion = Quaternion.AngleAxis(this.rotationY, Vector3.left);
            this.tRotation = this.originalRotation * yQuaternion;
            offsetVal = Mathf.Clamp(this.totalOffsetY * this.smoothFactor, 1, this.smoothFactor);
            if (this.smooth)
            {
                this.transform.localEulerAngles.x = Quaternion.Slerp(this.transform.localRotation, this.tRotation, ((Time.deltaTime * 25) / this.smoothFactor) * offsetVal).eulerAngles.x;
            }
            else
            {
                this.transform.localEulerAngles.x = this.tRotation.x;
            }
        }
        this.offsetY = 0;
        this.offsetX = 0;
    }

    public virtual void Start()
    {
        // Make the rigid body not change rotation
        if (this.GetComponent<Rigidbody>())
        {
            this.GetComponent<Rigidbody>().freezeRotation = true;
        }
        this.originalRotation = this.transform.localRotation;
        this.sensitivityX = this.sensitivityStandardX;
        this.sensitivityY = this.sensitivityStandardY;
        if (this.smoothFactor <= 1)
        {
            this.smoothFactor = 1;
        }
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

    public virtual void Aiming(float zoom)
    {
        this.sensitivityX = this.sensitivityX / zoom;
        this.sensitivityY = this.sensitivityY / zoom;
    }

    public virtual void StopAiming()
    {
        this.sensitivityX = this.sensitivityStandardX;
        this.sensitivityY = this.sensitivityStandardY;
    }

    public virtual void LockIt(int min, int max)
    {
        if (this.axes == RotationAxes.MouseX)
        {
            this.maxStored = this.maximumX;
            this.minStored = this.minimumX;
            this.maximumX = this.rotationX + max;
            this.minimumX = this.rotationX - min;
        }
        else
        {
            this.maxStored = this.maximumY;
            this.minStored = this.minimumY;
            this.maximumY = this.rotationY + max;
            this.minimumY = this.rotationY - min;
        }
    }

    public virtual void LockItSpecific(int min, int max)
    {
        if (this.axes == RotationAxes.MouseX)
        {
            this.maxStored = this.maximumX;
            this.minStored = this.minimumX;
            this.maximumX = max;
            this.minimumX = min;
        }
        else
        {
            this.maxStored = this.maximumY;
            this.minStored = this.minimumY;
            this.maximumY = max;
            this.minimumY = min;
        }
    }

    public virtual void UnlockIt()
    {
        if (this.axes == RotationAxes.MouseX)
        {
            this.maximumX = this.maxStored;
            this.minimumX = this.minStored;
        }
        else
        {
            this.maximumY = this.maxStored;
            this.minimumY = this.minStored;
        }
    }

    public virtual void UpdateIt()
    {
        this.rotationX = this.transform.localEulerAngles.y - this.originalRotation.eulerAngles.y;
        this.rotationY = this.transform.localEulerAngles.x - this.originalRotation.eulerAngles.x;
        this.totalOffsetX = 0;
    }

    public MouseLookDBJS()
    {
        this.axes = RotationAxes.MouseX;
        this.sensitivityX = 15f;
        this.sensitivityY = 15f;
        this.sensitivityStandardX = 15f;
        this.sensitivityStandardY = 15f;
        this.resetSpeed = 1;
        this.minimumX = -360f;
        this.maximumX = 360f;
        this.minimumY = -60f;
        this.maximumY = 60f;
        this.smooth = true;
        this.smoothFactor = 2;
        this.smoothIterations = new object[0];
        this.iterations = 10;
    }

}