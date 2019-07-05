using UnityEngine;
using System.Collections;

[System.Serializable]
[UnityEngine.AddComponentMenu("Physics/Constrain Motion")]
public partial class ConstrainMotion : MonoBehaviour
{
    public bool xMotion;
    public bool yMotion;
    public bool zMotion;
    public virtual void FixedUpdate()
    {
        Vector3 relativeSpeed = this.transform.InverseTransformDirection(this.GetComponent<Rigidbody>().velocity);
        if (!this.xMotion)
        {
            relativeSpeed.x = 0;
        }
        if (!this.yMotion)
        {
            relativeSpeed.y = 0;
        }
        if (!this.zMotion)
        {
            relativeSpeed.z = 0;
        }
        this.GetComponent<Rigidbody>().AddRelativeForce(-relativeSpeed, ForceMode.VelocityChange);
    }

    public ConstrainMotion()
    {
        this.xMotion = true;
        this.yMotion = true;
        this.zMotion = true;
    }

}