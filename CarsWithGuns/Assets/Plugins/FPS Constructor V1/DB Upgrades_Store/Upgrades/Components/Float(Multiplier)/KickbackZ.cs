using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class KickbackZ : MonoBehaviour
{
    public float multiplier;
    private float cache;
    private bool applied;
    public virtual void Apply(GunScript gscript)
    {
        this.cache = gscript.kickBackZ * (this.multiplier - 1);
        gscript.kickBackZ = gscript.kickBackZ + this.cache;
    }

    public virtual void Remove(GunScript gscript)
    {
        gscript.kickBackZ = gscript.kickBackZ - this.cache;
    }

    public KickbackZ()
    {
        this.multiplier = 1.5f;
    }

}