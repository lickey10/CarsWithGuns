using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class PenetrateVal : MonoBehaviour
{
    public int val;
    private int cache;
    private bool applied;
    public virtual void Apply(GunScript gscript)
    {
        this.cache = this.val - gscript.penetrateVal;
        gscript.penetrateVal = gscript.penetrateVal + this.cache;
    }

    public virtual void Remove(GunScript gscript)
    {
        gscript.penetrateVal = gscript.penetrateVal - this.cache;
    }

}