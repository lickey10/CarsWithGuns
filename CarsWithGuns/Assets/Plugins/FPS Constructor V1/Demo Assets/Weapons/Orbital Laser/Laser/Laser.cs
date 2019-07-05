using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Laser : MonoBehaviour
{
    public float dps;
    public float power;
    public float forceRadius;
    public float vFactor;
    public virtual void OnTriggerStay(Collider other)
    {
        object[] sendArray = new object[2];
        sendArray[0] = this.dps * Time.deltaTime;
        sendArray[1] = true;
        other.SendMessageUpwards("ApplyDamage", sendArray, SendMessageOptions.DontRequireReceiver);
        if (other.GetComponent<Rigidbody>())
        {
            other.GetComponent<Rigidbody>().AddExplosionForce(this.power, this.transform.position, this.forceRadius, this.vFactor);
        }
    }

    public virtual void Finish()
    {
        UnityEngine.Object.Destroy(this.transform.parent);
    }

    public virtual void ChargeLevel()
    {
        this.transform.parent.position = GameObject.FindWithTag("Laser").transform.position;
    }

}