using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Pitch : MonoBehaviour
{
    public float multiplier;
    private float cache;
    private bool applied;
    public virtual void Apply(GunScript gscript)
    {
        this.cache = gscript.firePitch * (this.multiplier - 1);
        gscript.firePitch = gscript.firePitch + this.cache;
    }

    public virtual void Remove(GunScript gscript)
    {
        gscript.firePitch = gscript.firePitch - this.cache;
    }

    public Pitch()
    {
        this.multiplier = 1.5f;
    }

}