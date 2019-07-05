using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class BurstFire : MonoBehaviour
{
    public bool val;
    private bool cache;
    private bool applied;
    public virtual void Apply(GunScript gScript)
    {
        this.cache = gScript.burstFire;
        gScript.burstFire = this.val;
    }

    public virtual void Remove(GunScript gScript)
    {
        gScript.burstFire = this.cache;
    }

}