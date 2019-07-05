using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class KickbackAngle : MonoBehaviour
{
    public float multiplier;
    private float cache;
    private bool applied;
    public virtual void Apply(GunScript gscript)
    {
        this.cache = gscript.kickbackAngle * (this.multiplier - 1);
        gscript.kickbackAngle = gscript.kickbackAngle + this.cache;
    }

    public virtual void Remove(GunScript gscript)
    {
        gscript.kickbackAngle = gscript.kickbackAngle - this.cache;
    }

    public KickbackAngle()
    {
        this.multiplier = 1.5f;
    }

}