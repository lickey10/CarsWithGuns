using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class AmmoSet : MonoBehaviour
{
    public int val;
    private int cache;
    private bool applied;
    public virtual void Apply(GunScript gscript)
    {
        gscript.ApplyToSharedAmmo();
        this.cache = gscript.ammoSetUsed;
        gscript.ammoSetUsed = this.val;
        gscript.AlignToSharedAmmo();
    }

    public virtual void Remove(GunScript gscript)
    {
        gscript.ApplyToSharedAmmo();
        gscript.ammoSetUsed = this.cache;
        gscript.AlignToSharedAmmo();
    }

}