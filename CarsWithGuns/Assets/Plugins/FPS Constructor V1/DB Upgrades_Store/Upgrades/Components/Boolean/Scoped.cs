using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Scoped : MonoBehaviour
{
    private AimMode gscript;
    public bool val;
    private bool cache;
    private bool applied;
    public virtual void Start()
    {
        this.gscript = (AimMode) ((GunScript) this.transform.parent.GetComponent(typeof(GunScript))).GetComponentInChildren(typeof(AimMode));
    }

    public virtual void Apply()
    {
        this.cache = this.gscript.scoped1;
        this.gscript.scoped = this.val;
        this.gscript.scoped1 = this.val;
    }

    public virtual void Remove()
    {
        this.gscript.scoped = this.cache;
        this.gscript.scoped1 = this.cache;
    }

}