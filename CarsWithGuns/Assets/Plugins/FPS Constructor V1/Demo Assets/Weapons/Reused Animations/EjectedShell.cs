using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class EjectedShell : MonoBehaviour
{
    public Vector3 force;
    public float randomFactorForce;
    //var gravity : float;
    public Vector3 torque;
    public float randomFactorTorque;
    public virtual void Start()
    {
        this.GetComponent<Rigidbody>().AddRelativeForce(this.force * Random.Range(1, this.randomFactorForce));
        this.GetComponent<Rigidbody>().AddRelativeTorque(this.torque * Random.Range(-this.randomFactorTorque, this.randomFactorTorque));
    }

}