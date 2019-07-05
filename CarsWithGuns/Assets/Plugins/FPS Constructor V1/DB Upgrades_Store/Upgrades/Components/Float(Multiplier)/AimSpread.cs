using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class AimSpread : MonoBehaviour
{
    public float multiplier;
    private float cache;
    private bool applied;
    public virtual void Apply(GunScript gScript)
    {
        this.cache = gScript.aimSpread * (this.multiplier - 1);
        gScript.aimSpread = gScript.aimSpread + this.cache;
    }

    public virtual void Remove(GunScript gScript)
    {
        gScript.aimSpread = gScript.aimSpread - this.cache;
    }

    public AimSpread()
    {
        this.multiplier = 1.5f;
    }

}