using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Zoom : MonoBehaviour
{
    private AimMode gscript;
    public float multiplier;
    private float cache;
    private bool applied;
    public virtual void Start()
    {
        this.gscript = (AimMode) ((GunScript) this.transform.parent.GetComponent(typeof(GunScript))).GetComponentInChildren(typeof(AimMode));
    }

    public virtual void Apply()
    {
        this.cache = this.gscript.zoomFactor1 * (this.multiplier - 1);
        this.gscript.zoomFactor1 = this.gscript.zoomFactor1 + this.cache;
        this.gscript.zoomFactor = this.gscript.zoomFactor1;
    }

    public virtual void Remove()
    {
        this.gscript.zoomFactor1 = this.gscript.zoomFactor1 - this.cache;
        this.gscript.zoomFactor = this.gscript.zoomFactor1;
    }

    public Zoom()
    {
        this.multiplier = 1.5f;
    }

}