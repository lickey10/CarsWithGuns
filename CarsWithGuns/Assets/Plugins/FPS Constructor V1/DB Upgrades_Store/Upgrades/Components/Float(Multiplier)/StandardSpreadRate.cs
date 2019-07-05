using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class StandardSpreadRate : MonoBehaviour
{
    public float multiplier;
    private float cache;
    private bool applied;
    public virtual void Apply(GunScript gscript)
    {
        this.cache = gscript.standardSpreadRate * (this.multiplier - 1);
        gscript.standardSpreadRate = gscript.standardSpreadRate + this.cache;
    }

    public virtual void Remove(GunScript gscript)
    {
        gscript.standardSpreadRate = gscript.standardSpreadRate - this.cache;
    }

    public StandardSpreadRate()
    {
        this.multiplier = 1.5f;
    }

}