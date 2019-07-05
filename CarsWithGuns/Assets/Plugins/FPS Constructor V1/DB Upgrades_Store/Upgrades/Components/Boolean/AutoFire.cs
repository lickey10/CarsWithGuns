using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class AutoFire : MonoBehaviour
{
    public bool val;
    private bool cache;
    private bool applied;
    public virtual void Apply(GunScript gScript)
    {
        this.cache = gScript.autoFire;
        gScript.autoFire = this.val;
    }

    public virtual void Remove(GunScript gScript)
    {
        gScript.autoFire = this.cache;
    }

}