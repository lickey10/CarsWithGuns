using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class ReloadTime : MonoBehaviour
{
    public float multiplier;
    private float cache;
    private bool applied;
    public virtual void Apply(GunScript gscript)
    {
        this.cache = gscript.reloadTime * (this.multiplier - 1);
        gscript.reloadTime = gscript.reloadTime + this.cache;
    }

    public virtual void Remove(GunScript gscript)
    {
        gscript.reloadTime = gscript.reloadTime - this.cache;
    }

    public ReloadTime()
    {
        this.multiplier = 1.5f;
    }

}