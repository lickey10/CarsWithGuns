using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class FireRate : MonoBehaviour
{
    public float multiplier;
    private float cache;
    private bool applied;
    public virtual void Apply(GunScript gScript)
    {
        this.cache = gScript.fireRate * (this.multiplier - 1);
        gScript.fireRate = gScript.fireRate + this.cache;
    }

    public virtual void Remove(GunScript gScript)
    {
        gScript.fireRate = gScript.fireRate - this.cache;
    }

    public FireRate()
    {
        this.multiplier = 1.5f;
    }

}