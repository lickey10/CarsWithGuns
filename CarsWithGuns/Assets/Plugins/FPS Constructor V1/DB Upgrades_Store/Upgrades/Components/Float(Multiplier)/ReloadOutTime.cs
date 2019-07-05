using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class ReloadOutTime : MonoBehaviour
{
    public float multiplier;
    private float cache;
    private bool applied;
    public virtual void Apply(GunScript gscript)
    {
        this.cache = gscript.reloadOutTime * (this.multiplier - 1);
        gscript.reloadOutTime = gscript.reloadOutTime + this.cache;
    }

    public virtual void Remove(GunScript gscript)
    {
        gscript.reloadOutTime = gscript.reloadOutTime - this.cache;
    }

    public ReloadOutTime()
    {
        this.multiplier = 1.5f;
    }

}