using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class InfiniteAmmo : MonoBehaviour
{
    public bool val;
    private bool cache;
    private bool applied;
    public virtual void Apply(GunScript gScript)
    {
        this.cache = gScript.infiniteAmmo;
        gScript.infiniteAmmo = this.val;
    }

    public virtual void Remove(GunScript gScript)
    {
        gScript.infiniteAmmo = this.cache;
    }

}