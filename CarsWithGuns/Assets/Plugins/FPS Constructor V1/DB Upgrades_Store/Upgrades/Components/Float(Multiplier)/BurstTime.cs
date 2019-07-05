using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class BurstTime : MonoBehaviour
{
    public float multiplier;
    private float cache;
    private bool applied;
    public virtual void Apply(GunScript gScript)
    {
        this.cache = gScript.burstTime * (this.multiplier - 1);
        gScript.burstTime = gScript.burstTime + this.cache;
    }

    public virtual void Remove(GunScript gScript)
    {
        gScript.burstTime = gScript.burstTime - this.cache;
        this.cache = 0;
    }

    public BurstTime()
    {
        this.multiplier = 1.5f;
    }

}