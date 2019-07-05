using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class ProgressiveReload : MonoBehaviour
{
    public bool val;
    private bool cache;
    private bool applied;
    public virtual void Apply(GunScript gScript)
    {
        this.cache = gScript.progressiveReload;
        gScript.progressiveReload = this.val;
    }

    public virtual void Remove(GunScript gScript)
    {
        gScript.progressiveReload = this.cache;
    }

}