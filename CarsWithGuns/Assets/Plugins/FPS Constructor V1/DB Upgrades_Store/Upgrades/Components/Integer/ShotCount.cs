using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class ShotCount : MonoBehaviour
{
    public int val;
    private int cache;
    private bool applied;
    public virtual void Apply(GunScript gscript)
    {
        this.cache = this.val - gscript.shotCount;
        gscript.shotCount = gscript.shotCount + this.cache;
    }

    public virtual void Remove(GunScript gscript)
    {
        gscript.shotCount = gscript.shotCount - this.cache;
    }

}