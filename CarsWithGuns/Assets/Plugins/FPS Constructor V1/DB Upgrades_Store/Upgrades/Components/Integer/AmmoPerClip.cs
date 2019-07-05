using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class AmmoPerClip : MonoBehaviour
{
    public int val;
    private int cache;
    private bool applied;
    public virtual void Apply(GunScript gscript)
    {
        this.cache = this.val - gscript.ammoPerClip;
        gscript.ammoPerClip = gscript.ammoPerClip + this.cache;
        if (gscript.ammoLeft > gscript.ammoPerClip)
        {
            gscript.ammoLeft = gscript.ammoPerClip;
        }
    }

    public virtual void Remove(GunScript gscript)
    {
        gscript.ammoPerClip = gscript.ammoPerClip - this.cache;
        if (gscript.ammoLeft > gscript.ammoPerClip)
        {
            gscript.ammoLeft = gscript.ammoPerClip;
        }
    }

}///when decreasing clip size add to clip reserve