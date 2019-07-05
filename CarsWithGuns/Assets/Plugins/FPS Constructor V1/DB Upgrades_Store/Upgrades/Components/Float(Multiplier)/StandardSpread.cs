using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class StandardSpread : MonoBehaviour
{
    public float multiplier;
    private float cache;
    private bool applied;
    public virtual void Apply(GunScript gscript)
    {
        this.cache = gscript.standardSpread * (this.multiplier - 1);
        gscript.standardSpread = gscript.standardSpread + this.cache;
    }

    public virtual void Remove(GunScript gscript)
    {
        gscript.standardSpread = gscript.standardSpread - this.cache;
    }

    public StandardSpread()
    {
        this.multiplier = 1.5f;
    }

}