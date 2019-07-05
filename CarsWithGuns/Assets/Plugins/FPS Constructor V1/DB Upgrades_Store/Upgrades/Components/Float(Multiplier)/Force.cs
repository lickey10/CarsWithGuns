using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Force : MonoBehaviour
{
    public float multiplier;
    private float cache;
    private bool applied;
    public virtual void Apply(GunScript gscript)
    {
        this.cache = gscript.force * (this.multiplier - 1);
        gscript.force = gscript.force + this.cache;
    }

    public virtual void Remove(GunScript gscript)
    {
        gscript.force = gscript.force - this.cache;
    }

    public Force()
    {
        this.multiplier = 1.5f;
    }

}