using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class SecondaryWeapon : MonoBehaviour
{
    private GunScript gscript;
    public int s;
    private GunScript script;
    private GunScript cache;
    private bool applied;
    public virtual void Start()
    {
        GunScript[] gscripts = this.transform.parent.GetComponents<GunScript>();
        int q = 0;
        while (q < gscripts.Length)
        {
            if ((gscripts[q] != null) && gscripts[q].isPrimaryWeapon)
            {
                this.gscript = gscripts[q];
            }
            if (q == this.s)
            {
                this.script = gscripts[q];
            }
            q++;
        }
        this.cache = this.gscript.secondaryWeapon;
    }

    public virtual void Apply()
    {
        this.gscript.secondaryWeapon = this.script;
    }

    public virtual void Remove()
    {
        this.gscript.secondaryWeapon = this.cache;
    }

}