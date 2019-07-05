using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Volume : MonoBehaviour
{
    public float multiplier;
    private float cache;
    private bool applied;
    public virtual void Apply(GunScript gscript)
    {
        this.cache = gscript.fireVolume * (this.multiplier - 1);
        gscript.fireVolume = gscript.fireVolume + this.cache;
    }

    public virtual void Remove(GunScript gscript)
    {
        gscript.fireVolume = gscript.fireVolume - this.cache;
    }

    public Volume()
    {
        this.multiplier = 1.5f;
    }

}