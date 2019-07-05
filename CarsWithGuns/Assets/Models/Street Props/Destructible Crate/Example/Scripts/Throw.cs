using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Throw : MonoBehaviour
{
    public Rigidbody ball;
    public float throwPower;
    public virtual void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Rigidbody clone = null;
            clone = UnityEngine.Object.Instantiate(this.ball, this.transform.position, this.transform.rotation);
            clone.velocity = this.transform.TransformDirection(Vector3.forward * this.throwPower);
        }
    }

    public Throw()
    {
        this.throwPower = 10;
    }

}