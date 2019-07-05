using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class ReloadInTime : MonoBehaviour
{
    public float multiplier;
    private float cache;
    private bool applied;
    public virtual void Apply(GunScript gscript)
    {
        this.cache = gscript.reloadInTime * (this.multiplier - 1);
        gscript.reloadInTime = gscript.reloadInTime + this.cache;
    }

    public virtual void Remove(GunScript gscript)
    {
        gscript.reloadInTime = gscript.reloadInTime - this.cache;
    }

    public ReloadInTime()
    {
        this.multiplier = 1.5f;
    }

}