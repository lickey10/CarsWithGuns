using UnityEngine;
using System.Collections;

public enum RotationAxes
{
    MouseXAndY = 0,
    MouseX = 1,
    MouseY = 2
}

[System.Serializable]
[UnityEngine.AddComponentMenu("Camera-Control/Smooth Mouse Look")]
public partial class MouseTimeLook : MonoBehaviour
{
    public RotationAxes axes;
    public float sensitivityX;
    public float sensitivityY;
    public float minimumX;
    public float maximumX;
    public float minimumY;
    public float maximumY;
    public float smoothTimeX;
    public float smoothTimeY;
    public bool clampX;
    public bool clampY;
    [UnityEngine.HideInInspector]
    public float rotationX;
    [UnityEngine.HideInInspector]
    public float rotationY;
    public virtual void Start()
    {
        Screen.lockCursor = true;
        // Make the rigid body not change rotation
        if (this.GetComponent<Rigidbody>())
        {
            this.GetComponent<Rigidbody>().freezeRotation = true;
        }
    }

    public virtual void LateUpdate()
    {
        //transform.localEulerAngles.z = 0;
        if (this.axes == RotationAxes.MouseX)
        {
            this.rotationX = this.rotationX + (Input.GetAxis("Mouse X") * this.sensitivityX);
        }
        else
        {
            if (this.axes == RotationAxes.MouseXAndY)
            {
                this.rotationX = this.rotationX + (Input.GetAxis("Mouse X") * this.sensitivityX);
                this.rotationY = this.rotationY + (Input.GetAxis("Mouse Y") * this.sensitivityY);
            }
            else
            {
                if (this.axes == RotationAxes.MouseY)
                {
                    this.rotationY = this.rotationY + (Input.GetAxis("Mouse Y") * this.sensitivityY);
                }
            }
        }
        if (this.clampY)
        {
            this.rotationY = Mathf.Clamp(this.rotationY, this.minimumY, this.maximumY);
        }
        if (this.clampX)
        {
            this.rotationX = Mathf.Clamp(this.rotationX, this.minimumX, this.maximumX);
        }
        this.transform.localEulerAngles.y = Mathf.LerpAngle(this.transform.localEulerAngles.y, this.rotationX, Time.smoothDeltaTime * this.smoothTimeX);
        this.transform.localEulerAngles.x = Mathf.LerpAngle(this.transform.localEulerAngles.x, -this.rotationY, Time.smoothDeltaTime * this.smoothTimeY);
    }

    public MouseTimeLook()
    {
        this.axes = RotationAxes.MouseXAndY;
        this.sensitivityX = 15;
        this.sensitivityY = 15;
        this.minimumX = -360f;
        this.maximumX = 360f;
        this.minimumY = -60f;
        this.maximumY = 60f;
        this.smoothTimeX = 5;
        this.smoothTimeY = 5;
        this.clampY = true;
    }

}