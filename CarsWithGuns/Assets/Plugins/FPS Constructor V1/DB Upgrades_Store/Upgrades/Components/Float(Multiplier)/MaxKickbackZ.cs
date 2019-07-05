using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class MaxKickbackZ : MonoBehaviour
{
    public float multiplier;
    private float cache;
    private bool applied;
    public virtual void Apply(GunScript gscript)
    {
        this.cache = gscript.maxZ * (this.multiplier - 1);
        gscript.maxZ = gscript.maxZ + this.cache;
    }

    public virtual void Remove(GunScript gscript)
    {
        gscript.maxZ = gscript.maxZ - this.cache;
    }

    public MaxKickbackZ()
    {
        this.multiplier = 1.5f;
    }

}