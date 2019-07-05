using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class SpreadDecreaseRate : MonoBehaviour
{
    public float multiplier;
    private float cache;
    private bool applied;
    public virtual void Apply(GunScript gscript)
    {
        this.cache = gscript.spDecRate * (this.multiplier - 1);
        gscript.spDecRate = gscript.spDecRate + this.cache;
    }

    public virtual void Remove(GunScript gscript)
    {
        gscript.spDecRate = gscript.spDecRate - this.cache;
    }

    public SpreadDecreaseRate()
    {
        this.multiplier = 1.5f;
    }

}