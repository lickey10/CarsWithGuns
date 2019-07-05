using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class AimSpreadRate : MonoBehaviour
{
    public float multiplier;
    private float cache;
    private bool applied;
    public virtual void Apply(GunScript gScript)
    {
        this.cache = gScript.aimSpreadRate * (this.multiplier - 1);
        gScript.aimSpreadRate = gScript.aimSpreadRate + this.cache;
    }

    public virtual void Remove(GunScript gScript)
    {
        gScript.aimSpreadRate = gScript.aimSpreadRate - this.cache;
    }

    public AimSpreadRate()
    {
        this.multiplier = 1.5f;
    }

}