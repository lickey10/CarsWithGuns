using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Damage : MonoBehaviour
{
    public float multiplier;
    private float cache;
    private bool applied;
    public virtual void Apply(GunScript gScript)
    {
        this.cache = gScript.damage * (this.multiplier - 1);
        gScript.damage = gScript.damage + this.cache;
    }

    public virtual void Remove(GunScript gScript)
    {
        gScript.damage = gScript.damage - this.cache;
    }

    public Damage()
    {
        this.multiplier = 1.5f;
    }

}