using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class MaxKickback : MonoBehaviour
{
    public float multiplier;
    private float cache;
    private bool applied;
    public virtual void Apply(GunScript gscript)
    {
        this.cache = gscript.maxKickback * (this.multiplier - 1);
        gscript.maxKickback = gscript.maxKickback + this.cache;
    }

    public virtual void Remove(GunScript gscript)
    {
        gscript.maxKickback = gscript.maxKickback - this.cache;
    }

    public MaxKickback()
    {
        this.multiplier = 1.5f;
    }

}