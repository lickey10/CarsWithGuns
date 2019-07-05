using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class EnemyDrop : MonoBehaviour
{
    public Transform drops;
    public int min;
    public int max;
    public float force;
    public virtual void Die()
    {
        Vector3 dir = default(Vector3);
        int amt = Random.Range(this.min, this.max);
        int i = 0;
        Transform t = null;
        while (i < amt)
        {
            t = UnityEngine.Object.Instantiate(this.drops, this.transform.position + new Vector3(0, 2, 0), this.transform.rotation);
            dir = Random.insideUnitSphere * this.force;
            t.GetComponent<Rigidbody>().AddForce(dir, ForceMode.Impulse);
            t.GetComponent<Rigidbody>().AddTorque(dir, ForceMode.Impulse);
            i++;
        }
    }

    public EnemyDrop()
    {
        this.max = 5;
        this.force = 5;
    }

}