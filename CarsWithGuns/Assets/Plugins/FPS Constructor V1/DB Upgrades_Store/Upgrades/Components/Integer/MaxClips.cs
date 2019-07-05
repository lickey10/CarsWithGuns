using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class MaxClips : MonoBehaviour
{
    public int val;
    private int cache;
    private bool applied;
    public virtual void Apply(GunScript gscript)
    {
        this.cache = this.val - gscript.maxClips;
        gscript.maxClips = gscript.maxClips + this.cache;
    }

    public virtual void Remove(GunScript gscript)
    {
        gscript.maxClips = gscript.maxClips - this.cache;
    }

}