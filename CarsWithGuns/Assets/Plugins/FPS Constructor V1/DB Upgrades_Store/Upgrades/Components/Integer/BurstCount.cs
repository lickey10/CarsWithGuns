using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class BurstCount : MonoBehaviour
{
    public int val;
    private int cache;
    private bool applied;
    public virtual void Apply(GunScript gscript)
    {
        this.cache = this.val - gscript.burstCount;
        gscript.burstCount = gscript.burstCount + this.cache;
    }

    public virtual void Remove(GunScript gscript)
    {
        gscript.burstCount = gscript.burstCount - this.cache;
    }

}