using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class AimPosition : MonoBehaviour
{
    [UnityEngine.HideInInspector]
    public AimMode gscript;
    public Vector3 val;
    private Vector3 cache;
    private bool applied;
    public virtual void Start()
    {
        this.gscript = (AimMode) ((GunScript) this.transform.parent.GetComponent(typeof(GunScript))).GetComponentInChildren(typeof(AimMode));
    }

    public virtual void Apply()
    {
        this.cache = this.gscript.aimPosition1;
        this.gscript.aimPosition1 = this.val;
        this.gscript.aimPosition = this.val;
    }

    public virtual void Remove()
    {
        this.gscript.aimPosition1 = this.cache;
        this.gscript.aimPosition = this.cache;
    }

}